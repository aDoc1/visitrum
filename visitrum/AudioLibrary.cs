using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Visitrum
{
    public class AudioLibrary
    {
        private SoundEffect menuBack;
        private SoundEffect menuSelect;
        private SoundEffect menuScroll;
        private Song backMusic;
        //private Song startMusic;

      

        public SoundEffect MenuBack
        {
            get { return menuBack; }
        }

        public SoundEffect MenuSelect
        {
            get { return menuSelect; }
        }

        public SoundEffect MenuScroll
        {
            get { return menuScroll; }
        }

        public Song BackMusic
        {
            get { return backMusic; }
        }

        //public Song StartMusic
        //{
            //get { return startMusic; }
        //}

        public void LoadContent(ContentManager Content)
        {
            backMusic = Content.Load<Song>("backMusic");
            //startMusic = Content.Load<Song>("startMusic");
            menuBack = Content.Load<SoundEffect>("menu_back");
            menuSelect = Content.Load<SoundEffect>("menu_select3");
            menuScroll = Content.Load<SoundEffect>("menu_scroll");
        }
    }
}
