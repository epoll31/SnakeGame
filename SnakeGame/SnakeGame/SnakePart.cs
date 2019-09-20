using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame
{
    public class SnakePart : Sprite
    {
        public SnakePart(Texture2D image, Vector2 position, Color color)
            : base(image , position, color)
        {
        }
    }
}
