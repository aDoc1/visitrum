using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements the Action Scene.
    /// </summary>
    public class NormalActionScene : GameScene
    {
        // Basics
        protected Texture2D actionTexture;

        //private AudioLibrary audio;
        protected SpriteBatch spriteBatch = null;

        // Game Elements
        protected Player player1;
        //protected SimpleRumblePad rumblePad;
        protected ImageComponent background;
        protected Score scorePlayer1;
        protected ColorBlock colorBlock;
        protected Color[] colors = new Color[16];
        protected int maxBlocks;
        protected int blocks;
        protected int level;
        protected Random ran;
        protected Color curBlockColor;
        protected int initLives = 3;
        protected GameText gameText;
        protected string[] gameTextAry = new string[9];

        // Gui Stuff
        protected Vector2 pausePosition;
        protected Vector2 gameoverPosition;
        protected Rectangle pauseRect = new Rectangle(1, 120, 200, 44);
        protected Rectangle gameoverRect = new Rectangle(1, 170, 350, 48);

        // GameState elements
        protected bool paused;
        protected bool levelover;
        protected bool gameOver;
        protected TimeSpan elapsedTime = TimeSpan.Zero;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="game">The main game object</param>
        /// <param name="theTexture">Texture with the sprite elements</param>
        /// <param name="backgroundTexture">Texture for the background</param>
        /// <param name="font">Font used in the score</param>
        public NormalActionScene(Game game, Texture2D theTexture,
            Texture2D backgroundTexture, SpriteFont font, Texture2D levelTextTexture, SpriteFont levelTextFont)
            : base(game)
        {
            background = new ImageComponent(game, backgroundTexture,
                ImageComponent.DrawMode.Stretch);
            Components.Add(background);

            actionTexture = theTexture;

            // Get the current sprite batch
            spriteBatch = (SpriteBatch)
                Game.Services.GetService(typeof(SpriteBatch));

            // Get the audio library
            //audio = (AudioLibrary) Game.Services.GetService(typeof(AudioLibrary));

#if XBOX360
            player1 = new Player(Game, ref actionTexture, PlayerIndex.One, new Rectangle(5, 10, 230, 26), 3);
#else
            player1 = new Player(Game, ref actionTexture, PlayerIndex.One,
                new Rectangle(2, 1, 230, 26), initLives);
#endif

            player1.Initialize();
            Components.Add(player1);

            scorePlayer1 = new Score(game, font, Color.Blue);
            
#if XBOX360
            scorePlayer1.Position = new Vector2(40, 15);

#else
            scorePlayer1.Position = new Vector2(10, 10);
#endif
            Components.Add(scorePlayer1);

            ran = new Random(this.GetHashCode());

            colors[0] = Color.Red;
            colors[1] = Color.Yellow;
            colors[2] = Color.Blue;
            colors[3] = Color.Green;
            colors[4] = Color.Black;
            colors[5] = Color.White;
            colors[6] = Color.Orange;
            colors[7] = Color.Cyan;
            colors[8] = Color.Purple;
            colors[9] = Color.Brown;
            colors[10] = Color.YellowGreen;
            colors[11] = Color.Gray;
            colors[12] = Color.DarkBlue;
            colors[13] = Color.DarkRed;
            colors[14] = Color.LightGreen;
            colors[15] = Color.Pink;

            int currentIndex = ran.Next(0, 4);
            curBlockColor = colors[currentIndex];
            colorBlock = new ColorBlock(game, ref actionTexture, curBlockColor);
            colorBlock.setSpeed(1);
            Components.Add(colorBlock);
            
            maxBlocks = 10;
            level = 1;
            blocks = 0;
            //rumblePad = new SimpleRumblePad(game);
            //Components.Add(rumblePad);

            gameTextAry[0] = "Level 1.  You will start out with 4 colors; red, green, blue, and yellow.  Get 6 blocks correct and move on to the next level.";
            gameTextAry[1] = "Level 5.  Two more colors are added, black and white.  10 blocks are needed to get to the next level.  Can  you handle it?";
            gameTextAry[2] = "Level 10. Are you ready for some color mixing?  Orange, Purple, Cyan, Yellow-Green, and Brown are added.  15 blocks are needed to get to the next level.";
            gameTextAry[3] = "Level 15. It's looking a little shady in here.  Dark-Blue, Grey, Dark-Red, Light-Green, and Pink are added.  Slight Increase in speed.  20 Blocks gets you to the next level.";
            gameTextAry[4] = "Level 20. Is it getting faster in here?  Can you keep up?";
            gameTextAry[5] = "Level 25. More speed Captain!.";
            gameTextAry[6] = "Level 30. Speed is doubled";
            gameTextAry[7] = "Level 40. Speed is more than doubled.";
            gameTextAry[8] = "Level 50. Speed at maximum.";

            gameText = new GameText(game, levelTextTexture, levelTextFont);
            gameText.SetTextItems(gameTextAry);
            gameText.RegularColor = Color.Gray;
            Components.Add(gameText);

        }

        /// <summary>
        /// Show the action scene
        /// </summary>
        public override void Show()
        {
            //MediaPlayer.Play(audio.BackMusic);

            player1.Reset();

            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width -
                pauseRect.Width) / 2;
            pausePosition.Y = (Game.Window.ClientBounds.Height -
                pauseRect.Height) / 2;

            gameOver = false;
            gameoverPosition.X = (Game.Window.ClientBounds.Width -
                gameoverRect.Width) / 2;
            gameoverPosition.Y = (Game.Window.ClientBounds.Height -
                gameoverRect.Height) / 2;

            base.Show();
        }

        /// <summary>
        /// Hide the scene
        /// </summary>
        public override void Hide()
        {
            // Stop the background music
            //MediaPlayer.Stop();
            // Stop the rumble
            //rumblePad.Stop(PlayerIndex.One);
            //rumblePad.Stop(PlayerIndex.Two);

            base.Hide();
        }

        /// <summary>
        /// True, if the game is in GameOver state
        /// </summary>
        public bool GameOver
        {
            get { return gameOver; }
        }

        /// <summary>
        /// Paused mode
        /// </summary>
        public bool Paused
        {
            get { return paused; }
            set
            {
                paused = value;
                if (paused)
                {
                    //MediaPlayer.Pause();
                }
                else
                {
                    //MediaPlayer.Resume();
                }
            }
        }

        private void HandleLevels()
        {
            blocks++;
            if (blocks == maxBlocks)
            {
                level++;
                blocks = 0;
                player1.Level = level;
                player1.Lives++;
            }
            if (level < 5)
            {
                curBlockColor = colors[ran.Next(0, 4)];
                maxBlocks = 10;
            }

            else if (level < 10)
            {
                curBlockColor = colors[ran.Next(0, 6)];
                maxBlocks = 15;
            }

            else if (level < 15)
            {
                curBlockColor = colors[ran.Next(0, 11)];
                maxBlocks = 20;
            }

            else if (level < 20)
            {
                curBlockColor = colors[ran.Next(0, 16)];
                maxBlocks = 25;
                colorBlock.setSpeed(1.1);
            }
            else if (level < 25)
            {
                colorBlock.setSpeed(1.3);
            }
            else if (level < 30)
            {
                colorBlock.setSpeed(1.5);
            }
            else if (level < 40)
            {
                colorBlock.setSpeed(2);
            }
            else if (level < 50)
            {
                colorBlock.setSpeed(2.5);
            }
            else
                colorBlock.setSpeed(2.7);

            colorBlock.BlockColor = curBlockColor;
            colorBlock.putInStartPosition();
        }

        /// <summary>
        /// Handle collisions
        /// </summary>
        private void HandleDamages()
        {
            // Check Collision for player 1
            if (colorBlock.CheckCollision(player1.GetBounds()))
            {
                // Check if paddle color matches block color
                if (player1.PaddleColor == curBlockColor)
                {
                    player1.Score += 10;
                }
                else
                {
                    player1.Lives--;
                }

                HandleLevels();
            }
        }

        /// <summary>
        /// Update lives 
        /// </summary>
        private void UpdateLives(Player player)
        {
            if (--player.Lives > 0)
                player.Reset();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if ((!paused) && (!gameOver))
            {
                // Check collisions with paddle
                HandleDamages();

                // Update score
                scorePlayer1.Value = player1.Score;
                scorePlayer1.Lives = player1.Lives;
                scorePlayer1.Level = player1.Level;

                // Check if player is dead
                gameOver = ((player1.Lives == 0));
                if (gameOver)
                {
                    player1.Visible = (player1.Score > 0);
                    // Stop the music
                    //MediaPlayer.Stop();
                    // Stop rumble
                    //rumblePad.Stop(PlayerIndex.One);
                    //rumblePad.Stop(PlayerIndex.Two);
                }

                // Update all other game components
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw all game components
            base.Draw(gameTime);

            if (paused)
            {
                // Draw the "pause" text
                spriteBatch.Draw(actionTexture, pausePosition, pauseRect,
                    Color.White);
            }
            if (gameOver)
            {
                // Draw the "gameover" text
                spriteBatch.Draw(actionTexture, gameoverPosition, gameoverRect,
                    Color.White);
            }
        }
    }
}