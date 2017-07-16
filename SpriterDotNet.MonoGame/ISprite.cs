using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriterDotNet.MonoGame
{
    public interface ISprite
    {
        float Width();
        float Height();

        void Draw(SpriteBatch spriteBatch, Vector2 pivot, Vector2 position, Vector2 scale, float rotation, Color color, float depth, bool stretchOut = true);
    }
}
