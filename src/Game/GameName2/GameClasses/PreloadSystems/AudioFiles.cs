//Läd alle audiofiles für das Spiel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BloodyPlumber
{
    public class AudioFiles
    {
        public List<SoundEffect> marioKillSounds;

        public SoundEffect thisAintMario;

        public SoundEffect marioDied;

        public SoundEffectInstance menuTheme;

        private SoundEffect letsgo, options, highscore, exit;

        public SoundEffect ninini;

        public SoundEffect wuhu;

        public SoundEffect akShot;

        public SoundEffect laserShot;

        public SoundEffectInstance s_levelOneTheme;

        public SoundEffectInstance s_endBossTheme;

        public SoundEffectInstance s_thisAintMario, s_letsgo, s_options, s_highscore, s_exit;

        public void Initialize()
        {
            marioKillSounds = new List<SoundEffect>();
        }

        public void LoadContent(ScreenManager screenmanager)
        {
            loadMarioKillSounds(screenmanager);

            SoundEffect mtheme = screenmanager.Game.Content.Load<SoundEffect>("Menu Theme");
            menuTheme = mtheme.CreateInstance();
            menuTheme.Volume = 0.1f;
            menuTheme.IsLooped = true;

            SoundEffect lOneTheme = screenmanager.Game.Content.Load<SoundEffect>("LevelTheme");
            s_levelOneTheme = lOneTheme.CreateInstance();
            s_levelOneTheme.IsLooped = true;
            s_levelOneTheme.Volume = 0.8f;

            SoundEffect endBossTheme = screenmanager.Game.Content.Load<SoundEffect>("EndbossTheme");
            s_endBossTheme = endBossTheme.CreateInstance();
            s_endBossTheme.IsLooped = true;
            s_endBossTheme.Volume = 0.8f;

            loadMenuListItems(screenmanager);

            thisAintMario = screenmanager.Game.Content.Load<SoundEffect>("This aint Mario");
            s_thisAintMario = thisAintMario.CreateInstance();

            marioDied = screenmanager.Game.Content.Load<SoundEffect>("Autsch");

            wuhu = screenmanager.Game.Content.Load<SoundEffect>("Wuhu");

            ninini = screenmanager.Game.Content.Load<SoundEffect>("Ninini");

            #region Weapons
            akShot = screenmanager.Game.Content.Load<SoundEffect>("akShot");
            laserShot = screenmanager.Game.Content.Load<SoundEffect>("laserShot");
            #endregion
        }
        public void loadMenuListItems(ScreenManager screenmanager)
        {
            letsgo = screenmanager.Game.Content.Load<SoundEffect>("Ok lets go");
            s_letsgo = letsgo.CreateInstance();
            options = screenmanager.Game.Content.Load<SoundEffect>("Options");
            s_options = options.CreateInstance();
            highscore = screenmanager.Game.Content.Load<SoundEffect>("Lets see hows best");
            s_highscore = highscore.CreateInstance();
            exit = screenmanager.Game.Content.Load<SoundEffect>("Plastic Shit");
            s_exit = exit.CreateInstance();
        }

        private void loadMarioKillSounds(ScreenManager screenmanager)
        {
            SoundEffect fuckYou = screenmanager.Game.Content.Load<SoundEffect>("Audio\\Mario Kill Sounds\\Fuck You");
            SoundEffect sonOfABitch = screenmanager.Game.Content.Load<SoundEffect>("Audio\\Mario Kill Sounds\\You son of a bitch");
            SoundEffect gotcha = screenmanager.Game.Content.Load<SoundEffect>("Audio\\Mario Kill Sounds\\Gotcha");
            SoundEffect fuckingCunt = screenmanager.Game.Content.Load<SoundEffect>("Audio\\Mario Kill Sounds\\Fucking Cunt");

            marioKillSounds.Add(fuckYou);
            marioKillSounds.Add(sonOfABitch);
            marioKillSounds.Add(gotcha);
            marioKillSounds.Add(fuckingCunt);
        }

        public void stopMarioSounds()
        {
            s_thisAintMario.Stop();
            s_letsgo.Stop();
            s_options.Stop();
            s_highscore.Stop();
            s_exit.Stop();
        }

        public void startEndbossTheme()
        {
            s_levelOneTheme.Stop();
            s_endBossTheme.Play();
        }



    }
}
