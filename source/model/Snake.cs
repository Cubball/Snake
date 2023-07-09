namespace SnakeWinForms;

public class Snake
{
    private readonly LinkedList<Point> _pieces;
    private readonly Size _mapSize;
    private readonly bool _movesThroughWalls;
    private Direction _currentDirection;

    public Snake(Size mapSize, bool movesThroughWalls)
        : this(mapSize, movesThroughWalls, new Point(mapSize.Width / 2, mapSize.Height / 2)) { }

    public Snake(Size mapSize, bool movesThroughWalls, Point startPosition)
    {
        _pieces = new();
        _pieces.AddFirst(startPosition);
        _mapSize = mapSize;
        _movesThroughWalls = movesThroughWalls;
    }

    public IEnumerable<Point> Pieces => _pieces;
    public int Length => _pieces.Count;

    public event EventHandler<AppleEatenEventArgs> AppleEaten;

    public bool Move(Direction newDirection, Point applePosition)
    {
        var newHead = GetNewHead(newDirection);
        var appleEaten = newHead.Equals(applePosition);
        if (!CanMoveTo(newHead, !appleEaten))
        {
            return false;
        }
        _pieces.AddFirst(newHead);
        if (appleEaten)
        {
            OnAppleEaten();
        }
        else
        {
            _pieces.RemoveLast();
        }
        return true;
    }

    protected virtual void OnAppleEaten()
    {
        AppleEaten?.Invoke(this, new AppleEatenEventArgs(_pieces));
    }

    private void TryChangeDirection(Direction direction)
    {
        if ((int)_currentDirection % 2 != (int)direction % 2 || Length == 1)
        {
            _currentDirection = direction;
        }
    }

    private Point GetNewHead(Direction newDirection)
    {
        var head = _pieces.First.Value;
        TryChangeDirection(newDirection);
        int dx = _currentDirection switch
        {
            Direction.Left => -1,
            Direction.Right => 1,
            _ => 0
        };
        int dy = _currentDirection switch
        {
            Direction.Up => -1,
            Direction.Down => 1,
            _ => 0
        };
        var newHead = new Point(head.X + dx, head.Y + dy);
        if (_movesThroughWalls)
        {
            newHead.X = (newHead.X + _mapSize.Width) % _mapSize.Width;
            newHead.Y = (newHead.Y + _mapSize.Height) % _mapSize.Height;
        }
        return newHead;
    }

    private bool CanMoveTo(Point point, bool tailMoved)
    {
        IEnumerable<Point> pieces = _pieces;
        if (tailMoved)
        {
            pieces = _pieces.SkipLast(1);
        }
        return !pieces.Any(p => p.Equals(point)) &&
                point.X < _mapSize.Width && point.Y < _mapSize.Height &&
                point.X >= 0 && point.Y >= 0;
    }
}
