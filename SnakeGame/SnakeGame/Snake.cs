using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public static class ExtensionFunctions
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
            }
            throw new Exception("This Should Not Happen");
        }
    }

    public class Snake
    {
        private Direction CurrentDirection;

        public LinkedList<SnakePart> SnakeParts { get; private set; }

        private Texture2D _snakePartImage;
        private Color _color;
        private int _padding;
        private int Count => SnakeParts.Count;

        private int _millisecondsBetweenUpdate;
        private int _minimumMillisecondsBetweenUpdate;
        private int _numberOfBlocksWide;
        private int _numberOfBlocksHigh;
        private double _elapsedMilliseconds;

        public SnakePart Head => SnakeParts.First();
        public SnakePart Tail => SnakeParts.Last();

        public int Score { get; private set; }

        private Direction _defaultDirection;
        private Vector2 _defaultPosition;

        private Queue<Direction> NextDirections;
        private int _partsToAdd;

        private Food food;

        public Snake(Texture2D snakePartImage, Vector2 startPosition, int millisecondsBetweenUpdate, int numberOfBlocksWide, int numberOfBlocksHigh)
            : this(snakePartImage, startPosition, millisecondsBetweenUpdate, 100, Direction.Right, Color.White, 0, numberOfBlocksWide, numberOfBlocksHigh)
        {

        }

        public Snake(Texture2D snakePartImage, Vector2 startPosition, int millisecondsBetweenUpdate, int minUpdateTime, Direction currentDirection, Color color, int padding, int numberOfBlocksWide, int numberOfBlocksHigh)
        {
            _snakePartImage = snakePartImage;
            _color = color;
            _padding = padding;
            _millisecondsBetweenUpdate = millisecondsBetweenUpdate;
            _minimumMillisecondsBetweenUpdate = minUpdateTime;
            _numberOfBlocksWide = numberOfBlocksWide;
            _numberOfBlocksHigh = numberOfBlocksHigh;

            _defaultDirection = currentDirection;
            _defaultPosition = startPosition;

            CurrentDirection = currentDirection;
            NextDirections = new Queue<Direction>();
            SetDirection(CurrentDirection);

            SnakeParts = new LinkedList<SnakePart>();
            SnakeParts.AddLast(new SnakePart(_snakePartImage, _defaultPosition, _color));

            food = new Food(_snakePartImage, Color.Red, _numberOfBlocksWide, _numberOfBlocksHigh, padding);
        }

        public void SetDirection(Direction newDirection, bool shouldSkipToUpdate = false)
        {
            if (NextDirections.Count == 0 || NextDirections.Last() != newDirection)
            {
                NextDirections.Enqueue(newDirection);
            }
            if (shouldSkipToUpdate)
            {
                _elapsedMilliseconds += _millisecondsBetweenUpdate;
            }
        }

        internal void Reset()
        {
            CurrentDirection = _defaultDirection;
            _partsToAdd = 0;
            Score = 0;
            SnakeParts.Clear();
            SnakeParts.AddLast(new SnakePart(_snakePartImage, _defaultPosition, _color));
            food.SetRandomPosition();
        }

        public void ChangeSpeed(int deltaMilliseconds)
        {
            if (_millisecondsBetweenUpdate + deltaMilliseconds > _minimumMillisecondsBetweenUpdate)
            {
                _millisecondsBetweenUpdate += deltaMilliseconds;
            }
        }

        public void AddParts(int numberOfParts)
        {
            _partsToAdd += numberOfParts;
        }

        public bool Update(GameTime gameTime)
        {
            _elapsedMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsedMilliseconds < _millisecondsBetweenUpdate)
            {
                return true;
            }
            _elapsedMilliseconds = 0;

            if (NextDirections.Count > 0)
            {
                Direction newDirection = NextDirections.Dequeue();
                if (Count == 1 || CurrentDirection != newDirection.Opposite())
                {
                    CurrentDirection = newDirection;
                }
            }
            if (_partsToAdd > 0)
            {
                AddPiece();
                _partsToAdd--;
            }

            MoveLast();

            if (Head.Intersects(food))
            {
                Score += 25;
                ChangeSpeed(-5);
                food.SetRandomPosition();
                AddParts(3);
            }

            if (HasCollidedWithSelf() || HasCollidedWithWall())
            {
                return false;
            }
            return true;
        }

        private void AddPiece()
        {
            SnakeParts.AddLast(new SnakePart(_snakePartImage, Tail.Position, _color));
        }

        private void MoveLast()
        {
            switch (CurrentDirection)
            {
                case Direction.Up:
                    if (CurrentDirection != Direction.Down)
                    {
                        Tail.Position = Head.Position - new Vector2(0, _snakePartImage.Width + _padding);
                    }
                    break;
                case Direction.Right:
                    if (CurrentDirection != Direction.Left)
                    {
                        Tail.Position = Head.Position + new Vector2(_snakePartImage.Width + _padding, 0);
                    }
                    break;
                case Direction.Down:
                    if (CurrentDirection != Direction.Up)
                    {
                        Tail.Position = Head.Position + new Vector2(0, _snakePartImage.Width + _padding);
                    }
                    break;
                case Direction.Left:
                    if (CurrentDirection != Direction.Right)
                    {
                        Tail.Position = Head.Position - new Vector2(_snakePartImage.Width + _padding, 0);
                    }
                    break;
            }
            SnakePart newHead = Tail;
            SnakeParts.RemoveLast();
            SnakeParts.AddFirst(newHead);
        }

        private bool HasCollidedWithSelf()
        {
            foreach (SnakePart part in SnakeParts)
            {
                if (part == Head)
                {
                    continue;
                }

                if (part.Intersects(Head))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasCollidedWithWall() 
            => (SnakeParts.First.Value.Position.X < 0 || SnakeParts.First.Value.Position.X > _numberOfBlocksWide * (_snakePartImage.Width + _padding) ||
                SnakeParts.First.Value.Position.Y < 0 || SnakeParts.First.Value.Position.Y > _numberOfBlocksHigh * (_snakePartImage.Height + _padding));

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (SnakePart part in SnakeParts)
            {
                part.Draw(spriteBatch);
            }

            food.Draw(spriteBatch);
        }
    }
}
