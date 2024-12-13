using System.Diagnostics;

namespace MazeRunner.Core
{
    sealed class Map(int rowCount, int columnCount)
    {
        public static readonly HashSet<ObjectType> ImpassableObjects = [ObjectType.Wall, ObjectType.Enemy];

        private static readonly Random s_random = new();

        private readonly ObjectType[,] _map = new ObjectType[rowCount, columnCount];

        public int RowCount => _map.GetLength(0);
        public int ColumnCount => _map.GetLength(1);
        public int TrueMapSize => (RowCount - 2) * (ColumnCount - 2);

        public ObjectType this[Position position]
        {
            get
            {
                Debug.Assert(IsInBounds(position), "Position is out of bounds!");
                return _map[position.Row, position.Column];
            }
            set
            {
                Debug.Assert(IsInBounds(position), "Position is out of bounds!");
                _map[position.Row, position.Column] = value;
            }
        }

        public bool IsInBounds(Position position) =>
            0 <= position.Row && position.Row < RowCount && 0 <= position.Column && position.Column < ColumnCount;

        public bool IsImpassable(Position position) => ImpassableObjects.Contains(this[position]);

        public void Generate(int wallCount, int coinCount)
        {
            Clear();

            var positions = GetPossibleSpawnPositions(wallCount + coinCount);
            foreach (var position in positions.Take(wallCount))
                this[position] = ObjectType.Wall;
            foreach (var position in positions.TakeLast(coinCount))
                this[position] = ObjectType.Coin;
        }

        public Position SpawnPlayer()
        {
            var position = GetPossibleSpawnPositions(1).First();
            this[position] = ObjectType.Player;

            return position;
        }

        public IEnumerable<Position> SpawnEnemies(int count)
        {
            var positions = GetPossibleSpawnPositions(count);
            foreach (var position in positions)
                this[position] = ObjectType.Enemy;

            return positions;
        }

        public void Clear()
        {
            bool IsCornerWall(int row, int column) => row == 0 || row == RowCount - 1
                                                   || column == 0 || column == ColumnCount - 1;

            for (int row = 0; row < RowCount; ++row)
            {
                for (int column = 0; column < ColumnCount; ++column)
                {
                    if (IsCornerWall(row, column))
                        _map[row, column] = ObjectType.Wall;
                    else
                        _map[row, column] = ObjectType.None;
                }
            }
        }

        private Position[] GetPossibleSpawnPositions(int count)
        {
            if (count == 0) return [];

            List<Position> positions = new(TrueMapSize);

            for (int row = 1; row < _map.GetLength(0) - 1; ++row)
            {
                for (int column = 1; column < _map.GetLength(1) - 1; ++column)
                {
                    if (_map[row, column] == ObjectType.None)
                        positions.Add(new(row, column));
                }
            }

            Debug.Assert(positions.Count >= count, "Not enough spawn positions available!");
            return positions.OrderBy(_ => s_random.Next()).Take(count).ToArray();
        }
    }
}