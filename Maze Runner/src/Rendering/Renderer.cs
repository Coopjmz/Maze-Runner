using System.Runtime.InteropServices;
using System.Text;

using MazeRunner.Core;

namespace MazeRunner.Rendering
{
    readonly struct Texture(char data, ConsoleColor color)
    {
        public char Data { get; init; } = data;
        public ConsoleColor Color { get; init; } = color;
    }

    static class Renderer
    {
        private const ConsoleColor ERROR_COLOR = ConsoleColor.DarkRed;

        private static readonly Dictionary<ObjectType, Texture> s_textures = new()
        {
            {ObjectType.Player, new((char)2, ConsoleColor.Green)},
            {ObjectType.Enemy, new((char)1, ConsoleColor.DarkRed)},
            {ObjectType.Heart, new((char)3, ConsoleColor.Red)},
            {ObjectType.Wall, new('#', ConsoleColor.DarkBlue)},
            {ObjectType.Coin, new('0', ConsoleColor.DarkYellow)},
            {ObjectType.None, new(' ', ConsoleColor.Black)}
        };

        static Renderer()
        {
            Console.OutputEncoding = CodePagesEncodingProvider.Instance.GetEncoding(437) ?? Console.OutputEncoding;

            Console.Title = Settings.TITLE;

            Console.SetWindowSize(Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Console.SetBufferSize(Settings.WINDOW_WIDTH, Settings.WINDOW_HEIGHT);
        }

        public static void PrintText(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        public static void PrintTextSameLine(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        public static void PrintError(string message) => PrintText($"ERROR: {message}", ERROR_COLOR);

        public static void Render()
        {
            ClearScreen();
            RenderMap();
            RenderUI();
        }

        public static void ClearScreen(ConsoleColor clearColor = ConsoleColor.Black)
        {
            Console.BackgroundColor = clearColor;
            Console.Clear();
        }

        private static void RenderMap()
        {
            var map = Game.Instance.Map;

            for (int row = 0; row < map.RowCount; ++row)
            {
                for (int column = 0; column < map.ColumnCount; ++column)
                    DrawObject(map[new(row, column)]);
                Console.WriteLine();
            }
        }

        private static void RenderUI()
        {
            var game = Game.Instance;
            var heartTexture = s_textures[ObjectType.Heart];

            PrintText($"Lives: {new string(heartTexture.Data, game.Lives)}", heartTexture.Color);
            PrintText($"Coins: {game.CoinsCollected}/{game.CoinCount}", ConsoleColor.DarkYellow);
            PrintText($"Level: {game.Level}/{Settings.MAX_LEVEL}", ConsoleColor.Blue);
        }

        private static void DrawObject(ObjectType obj)
        {
            var texture = s_textures[obj];
            Console.ForegroundColor = texture.Color;
            Console.Write(texture.Data);
        }
    }
}
