using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.UI;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;

namespace SaberGame.Core;

public class Max7219DisplayService : IDisplayService
{
    private Max7219 _display = default!;

    private DisplayScreen _screen;
    private DisplayLabel leftLabel;
    private DisplayLabel rightLabel;
    private DisplayBox leftBox;
    private DisplayBox rightBox;

    public Max7219DisplayService(ISpiBus bus, IDigitalOutputPort cs)
    {
        _display = new Max7219(bus, cs, 4);

        // displays are numbered from right-end back, so 0,1 are red, 2,3 are green
        // green display are much dimmer than red, so try to equalize here
        _display.SetBrightness(6, 0);
        _display.SetBrightness(6, 1);
        _display.SetBrightness(15, 2);
        _display.SetBrightness(15, 3);

        _screen = new DisplayScreen(_display, RotationType._270Degrees);

        leftLabel = new DisplayLabel(1, 0, _screen.Width / 2, _screen.Height);
        rightLabel = new DisplayLabel(_screen.Width / 2, 0, _screen.Width / 2, _screen.Height);
        leftBox = new DisplayBox(0, 0, _screen.Width / 2, _screen.Height);
        rightBox = new DisplayBox(_screen.Width / 2, 0, _screen.Width / 2, _screen.Height);

        leftLabel.BackColor = Color.Black;
        leftLabel.TextColor = Color.White; // "white" just means "on" for the 7219

        rightLabel.BackColor = Color.Black;
        rightLabel.TextColor = Color.White; // "white" just means "on" for the 7219
        rightLabel.HorizontalAlignment = HorizontalAlignment.Right;

        leftBox.ForeColor = Color.White;
        leftBox.Filled = true;
        leftBox.Visible = false;

        rightBox.ForeColor = Color.White;
        rightBox.Filled = true;
        rightBox.Visible = false;

        leftLabel.Visible = true;
        rightLabel.Visible = true;

        _screen.Controls.Add(
            leftBox,
            rightBox,
            leftLabel,
            rightLabel);
    }

    public void ShowText(string text)
    {
        rightBox.Visible = leftBox.Visible = rightLabel.Visible = false;
        leftLabel.Text = text;
    }

    public void ShowScore(int left, int right)
    {
        rightBox.Visible = leftBox.Visible = false;
        leftLabel.Text = left.ToString();
        rightLabel.Text = right.ToString();
        leftLabel.Visible = rightLabel.Visible = true;
    }

    public void ShowTouchLeft()
    {
        leftBox.Visible = true;
        rightBox.Visible = false;
        leftLabel.Visible = rightLabel.Visible = false;
    }

    public void ShowTouchRight()
    {
        leftBox.Visible = false;
        rightBox.Visible = true;
        leftLabel.Visible = rightLabel.Visible = false;
    }
}