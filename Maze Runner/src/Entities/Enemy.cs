using MazeRunner.AI;
using MazeRunner.Core;

namespace MazeRunner.Entities
{
    sealed class Enemy(Position position) : Entity(ObjectType.Enemy, position)
    {
        private static readonly Random s_random = new();

        private static int GetTurnsUntilNextMove()
            => s_random.Next(Settings.MIN_TURNS_NEEDED_FOR_ENEMY_TO_MOVE, Settings.MAX_TURNS_NEEDED_FOR_ENEMY_TO_MOVE + 1);

        private readonly TickTimer _moveTimer = new(GetTurnsUntilNextMove());

        private bool _isStandingOnCoin;
        
        public void TryToMoveTowardsPlayer()
        {
            if (_moveTimer.IsNotFinished)
            {
                _moveTimer.Update();
                return;
            }
            _moveTimer.Reset(GetTurnsUntilNextMove());

            var player = Game.Instance.Player;

            var path = PathFinder.FindPath(Position, player.Position);
            if (path.Count <= 0) return;

            var newPosition = path.First();
            bool isCollidingWithCoin = false;

            switch (GetCollidingObject(newPosition))
            {
                case ObjectType.Coin:
                    isCollidingWithCoin = true;
                    break;
                case ObjectType.Player:
                    player.Die();
                    break;
            }

            var oldPosition = Position;
            Move(newPosition);

            if (_isStandingOnCoin)
                Game.Instance.Map[oldPosition] = ObjectType.Coin;
            _isStandingOnCoin = isCollidingWithCoin;
        }
    }
}
