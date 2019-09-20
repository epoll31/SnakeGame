using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Snake snake;
        Food food;

        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;

        const int numberOfGridBlocksWide = 60;
        const int numberOfGridBlocksHigh = 30;
        const int screenWidth = 1500;
        const int screenHeight = 750;
        const int snakePartSize = screenWidth / numberOfGridBlocksWide;
        const int padding = 1;
        int score = 0;

        Texture2D BlockImage;
        SpriteFont MainFont;

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

            snake = new Snake(BlockImage, new Vector2((snakePartSize + padding) * 5), 200, 100, Direction.Right, Color.Black, padding);
            food = new Food(BlockImage, Color.Red, numberOfGridBlocksWide, numberOfGridBlocksHigh, padding);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();

            snake.Update(gameTime);

            if (snake.Head.Intersects(food))
            {
                score += 25;
                snake.ChangeSpeed(-5);
                food.SetRandomPosition();
                snake.AddParts(3);
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

            spriteBatch.DrawString(MainFont, $"Score: {score}", new Vector2(50, 50), Color.Black);


            snake.Draw(spriteBatch);
            food.Draw(spriteBatch);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
