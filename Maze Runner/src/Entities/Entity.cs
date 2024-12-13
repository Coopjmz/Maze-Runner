using System.Diagnostics;

using MazeRunner.Core;

namespace MazeRunner.Entities
{
    abstract class Entity(ObjectType type, Position position)
    {
        public ObjectType Type { get; init; } = type;
        public Position Position { get; private set; } = position;

        protected static ObjectType GetCollidingObject(Position atPosition) => Game.Instance.Map[atPosition];
        protected ObjectType GetCollidingObject(Direction direction) => GetCollidingObject(Position + direction);

        public virtual void Move(Position newPosition)
        {
            var map = Game.Instance.Map;

            Debug.Assert(map.IsInBounds(newPosition), "New position is out of bounds!");

            map[newPosition] = Type;
            map[Position] = ObjectType.None;

            Position = newPosition;
        }

        public virtual void Move(Direction direction)
        {
            var newPosition = Position + direction;
            if (direction != Direction.ZERO && !Game.Instance.Map.IsImpassable(newPosition))
                Move(newPosition);
        }
    }
}
