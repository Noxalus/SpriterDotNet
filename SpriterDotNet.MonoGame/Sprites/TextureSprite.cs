// Copyright (c) 2015 The original author or authors
//
// This software may be modified and distributed under the terms
// of the zlib license.  See the LICENSE file for details.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpriterDotNet.MonoGame.Sprites
{
    /// <summary>
    /// A drawable wrapper for a Texture2D
    /// </summary>
    public class TextureSprite : ISprite
    {
        private readonly Texture2D texture;

        public TextureSprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pivot, Vector2 position, Vector2 scale, float rotation, Color color, float depth, bool stretchOut)
        {
            SpriteEffects effects = SpriteEffects.None;

            float originX = pivot.X * texture.Width;
            float originY = pivot.Y * texture.Height;

            if (scale.X < 0)
            {
                effects |= SpriteEffects.FlipHorizontally;
                originX = texture.Width - originX;
            }

            if (scale.Y < 0)
            {
                effects |= SpriteEffects.FlipVertically;
                originY = texture.Height - originY;
            }

            scale = new Vector2(Math.Abs(scale.X), Math.Abs(scale.Y));
            Vector2 origin = new Vector2(originX, originY);

            // If we don't want to deform the sprite when the scale change
            if (!stretchOut && scale != Vector2.One)
            {
                var drawCount = new Point(1 + (int)Math.Floor(scale.X), 1 + (int)Math.Floor(scale.Y));
                var spritePartOrigin = position;
                var rotationCos = Math.Cos(rotation);
                var rotationSin = Math.Sin(rotation);

                for (int x = 0; x < drawCount.X; x++)
                {
                    for (int y = 0; y < drawCount.Y; y++)
                    {
                        var positionOffset = new Point(texture.Width * x, texture.Height * y);

                        var newPosition = new Point(
                            (int)(position.X + positionOffset.X),
                            (int)(position.Y + positionOffset.Y)
                        );

                        // Take rotation value into account
                        newPosition = new Point(
                            (int)(spritePartOrigin.X + (newPosition.X - spritePartOrigin.X) * rotationCos - (newPosition.Y - spritePartOrigin.Y) * rotationSin),
                            (int)(spritePartOrigin.Y + (newPosition.X - spritePartOrigin.X) * rotationSin + (newPosition.Y - spritePartOrigin.Y) * rotationCos)
                        );

                        var destinationRectangle = new Rectangle(newPosition.X, newPosition.Y, texture.Width, texture.Height);

                        if (x == drawCount.X - 1)
                            destinationRectangle.Width = (int)(destinationRectangle.Width * (scale.X - x));
                        else if (scale.X < 1f)
                            destinationRectangle.Width = (int)(destinationRectangle.Width * scale.X);

                        if (y == drawCount.Y - 1)
                            destinationRectangle.Height = (int)(destinationRectangle.Height * (scale.Y - y));
                        else if (scale.Y < 1f)
                            destinationRectangle.Height = (int)(destinationRectangle.Height * scale.Y);

                        var sourceRectangle = new Rectangle(0, 0, destinationRectangle.Width, destinationRectangle.Height);

                        spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, depth);
                    }
                }
            }
            else
            {
                spriteBatch.Draw
                (
                    texture: texture,
                    origin: origin,
                    position: position,
                    scale: scale,
                    rotation: rotation,
                    color: color,
                    layerDepth: depth,
                    effects: effects
                );
            }
        }

        public float Height()
        {
            return texture.Width;
        }

        public float Width()
        {
            return texture.Height;
        }
    }
}
