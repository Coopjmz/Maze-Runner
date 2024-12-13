using MazeRunner.AI;
using MazeRunner.Core;

namespace MazeRunner.Entities
{
    sealed class Enemy(Position position) : Entity(ObjectType.Enemy, position)
    {
        private static readonly Random s_random = new();

        private static int GetTurnsUntilNextMove()
            => s_random.Next(Settings.MIN_TURNS_NEEDED_FOR_ENEMY_TO_MOVE, Settings.MAX_TURNS_NEEDED_FOR_ENEMY_TO_MOVE + 1);

        private int _turnsUntilNextMove;
        private bool _shouldRespawnCoin;
        
        public void TryToMoveTowardsPlayer()
        {
            if (_turnsUntilNextMove-- > 0) return;
            _turnsUntilNextMove = GetTurnsUntilNextMove();

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

            if (_shouldRespawnCoin)
                Game.Instance.Map[oldPosition] = ObjectType.Coin;
            _shouldRespawnCoin = isCollidingWithCoin;
        }
    }
}
