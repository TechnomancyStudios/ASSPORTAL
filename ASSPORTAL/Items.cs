using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ASSPORTAL
{
    class Items
    {
        #region Shield
        public Texture2D shieldPickUpTexture;

        // The position of the enemy ship relative to the top left corner of thescreen
        public Vector2 Position;

        // The state of the Enemy Ship
        public bool Active;

        // Get the width of the shield
        public int Width
        {
            get { return shieldPickUpTexture.Width; }
        }

        // Get the height of the shield
        public int Height
        {
            get { return shieldPickUpTexture.Height; }
        }

        // The speed at which the item moves
        float itemMoveSpeed;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            // Load the enemy ship texture
            shieldPickUpTexture = texture;

            // Set the position of the enemy
            Position = position;

            // We initialize the enemy to be active so it will be update in the game
            Active = true;

            // Set how fast the enemy moves
            itemMoveSpeed = 6f;

        }

        public void Update(GameTime gameTime)
        {
            // The enemy always moves to the left so decrement it's xposition
            Position.X -= itemMoveSpeed;


            // If the enemy is past the screen deactivateit
            if (Position.X < -Width)
            {
                // By setting the Active flag to false, the game will remove this objet fromthe 
                // active game list
                Active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shieldPickUpTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
        #endregion
     
    }
}
