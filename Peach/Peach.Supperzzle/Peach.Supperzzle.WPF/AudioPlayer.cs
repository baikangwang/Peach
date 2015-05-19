using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pean.Supperzzle.WPF
{
    using System.IO;
    using System.Media;
    using System.Threading;

    using Pean.Supperzzle.WPF.Properties;

    public class AudioPlayer
    {
        private static AudioPlayer _pass = new AudioPlayer(Resources.pass);
        private static AudioPlayer _fail=new AudioPlayer(Resources.fail);
        private static AudioPlayer _finish=new AudioPlayer(Resources.complete);

        private System.Media.SoundPlayer _player;


        public static AudioPlayer Pass
        {
            get
            {
                return _pass;
            }
        }

        public static AudioPlayer Fail
        {
            get
            {
                return _fail;
            }
        }

        public static AudioPlayer Finish
        {
            get
            {
                return _finish;
            }
        }

        protected AudioPlayer(Stream file)
        {
            this._player=new SoundPlayer(file);
        }

        public void Load()
        {
            this._player.Load();
            while (!this._player.IsLoadCompleted)
            {
                Thread.Sleep(0);
            }
        }

        public virtual void Play()
        {
            this._player.Play();
        }

        public virtual void Stop()
        {
            this._player.Stop();
        }
    }
}
