namespace SnakeWinForms;

public class GameField : Panel
{
    private readonly Settings _settings;
    private GameState _gameState;
    private Brush _snakeBrush;
    private Brush _appleBrush;

    public GameField(Settings settings)
    {
        DoubleBuffered = true;
        BackColor = Color.Black;
        _settings = settings;
    }

    public void LoadGameState(GameState gameState)
    {
        _gameState = gameState;
        _snakeBrush?.Dispose();
        _appleBrush?.Dispose();
        _snakeBrush = new SolidBrush(_settings.SnakeColor);
        _appleBrush = new SolidBrush(_settings.AppleColor);
        var minHeight = Math.Max(Screen.PrimaryScreen.WorkingArea.Height / 3, _settings.MapSize.Height * 10);
        var minWidth = minHeight * _settings.MapSize.Width / _settings.MapSize.Height;
        MinimumSize = new Size(minWidth, minHeight);
    }

    public void ResizeAndReposition(Size containerSize)
    {
        var maxWidth = containerSize.Width * 5 / 7;
        var maxHeight = containerSize.Height;
        var widthScale = maxWidth / _settings.MapSize.Width;
        var heightScale = maxHeight / _settings.MapSize.Height;
        var scale = Math.Min(widthScale, heightScale);
        ClientSize = new Size(scale * _settings.MapSize.Width, scale * _settings.MapSize.Height);
        var dy = (maxHeight - ClientSize.Height) / 2;
        Location = new Point(0, dy);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (_gameState == null)
        {
            return;
        }

        var pixelSize = new Size(ClientSize.Width / _settings.MapSize.Width, ClientSize.Height / _settings.MapSize.Height);
        var pixels = _gameState.SnakePieces.Select(p => new Rectangle(new Point(p.X * pixelSize.Width, p.Y * pixelSize.Height), pixelSize)).ToArray();
        e.Graphics.FillRectangles(_snakeBrush, pixels);

        e.Graphics.FillEllipse(_appleBrush, new Rectangle(
            new Point(_gameState.ApplePostition.X * pixelSize.Width, _gameState.ApplePostition.Y * pixelSize.Height),
            pixelSize));
    }
}
