using Newtonsoft.Json;

namespace SnakeWinForms;

public class Settings
{
    public const int MinMapWidth = 10;
    public const int MaxMapWidth = 50;
    public const int MinMapHeight = 10;
    public const int MaxMapHeight = 50;
    public const int MinGameStateUpdateDelay = 50;
    public const int MaxGameStateUpdateDelay = 300;
    public const int DefaultMapWidth = 20;
    public const int DefaultMapHeight = 20;
    public const int DefaultGameStateUpdateDelay = 150;
    public const bool DefaultSnakeBehavior = false;
    public const bool DefaultGameBehavior = true;
    private const string SettingsFilePath = "settings.json";

    [JsonProperty]
    public Size MapSize { get; private set; }
    [JsonProperty]
    public int GameStateUpdateDelay { get; private set; }
    [JsonProperty]
    public Color SnakeColor { get; private set; }
    [JsonProperty]
    public Color AppleColor { get; private set; }
    [JsonProperty]
    public bool SnakeMovesThroughWalls { get; private set; }
    [JsonProperty]
    public bool GameStartsOnLoad { get; private set; }

    public static Settings Saved
    {
        get
        {
            var fileInfo = new FileInfo(SettingsFilePath);
            if (!fileInfo.Exists || fileInfo.Length == 0)
            {
                return GetDefaultAndSaveToFile();
            }

            try
            {
                using var sr = new StreamReader(SettingsFilePath);
                return JsonConvert.DeserializeObject<Settings>(sr.ReadToEnd());
            }
            catch (Exception)
            {
                return GetDefaultAndSaveToFile();
            }
        }
    }

    public void SetToDefault()
    {
        SetAppleColor(Color.Green)
        .SetSnakeColor(Color.Red)
        .SetMapWidth(DefaultMapWidth)
        .SetMapHeight(DefaultMapHeight)
        .SetGameStateUpdateDelay(DefaultGameStateUpdateDelay)
        .SetSnakeMovingThroughWalls(DefaultSnakeBehavior)
        .SetGameStartingOnLoad(DefaultGameBehavior);
    }

    public Settings SetMapWidth(int mapWidth)
    {
        if (mapWidth > MaxMapWidth || mapWidth < MinMapWidth)
        {
            throw new ArgumentOutOfRangeException($"Ширина ігрового поля має бути в межах: [{MinMapWidth}; {MaxMapWidth}].\nОтримана ширина: {mapWidth}");
        }
        MapSize = new Size(mapWidth, MapSize.Height);
        return this;
    }

    public Settings SetMapHeight(int mapHeight)
    {
        if (mapHeight > MaxMapHeight || mapHeight < MinMapHeight)
        {
            throw new ArgumentOutOfRangeException($"Висота ігрового поля має бути в межах: [{MinMapHeight}; {MaxMapHeight}].\nОтримана висота: {mapHeight}");
        }
        MapSize = new Size(MapSize.Width, mapHeight);
        return this;
    }

    public Settings SetGameStateUpdateDelay(int gameStateUpdateDelay)
    {
        if (gameStateUpdateDelay > MaxGameStateUpdateDelay || gameStateUpdateDelay < MinGameStateUpdateDelay)
        {
            throw new ArgumentOutOfRangeException($"Затримка між оновленнями ігрового стану має бути в межах: " +
                $"[{MinGameStateUpdateDelay}; {MaxGameStateUpdateDelay}].\nОтримана затримка: {gameStateUpdateDelay}");
        }
        GameStateUpdateDelay = gameStateUpdateDelay;
        return this;
    }

    public Settings SetSnakeColor(Color snakeColor)
    {
        SnakeColor = snakeColor;
        return this;
    }

    public Settings SetAppleColor(Color appleColor)
    {
        AppleColor = appleColor;
        return this;
    }

    public Settings SetSnakeMovingThroughWalls(bool snakeMovingThroughWalls)
    {
        SnakeMovesThroughWalls = snakeMovingThroughWalls;
        return this;
    }

    public Settings SetGameStartingOnLoad(bool gameStartsOnLoad)
    {
        GameStartsOnLoad = gameStartsOnLoad;
        return this;
    }

    public void SaveSettingsToFile()
    {
        using var sw = new StreamWriter(SettingsFilePath);
        sw.Write(JsonConvert.SerializeObject(this));
    }

    private static Settings GetDefaultAndSaveToFile()
    {
        var settings = new Settings();
        settings.SetToDefault();
        settings.SaveSettingsToFile();
        return settings;
    }
}
