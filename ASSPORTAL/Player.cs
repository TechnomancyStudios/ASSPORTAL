using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ASSPORTAL
{
    /// <summary>
    ///  This is where faggots are made.
    /// </summary>

    class Player
    {
        public Texture2D playerTexture;

        public Vector2 Position;

        public bool Active;

        public int Health;

        public int Width
        {
            get { return playerTexture.Width; }
        }

        public int Height
        {
            get { return playerTexture.Height; }
        }


        //This Initializes the player, Faggot.
        public void Initialize(Texture2D texture, Vector2 position)
        {
            playerTexture = texture;

            Position = position;

            Active = true;

            Health = 100;
        }

        public void Update()
        {
            if (Health <= 0)
            {
                Active = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
        

    }
}
