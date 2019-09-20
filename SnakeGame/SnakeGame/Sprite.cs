using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Sprite
    {
        public Texture2D Image { get; protected set; }
        private Vector2 position;
        public ref Vector2 Position => ref position;
        public Color Color { get; protected set; }

        public Rectangle HitBox => new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);

        public Sprite(Texture2D image, Vector2 position, Color color)
        {
            Image = image;
            Position = position;
            Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, Color);
        }

        public bool Intersects(Sprite sprite)
        {
            return HitBox.Intersects(sprite.HitBox);
        }

        public static Texture2D CreateSquareTexture(GraphicsDevice graphicsDevice, int sideLength, Color color)
            => CreateRectangleTexture(graphicsDevice, sideLength, sideLength, color);

        public static Texture2D CreateRectangleTexture(GraphicsDevice graphicsDevice, int width, int height, Color color)
        {
            Texture2D image = new Texture2D(graphicsDevice, width, height);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            image.SetData(pixels);
            return image;
        }
    }
}
