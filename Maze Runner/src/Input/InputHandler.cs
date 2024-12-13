using System.Text;

using MazeRunner.Core;
using MazeRunner.Rendering;

namespace MazeRunner.Input
{
    static class InputHandler
    {
        static InputHandler() => Console.InputEncoding = CodePagesEncodingProvider.Instance.GetEncoding(437) ?? Console.InputEncoding;

        public static ConsoleKey GetKeyPressed() => Console.ReadKey().Key;

        public static bool Prompt(string text, ConsoleKey? key = null, ConsoleColor color = ConsoleColor.Gray)
        {
            Renderer.PrintTextSameLine(text, color);
            var keyPressed = GetKeyPressed();
            return key == null || key == keyPressed;
        }

        public static Direction GetPlayerDirection()
            => GetKeyPressed() switch
            {
                ConsoleKey.W => new(-1, 0),
                ConsoleKey.A => new(0, -1),
                ConsoleKey.S => new(+1, 0),
                ConsoleKey.D => new(0, +1),
                ConsoleKey.UpArrow => new(-1, 0),
                ConsoleKey.LeftArrow => new(0, -1),
                ConsoleKey.DownArrow => new(+1, 0),
                ConsoleKey.RightArrow => new(0, +1),
                _ => new(0, 0)
            };
    }
}