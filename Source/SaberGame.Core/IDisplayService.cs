namespace SaberGame.Core;

public interface IDisplayService
{
    void ShowText(string text);
    void ShowScore(int left, int right);
    void ShowTouchLeft();
    void ShowTouchRight();
}
