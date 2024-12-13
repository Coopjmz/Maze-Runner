using MazeRunner.Core;

namespace MazeRunner.Entities
{
    sealed class Player(Position position) : Entity(ObjectType.Player, position)
    {
        public bool IsDead { get; private set; }

        public override void Move(Direction direction)
        {
            if (IsDead) return;

            switch (GetCollidingObject(direction))
            {
                case ObjectType.Coin:
                    Game.Instance.CollectCoin();
                    break;
                case ObjectType.Enemy:
                    Die();
                    return;
            }

            base.Move(direction);
        }

        public void Die()
        {
            IsDead = true;
            Game.Instance.Map[Position] = ObjectType.None;
        }
    }
}