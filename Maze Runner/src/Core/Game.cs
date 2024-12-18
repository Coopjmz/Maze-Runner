using System.Diagnostics;

using MazeRunner.Entities;
using MazeRunner.Input;
using MazeRunner.Rendering;

namespace MazeRunner.Core
{
    sealed class Game
    {
        private static Game? s_instance;
        public static Game Instance
        {
            get
            {
                Debug.Assert(s_instance != null, "Call Start method before accessing Instance property!");
                return s_instance;
            }
        }

        public static void Start()
        {
            s_instance = new();
            s_instance.StartLevel(1);
        }

        public Map Map { get; init; }

        public Player Player { get; private set; }

        public int Lives { get; private set; }
        public int Level { get; private set; }

        public int CoinsCollected { get; private set; }
        public int CoinCount { get; private set; }

        private bool AllCoinsAreCollected => CoinsCollected >= CoinCount;
        private bool LevelNotCompleted => !AllCoinsAreCollected && !Player.IsDead;

        private readonly List<Enemy> _enemies;

#pragma warning disable CS8618
        public Game()
#pragma warning restore CS8618
        {
            Map = new(Settings.ROW_COUNT, Settings.COLUMN_COUNT);
            Lives = Settings.MAX_LIVES;
            _enemies = new(capacity: Settings.MAX_LEVEL);
        }

        public void CollectCoin() => ++CoinsCollected;

        private void StartLevel(int level)
        {
            Debug.Assert(0 < level && level <= Settings.MAX_LEVEL, "Level out of bounds!");

            int wallCount, enemyCount;
            if (level == Settings.MAX_LEVEL)
            {
                wallCount = 0;
                enemyCount = 0;
                CoinCount = Map.TrueMapSize - 1;
            }
            else
            {
                wallCount = 100;
                enemyCount = level;
                CoinCount = Settings.COIN_COUNT;
            }

            Map.Generate(wallCount, CoinCount);

            Player = new(Map.SpawnPlayer());

            _enemies.Clear();
            foreach (var position in Map.SpawnEnemies(enemyCount))
                _enemies.Add(new(position));

            CoinsCollected = 0;
            Level = level;

            Run();
        }

        private void Run()
        {
            Renderer.Render();

            while (LevelNotCompleted)
            {
                Direction direction;
                while ((direction = InputHandler.GetPlayerDirection()) == Direction.ZERO)
                {
                    Renderer.Render();
                    Renderer.PrintError("Invalid input!");
                }

                Player.Move(direction);

                if (LevelNotCompleted)
                {
                    foreach (var enemy in _enemies)
                        enemy.TryToMoveTowardsPlayer();
                }

                Renderer.Render();
            }

            if (AllCoinsAreCollected)
            {
                if (Level < Settings.MAX_LEVEL)
                {
                    Renderer.PrintText("Level complete!", ConsoleColor.Green);
                    InputHandler.Prompt("Press any key to advance to next level...");
                    StartLevel(Level + 1);
                }
                else
                {
                    Renderer.PrintText("!!! YOU WIN !!!", ConsoleColor.Green);
                    InputHandler.Prompt("Press any key to play again...");
                    Restart();
                }
            }
            else
            {
                --Lives;
                Renderer.Render();

                if (Lives > 0)
                {
                    Renderer.PrintText($"Try again... Lives remaining: {Lives}", ConsoleColor.Red);
                    InputHandler.Prompt("Press any key to retry current level...");
                    StartLevel(Level);
                }
                else
                {
                    Renderer.PrintText("!!! GAME OVER !!!", ConsoleColor.Red);
                    InputHandler.Prompt("Press any key to try again...");
                    Restart();
                }
            }
        }

        private void Restart()
        {
            Lives = Settings.MAX_LIVES;
            StartLevel(1);
        }
    }
}
