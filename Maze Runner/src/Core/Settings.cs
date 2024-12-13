namespace MazeRunner.Core
{
    static class Settings
    {
        public const string TITLE = "Maze Runner";

        public const int WINDOW_WIDTH = COLUMN_COUNT;
        public const int WINDOW_HEIGHT = ROW_COUNT + 5;

        public const int ROW_COUNT = 20;
        public const int COLUMN_COUNT = 50;

        public const int WALL_COUNT = 100;
        public const int COIN_COUNT = 10;

        public const int MAX_LIVES = 3;
        public const int MAX_LEVEL = 10;

        public const int MIN_TURNS_NEEDED_FOR_ENEMY_TO_MOVE = 0;
        public const int MAX_TURNS_NEEDED_FOR_ENEMY_TO_MOVE = 2;
    }
}
