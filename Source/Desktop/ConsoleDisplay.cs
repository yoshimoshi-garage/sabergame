using SaberGame.Core;

namespace SaberGame_Desktop
{
    public class ConsoleDisplay : IGameDisplay
    {
        public void ShowScore(int left, int right)
        {
            Console.WriteLine($"Score is now L:{left} R:{right}");
        }

        public void ShowTouchLeft()
        {
            Console.WriteLine("Touch left!");
        }

        public void ShowTouchRight()
        {
            Console.WriteLine("Touch Right!");
        }
    }
}