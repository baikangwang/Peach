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
using System.Windows.Shapes;

namespace Pean.Supperzzle.WPF
{
    using System.Collections.Specialized;
    using System.Threading;
    using System.Threading.Tasks;
    using Pean.Supperzzle.WPF.Controls;

    /// <summary>
    /// Interaction logic for fmGame.xaml
    /// </summary>
    public partial class fmGame : Window
    {
        public class Sound
        {
            public const string FAIL = "Fail";

            public const string SUCCESS = "Success";

            public const string FINISHED = "Finished";
        }

        private Options _options;

        private ChessMaxtrix _maxtrix;

        private IList<ChessFore> _chessfs;

        private TaskScheduler _taskScheduler;

        public fmGame()
        {
            this.InitializeComponent();
#if DEBUG
            this.Topmost = false;
#else
            this.Topmost = true;
#endif
            this.Loaded += this.Game_Load;
            this.HideButtons();
            this._maxtrix = new ChessMaxtrix();
            this._maxtrix.Margin=new Thickness(40);
            Grid.SetColumn(this._maxtrix,0);
            Grid.SetRow(this._maxtrix, 0);
            this.panel.Children.Add(this._maxtrix);
            this._maxtrix.ComparedFail += this.Maxtrix_ComparedFail;
            this._maxtrix.ComparedSuccess += this.Maxtrix_ComparedSuccess;
            this._maxtrix.Finished += this.Maxtrix_Finished;
            this._taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        }

        private void Maxtrix_ComparedSuccess(object sender, EventArgs e)
        {
            Task.Factory.StartNew(
                () =>
                {
                    this.PlaySound(Sound.SUCCESS);
                }
                //,
                //CancellationToken.None,
                //TaskCreationOptions.None,
                //this._taskScheduler
                );
        }

        private void Maxtrix_ComparedFail(object sender, EventArgs e)
        {
            Task.Factory.StartNew(
                () =>
                {
                    this.PlaySound(Sound.FAIL);
                }
                //,
                //CancellationToken.None,
                //TaskCreationOptions.None,
                //this._taskScheduler
                );
        }

        private void Maxtrix_Finished(object sender, EventArgs e)
        {
            Task.Factory.StartNew(
                () =>
                {
                    this.PlaySound(Sound.FINISHED);
                }
                //,
                //CancellationToken.None,
                //TaskCreationOptions.None,
                //this._taskScheduler
                );

            Task.Factory.StartNew(
                this.ShowButtons,
                CancellationToken.None,
                TaskCreationOptions.None,
                this._taskScheduler);
        }

        private void Game_Load(object sender, RoutedEventArgs e)
        {
            this.Game(true);
        }

        private void Game(bool isfirst)
        {
            Task.Factory.StartNew(
                () =>
                {
                    // hidden Restart & Quit button
                    this.HideButtons();
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                this._taskScheduler);

            Task.Factory.StartNew(() => {if(!isfirst) this.Ready(); },CancellationToken.None, TaskCreationOptions.None, this._taskScheduler)
                .ContinueWith(task=>{if(!isfirst) Thread.Sleep(1000);})
                .ContinueWith(task=>
                {
                    if (isfirst)
                    {
                        // Load supperzzle options
                        this._options = this.LoadOptions();
                        this._chessfs = this.InitChessFores(this._options.ForeImages);
                    }
                    // Random foreimages
                    IList<ChessFore> chessFores = this.RandomForeImages(this._chessfs);
                    return chessFores;
                }).ContinueWith(
                    task =>
                    {
                        IList<ChessFore> chessFores = task.Result;

                        if (isfirst)
                        {
                            // init maxtrix
                            this.Init(chessFores, this._options);
                        }
                        else
                            this.Rest(chessFores);

                        // Preview
                        this.Preview();
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.None,
                    this._taskScheduler).ContinueWith(
                        task =>
                        {
                            // release contorl
                            Thread.Sleep(0);
                        }).ContinueWith(
                            task =>
                            {
                                // Show loading panel
                                this.ShowLoadingPanel();
                            },
                            CancellationToken.None,
                            TaskContinuationOptions.None,
                            this._taskScheduler).ContinueWith(task => { Thread.Sleep(0); }).ContinueWith(
                                task =>
                                {
                                    // Show CountDown
                                    this.ShowCountDown();
                                }).ContinueWith(
                                    task =>
                                    {
                                        // hide loading panel
                                        this.HideLoadingPanel();
                                    },
                                    CancellationToken.None,
                                    TaskContinuationOptions.None,
                                    this._taskScheduler).ContinueWith(task => { Thread.Sleep(0); }).ContinueWith(
                                        task =>
                                        {
                                            // Ready
                                            this.Ready();
                                        },
                                        CancellationToken.None,
                                        TaskContinuationOptions.None,
                                        this._taskScheduler);
        }

        private void btRestart_Click(object sender, RoutedEventArgs e)
        {
            this.Game(false);
        }

        private void btQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private IList<ChessFore> InitChessFores(IList<byte[]> foreImages)
        {
            IList<ChessFore> chessfs = new List<ChessFore>();
            for (int i = 0; i < foreImages.Count; i++)
            {
                byte[] foreImage = foreImages[i];
                ChessFore chessf1 = new ChessFore(foreImage, i);
                ChessFore chessf2 = new ChessFore(foreImage, i);
                chessfs.Add(chessf1);
                chessfs.Add(chessf2);
            }

            return chessfs;
        }

        private IList<ChessFore> RandomForeImages(IList<ChessFore> chessFores)
        {
            IList<int> ids = chessFores.Select((f, i) => i).ToList();
            IList<ChessFore> random = new List<ChessFore>();

            int j = 0;
            while (random.Count != chessFores.Count)
            {
                int min = 0;
                int max = ids.Count - 1;
                long seed = DateTime.Now.Ticks;
                Random r = new Random((int)seed);
                int next = r.Next(min, max);
                int id = ids[next];
                ChessFore chessf = chessFores[j];
                chessf.Id = id;
                random.Add(chessf);
                ids.RemoveAt(next);
                j++;
            }

            return random;
        }

        private Options LoadOptions()
        {
            Options options = new Options(Setting.INFO);

            return options;
        }

        private void HideLoadingPanel()
        {
            this.pnDownCount.Visibility = Visibility.Hidden;
        }

        private void ShowLoadingPanel()
        {
            this.pnDownCount.Visibility = Visibility.Visible;
        }

        private void Init(IList<ChessFore> chessfs, Options options)
        {
            int columns = (int)options.Size.Width;
            int rows = (int)options.Size.Height;
            byte[] chessBg = options.ForeBackground;
            IList<byte[]> puzzleBg = options.PuzzleBackground;
            this._maxtrix.Init(columns, rows, chessfs, chessBg, puzzleBg);
        }

        private void ShowCountDown()
        {
#if DEBUG
            int count = 3;
#else
            int count = this._options.PreparingPreoid;
#endif
            while (count >0)
            {
                int number = count;
                Task.Factory.StartNew(
                    () =>
                    {
                        this.lblDownCount.Content = number.ToString();
                    },
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    this._taskScheduler);
                count--;
                Thread.Sleep(1000);
            }

            Task.Factory.StartNew(
                () =>
                {
                    this.lblDownCount.Content = "Ready";
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                this._taskScheduler);

            Thread.Sleep(1500);

            Task.Factory.StartNew(
                () =>
                {
                    this.lblDownCount.Content = "Go";
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                this._taskScheduler);

            Thread.Sleep(1500);

            Task.Factory.StartNew(
                () =>
                {
                    this.lblDownCount.Content = string.Empty;
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                this._taskScheduler);
        }

        private void Ready()
        {
            this._maxtrix.Ready();
        }

        private void Preview()
        {
            this._maxtrix.Preview();
        }

        private void Rest(IList<ChessFore> chessfs)
        {
            this._maxtrix.Rest(chessfs);
        }

        private void PlaySound(string sound)
        {
            switch (sound)
            {
                case Sound.FAIL:
                    AudioPlayer.Fail.Play();
                    break;
                case Sound.SUCCESS:
                    AudioPlayer.Pass.Play();
                    break;
                case Sound.FINISHED:
                    AudioPlayer.Finish.Play();
                    break;
            }
        }

        private void ShowButtons()
        {
            this.pnButtons.Visibility=Visibility.Visible;
        }

        private void HideButtons()
        {
            this.pnButtons.Visibility = Visibility.Hidden;
        }
    }
}
