namespace MazeRunner.Core
{
    struct Position(int row, int column)
    {
        public int Row { get; set; } = row;
        public int Column { get; set; } = column;

        public override readonly bool Equals(object? obj)
        {
            var other = obj as Position?;
            return other is not null && this == other;
        }

        public override readonly int GetHashCode() => (Row, Column).GetHashCode();

        public static Position operator +(Position position, Direction direction)
            => new(position.Row + direction.RowChange, position.Column + direction.ColumnChange);

        public static bool operator ==(Position position1, Position position2)
            => position1.Row == position2.Row && position1.Column == position2.Column;

        public static bool operator !=(Position position1, Position position2)
            => !(position1 == position2);
    }

    struct Direction(int rowChange, int columnChange)
    {
        public static readonly Direction ZERO = new(0, 0);

        public int RowChange { get; set; } = rowChange;
        public int ColumnChange { get; set; } = columnChange;

        public override readonly bool Equals(object? obj)
        {
            var other = obj as Direction?;
            return other is not null && this == other;
        }

        public override readonly int GetHashCode() => (RowChange, ColumnChange).GetHashCode();

        public static Position operator +(Direction direction, Position position)
            => position + direction;

        public static bool operator ==(Direction direction1, Direction direction2)
            => direction1.RowChange == direction2.RowChange && direction1.ColumnChange == direction2.ColumnChange;

        public static bool operator !=(Direction direction1, Direction direction2)
            => !(direction1 == direction2);
    }
}
