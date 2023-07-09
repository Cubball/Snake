namespace SnakeWinForms;

public partial class MainForm : Form
{
    private readonly Color[] _availableColors;
    private readonly Settings _settings;
    private readonly Panel _startMenuPanel;
    private readonly TableLayoutPanel _settingsPanel;
    private readonly Game _game;

    public MainForm()
    {
        InitializeComponent();

        Icon = Properties.Resources.Icon;
        ShowIcon = false;
        DoubleBuffered = true;
        SetMenuSizeLimits();

        _settings = Settings.Saved;
        _availableColors = new Color[]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.Purple,
            Color.LimeGreen,
            Color.White
        };

        _startMenuPanel = ControlsManager.Panel;
        PopulateStartMenu();
        Controls.Add(_startMenuPanel);

        _settingsPanel = ControlsManager.TableLayoutPanel;
        PopulateSettingsMenu();
        Controls.Add(_settingsPanel);

        _game = (Game)ControlsManager.SetupControl(new Game(_settings));
        Controls.Add(_game);

        ShowMenu();
    }

    public void ShowMenu()
    {
        Text = "Змійка";
        SetMenuSizeLimits();

        _game.Visible = false;
        _settingsPanel.Visible = false;
        _startMenuPanel.Visible = true;
        OnClientSizeChanged(EventArgs.Empty);
    }

    private void ShowSettings()
    {
        Text = "Налаштування";

        _game.Visible = false;
        _startMenuPanel.Visible = false;
        _settingsPanel.Visible = true;
    }

    private void ShowGame()
    {
        _game.Load();
        MinimumSize = SizeFromClientSize(_game.MinimumSize);

        _game.Visible = true;
        _settingsPanel.Visible = false;
        _startMenuPanel.Visible = false;

        _game.Play();
    }

    private void PopulateStartMenu()
    {
        var title = ControlsManager.Title;
        title.Text = "Змійка";
        _startMenuPanel.Controls.Add(title);

        var startGameButton = ControlsManager.Button;
        startGameButton.Text = "Грати";
        startGameButton.Click += (source, e) =>
        {
            ShowGame();
        };
        _startMenuPanel.Controls.Add(startGameButton);

        var settingsButton = ControlsManager.Button;
        settingsButton.Text = "Налаштування";
        settingsButton.Click += (source, e) =>
        {
            ShowSettings();
        };
        _startMenuPanel.Controls.Add(settingsButton);

        var exitButton = ControlsManager.Button;
        exitButton.Text = "Вийти";
        exitButton.Click += (source, e) =>
        {
            Close();
        };
        _startMenuPanel.Controls.Add(exitButton);

        ClientSizeChanged += (source, e) =>
        {
            var maxWidth = Screen.PrimaryScreen.WorkingArea.Width / 6;
            var maxHeight = Screen.PrimaryScreen.WorkingArea.Height / 9;

            title.Size = new Size(_startMenuPanel.Width, _startMenuPanel.Height / 2);
            title.Font = new Font("Century Gothic", Math.Max(title.Height / 7, 1));

            startGameButton.Size = new Size(Math.Min(maxWidth, _startMenuPanel.Width / 3), Math.Min(maxHeight, _startMenuPanel.Height / 9));
            startGameButton.Font = new Font(startGameButton.Font.FontFamily, Math.Max(startGameButton.Height / 5, 1));
            startGameButton.Location = new Point(_startMenuPanel.Width / 2 - startGameButton.Width / 2, title.Bottom);

            settingsButton.Size = startGameButton.Size;
            settingsButton.Font = startGameButton.Font;
            settingsButton.Location = new Point(startGameButton.Left, startGameButton.Bottom + settingsButton.Height / 2);

            exitButton.Size = startGameButton.Size;
            exitButton.Font = startGameButton.Font;
            exitButton.Location = new Point(startGameButton.Left, settingsButton.Bottom + exitButton.Height / 2);
        };
    }

    private void PopulateSettingsMenu()
    {
        var rowHeight = MinimumSize.Height / 7;
        _settingsPanel.Visible = false;
        _settingsPanel.RowStyles.Clear();
        _settingsPanel.ColumnStyles.Clear();
        for (int i = 0; i < 9; i++)
        {
            _settingsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
        }
        for (int i = 0; i < 4; i++)
        {
            _settingsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        }
        for (int i = 0; i < 9; i++)
        {
            _settingsPanel.Controls.Add(ControlsManager.Placeholder, 0, i);
            _settingsPanel.Controls.Add(ControlsManager.Placeholder, 3, i);
        }
        for (int i = 0; i < 4; i++)
        {
            _settingsPanel.Controls.Add(ControlsManager.Placeholder, i, 9);
        }

        var mapWidthLabel = ControlsManager.DockedLabel;
        mapWidthLabel.Text = "Ширина ігрового поля: ";
        _settingsPanel.Controls.Add(mapWidthLabel, 1, 1);

        var mapWidthInput = ControlsManager.NumericUpDown;
        mapWidthInput.Minimum = Settings.MinMapWidth;
        mapWidthInput.Maximum = Settings.MaxMapWidth;
        mapWidthInput.Paint += (source, e) =>
        {
            ((NumericUpDown)source).Value = _settings.MapSize.Width;
        };
        _settingsPanel.Controls.Add(mapWidthInput, 2, 1);

        var mapHeightLabel = ControlsManager.DockedLabel;
        mapHeightLabel.Text = "Висота ігрового поля: ";
        _settingsPanel.Controls.Add(mapHeightLabel, 1, 2);

        var mapHeightInput = ControlsManager.NumericUpDown;
        mapHeightInput.Minimum = Settings.MinMapHeight;
        mapHeightInput.Maximum = Settings.MaxMapHeight;
        mapHeightInput.Paint += (source, e) =>
        {
            ((NumericUpDown)source).Value = _settings.MapSize.Height;
        };
        _settingsPanel.Controls.Add(mapHeightInput, 2, 2);

        var snakeColorLabel = ControlsManager.DockedLabel;
        snakeColorLabel.Text = "Колір змійки: ";
        _settingsPanel.Controls.Add(snakeColorLabel, 1, 3);

        var snakeColorInput = ControlsManager.ComboBox;
        snakeColorInput.DrawItem += (source, e) =>
        {
            if (e.Index >= 0)
            {
                using var brush = new SolidBrush((Color)((ComboBox)source).Items[e.Index]);
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
        };
        foreach (var color in _availableColors)
        {
            snakeColorInput.Items.Add(color);
        }
        snakeColorInput.SelectedIndex = Array.IndexOf(_availableColors, _settings.SnakeColor);
        _settingsPanel.Controls.Add(snakeColorInput, 2, 3);

        var appleColorLabel = ControlsManager.DockedLabel;
        appleColorLabel.Text = "Колір яблука: ";
        _settingsPanel.Controls.Add(appleColorLabel, 1, 4);

        var appleColorInput = ControlsManager.ComboBox;
        appleColorInput.DrawItem += (source, e) =>
        {
            if (e.Index >= 0)
            {
                using var brush = new SolidBrush((Color)((ComboBox)source).Items[e.Index]);
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
        };
        foreach (var color in _availableColors)
        {
            appleColorInput.Items.Add(color);
        }
        appleColorInput.SelectedIndex = Array.IndexOf(_availableColors, _settings.AppleColor);
        _settingsPanel.Controls.Add(appleColorInput, 2, 4);

        var snakeSpeedLabel = ControlsManager.DockedLabel;
        snakeSpeedLabel.Text = "Швидкість змійки (" + 100 * Settings.DefaultGameStateUpdateDelay / _settings.GameStateUpdateDelay + "%): ";
        _settingsPanel.Controls.Add(snakeSpeedLabel, 1, 5);

        var snakeSpeedInput = ControlsManager.TrackBar;
        snakeSpeedInput.Minimum = 100 * Settings.DefaultGameStateUpdateDelay / Settings.MaxGameStateUpdateDelay;
        snakeSpeedInput.Maximum = 100 * Settings.DefaultGameStateUpdateDelay / Settings.MinGameStateUpdateDelay;
        snakeSpeedInput.Value = 100 * Settings.DefaultGameStateUpdateDelay / _settings.GameStateUpdateDelay;
        snakeSpeedInput.ValueChanged += (source, e) =>
        {
            snakeSpeedLabel.Text = "Швидкість змійки: (" + ((TrackBar)source).Value.ToString() + "%): ";
        };
        _settingsPanel.Controls.Add(snakeSpeedInput, 2, 5);

        var snakeMovesThroughWallsLabel = ControlsManager.DockedLabel;
        snakeMovesThroughWallsLabel.Text = "Змійка проходить крізь мапу: ";
        _settingsPanel.Controls.Add(snakeMovesThroughWallsLabel, 1, 6);

        var snakeMovesThroughWallsInput = ControlsManager.CheckBox;
        snakeMovesThroughWallsInput.Checked = _settings.SnakeMovesThroughWalls;
        _settingsPanel.Controls.Add(snakeMovesThroughWallsInput, 2, 6);

        var gameStartsOnLoadLabel = ControlsManager.DockedLabel;
        gameStartsOnLoadLabel.Text = "Гра розпочинається одразу: ";
        _settingsPanel.Controls.Add(gameStartsOnLoadLabel, 1, 7);

        var gameStartsOnLoadInput = ControlsManager.CheckBox;
        gameStartsOnLoadInput.Checked = _settings.GameStartsOnLoad;
        _settingsPanel.Controls.Add(gameStartsOnLoadInput, 2, 7);

        var saveButton = ControlsManager.DockedButton;
        saveButton.Text = "Зберегти";
        saveButton.Click += (source, e) =>
        {
            _settings
                 .SetMapWidth((int)mapWidthInput.Value)
                 .SetMapHeight((int)mapHeightInput.Value)
                 .SetSnakeColor((Color)snakeColorInput.SelectedItem)
                 .SetAppleColor((Color)appleColorInput.SelectedItem)
                 .SetGameStateUpdateDelay(100 * Settings.DefaultGameStateUpdateDelay / snakeSpeedInput.Value)
                 .SetSnakeMovingThroughWalls(snakeMovesThroughWallsInput.Checked)
                 .SetGameStartingOnLoad(gameStartsOnLoadInput.Checked);
            _settings.SaveSettingsToFile();
            ShowMenu();
        };
        _settingsPanel.Controls.Add(saveButton, 1, 8);

        var defaultButton = ControlsManager.DockedButton;
        defaultButton.Text = "Скинути налаштування";
        defaultButton.Click += (source, e) =>
        {
            _settings.SetToDefault();
            _settings.SaveSettingsToFile();
            ShowMenu();
            appleColorInput.SelectedIndex = Array.IndexOf(_availableColors, _settings.AppleColor);
            snakeColorInput.SelectedIndex = Array.IndexOf(_availableColors, _settings.SnakeColor);
            snakeSpeedInput.Value = 100 * Settings.DefaultGameStateUpdateDelay / _settings.GameStateUpdateDelay;
            snakeMovesThroughWallsInput.Checked = _settings.SnakeMovesThroughWalls;
            gameStartsOnLoadInput.Checked = _settings.GameStartsOnLoad;
        };

        _settingsPanel.Controls.Add(defaultButton, 2, 8);
    }

    private void SetMenuSizeLimits()
    {
        MinimumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width / 3, Screen.PrimaryScreen.WorkingArea.Size.Height / 3);
        MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
    }
}