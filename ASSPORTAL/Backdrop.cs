using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ASSPORTAL
{
    class Backdrop
    {
        public Texture2D backGround;

        public Vector2 [] bgPos;

        int speed;

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
        {
            backGround = content.Load<Texture2D>(texturePath);

            this.speed = speed;

            bgPos = new Vector2[screenWidth / backGround.Width + 1];

            for (int i = 0; i < bgPos.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                bgPos[i] = new Vector2(i * backGround.Width, 0);
            }
        }

        public void Update()
        {
            for (int i = 0; i < bgPos.Length; i++)
            {
                bgPos[i].X += speed;

                if (speed <= 0)
                {
                    if (bgPos[i].X <= -backGround.Width)
                    {
                        bgPos[i].X = backGround.Width * (bgPos.Length - 1);
                    }
                }
                else
                {
                    if (bgPos[i].X >= backGround.Width * (bgPos.Length - 1))
                    {
                        bgPos[i].X = -backGround.Width;
                    }
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bgPos.Length; i++)
            {
                spriteBatch.Draw(backGround, bgPos[i], Color.White);
            }
        }
    }
}
