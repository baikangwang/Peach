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

namespace Pean.Supperzzle.WPF
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Media;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += Main_Load;
            this.Closing += Main_Closing;
            Task.Factory.StartNew(
                () =>
                {
                    AudioPlayer.Pass.Load();
                    AudioPlayer.Fail.Load();
                    AudioPlayer.Finish.Load();
                });
        }

        private void Main_Closing(object sender, CancelEventArgs e)
        {
            //Task.Factory.StartNew(
            //    () =>
            //    {
            //        this.StopSound();
            //    });
        }

        private void Main_Load(object sender, RoutedEventArgs e)
        {
            //Task.Factory.StartNew(
            //    () =>
            //    {
            //        //play sound
            //        this.PlaySound();
            //    });
            this.lblVersion.Content = string.Format("Version: {0}", this.GetVersion());
        }

        private string GetVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
            return String.Format("{0}.{1}", fvi.FileMajorPart, fvi.FileMinorPart);
        }

        private void PlaySound()
        {
            //AudioPlayer.Global.Play();
        }

        private void StopSound()
        {
            //AudioPlayer.Global.Stop();
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            fmGame game = new fmGame();
            game.ShowDialog();
            this.Show();
        }

        private void btOptions_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            fmConfig config = new fmConfig();
            config.ShowDialog();
            this.Show();
        }

        private void btQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
