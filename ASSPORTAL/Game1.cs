using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ASSPORTAL
{
    /// <summary>
    /// ASSPORTAL By: MAL.Ware
    /// Platforms: PC(Not Macs because they suck.), Gapestation 4, xBoner. PC IS MASTER RACE.
    /// Main Game shit goes here I guess.
    /// This game may be open-source.
    /// Go to www.gapenewell.com for free snowdogs.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Backdrop background;
        Texture2D bg;
        int score;


        Texture2D projectileTexture;
        List<projectiles> projectiles;

        // The rate of fire of the player laser
        TimeSpan fireTime;
        TimeSpan previousFireTime;

        // Enemies
        Texture2D enemyTexture;
        List<Enemy> enemies;

        // The rate at which the enemies appear
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        TimeSpan itemSpawnTime;
        Texture2D shieldPickUpTexture;
        List<Items> item;

        // A random number generator
        Random random;
        Random random2;

        SpriteFont assFont;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        float playerMoveSpeed;
       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Player();
            background = new Backdrop();

            score = 0;
            
            playerMoveSpeed = 8.0f;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 720;

            // Initialize the enemies list
            enemies = new List<Enemy>();

            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;

            // Used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(0.5f);

            itemSpawnTime = TimeSpan.FromSeconds(0.4f);
            item = new List<Items>();

            // Initialize our random number generator
            random = new Random();
            random2 = new Random();

            projectiles = new List<projectiles>();

            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(.15f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Player/AssGuy_Final"), playerPosition);
            Vector2 bgPosition = new Vector2(GraphicsDevice.Viewport.X,GraphicsDevice.Viewport.Y);
            background.Initialize(Content,"BG/Background2", GraphicsDevice.Viewport.Width, -1);
            bg = Content.Load<Texture2D>("BG/Background1");
            enemyTexture = Content.Load<Texture2D>("Etc/Enemy");
            projectileTexture = Content.Load<Texture2D>("Etc/portalBullet");
            shieldPickUpTexture = Content.Load<Texture2D>("Items/ShieldPickUp");

            assFont = Content.Load<SpriteFont>("AssFont");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

          

            UpdatePlayer(gameTime);
            background.Update();
            UpdateEnemies(gameTime);
            UpdateCollision();
            UpdateProjectiles();
            //UpdateItems(gameTime);
            

            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            //thumbstik controlls 4 xBox 1
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            if (player.Health <= 0)
            {
                player.Health = 100;
                score = 0;
                player.Position = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            }

           // player.Position.Y += 10;

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
            currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Space) ||
            currentGamePadState.Buttons.A == ButtonState.Pressed)
            {
                player.Position.Y -= 20;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
            currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
            currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
            currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.OemComma))
            {
                enemySpawnTime = TimeSpan.FromSeconds(0.0f);
            }
            if (currentKeyboardState.IsKeyDown(Keys.OemPeriod))
            {
                enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);

            // Fire only every interval we set as the fireTime
            if (gameTime.TotalGameTime - previousFireTime > fireTime)
            {
                // Reset our current time
                previousFireTime = gameTime.TotalGameTime;

                // Add the projectile, but add it to the front and center of the player
                AddProjectile(player.Position + new Vector2(player.Width / 2*2f, 60));
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                AddEnemy();
            }

            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void UpdateCollision()
        {
            Rectangle rect1;
            Rectangle rect2;
            
            rect1 = new Rectangle((int)player.Position.X,(int)player.Position.Y,player.Width,player.Height);

            for (int i = 0; i < enemies.Count; i++)
            {
                rect2 = new Rectangle((int)enemies[i].Position.X,
                (int)enemies[i].Position.Y,
                enemies[i].Width,
                enemies[i].Height);

                // Determine if the two objects collided with each
                // other
                if (rect1.Intersects(rect2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= enemies[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    enemies[i].Health = 0;

                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                        player.Active = false;
                }

            }
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rect1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                    rect2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                    (int)enemies[j].Position.Y - enemies[j].Height / 2,
                    enemies[j].Width, enemies[j].Height);

                    // Determine if the two objects collided with each other
                    if (rect1.Intersects(rect2))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        score += enemies[j].Value;
                        projectiles[i].Active = false;
                    }
                }
            }
        }
        //private void UpdateItems(GameTime gameTime)
        //{
           
        //    if (gameTime.TotalGameTime - previousSpawnTime > itemSpawnTime)
        //    {
        //        previousSpawnTime = gameTime.TotalGameTime;

        //        // Add an item
        //        AddEnemy();
        //    }

        //    // Update the Enemies
        //    for (int i = item.Count - 1; i >= 0; i--)
        //    {
        //        item[i].Update(gameTime);

        //        if (item[i].Active == false)
        //        {
        //            item.RemoveAt(i);
        //        }
        //    }
        //}

        private void AddProjectile(Vector2 position)
        {
            projectiles projectile = new projectiles();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position);
            projectiles.Add(projectile);
        }

        private void AddEnemy()
        {      
            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));

            // Create an enemy
            Enemy enemy = new Enemy();

            // Initialize the enemy
            enemy.Initialize(enemyTexture, position);

            // Add the enemy to the active enemies list
            enemies.Add(enemy);
        }
        //private void AddItems()
        //{
        //    // Randomly generate the position of the enemy
        //    Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + shieldPickUpTexture.Width / 2, random2.Next(100, GraphicsDevice.Viewport.Height - 100));

        //    // Create an enemy
        //    Items items = new Items();

        //    // Initialize the enemy
        //    items.Initialize(shieldPickUpTexture, position);

        //    // Add the enemy to the active enemies list
        //    item.Add(items);
        //}
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Vector2 Corner = new Vector2(0,0);
            String Health = player.Health.ToString();
            String Score = score.ToString();

            spriteBatch.Begin();

            spriteBatch.Draw(bg, Vector2.Zero, Color.White);
            background.Draw(spriteBatch);
            player.Draw(spriteBatch);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }
            for (int i = 0; i < item.Count; i++)
            {
                item[i].Draw(spriteBatch);
            }
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }
            spriteBatch.DrawString(assFont,"HP:" +Health, Corner, Color.Black);
            spriteBatch.DrawString(assFont, "Score:" + Score, new Vector2(0,30), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
