using System.Text;

using MazeRunner.Core;
using MazeRunner.Rendering;

namespace MazeRunner.Input
{
    enum PlayerAction
    {
        Invalid,
        MoveUp,
        MoveLeft,
        MoveDown,
        MoveRight,
        ToggleSuperpower
    }

    static class Controls
    {
        public static Direction GetPlayerDirection(PlayerAction action)
            => action switch
            {
                PlayerAction.MoveUp => new(-1, 0),
                PlayerAction.MoveLeft => new(0, -1),
                PlayerAction.MoveDown => new(+1, 0),
                PlayerAction.MoveRight => new(0, +1),
                _ => new(0, 0)
            };
    }

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

        public static PlayerAction GetPlayerAction()
            => GetKeyPressed() switch
            {
                ConsoleKey.W => PlayerAction.MoveUp,
                ConsoleKey.A => PlayerAction.MoveLeft,
                ConsoleKey.S => PlayerAction.MoveDown,
                ConsoleKey.D => PlayerAction.MoveRight,
                ConsoleKey.UpArrow => PlayerAction.MoveUp,
                ConsoleKey.LeftArrow => PlayerAction.MoveLeft,
                ConsoleKey.DownArrow => PlayerAction.MoveDown,
                ConsoleKey.RightArrow => PlayerAction.MoveRight,
                ConsoleKey.Spacebar => PlayerAction.ToggleSuperpower,
                _ => PlayerAction.Invalid
            };
    }
}