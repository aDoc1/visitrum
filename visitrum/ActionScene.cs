using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;

namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements the Action Scene.
    /// </summary>
    public class ActionScene : GameScene
    {
        // Basics
        protected Texture2D actionTexture;

        private AudioLibrary audio;
        protected SpriteBatch spriteBatch = null;

        // Game Elements
        protected Player player1;
        //protected SimpleRumblePad rumblePad;
        protected ImageComponent background;
        protected Score scorePlayer1;
        //protected HighScoreTable highscore = new HighScoreTable();
        protected ColorBlock colorBlock;
        protected Color[] colors = new Color[16];
        
        protected int blocks;
        protected int level;
        protected Random ran;
        protected Color curBlockColor;
        protected GameText gameText; 
        protected int levelgroup;
        protected int difficulty;

        // Game level variables
        protected string[][] gameTextAry = new string[3][];
        protected int[][] maxBlocks = new int[3][];
        protected double[][] speed = new double[3][];
        protected int[] maxColors = new int[11];
        protected int[] initLives = new int[3];
        protected int[] maxLevelInGroup = new int[11];

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
        public ActionScene(Game game, Texture2D theTexture,
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
            audio = (AudioLibrary) Game.Services.GetService(typeof(AudioLibrary));

#if XBOX360
            player1 = new Player(Game, ref actionTexture, PlayerIndex.One, new Rectangle(5, 10, 230, 26));

#else 
            player1 = new Player(Game, ref actionTexture, PlayerIndex.One, new Rectangle(2, 1, 230, 26));
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
            colors[9] = Color.Chocolate;
            colors[10] = Color.YellowGreen;
            colors[11] = Color.Gray;
            colors[12] = Color.DarkBlue;
            colors[13] = Color.DarkRed;
            colors[14] = Color.Chartreuse;
            colors[15] = Color.Pink;

            int currentIndex = ran.Next(0, 4);
            curBlockColor = colors[currentIndex];
            colorBlock = new ColorBlock(game, ref actionTexture, curBlockColor);
            Components.Add(colorBlock);
            
 
            //rumblePad = new SimpleRumblePad(game);
            //Components.Add(rumblePad);

            gameText = new GameText(game, levelTextTexture, levelTextFont);
            gameText.RegularColor = Color.Gray;
            Components.Add(gameText);

            Reset();
        }

        public Game1 Game1 { get { return (Game1)Game; } }


        protected void Init()
        {
            gameTextAry[0] = new string[11];
            gameTextAry[1] = new string[11];
            gameTextAry[2] = new string[11];

            // Easy settings
            gameTextAry[0][0] = "Level 1.  You will start out with 4 colors; red, green, blue, and yellow.\n\nGet 10 blocks correct and move on to the next level.";
            gameTextAry[0][1] = "Level 5.  Two more colors are added, black and white.\n10 blocks are needed to get to the next level.  Can  you handle it?";
            gameTextAry[0][2] = "Level 10. Are you ready for some color mixing?\nOrange, Purple, Cyan, Yellow-Green, and Brown are added.";
            gameTextAry[0][3] = "Level 15. Now let's try combining everything you've learned so far.";
            gameTextAry[0][4] = "Level 20. It's looking a little shady in here.\nDark-Blue, Grey, Dark-Red, Light-Green, and Pink are added.";
            gameTextAry[0][5] = "Level 25. Can you handle all the colors?";
            gameTextAry[0][6] = "Level 30. Is it getting faster in here?\n\nSmall increase of speed, can you keep up?";
            gameTextAry[0][7] = "Level 35. Speed is increased again.\n\nGood luck!";
            gameTextAry[0][8] = "Level 40. More speed, Captain!";
            gameTextAry[0][9] = "Level 45. Speed is doubled.";
            gameTextAry[0][10] = "Level 50. Speed at maximum.";

            // Normal settings
            gameTextAry[1][0] = "Level 1.  You will start out with 4 colors; red, green, blue, and yellow.\n\nGet 6 blocks correct and move on to the next level.";
            gameTextAry[1][1] = "Level 5.  Two more colors are added, black and white.\n10 blocks are needed to get to the next level.  Can  you handle it?";
            gameTextAry[1][2] = "Level 10. Are you ready for some color mixing?\nOrange, Purple, Cyan, Yellow-Green, and Brown are added.";
            gameTextAry[1][3] = "Level 15. Now let's try combining everything you've learned so far.";
            gameTextAry[1][4] = "Level 20. It's looking a little shady in here.\nDark-Blue, Grey, Dark-Red, Light-Green, and Pink are added.";
            gameTextAry[1][5] = "Level 25. Can you handle all the colors?";
            gameTextAry[1][6] = "Level 30. Is it getting faster in here?\n\nSmall increase of speed, can you keep up?";
            gameTextAry[1][7] = "Level 35. Speed is increased again.\n\nGood luck!";
            gameTextAry[1][8] = "Level 40. More speed, Captain!";
            gameTextAry[1][9] = "Level 45. Speed is doubled.";
            gameTextAry[1][10] = "Level 50. Speed at maximum.";


            // Hard settings
            gameTextAry[2][0] = "Level 1.  You will start out with 4 colors; red, green, blue, and yellow.\n\nGet 15 blocks correct and move on to the next level.";
            gameTextAry[2][1] = "Level 5.  Two more colors are added, black and white.\n10 blocks are needed to get to the next level.  Can  you handle it?";
            gameTextAry[2][2] = "Level 10. Are you ready for some color mixing?\nOrange, Purple, Cyan, Yellow-Green, and Brown are added.";
            gameTextAry[2][3] = "Level 15. Now let's try combining everything you've learned so far.";
            gameTextAry[2][4] = "Level 20. It's looking a little shady in here.\nDark-Blue, Grey, Dark-Red, Light-Green, and Pink are added.";
            gameTextAry[2][5] = "Level 25. Can you handle all the colors?";
            gameTextAry[2][6] = "Level 30. Is it getting faster in here?\n\nSmall increase of speed, can you keep up?";
            gameTextAry[2][7] = "Level 35. Speed is increased again.\n\nGood luck!";
            gameTextAry[2][8] = "Level 40. More speed, Captain!";
            gameTextAry[2][9] = "Level 45. Speed is doubled.";
            gameTextAry[2][10] = "Level 50. Speed at maximum.";

            //Easy max blocks before level change
            //maxBlocks[0] = new int[11] {6, 10, 15, 20, 20, 20, 20, 20, 20, 20, 20};

            maxBlocks[0] = new int[11] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

            //Normal max blocks before level change
            maxBlocks[1] = new int[11] {10, 15, 20, 25, 25, 25, 25, 25, 25, 25, 25};

            //Hard max blocks before level change
            maxBlocks[2] = new int[11] {15, 15, 20, 25, 25, 25, 25, 25, 30, 30, 30};
            
            //Easy block speeds
            speed[0] = new double[11] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.1, 1.3, 1.5, 2.0, 2.5 };

            //Normal block speeds
            speed[1] = new double[11] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.1, 1.3, 1.5, 2.0, 2.5, 2.7 };

            //Hard block speeds
            speed[2] = new double[11] { 1.0, 1.0, 1.0, 1.0, 1.1, 1.3, 1.5, 1.7, 2.0, 2.5, 3.0 };

            //Difficulty Color Block groups
            maxColors[0] = 4;
            maxColors[1] = 6;
            maxColors[2] = 10;
            maxColors[3] = 10;
            maxColors[4] = 16;
            maxColors[5] = 16;
            maxColors[6] = 16;
            maxColors[7] = 16;
            maxColors[8] = 16;
            maxColors[9] = 16;
            maxColors[10] = 16;

            //Initial Lives
            initLives[0] = 5; //Easy
            initLives[1] = 3; //Normal
            initLives[2] = 1; //Hard

            //Levels per group
            maxLevelInGroup[0] = 5;
            maxLevelInGroup[1] = 10;
            maxLevelInGroup[2] = 15;
            maxLevelInGroup[3] = 20;
            maxLevelInGroup[4] = 25;
            maxLevelInGroup[5] = 30;
            maxLevelInGroup[6] = 35;
            maxLevelInGroup[7] = 40;
            maxLevelInGroup[8] = 45;
            maxLevelInGroup[9] = 50;
            maxLevelInGroup[10] = 99999;

     }

        protected void Reset()
        {
            Init();
            player1.Reset(initLives[difficulty]);
            scorePlayer1.Reset();
            colorBlock.setSpeed(speed[difficulty][0]);
            colorBlock.putInStartPosition();
            gameText.SetTextItems(gameTextAry[difficulty]);
            gameText.displayItem = 0;
            level = 1;
            blocks = 0;
            levelgroup = 0;

            paused = false;
            levelover = true;
       }

       public int Difficulty
       {
           get { return difficulty; }
           set { difficulty = value; }
       }

        /// <summary>
        /// Show the action scene
        /// </summary>
        public override void Show()
        {
            MediaPlayer.Play(audio.BackMusic);

            Init();

            Reset();

            Game1.LoadHighScores();

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
            MediaPlayer.Stop();
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
                    MediaPlayer.Pause();
                }
                else
                {
                    MediaPlayer.Resume();
                }
            }
        }

        public bool LevelOver
        {
            get { return levelover; }
            set { levelover = value; }
        }

        private void HandleLevels()
        {
            blocks++;
            if (blocks == maxBlocks[difficulty][levelgroup])
            {
                level++;
                if (level == maxLevelInGroup[levelgroup])
                {
                    levelgroup++;
                    levelover = true;
                    gameText.showText = true;
                    gameText.displayItem = levelgroup;
                    colorBlock.setSpeed(speed[difficulty][levelgroup]);
                }
                blocks = 0;
                player1.Level = level;
                player1.Lives++;
            }
 
            // Randomly pick the block color
            if (levelgroup == 2)
            {
                curBlockColor = colors[ran.Next(6, maxColors[levelgroup])];
            }
            else if (levelgroup == 4)
            {
                curBlockColor = colors[ran.Next(11, maxColors[levelgroup])];
            }
            else
                curBlockColor = colors[ran.Next(0, maxColors[levelgroup])];

            colorBlock.BlockColor = curBlockColor;
            colorBlock.putInStartPosition();
        }

        /// <summary>
        /// Handle collision with the block
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
                player.Reset(initLives[difficulty]);
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (levelover)
                gameText.showText = true;

            if ((!paused) && (!levelover) && (!gameOver))
            {
                // Check collisions with paddle
                HandleDamages();

                // Update score
                scorePlayer1.Value = player1.Score;
                scorePlayer1.Lives = player1.Lives;
                scorePlayer1.Level = player1.Level;

                // Clear level text
                gameText.showText = false;

                // Check if player is dead
                gameOver = ((player1.Lives == 0));
                if (gameOver)
                {
                    //player1.Visible = (player1.Score > 0);
                    //Save score
                    string player = SignedInGamer.SignedInGamers[PlayerIndex.One] == null ? "Guest" : SignedInGamer.SignedInGamers[PlayerIndex.One].Gamertag;
                    Game1.SaveHighScore(player, player1.Level, player1.Score, true);
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