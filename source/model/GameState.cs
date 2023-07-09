namespace SnakeWinForms;

public class GameState
{
    private readonly Snake _snake;
    private readonly Apple _apple;
    private readonly Size _mapSize;
    private int _score;
    private Direction _direction;
    private bool _isRunning;

    public GameState(Settings settings)
    {
        _mapSize = settings.MapSize;
        _snake = new(_mapSize, settings.SnakeMovesThroughWalls);
        _apple = new(_mapSize, _snake.Pieces);
        _score = 0;
        _isRunning = true;
        _direction = Direction.Up;
        SubscribeSnakeApple();
    }

    public GameState(Size mapSize, Snake snake, Apple apple)
    {
        _mapSize = mapSize;
        _snake = snake;
        _apple = apple;
        _score = _snake.Length - 1;
        _isRunning = true;
        _direction = Direction.Up;
        SubscribeSnakeApple();
    }

    public int Score => _score;
    public bool IsRunning => _isRunning;
    public IEnumerable<Point> SnakePieces => _snake.Pieces;
    public Point ApplePostition => _apple.Postition;
    public Direction Direction => _direction;

    public void Update()
    {
        if (_isRunning)
        {
            _isRunning = _snake.Move(_direction, _apple.Postition);
        }
    }

    public void ChangeDirection(Direction direction)
    {
        _direction = direction;
    }

    private void SubscribeSnakeApple()
    {
        _snake.AppleEaten += _apple.OnAppleEaten;
        _snake.AppleEaten += OnAppleEaten;
    }

    private void OnAppleEaten(object source, AppleEatenEventArgs e)
    {
        _score++;
    }
}
