#region Using Statements

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
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

#endregion


namespace Visitrum
{
    public struct Highscore
    {
        public string PlayerName;
        public int Level;
        public int Score;

        public Highscore(string p, int l, int s)
        {
            PlayerName = p;
            Level = l;
            Score = s;
        }

        // Copy constructor.
        public Highscore(Highscore prevHighScores)
        {
            PlayerName = prevHighScores.PlayerName;
            Level = prevHighScores.Level;
            Score = prevHighScores.Score;
        }

    }

    public class HighScoreTable
    {
        protected List<Highscore> highscores;
        protected List<Highscore> displayHighScores;
        protected StorageDevice device;
        protected IAsyncResult asyncResult;
        protected StorageContainer storageContainer;
        protected string containername = "Highscores";
        protected string filename = "Highscores.xml";
        protected PlayerIndex playerIndex = PlayerIndex.One;
        protected int highScoreIndex = 0;

        public HighScoreTable()
        {
            if (highscores == null)
                highscores = new List<Highscore>();
            if (displayHighScores == null)
                displayHighScores = new List<Highscore>();
        }

        public HighScoreTable(StorageDevice dev)
        {
            device = dev;
        }

        //public Game1 Game1 { get { return (Game1)Game; } }
        public void prepareForDisplay(int numScores)
        {
            displayHighScores.Clear();
            for (int i = 0; i < highscores.Count; i++)
            {
                displayHighScores.Add(new Highscore(highscores[i]));
            }
            int prevScores = displayHighScores.Count;
            for (int i = prevScores - 1; i < prevScores + numScores; i++)
            {
                displayHighScores.Add(new Highscore("-----------", 0, 0));
            }
        }

        public List<Highscore> getDisplayHighScores()
        {
            return displayHighScores;
        }

        public List<Highscore> getHighScores()
        {
            return highscores;
        }

        public int getHighScoreIndex()
        {
            return highScoreIndex;
        }

        public void ClearHighScore()
        {
            highscores.Clear();
        }

        public void SetHighScores(List<Highscore> hs)
        {
            if (highscores == null)
                highscores = new List<Highscore>();

            highscores = hs;
        }

        public void AddHighScore(string player, int level, int score)
        {
            // highscores list is sorted from high to low.  Find the index to insert the new score.
            int scoreIndex = highscores.Count;
            for (int i = 0; i < highscores.Count; i++)
            {
                if (score > highscores[i].Score || (score == highscores[i].Score && level > highscores[i].Level))
                {
                    scoreIndex = i;
                    break;
                }
            }
            highscores.Insert(scoreIndex, new Highscore(player, level, score));
            highScoreIndex = scoreIndex;
        }
    }
}