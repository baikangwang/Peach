using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pean.Supperzzle.WPF.Controls
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public enum State
    {
        Inverting,
        Inverted,
        Normal
    }

    public delegate Chess ChessSearchingEventHandler(object sender, EventArgs e);

    public delegate bool ChessCheckingEventHanlder(object sender, EventArgs e);

    /// <summary>
    /// Interaction logic for Chess.xaml
    /// </summary>
    public partial class Chess : UserControl
    {
        #region Field
        private static object _locker = new object();

        private TaskScheduler _taskSchduler;

        private State _state;

        public State State
        {
            get
            {
                lock (_locker)
                {
                    return this._state;
                }
            }
            private set
            {
                lock (_locker)
                {
                    this._state = value;
                }
            }
        }

        private Timer _downCounter;

        private ImageBrush _forebg;

        private ImageBrush _foreImg;

        private ImageBrush _puzzleBg;

        //private byte[] _forebg;

        //private byte[] _foreImg;

        //private byte[] _puzzleBg;

        #endregion

        #region Property

        public int Token { get; private set; }

        public int Id { get; private set; }

        public static int DOWNCOUNT = 5;

        #endregion

        #region Event
        public event ChessSearchingEventHandler SearchingOtherChess;

        public event ChessCheckingEventHanlder CheckingFinished;

        public event EventHandler ComparedFail;

        public event EventHandler ComparedSuccess;

        public event EventHandler Finished;
        #endregion

        public Chess()
        {
            this.InitializeComponent();
            this.State = State.Normal;
            this._downCounter  = new Timer((state) => { this.ChangeToNormal(); }, null, -1, -1);
            this._taskSchduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        #region Init

        public void Init(int id, int token, byte[] forebg, byte[] foreImg, byte[] puzzlebg)
        {
            this._forebg = this.ConvertToImage(forebg);
            this._foreImg = this.ConvertToImage(foreImg);
            this._puzzleBg = this.ConvertToImage(puzzlebg);
            this.Token = token;
            this.Id = id;
            this.btflag.Background = this._forebg;
        }

        private ImageBrush ConvertToImage(byte[] binary)
        {
            MemoryStream ms = new MemoryStream(binary, 0, binary.Length);
            ImageBrush image = new ImageBrush();
            var imgSource = new BitmapImage();
            imgSource.BeginInit();
            imgSource.StreamSource = ms;
            imgSource.EndInit();
            image.ImageSource = imgSource;
            return image;
        }

        public void Rest(int token, byte[] foreImg)
        {
            this.btflag.IsEnabled = false;
            this._foreImg = this.ConvertToImage(foreImg);
            this.Token = token;
            this.State = State.Normal;
            this.btflag.Background = this._forebg;
            this.btflag.IsEnabled = true;
        }

        public void Preview()
        {
            this.btflag.IsEnabled = false;
            this.btflag.Background = this._foreImg;
        }

        public void Ready()
        {
            this.btflag.Background = this._forebg;
            this.btflag.IsEnabled = true;
        }

        #endregion

        #region Convert State
        public void ChangeToInverted()
        {
            this.State = State.Inverted;

            Task.Factory.StartNew(
                () =>
                {
                    this.btflag.IsEnabled = false;
                    this.btflag.Background = this._puzzleBg;
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                this._taskSchduler);
        }

        public void ChangeToInverting()
        {
            this.State = State.Inverting;

            Task.Factory.StartNew(
                () =>
                {
                    this.btflag.IsEnabled = false;
                    this.btflag.Background = this._foreImg;
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                this._taskSchduler);

            this._downCounter.Change(TimeSpan.FromSeconds(DOWNCOUNT), TimeSpan.FromSeconds(DOWNCOUNT));

        }

        public void ChangeToNormal()
        {
            this.State = State.Normal;
            Task.Factory.StartNew(
                () =>
                {
                    this._downCounter.Change(-1, -1);
                }).ContinueWith(
                    task =>
                    {
                        this.btflag.Background = this._forebg;
                        this.btflag.IsEnabled = true;
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.None,
                    this._taskSchduler);
        }
        #endregion

        #region Timer
        public void ResetTimer()
        {
            this._downCounter.Change(TimeSpan.FromSeconds(DOWNCOUNT), TimeSpan.FromSeconds(DOWNCOUNT));
        }

        public void StopTimer()
        {
            this._downCounter.Change(-1, -1);
        }
        #endregion

        #region Game
        public bool RightRightTouch(Chess chess,Chess other)
        {
            return chess.Token == other.Token;
        }

        private void btflag_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeToInverting();

            Chess other = this.OnSearchingOtherChess();

            if (other == null) return;

            this.StopTimer();
            other.StopTimer();
            bool isSame = this.RightRightTouch(this, other);

            if (!isSame)
            {
                Task.Factory.StartNew(
                    () =>
                    {
                        this.ChangeToNormal();
                        other.ChangeToNormal();
                    });

                Task.Factory.StartNew(
                    this.OnComparedFail);
            }
            else
            {
                Task.Factory.StartNew(
                    () =>
                    {
                        this.ChangeToInverted();
                        other.ChangeToInverted();
                    }).ContinueWith(
                        (task) =>
                        {
                            bool isFinished = this.OnCheckingFinished();
                            if (isFinished)
                            {
                                this.OnFinished();
                            }
                            else
                            {
                                this.OnComparedSuccess();
                            }
                        });
            }
        }

        protected virtual Chess OnSearchingOtherChess()
        {
            var handler = this.SearchingOtherChess;
            if (handler != null)
            {
              return  handler(this, EventArgs.Empty);
            }
            return null;
        }

        protected virtual bool OnCheckingFinished()
        {
            var handler = this.CheckingFinished;
            if (handler != null)
            {
              return  handler(this, EventArgs.Empty);
            }
            return false;
        }

        protected virtual void OnComparedFail()
        {
            var handler = this.ComparedFail;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnComparedSuccess()
        {
            var handler = this.ComparedSuccess;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnFinished()
        {
            var handler = this.Finished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}
