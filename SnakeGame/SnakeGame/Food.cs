using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame
{
    public class Food : Sprite
    {
        private Random random;

        private int _numberOfGridBlocksWide;
        private int _numberOfGridBlocksHigh;
        private int _padding;

        public Food(Texture2D image, Color color, int numberOfGridBlocksWide, int numberOfGridBlocksHigh, int padding)
            : base(image, Vector2.Zero, color)
        {
            random = new Random();

            _numberOfGridBlocksWide = numberOfGridBlocksWide;
            _numberOfGridBlocksHigh = numberOfGridBlocksHigh;
            _padding = padding;

            SetRandomPosition();
        }

        public void SetRandomPosition()
        {
            Position = new Vector2(random.Next(_numberOfGridBlocksWide-1), random.Next(_numberOfGridBlocksHigh-1)) * (Image.Width + _padding);
        }
    }
}
