using Folleach.StreamNet.Server;
using MultiplayerSnake.Models.CSModels;
using MultiplayerSnake.Models.SCModels;
using PythonIsGame.Common.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace MultiplayerSnake.Server
{
    class Program
    {
        private static StreamNetServer server;

        private static AreaMapGenerator mapGenerator;
        private static ChunkedMap map;

        private static GameState game;

        private static int leaderboardCount = 8;

        private static int MapX = 0;
        private static int MapY = 0;
        private static int MapWidth = 70;
        private static int MapHeight = 60;

        private static bool IsWork = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Создание игрового пространства...");
            mapGenerator = new AreaMapGenerator(MapX, MapY, MapWidth, MapHeight, false, 128);
            map = new ChunkedMap(mapGenerator, 1);

            game = new GameState(map, mapGenerator, leaderboardCount);

            Console.WriteLine("Запуск сервера...");
            var listener = new TcpStreamListener(IPAddress.Any, 51777);
            listener.Connected += Listener_Connected;
            listener.Disconnected += Listener_Disconnected;
            server = new StreamNetServer(listener);
            server.RegisterCSID(1, InitializeNewUser);
            server.RegisterCSID(2, RequestChunk);
            server.RegisterCSID(3, UserChangeDirecion);
            server.Start();

            Console.WriteLine("Сервер запущен");
            while (IsWork)
                Thread.Sleep(600000);
        }

        private static void UserChangeDirecion(ServerUser user, byte[] data)
        {
            var model = ChangeDirection.Unpack(data);
            game.SetDirection(user, model.Direction);
        }

        private static void RequestChunk(ServerUser user, byte[] data)
        {
            var model = ChunkRequestData.Unpack(data);
            Console.WriteLine($"Запрошен кусок карты на ({model.X},{model.Y})");
            user.Send(2, new ChunkData() { ChunkObject = map.GetChunk(new Point(model.X, model.Y)) });
        }

        private static void InitializeNewUser(ServerUser user, byte[] data)
        {
            var model = LoginData.Unpack(data);
            Console.WriteLine($"Инициализация нового игрока {model.UserName}");
            var initPosition = mapGenerator.GetRandomPointInArea();
            var guild = Guid.NewGuid();
            var outputData = new ServerInitializePropertiesData()
            {
                ChunkSize = mapGenerator.ChunkSize,
                InitX = initPosition.X,
                InitY = initPosition.Y,
                GUID = guild,
                LeaderboardCount = leaderboardCount,
                MapArea = new Rectangle(MapX, MapY, MapWidth, MapHeight)
            };
            user.Send(1, outputData);
            game.AddPlayer(user, initPosition.X, initPosition.Y, model.UserName, guild);
        }

        private static void Listener_Disconnected(ServerUser user)
        {
            game.RemovePlayer(user);
            Console.WriteLine("Кто-то был выкинут с подключений.");
        }

        private static void Listener_Connected(ServerUser user)
        {
            Console.WriteLine("Попытка инициализации подключения.");
        }
    }
}
