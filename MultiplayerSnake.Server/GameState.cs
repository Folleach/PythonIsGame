using Folleach.StreamNet.Server;
using MultiplayerSnake.Models;
using MultiplayerSnake.Models.SCModels;
using PythonIsGame.Common;
using PythonIsGame.Common.Entities;
using PythonIsGame.Common.Map;
using PythonIsGame.Common.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MultiplayerSnake.Server
{
    public class GameState
    {
        private static readonly Type WallMaterialType = typeof(WallMaterial);
        private static readonly Type AppleMaterialType = typeof(AppleMaterial);
        private static readonly Type SnakeNetworkBodyType = typeof(NetworkBody);

        private Dictionary<ServerUser, NetworkSnake> players = new Dictionary<ServerUser, NetworkSnake>();
        private Timer updater = new Timer(400);
        private IMap map;
        private AreaMapGenerator mapGenerator;
        private object updateLocker = new object();

        private int leaderboardCount;

        public GameState(IMap map, AreaMapGenerator mapGenerator, int board)
        {
            this.map = map;
            this.mapGenerator = mapGenerator;
            this.leaderboardCount = board;
            updater.Elapsed += (s, e) => Update();
            updater.Start();
            lock (updateLocker)
            {
                SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
                SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
                SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
            }
        }

        private void Update()
        {
            var gameState = new UpdateGameStateData();
            lock (updateLocker)
            {
                foreach (var player in players)
                {
                    player.Value.StepTo(player.Value.Direction);
                    gameState.Positions.Add(new UserStateData()
                    {
                        GUID = player.Value.GUID,
                        Position = player.Value.Head.Position
                    });
                }
                foreach (var player in players)
                {
                    player.Key.Send(3, gameState);
                }
                map.Update();
            }
        }

        private void AddTailSegment(NetworkSnake snake)
        {
            snake.AddTailSegment();
            var tailData = new ChangeTailData()
            {
                GUID = snake.GUID,
                DeltaTailSegment = 1
            };
            foreach (var player in players)
                player.Key.Send(6, tailData);
        }

        private void SetMaterial(IMaterial material, Point position)
        {
            map.SetMaterial(material, position);
            var materialData = new SetMaterialData()
            {
                Position = position,
                Material = material
            };
            foreach (var player in players)
                player.Key.Send(7, materialData);
        }

        private void IntersectWithWall(ServerUser user, PositionMaterial material)
        {
            var snake = players[user];
            user.Send(8, new KickData()
            {
                Message = "Вы были убиты стеной.",
                Score = snake.Score
            }, true);
        }

        private void IntersectWithApple(ServerUser user, PositionMaterial material)
        {
            AddTailSegment(players[user]);
            SetMaterial(new AppleMaterial(), mapGenerator.GetRandomPointInArea());
            SetMaterial(null, material.Position);
            players[user].Score++;
            UpdateLeaderboard();
        }

        private void IntersectWithBody(ServerUser user, IEntity entity)
        {
            var snake = players[user];
            var body = entity as NetworkBody;
            user.Send(8, new KickData()
            {
                Message = $"Вы были убиты игроком {body.Snake.Name}.",
                Score = snake.Score
            }, true);
        }

        private void UpdateLeaderboard()
        {
            var leader = new LeaderboardSetData();
            leader.Leaderboard = new Dictionary<string, int>();
            lock (updateLocker)
            {
                var top = players.OrderByDescending(x => x.Value.Score).Take(leaderboardCount).Select(x => x.Value);
                foreach (var player in top)
                    leader.Leaderboard.Add(player.Name, player.Score);
            }
            foreach (var player in players)
                player.Key.Send(9, leader);
        }

        public void AddPlayer(ServerUser user, int x, int y, string name, Guid guid)
        {
            var networkSnake = new NetworkSnake(x, y, map, name, guid);
            var spawnSnake = new SpawnSnakeData()
            {
                Name = name,
                X = x,
                Y = y,
                GUID = guid
            };
            lock (updateLocker)
            {
                foreach (var player in players)
                {
                    if (player.Value.Name == networkSnake.Name)
                    {
                        user.Send(8, new KickData()
                        {
                            Message = "Такой ник уже есть на сервере.",
                            Score = -10101001
                        });
                        return;
                    }
                }
                foreach (var player in players)
                {
                    player.Key.Send(4, spawnSnake);
                    user.Send(4, new SpawnSnakeData()
                    {
                        Name = player.Value.Name,
                        X = player.Value.Position.X,
                        Y = player.Value.Position.Y,
                        GUID = player.Value.GUID,
                        Tail = player.Value.GetEntities().Skip(1).Select(body => body.Position).ToArray()
                    });
                }
                players.Add(user, networkSnake);
                AddTailSegment(networkSnake);
                map.RegisterIntersectionWithMaterial(networkSnake.Head, WallMaterialType, m => IntersectWithWall(user, m));
                map.RegisterIntersectionWithMaterial(networkSnake.Head, AppleMaterialType, m => IntersectWithApple(user, m));
                map.RegisterIntersectionWithEntity(networkSnake.Head, SnakeNetworkBodyType, e => IntersectWithBody(user, e));
                UpdateLeaderboard();
            }
            Console.WriteLine($"{name} подключился к серверу. <{networkSnake.GUID}>");
        }

        public void RemovePlayer(ServerUser user)
        {
            NetworkSnake toremove = null;
            lock (updateLocker)
            {
                if (!players.ContainsKey(user))
                    return;
                toremove = players[user];
                map.UnregisterIntersectionWithMaterial(toremove.Head, WallMaterialType);
                map.UnregisterIntersectionWithMaterial(toremove.Head, AppleMaterialType);
                map.UnregisterIntersectionWithEntity(toremove.Head, SnakeNetworkBodyType);
                toremove.Kill();
            }
            var despawnSnake = new DespawnSnakeData()
            {
                GUID = toremove.GUID
            };
            lock (updateLocker)
            {
                players.Remove(user);
                foreach (var player in players)
                    player.Key.Send(5, despawnSnake);
                UpdateLeaderboard();
            }
            Console.WriteLine($"{toremove.Name} покинул сервер.");
        }

        public void SetDirection(ServerUser user, Direction direction)
        {
            if (players.ContainsKey(user))
            {
                var player = players[user];
                player.Direction = direction;
            }
        }
    }
}
