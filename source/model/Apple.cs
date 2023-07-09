namespace SnakeWinForms;

public class Apple
{
    private readonly Random _random;
    private readonly Size _mapSize;
    private Point _position;

    public Apple(Size mapSize, IEnumerable<Point> snakePieces)
    {
        _random = new();
        _mapSize = mapSize;
        SpawnAtRandomPostition(snakePieces);
    }

    public Apple(Size mapSize, Point startPosition)
    {
        _random = new();
        _mapSize = mapSize;
        _position = startPosition;
    }

    public Point Postition => _position;

    public void OnAppleEaten(object source, AppleEatenEventArgs e)
    {
        SpawnAtRandomPostition(e.SnakePieces);
    }

    private void SpawnAtRandomPostition(IEnumerable<Point> snakePieces)
    {
        int newX;
        int newY;
        do
        {
            newX = _random.Next(_mapSize.Width);
            newY = _random.Next(_mapSize.Height);
        } while (snakePieces.Any(p => p.X == newX && p.Y == newY));
        _position = new(newX, newY);
    }
}
