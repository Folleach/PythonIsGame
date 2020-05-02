using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PythonIsGame.Common.Extensions;
using System.Text;
using System.Threading.Tasks;

namespace PythonIsGame.Common.Algorithms
{
    using Track = SinglyLinkedList<Point>;
    public class BFS
    {
        public static LinkedList<Direction> FindPath(IMap map, Point start, Type typeOfSeacrh, Func<Point, bool> canStepTo)
        {
            var queue = new Queue<Point>();
            var visited = new HashSet<Point>();
            var track = new Dictionary<Point, Track>();
            queue.Enqueue(start);
            track.Add(start, new Track(start));
            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                foreach (var point in Neighbors(current.X, current.Y))
                {
                    if (visited.Contains(point) || !canStepTo(point))
                        continue;
                    if (!track.ContainsKey(point))
                        track.Add(point, new Track(point, track[current]));
                    var material = map.GetMaterial(point).Material;
                    if (material != null && material.GetType() == typeOfSeacrh)
                    {
                        return TrackToDirections(track[point], true);
                    }
                    visited.Add(point);
                    queue.Enqueue(point);
                }
            }
            return new LinkedList<Direction>();
        }

        private static LinkedList<Direction> TrackToDirections(Track track, bool reverse = false)
        {
            var list = TrackToReversedList(track, reverse);
            var result = new LinkedList<Direction>();
            var bigrams = list.GetBigrams();
            foreach (var bigram in bigrams)
            {
                var dx = bigram.Item2.X - bigram.Item1.X;
                var dy = bigram.Item2.Y - bigram.Item1.Y;
                if (dx == -1)
                    result.AddLast(Direction.Left);
                else if (dx == 1)
                    result.AddLast(Direction.Right);
                else if (dy == -1)
                    result.AddLast(Direction.Up);
                else if (dy == 1)
                    result.AddLast(Direction.Down);
            }
            return result;
        }

        private static List<Point> TrackToReversedList(Track track, bool reverse)
        {
            var list = new List<Point>();
            while (track != null)
            {
                list.Add(track.Value);
                track = track.Previous;
            }
            if (reverse)
                list.Reverse();
            return list;
        }

        private static IEnumerable<Point> Neighbors(int x, int y)
        {
            yield return new Point(x, y + 1);
            yield return new Point(x, y - 1);
            yield return new Point(x + 1, y);
            yield return new Point(x - 1, y);
        }
    }
}
