namespace SnakeWinForms;

public class Game : Control
{
    private readonly System.Windows.Forms.Timer _timer;
    private readonly Settings _settings;
    private readonly GameField _field;
    private GameState _state;
    private Label _scoreLabel;
    private PictureBox _pauseButton;
    private PictureBox _exitButton;

    public Game(Settings settings)
    {
        Visible = false;
        DoubleBuffered = true;
        BackColor = ControlsManager.BackgroundColor;

        _settings = settings;
        _timer = new System.Windows.Forms.Timer()
        {
            Interval = _settings.GameStateUpdateDelay
        };
        _timer.Tick += OnTick;
        _field = new GameField(_settings);
        Controls.Add(_field);

        PopulateSideBar();
    }

    public void Load()
    {
        _state = new GameState(_settings);
        _field.LoadGameState(_state);
        MinimumSize = new Size(_field.MinimumSize.Width * 7 / 5, _field.MinimumSize.Height);

        _timer.Interval = _settings.GameStateUpdateDelay;

        OnSizeChanged(EventArgs.Empty);
        _pauseButton.Image = Properties.Resources.Pause;
        UpdateScoreLabel();
    }

    public void Play()
    {
        KeyUp += OnKeyUp;
        Focus();
        if (_settings.GameStartsOnLoad)
        {
            _timer.Start();
        }
        else
        {
            _pauseButton.Image = Properties.Resources.Play;
        }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        _field.ResizeAndReposition(Size);
        var dx = _field.Right;
        var width = Width - dx;
        var height = Height / 3;
        var horizontalMargin = width / 6;
        var verticalMargin = height / 6;
        var size = Math.Min(width - 2 * horizontalMargin, height - 2 * verticalMargin);

        _pauseButton.Size = new Size(size, size);
        _pauseButton.Location = new Point(dx + width / 2 - size / 2, verticalMargin);

        _scoreLabel.Size = new Size(width, height);
        _scoreLabel.Location = new Point(dx, _pauseButton.Bottom + verticalMargin);
        _scoreLabel.Font = new Font("Century Gothic", height / 10);

        _exitButton.Size = new Size(size, size);
        _exitButton.Location = new Point(dx + width / 2 - size / 2, _scoreLabel.Bottom + verticalMargin);
    }

    private void OnTick(object source, EventArgs e)
    {
        _state.Update();
        _field.Invalidate();
        UpdateScoreLabel();
        if (!_state.IsRunning)
        {
            Stop();
        }
    }

    private void Stop()
    {
        KeyUp -= OnKeyUp;
        _timer.Stop();
        _scoreLabel.Text = "Гру завершено!\n" + _scoreLabel.Text;
        _pauseButton.Image = Properties.Resources.Restart;
    }

    private void PopulateSideBar()
    {
        _pauseButton = ControlsManager.PictureBox;
        _pauseButton.Click += (source, e) =>
        {
            Focus();
            if (!_state.IsRunning)
            {
                Load();
                Play();
                return;
            }
            if (_timer.Enabled)
            {
                _timer.Stop();
                _pauseButton.Image = Properties.Resources.Play;
            }
            else
            {
                _timer.Start();
                _pauseButton.Image = Properties.Resources.Pause;
            }
        };
        Controls.Add(_pauseButton);

        _scoreLabel = ControlsManager.DockedLabel;
        _scoreLabel.Dock = DockStyle.None;
        _scoreLabel.TextAlign = ContentAlignment.MiddleCenter;
        Controls.Add(_scoreLabel);

        _exitButton = ControlsManager.PictureBox;
        _exitButton.Image = Properties.Resources.Exit;
        _exitButton.Click += (source, e) =>
        {
            var form = (MainForm)FindForm();
            Stop();
            form.ShowMenu();
        };
        Controls.Add(_exitButton);
    }

    private void UpdateScoreLabel()
    {
        _scoreLabel.Text = "Рахунок:\n" + _state.Score.ToString();
    }

    private void OnKeyUp(object source, KeyEventArgs e)
    {
        Direction? direction = e.KeyCode switch
        {
            Keys.W => Direction.Up,
            Keys.A => Direction.Left,
            Keys.S => Direction.Down,
            Keys.D => Direction.Right,
            Keys.Up => Direction.Up,
            Keys.Down => Direction.Down,
            Keys.Left => Direction.Left,
            Keys.Right => Direction.Right,
            _ => null
        };
        e.Handled = true;
        if (direction.HasValue && _timer.Enabled)
        {
            _state.ChangeDirection(direction.Value);
        }
    }
}
