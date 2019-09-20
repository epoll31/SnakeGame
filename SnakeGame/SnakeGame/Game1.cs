using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame
{
    public enum GameState
    {
        Playing = 0,
        GameOver = 1,
        Paused = 2
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Snake snake;

        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;

        const int numberOfGridBlocksWide = 60;
        const int numberOfGridBlocksHigh = 30;
        const int screenWidth = 1500;
        const int screenHeight = 750;
        const int padding = 1;
        const int snakePartSize = (screenWidth - padding * numberOfGridBlocksWide) / numberOfGridBlocksWide;

        Texture2D BlockImage;
        SpriteFont MainFont;

        GameState currentState = GameState.Playing;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = screenWidth,
                PreferredBackBufferHeight = screenHeight
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            BlockImage = Sprite.CreateSquareTexture(GraphicsDevice, snakePartSize, Color.White);
            MainFont = Content.Load<SpriteFont>("MainFont");

            snake = new Snake(BlockImage, new Vector2((snakePartSize + padding) * 5), 200, 100, Direction.Right, Color.Black, padding, numberOfGridBlocksWide, numberOfGridBlocksHigh);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();

            if (currentState == GameState.Playing)
            {
                if (!snake.Update(gameTime))
                {
                    currentState = GameState.GameOver;
                }

                if (IsKeyPressed(Keys.W) || IsKeyPressed(Keys.Up))
                {
                    snake.SetDirection(Direction.Up, true);
                }
                if (IsKeyPressed(Keys.D) || IsKeyPressed(Keys.Right))
                {
                    snake.SetDirection(Direction.Right, true);
                }
                if (IsKeyPressed(Keys.S) || IsKeyPressed(Keys.Down))
                {
                    snake.SetDirection(Direction.Down, true);
                }
                if (IsKeyPressed(Keys.A) || IsKeyPressed(Keys.Left))
                {
                    snake.SetDirection(Direction.Left, true);
                }
                if (IsKeyPressed(Keys.Space))
                {
                    currentState = GameState.Paused;
                }
            }
            else if (currentState == GameState.Paused)
            {
                if (IsKeyPressed(Keys.Space))
                {
                    currentState = GameState.Playing;
                }
            }
            else if (currentState == GameState.GameOver)
            {
                if (IsKeyPressed(Keys.Space))
                {
                    snake.Reset();
                    //save data
                    currentState = GameState.Playing;
                }
            }


            lastKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        private bool IsKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (currentState == GameState.GameOver)
            {
                spriteBatch.DrawString(MainFont, $"you scored {snake.Score}!", new Vector2(screenWidth / 2 - MainFont.MeasureString($"you scored {snake.Score}!").X / 2, 50), Color.Red);
                spriteBatch.DrawString(MainFont, "press space to start over", new Vector2(screenWidth / 2 - MainFont.MeasureString("press space to start over").X / 2, screenHeight - 50 - MainFont.MeasureString("press space to start over").Y), Color.Red);
            }
            else
            {
                spriteBatch.DrawString(MainFont, $"score: {snake.Score}", new Vector2(50, 50), Color.Black);

                if (currentState == GameState.Paused)
                {
                    spriteBatch.DrawString(MainFont, "press space to play", new Vector2(screenWidth / 2 - MainFont.MeasureString("press space to play").X / 2, screenHeight - 50 - MainFont.MeasureString("press space to play").Y), Color.Red);
                }
            }

            snake.Draw(spriteBatch);


            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
