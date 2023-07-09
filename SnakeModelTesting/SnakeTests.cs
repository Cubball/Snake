using System.Drawing;
using SnakeWinForms;

namespace SnakeModelTesting;

public class SnakeTests
{
    private Size _mapSize;

    [SetUp]
    public void SetUp()
    {
        _mapSize = new Size(20, 20);
    }

    [Test]
    public void OneLongSnakeMoves()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(20, 20));
        var gameState = new GameState(_mapSize, snake, apple);
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 9), 1)));
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 8), 1)));
        gameState.ChangeDirection(Direction.Left);
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(9, 8), 1)));
        gameState.ChangeDirection(Direction.Down);
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(9, 9), 1)));
    }

    [Test]
    public void LongSnakeMoves()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 10));
        var expectedSnakePieces = new LinkedList<Point>();
        expectedSnakePieces.AddFirst(new Point(10, 10));
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
        for (int i = 1; i <= 5; i++)
        {
            snake.Move(Direction.Right, new Point(10 + i, 10));
            expectedSnakePieces.AddFirst(new Point(10 + i, 10));
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
        }
        snake.Move(Direction.Right, new Point(0, 0));
        expectedSnakePieces.AddFirst(new Point(16, 10));
        expectedSnakePieces.RemoveLast();
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
        snake.Move(Direction.Up, new Point(0, 0));
        expectedSnakePieces.AddFirst(new Point(16, 9));
        expectedSnakePieces.RemoveLast();
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
        snake.Move(Direction.Left, new Point(0, 0));
        expectedSnakePieces.AddFirst(new Point(15, 9));
        expectedSnakePieces.RemoveLast();
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
        snake.Move(Direction.Left, new Point(0, 0));
        expectedSnakePieces.AddFirst(new Point(14, 9));
        expectedSnakePieces.RemoveLast();
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
    }

    [Test]
    public void OneLongSnakeCanMoveOpposite()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(0, 0));
        var gameState = new GameState(_mapSize, snake, apple);
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 10), 1)));
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 9), 1)));
        gameState.ChangeDirection(Direction.Down);
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 10), 1)));
    }

    [Test]
    public void LongSnakeCannotMoveOpposite()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 19));
        var gameState = new GameState(_mapSize, snake, new Apple(_mapSize, new Point(0, 0)));
        var expectedSnakePieces = new LinkedList<Point>();
        expectedSnakePieces.AddFirst(new Point(10, 19));
        for (int i = 1; i <= 4; i++)
        {
            snake.Move(Direction.Up, new Point(10, 19 - i));
            expectedSnakePieces.AddFirst(new Point(10, 19 - i));
        }
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));

        gameState.Update();
        expectedSnakePieces.RemoveLast();
        expectedSnakePieces.AddFirst(new Point(10, 14));
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));

        gameState.ChangeDirection(Direction.Down);
        gameState.Update();
        expectedSnakePieces.RemoveLast();
        expectedSnakePieces.AddFirst(new Point(10, 13));
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));

        snake.Move(Direction.Down, new Point(0, 0));
        expectedSnakePieces.RemoveLast();
        expectedSnakePieces.AddFirst(new Point(10, 12));
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
    }

    [Test]
    public void SnakeLengthIncreasesOnEatingApple()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(10, 9));
        var gameState = new GameState(_mapSize, snake, apple);
        var expectedSnakePieces = new Point[] { new Point(10, 9), new Point(10, 10) };
        Assert.That(snake.Length, Is.EqualTo(1));
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 10), 1)));
        gameState.Update();
        Assert.That(snake.Length, Is.EqualTo(2));
        Assert.That(snake.Pieces, Is.EqualTo(expectedSnakePieces));
    }

    [Test]
    public void AppleRespawnsWhenEaten()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(10, 9));
        var gameState = new GameState(_mapSize, snake, apple);
        Assert.That(apple.Postition, Is.EqualTo(new Point(10, 9)));
        gameState.Update();
        Assert.That(apple.Postition, Is.Not.EqualTo(new Point(10, 9)));
    }

    [Test]
    public void SnakeCollidesWithMapBorderTopBottom()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(0, 0));
        var gameState = new GameState(_mapSize, snake, apple);
        for (int i = 0; i < 11; i++)
        {
            Assert.That(gameState.IsRunning, Is.True);
            gameState.Update();
        }
        Assert.That(gameState.IsRunning, Is.Not.True);
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 0), 1)));

        _mapSize = new Size(20, 20);
        snake = new Snake(_mapSize, false, new Point(10, 10));
        apple = new Apple(_mapSize, new Point(0, 0));
        gameState = new GameState(_mapSize, snake, apple);
        gameState.ChangeDirection(Direction.Down);
        for (int i = 0; i < 10; i++)
        {
            Assert.That(gameState.IsRunning, Is.True);
            gameState.Update();
        }
        Assert.That(gameState.IsRunning, Is.Not.True);
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 19), 1)));
    }

    [Test]
    public void SnakeCollidesWithMapBorderLeftRight()
    {
        var snake = new Snake(_mapSize, false, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(0, 0));
        var gameState = new GameState(_mapSize, snake, apple);
        gameState.ChangeDirection(Direction.Left);
        for (int i = 0; i < 11; i++)
        {
            Assert.That(gameState.IsRunning, Is.True);
            gameState.Update();
        }
        Assert.That(gameState.IsRunning, Is.Not.True);
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(0, 10), 1)));

        snake = new Snake(_mapSize, false, new Point(10, 10));
        apple = new Apple(_mapSize, new Point(0, 0));
        gameState = new GameState(_mapSize, snake, apple);
        gameState.ChangeDirection(Direction.Right);
        for (int i = 0; i < 10; i++)
        {
            Assert.That(gameState.IsRunning, Is.True);
            gameState.Update();
        }
        Assert.That(gameState.IsRunning, Is.Not.True);
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(19, 10), 1)));
    }

    [Test]
    public void OneLongSnakeLoopsAroundTheMapTopBottom()
    {
        var snake = new Snake(_mapSize, true, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(0, 0));
        var gameState = new GameState(_mapSize, snake, apple);
        gameState.ChangeDirection(Direction.Down);
        for (int i = 0; i < 10; i++)
        {
            Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 10 + i), 1)));
            gameState.Update();
        }
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 0), 1)));
        Assert.That(gameState.IsRunning, Is.True);

        snake = new Snake(_mapSize, true, new Point(10, 10));
        apple = new Apple(_mapSize, new Point(0, 0));
        gameState = new GameState(_mapSize, snake, apple);
        for (int i = 0; i < 11; i++)
        {
            Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 10 - i), 1)));
            gameState.Update();
        }
        Assert.That(gameState.IsRunning, Is.True);
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10, 19), 1)));
    }

    [Test]
    public void OneLongSnakeLoopsAroundTheMapLeftRight()
    {
        var snake = new Snake(_mapSize, true, new Point(10, 10));
        var apple = new Apple(_mapSize, new Point(0, 0));
        var gameState = new GameState(_mapSize, snake, apple);
        gameState.ChangeDirection(Direction.Right);
        for (int i = 0; i < 10; i++)
        {
            Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10 + i, 10), 1)));
            gameState.Update();
        }
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(0, 10), 1)));
        Assert.That(gameState.IsRunning, Is.True);

        snake = new Snake(_mapSize, true, new Point(10, 10));
        apple = new Apple(_mapSize, new Point(0, 0));
        gameState = new GameState(_mapSize, snake, apple);
        gameState.ChangeDirection(Direction.Left);
        for (int i = 0; i < 11; i++)
        {
            Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(10 - i, 10), 1)));
            gameState.Update();
        }
        Assert.That(gameState.IsRunning, Is.True);
        Assert.That(snake.Pieces, Is.EqualTo(Enumerable.Repeat(new Point(19, 10), 1)));
    }

    [Test]
    public void LongSnakeLoopsAroundMapTopBottom()
    {
        var snake = new Snake(_mapSize, true, new Point(10, 10));
        var expectedSnake = new LinkedList<Point>();
        expectedSnake.AddFirst(new Point(10, 10));
        for (int i = 1; i <= 4; i++)
        {
            snake.Move(Direction.Down, new Point(10, 10 + i));
            expectedSnake.AddFirst(new Point(10, 10 + i));
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 1; i <= 5; i++)
        {
            snake.Move(Direction.Down, new Point(0, 0));
            expectedSnake.AddFirst(new Point(10, 14 + i));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 0; i < 2; i++)
        {
            snake.Move(Direction.Down, new Point(0, 0));
            expectedSnake.AddFirst(new Point(10, i));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }

        snake = new Snake(_mapSize, true, new Point(10, 10));
        expectedSnake = new LinkedList<Point>();
        expectedSnake.AddFirst(new Point(10, 10));
        for (int i = 1; i <= 4; i++)
        {
            snake.Move(Direction.Up, new Point(10, 10 - i));
            expectedSnake.AddFirst(new Point(10, 10 - i));
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 1; i <= 6; i++)
        {
            snake.Move(Direction.Up, new Point(0, 0));
            expectedSnake.AddFirst(new Point(10, 6 - i));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 0; i < 2; i++)
        {
            snake.Move(Direction.Up, new Point(0, 0));
            expectedSnake.AddFirst(new Point(10, 19 - i));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
    }

    [Test]
    public void LongSnakeLoopsAroundMapLeftRight()
    {
        var snake = new Snake(_mapSize, true, new Point(10, 10));
        var expectedSnake = new LinkedList<Point>();
        expectedSnake.AddFirst(new Point(10, 10));
        for (int i = 1; i <= 4; i++)
        {
            snake.Move(Direction.Right, new Point(10 + i, 10));
            expectedSnake.AddFirst(new Point(10 + i, 10));
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 1; i <= 5; i++)
        {
            snake.Move(Direction.Right, new Point(0, 0));
            expectedSnake.AddFirst(new Point(14 + i, 10));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 0; i < 2; i++)
        {
            snake.Move(Direction.Right, new Point(0, 0));
            expectedSnake.AddFirst(new Point(i, 10));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }

        snake = new Snake(_mapSize, true, new Point(10, 10));
        expectedSnake = new LinkedList<Point>();
        expectedSnake.AddFirst(new Point(10, 10));
        for (int i = 1; i <= 4; i++)
        {
            snake.Move(Direction.Left, new Point(10 - i, 10));
            expectedSnake.AddFirst(new Point(10 - i, 10));
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 1; i <= 6; i++)
        {
            snake.Move(Direction.Left, new Point(0, 0));
            expectedSnake.AddFirst(new Point(6 - i, 10));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
        for (int i = 0; i < 2; i++)
        {
            snake.Move(Direction.Left, new Point(0, 0));
            expectedSnake.AddFirst(new Point(19 - i, 10));
            expectedSnake.RemoveLast();
            Assert.That(snake.Pieces, Is.EqualTo(expectedSnake));
        }
    }
    [Test]
    public void SnakeLoopsWithoutColliding()
    {
        var snake = new Snake(_mapSize, true, new Point(0, 0));
        var apple = new Apple(_mapSize, new Point(19, 19));
        var gameState = new GameState(_mapSize, snake, apple);
        for (int i = 1; i <= 3; i++)
        {
            snake.Move(Direction.Right, new Point(i, 0));
        }
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Down);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Left);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Up);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.True);
    }

    [Test]
    public void SnakeCollidesWithItselfFromSide()
    {
        var snake = new Snake(_mapSize, true, new Point(0, 0));
        var apple = new Apple(_mapSize, new Point(19, 19));
        var gameState = new GameState(_mapSize, snake, apple);
        for (int i = 1; i <= 4; i++)
        {
            snake.Move(Direction.Right, new Point(i, 0));
        }
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Down);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Left);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Up);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.Not.True);

        snake = new Snake(_mapSize, true, new Point(0, 0));
        apple = new Apple(_mapSize, new Point(19, 19));
        gameState = new GameState(_mapSize, snake, apple);
        for (int i = 1; i <= 5; i++)
        {
            snake.Move(Direction.Right, new Point(i, 0));
        }
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Down);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Left);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Up);
        gameState.Update();
        Assert.That(gameState.IsRunning, Is.Not.True);
    }

    [Test]
    public void SnakeCollidesWithItSelfFromBack()
    {
        var mapSize = new Size(5, 1);
        var snake = new Snake(mapSize, true, new Point(0, 0));
        var apple = new Apple(mapSize, new Point(1, 0));
        var gameState = new GameState(mapSize, snake, apple);
        gameState.ChangeDirection(Direction.Right);
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(new Point[] { new Point(1, 0), new Point(0, 0) }));
        Assert.That(apple.Postition, Is.Not.EqualTo(new Point(1, 0)));
        Assert.That(gameState.IsRunning, Is.True);
        gameState.ChangeDirection(Direction.Down);
        gameState.Update();
        Assert.That(snake.Pieces, Is.EqualTo(new Point[] { new Point(1, 0), new Point(0, 0) }));
        Assert.That(gameState.IsRunning, Is.Not.True);
    }

    [Test]
    public void ScoreIncreasesOnAppleEaten()
    {
        var mapSize = new Size(5, 1);
        var snake = new Snake(mapSize, true, new Point(0, 0));
        var apple = new Apple(mapSize, new Point(2, 0));
        var gameState = new GameState(mapSize, snake, apple);
        Assert.That(gameState.Score, Is.EqualTo(0));
        gameState.ChangeDirection(Direction.Right);
        gameState.Update();
        Assert.That(gameState.Score, Is.EqualTo(0));
        gameState.Update();
        Assert.That(gameState.Score, Is.EqualTo(1));
    }
}