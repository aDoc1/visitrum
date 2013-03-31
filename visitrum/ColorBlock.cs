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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements the rocks the player must avoid.
    /// </summary>
    public class ColorBlock : DrawableGameComponent
    {
        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        protected double Yspeed;
        protected double Xspeed;
        protected SpriteBatch sBatch;
        protected double speedMultiplyer = 1.0;

        //Width and height of sprites
        protected const int BLOCKWIDTH = 52;
        protected const int BLOCKHEIGHT = 51;

        protected Color currentColor;


        public ColorBlock(Game game, ref Texture2D theTexture, Color blockColor)
            : base(game)
        {
            texture = theTexture;
            position = new Vector2();

            currentColor = blockColor;

            // Get the current sprite batch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            spriteRectangle = new Rectangle(2, 53, BLOCKWIDTH, BLOCKHEIGHT);

            //Initialize the random number generator and put the meteor in its starting position
            //ran = new Random(this.GetHashCode());
            putInStartPosition();
        }

        public Color BlockColor
        {
            get { return currentColor; }
            set { currentColor = value; }
        }

        public void setSpeed(double spdMult)
        {
            speedMultiplyer = spdMult;
            Yspeed = 5 * speedMultiplyer;
            Xspeed = 0;
        }


        //Block starting positions and velocity
        public void putInStartPosition()
        {
            position.X = (Game.Window.ClientBounds.Width - BLOCKWIDTH) / 2;
            position.Y = -80;
        }

        /// <summary>
        /// Allows the game component to draw your content in the game screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //Draw the meteor
            sBatch.Draw(texture, position, spriteRectangle, currentColor);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Move block
            position.Y += (float)Yspeed;
            position.X += (float)Xspeed;

            base.Update(gameTime);
        }

        /// <summary>
        /// Check if the block interesects with the specified rectangle
        /// </summary>
        public bool CheckCollision(Rectangle rect)
        {
            Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, BLOCKWIDTH, BLOCKHEIGHT);

            return spriterect.Intersects(rect);
        }
    }
}