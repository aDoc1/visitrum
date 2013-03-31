#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Visitrum
{
    /// <summary>
    /// This is a game component thats represents the Instructions Scene
    /// </summary>
    public class HelpScene : GameScene
    {
        public HelpScene(Game game, Texture2D textureBack, Texture2D textureFront)
            : base(game)
        {
            Components.Add(new ImageComponent(game, textureBack,
                ImageComponent.DrawMode.Stretch));
            Components.Add(new ImageComponent(game, textureFront,
                ImageComponent.DrawMode.Center));
        }
    }
}