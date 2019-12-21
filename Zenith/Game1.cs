using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Zenith
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<GameObject> gameObjects;

        // Texture2D[] textures;

        /*string[] assetsToLoad = {
            "",
            "blue_01",
            "tankbase_01",
            "darkgrey_01",
            "purple_06",
            "large_grey_01",
            "large_purple_01",
            "large_grey_02",
            "large_red_01",
            "large_green_01",
            "healthbar_0",
            "healthbar_1",
            "healthbar_2",
            "healthbar_3",
            "healthbar_4",
            "healthbar_5",
            "healthbar_6",
            "healthbar_7",
            "healthbar_8",
            "healthbar_9",
            "healthbar_10",
            "Aster1",
            "Projectiles\\projectile-blue",
        };*/

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
            // TODO: Add your initialization logic here

            gameObjects = new List<GameObject>();

            Window.Title = "Zenith";

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            World.Instance.SetScreenDimensions(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);

            World.Instance.CreatePlayer();

            base.Initialize();
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            World.Instance.SetScreenDimensions(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);

            // MessageBox.Show("HELLO", Window.ClientBounds.ToString(), s);
            // Window.ClientBounds.Width = 900;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            World.Instance.AddObject(new Enemy1(new Vector2(20, 20)));

            AssetManager.Instance.ContentManager = Content;
            AssetManager.Instance.LoadAssets("assetsToLoad.txt");

            // game
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (World.Instance.GameTick % 100 == 0)
            {
                Console.WriteLine("Objects: " + World.Instance.Objects.Count);
            }

            World.Instance.Update();

            var kstate = Keyboard.GetState();

            World.Instance.PlayerController.Up = kstate.IsKeyDown(Keys.Up);
            World.Instance.PlayerController.Down = kstate.IsKeyDown(Keys.Down);
            World.Instance.PlayerController.Left = kstate.IsKeyDown(Keys.Left);
            World.Instance.PlayerController.Right = kstate.IsKeyDown(Keys.Right);
            World.Instance.PlayerController.Fire = kstate.IsKeyDown(Keys.Space);

            foreach (var gameObject in gameObjects)
            {
                gameObject.Update();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Console.WriteLine(World.Instance.Objects.Count);

            // TODO: Add your drawing code here
            // Source: https://gamedev.stackexchange.com/questions/102681/in-monogame-how-can-i-disable-font-anti-aliasing
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Texture2D texture;

            foreach (var gameObject in World.Instance.Objects)
            {
                // texture = textures[(int)gameObject.GameImage];
                texture = AssetManager.Instance.Images[(int)gameObject.GameImage];
                if (texture == null) continue;

                /*spriteBatch.Draw(
                    texture,
                    gameObject.Position,
                    null,
                    Color.White,
                    gameObject.Angle + gameObject.ImageRotation,
                    new Vector2(texture.Width / 2, texture.Height / 2),
                    gameObject.Scale,
                    SpriteEffects.None,
                    0f
               );*/

                spriteBatch.Draw(
                    texture,
                    new Rectangle(
                        (int)gameObject.Position.X,
                        (int)gameObject.Position.Y,
                        (int)gameObject.Size.X,
                        (int)gameObject.Size.Y
                        ),
                    null, // new Rectangle(texture.Width / 2, texture.Height / 2, 0, 0),
                    Color.White,
                    gameObject.Angle + gameObject.ImageRotation,
                    new Vector2(texture.Width / 2, texture.Height / 2),
                    SpriteEffects.None,
                    0f
                    );
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
