namespace SnakeWinForms;

public class AppleEatenEventArgs : EventArgs
{
    public IEnumerable<Point> SnakePieces { get; }

    public AppleEatenEventArgs(IEnumerable<Point> snakePieces)
    {
        SnakePieces = snakePieces;
    }
}
