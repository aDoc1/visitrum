using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using EasyStorage;
//using Highscores2;


namespace Visitrum
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public IAsyncSaveDevice saveDevice;

        //Textures
        private Texture2D rainbow_background, colorblockTexture, paddleTexture; //randomizer;
        protected Texture2D helpBackgroundTexture, helpForegroundTexture;
        protected Texture2D startBackgroundTexture, startElementsTexture;
        protected Texture2D actionElementsTexture, actionBackgroundTexture;
        protected Texture2D livesSprite;
        protected Texture2D boxColor;
        protected Texture2D BButton, AButton, XButton, YButton, leftTrigger, RightTrigger;

        // Fonts
        private SpriteFont smallFont, largeFont, scoreFont, levelTextFont;

        //Game Text
        protected GameText gameText;

        // Game Scenes
        protected HelpScene helpScene;
        protected StartScene startScene;
        protected DifficultyScene difficultyScene;
        protected ActionScene actionScene;
        protected GameScene activeScene;
        protected HighScoreScene highscoreScene;
        protected PauseScene pauseScene;

        //Game states
        //private GamePadState gamepadstatus;
        //private KeyboardState keyboardstatus;

        // Used for handle input
        protected KeyboardState oldKeyboardState;
        protected GamePadState oldGamePadState;

        // Audio Stuff
        private AudioLibrary audio;
        //private Song backMusic;

        // High score stuff
        public HighScoreTable highScores;
        public int scoreIndex;
        public bool gameIsOver = false;
        protected bool highscoreIsLoaded = false;
        protected string containername = "Highscores";
        protected string filename = "Highscores.xml";

        private GameScene _savedScene;

        //private SpriteFont gameFont;

        //private SimpleRumblePad rumblePad;

        //private Random ran;

        public GameScene SavedScene
        {
            get { return _savedScene; }
            set { _savedScene = value; }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Used for input handling
            oldKeyboardState = Keyboard.GetState();
            oldGamePadState = GamePad.GetState(PlayerIndex.One);

#if XBOX360
    // On the 360, we are always fullscreen and we always render to the user's 
    // prefered resolution
            graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            // We also get multisampling essentially for free on the 360, 
            // so turn it on
            graphics.PreferMultiSampling = true;
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //rumblePad = new SimpleRumblePad(this);
            //Components.Add(rumblePad);
            //ran = new Random(this.GetHashCode());

            // create and add our SaveDevice
            SharedSaveDevice sharedSaveDevice = new SharedSaveDevice();
            Components.Add(sharedSaveDevice);

            // make sure we hold on to the device
            saveDevice = sharedSaveDevice;

            // hook two event handlers to force the user to choose a new device if they cancel the
            // device selector or if they disconnect the storage device after selecting it
            sharedSaveDevice.DeviceSelectorCanceled += (s, e) => e.Response = SaveDeviceEventResponse.Force;
            sharedSaveDevice.DeviceDisconnected += (s, e) => e.Response = SaveDeviceEventResponse.Force;

            // prompt for a device on the first Update we can
            sharedSaveDevice.PromptForDevice();
#if XBOX
			// add the GamerServicesComponent
			Components.Add(new Microsoft.Xna.Framework.GamerServices.GamerServicesComponent(this));
#endif

            // hook an event so we can see that it does fire
            saveDevice.SaveCompleted += new SaveCompletedEventHandler(saveDevice_SaveCompleted);

            // Create the Highscore instance
            highScores = new HighScoreTable();
            scoreIndex = 0;

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

            //Add the SpriteBatch service
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            // Load Audio Elements
            audio = new AudioLibrary();
            audio.LoadContent(Content);
            Services.AddService(typeof(AudioLibrary), audio);

            rainbow_background = Content.Load<Texture2D>("rainbow_background");
            paddleTexture = Content.Load<Texture2D>("paddleTexture");
            colorblockTexture = Content.Load<Texture2D>("colorblockTexture");
            boxColor = Content.Load<Texture2D>("startbackground");
            //randomizer = Content.Load<Texture2D>("randomizer");
            BButton = Content.Load<Texture2D>("xboxControllerButtonB");
            AButton = Content.Load<Texture2D>("xboxControllerButtonA");
            XButton = Content.Load<Texture2D>("xboxControllerButtonX");
            YButton = Content.Load<Texture2D>("xboxControllerButtonY");
            leftTrigger = Content.Load<Texture2D>("xboxControllerLeftTrigger");
            RightTrigger = Content.Load<Texture2D>("xboxControllerRightTrigger");
     
            // Create the Start Scene
            smallFont = Content.Load<SpriteFont>("menuSmall");
            largeFont = Content.Load<SpriteFont>("menuLarge");
            
#if XBOX360
            startBackgroundTexture = Content.Load<Texture2D>("startbackground360");
            startElementsTexture = Content.Load<Texture2D>("startSceneElements");

#else
            startBackgroundTexture = Content.Load<Texture2D>("startbackground");
            startElementsTexture = Content.Load<Texture2D>("startSceneElements");

#endif
            startScene = new StartScene(this, smallFont, largeFont,
                startBackgroundTexture, startElementsTexture);
            Components.Add(startScene);
            this.SavedScene = startScene;

            // Create the Difficulty Scene
            smallFont = Content.Load<SpriteFont>("menuSmall");
            largeFont = Content.Load<SpriteFont>("menuLarge");

            startElementsTexture = Content.Load<Texture2D>("startSceneElements");
            difficultyScene = new DifficultyScene(this, smallFont, largeFont,
                startBackgroundTexture, startElementsTexture);
            Components.Add(difficultyScene);

            // Create the Action Scenes
            actionElementsTexture = Content.Load<Texture2D>("VisitrumElements");
            actionBackgroundTexture = Content.Load<Texture2D>("rainbow_background");
            //livesSprite = Content.Load<Texture2D>("lives sprite");
#if XBOX360     
            scoreFont = Content.Load<SpriteFont>("score360");
            levelTextFont = Content.Load<SpriteFont>("score360");
#else
            scoreFont = Content.Load<SpriteFont>("score");
            levelTextFont = Content.Load<SpriteFont>("levelText");
#endif

            //livesFont = Content.Load<SpriteFont>("Lives font");
            //actionScene = new ActionScene(this, actionElementsTexture,
            //    actionBackgroundTexture, livesSprite, scoreFont, livesFont);
            actionScene = new ActionScene(this, actionElementsTexture, actionBackgroundTexture, scoreFont, boxColor, levelTextFont);
            Components.Add(actionScene);

            highscoreScene = new HighScoreScene(this, startBackgroundTexture, scoreFont, largeFont);
            Components.Add(highscoreScene);

            pauseScene = new PauseScene(this, smallFont, largeFont, startBackgroundTexture, startElementsTexture);
            Components.Add(pauseScene);

            // Start the game in the start Scene :)
            startScene.Show();
            activeScene = startScene;
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
        /// Open a new scene
        /// </summary>
        /// <param name="scene">Scene to be opened</param>
        protected void ShowScene(GameScene scene)
        {
            activeScene.Hide();
            activeScene = scene;
            scene.Show();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Load the High Scores table
            LoadHighScores();

            // Handle Game Inputs
            HandleScenesInput();

            // Update everything else
            base.Update(gameTime);
        }

        /// <summary>
        /// Handle input of all game scenes
        /// </summary>
        private void HandleScenesInput()
        {
            // Handle Start Scene Input
            if (activeScene == startScene)
            {
                HandleStartSceneInput();
            }
            // Handle Difficulty Scene input
            else if (activeScene == difficultyScene)
            {
                HandleDifficultySceneInput();
            }
            // Handle Help Scene input
            else if (activeScene == helpScene)
            {
                if (CheckEnterA())
                {
                    ShowScene(startScene);
                }
            }
            // Handle Action Scene Input
            else if (activeScene == actionScene)
            {
                HandleActionInput();
            }
            // Handle Highscore Scene Input
            else if (activeScene == highscoreScene)
            {
                HandleHighScoreInput();
            }
            else if (activeScene == pauseScene)
            {
                HandlePauseSceneInput();
            }
        }

        /// <summary>
        /// Check if the Enter Key or 'A' button was pressed
        /// </summary>
        /// <returns>true, if enter key or 'A' button was pressed</returns>
        private bool CheckEnterA()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool result = (oldKeyboardState.IsKeyDown(Keys.Enter) &&
                (keyboardState.IsKeyUp(Keys.Enter)));
            result |= (oldGamePadState.Buttons.A == ButtonState.Pressed) &&
                      (gamepadState.Buttons.A == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            return result;
        }

        /// <summary>
        /// Check if the Escape or back button was pressed
        /// </summary>
        /// <returns>true, if escape key or back button are pressed.
        private void HandleActionInput()
        {
            // Get the Keyboard and GamePad state
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool backKey = (oldKeyboardState.IsKeyDown(Keys.Escape) &&
                (keyboardState.IsKeyUp(Keys.Escape)));
            backKey |= (oldGamePadState.Buttons.Back == ButtonState.Pressed) &&
                       (gamepadState.Buttons.Back == ButtonState.Released);

            bool enterKey = (oldKeyboardState.IsKeyDown(Keys.Enter) &&
                (keyboardState.IsKeyUp(Keys.Enter)));
            enterKey |= (oldGamePadState.Buttons.Start == ButtonState.Pressed) &&
                        (gamepadState.Buttons.Start == ButtonState.Released);

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;

            if (enterKey)
            {
                if (actionScene.GameOver)
                {
                    ShowScene(highscoreScene);
                    gameIsOver = true;
                    //ShowScene(startScene);
                }
                else
                {
                    if (actionScene.LevelOver)
                        actionScene.LevelOver = false;
                    else
                    {
                        //audio.MenuBack.Play();
                        actionScene.Paused = !actionScene.Paused;
                        if (actionScene.Paused)
                        {
                            //actionScene.ShowPauseMenu(this, levelTextFont);
                            this.SavedScene = actionScene;
                            ShowScene(pauseScene);
                        }
                    }
                }
            }

            if (backKey)
            {
                ShowScene(startScene);
            }
        }

        /// <summary>
        /// Handle buttons and keyboard in StartScene
        /// </summary>
        private void HandleStartSceneInput()
        {
            if (CheckEnterA())
            {
                //audio.MenuSelect.Play();
                switch (startScene.SelectedMenuIndex)
                {
                    case 0:
                        ShowScene(difficultyScene);
                        break;
                    case 1:
                        this.SavedScene = startScene;
                        ShowScene(highscoreScene);
                        break;
                    case 2:
                        //ShowScene(helpScene);
                        break;
                    case 3:
                        Exit();
                        break;
                }
            }
        }

        //public Texture2D getButtonTexture(Texture2D buttonTexture)
        //{
        //}

        private void HandleDifficultySceneInput()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            if (CheckEnterA())
            {
                //audio.MenuSelect.Play();
                switch (difficultyScene.SelectedMenuIndex)
                {
                    //easy
                    case 0:
                        actionScene.Difficulty = 0;
                        ShowScene(actionScene);
                        break;
                    //Normal
                    case 1:
                        actionScene.Difficulty = 1;
                        ShowScene(actionScene);
                        break;
                    //hard
                    case 2:
                        actionScene.Difficulty = 2;
                        ShowScene(actionScene);
                        break;
                    case 3:
                        Exit();
                        break;
                }
            }
            else if (gamepadState.Buttons.B == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            {
                ShowScene(startScene);
            }
        }

        private void HandlePauseSceneInput()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            if (CheckEnterA())
            {
                switch (pauseScene.SelectedMenuIndex)
                {
                    case 0:
                        ShowScene(actionScene);
                        break;
                    case 1:
                        this.SavedScene = pauseScene;
                        ShowScene(highscoreScene);
                        break;
                    case 2:
                        //ShowScene(helpScene);
                        break;
                    case 3:
                        ShowScene(startScene);
                        break;
                }
            }
        }

        private void HandleHighScoreInput()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            
            if (gamepadState.Buttons.X == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Space))
            {
                highScores.ClearHighScore();
                SaveHighScore("", 0, 0, false);
                ShowScene(highscoreScene);   
            }
            else if (gamepadState.Buttons.B == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            {
                gameIsOver = false;
                ShowScene(this.SavedScene);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(rainbow_background, new Rectangle(0, 0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height), Color.LightGray);
            spriteBatch.End();

            //Draw Sprites
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }

        void saveDevice_SaveCompleted(object sender, FileActionCompletedEventArgs args)
        {
            // just write some debug output for our verification
            Debug.WriteLine("SaveCompleted!");
        }

        public void SaveHighScore(string player, int level, int score, bool addPlayerScore)
        {
            // Insert the new high score at the position
            if(addPlayerScore)
                highScores.AddHighScore(player, level, score);

            if (saveDevice.IsReady)
            {
                // save a file asynchronously. this will trigger IsBusy to return true
                // for the duration of the save process.
                saveDevice.SaveAsync(
                    containername,
                    filename,
                    stream =>
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
                        serializer.Serialize(stream, highScores.getHighScores());
                    }
                );
            }

        }

        public void LoadHighScores()
        {
            if (saveDevice.IsReady && !highscoreIsLoaded)
            {
                if (saveDevice.FileExists(containername, filename))
                {
                    // save a file asynchronously. this will trigger IsBusy to return true
                    // for the duration of the save process.
                    saveDevice.LoadAsync(
                        containername,
                        filename,
                        stream =>
                        {
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                // Read the data from the file.
                                XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
                                List<Highscore> data = (List<Highscore>)serializer.Deserialize(stream);

                                highScores.SetHighScores(data);

                                // Report the data to the console.
                                foreach (Highscore hs in data)
                                {
                                    Debug.WriteLine("Player, Score, Level\n{0}, {1}, {2}",
                                        hs.PlayerName,
                                        hs.Score.ToString(),
                                        hs.Level.ToString());
                                }
                            }

                        }
                    );

                    highscoreIsLoaded = true;
                }
            }

        }

    }
}
