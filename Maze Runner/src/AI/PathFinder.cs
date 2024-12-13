using System.Diagnostics;

using MazeRunner.Core;

namespace MazeRunner.AI
{
    class Node(Position position, Position end, Node? parent = null)
    {
        public Position Position { get; init; } = position;
        public Node? Parent { get; set; } = parent;

        public uint GCost { get; set; } = parent != null ? parent.GCost + 1 : 0;
        public uint HCost { get; init; } = (uint)(Math.Abs(position.Row - end.Row) + Math.Abs(position.Column - end.Column));

        public uint FCost => GCost + HCost;

        // PriorityQueue searches first by FCost then by HCost
        public ulong Priority => (FCost << 16) + HCost;
    }

    // A* Search Algorithm
    static class PathFinder
    {
        public static List<Position> FindPath(Position start, Position end)
        {
            var map = Game.Instance.Map;

            Debug.Assert(map.IsInBounds(start), "Start position out of bounds!");
            Debug.Assert(map.IsInBounds(end), "End position out of bounds!");

            if (start == end)
                return [];

            PriorityQueue<Node, ulong> nodesToBeExplored = new();
            HashSet<Position> exploredPoints = [];

            nodesToBeExplored.Enqueue(new(start, end), 0UL);

            while (nodesToBeExplored.Count > 0)
            {
                Node current = nodesToBeExplored.Dequeue();
                exploredPoints.Add(current.Position);

                if (current.Position == end)
                    return RetracePath(current);

                IEnumerable<Position> neighborPositions =
                [
                    current.Position + new Direction(-1, 0),
                    current.Position + new Direction(+1, 0),
                    current.Position + new Direction(0, -1),
                    current.Position + new Direction(0, +1)
                ];

                IEnumerable<Node> neighbors = neighborPositions
                    .Where(position => !map.IsImpassable(position) && !exploredPoints.Contains(position))
                    .Select(position => new Node(position, end, current));

                foreach (var neighbor in neighbors)
                {
                    Node? node = nodesToBeExplored.UnorderedItems.FirstOrDefault(item => item.Element.Position == neighbor.Position).Element;
                    if (node == null)
                    {
                        nodesToBeExplored.Enqueue(neighbor, neighbor.Priority);
                    }
                    else if (node.FCost > neighbor.FCost)
                    {
                        node.Parent = current;
                        node.GCost = current.GCost + 1;
                    }
                }
            }

            return [];
        }

        public static bool PathExistsBetween(Position start, Position end) => start == end || FindPath(start, end).Count > 0;

        private static List<Position> RetracePath(Node end)
        {
            List<Position> path = [];

            Node current = end;
            while (current.Parent != null)
            {
                path.Add(current.Position);
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}