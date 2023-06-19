namespace SaberGame.Core;

public interface IGameDisplay
{
    void ShowTouchLeft();
    void ShowTouchRight();
    void ShowScore(int left, int right);
}
