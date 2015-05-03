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

namespace Peach.Downloader.Ting56
{
    using System.Threading;
    using System.Threading.Tasks;

    using Peach.Downloader.Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDictionary<int, TreeViewItem> _headers;

        private IDictionary<int, SortedList<int, TreeViewItem>> _seeds;

        private TaskScheduler _scheduler;

        private Label _label;
        
        public MainWindow()
        {
            InitializeComponent();
            PendingQueue.Default.Ready+=this.Default_Ready;
            PendingQueue.Default.StatusChanged += this.Default_StatusChanged;
            DownloadingQueue.Default.SeedCompleted += this.Default_SeedCompleted;
            DownloadingQueue.Default.SeedFail += this.Default_SeedFail;
            DownloadingQueue.Default.SeedStatusChanged += this.Default_SeedStatusChanged;
            this._scheduler=TaskScheduler.FromCurrentSynchronizationContext();
            this._label=new Label();
            this.StatusBar.Items.Add(this._label);
        }

        void Default_SeedStatusChanged(ISeed sender, int changed)
        {
            //change progress
            Task.Factory.StartNew(
                () =>
                {
                    this._seeds[sender.Chapter][sender.Episode].Header = string.Format("{0}-{1}-{2}%", sender.Title, sender.Status, changed);
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler);
        }

        void Default_SeedFail(ISeed sender)
        {
            // change to fail
            Task.Factory.StartNew(
                () =>
                {
                    this._seeds[sender.Chapter][sender.Episode].Header = string.Format("{0}-{1}", sender.Title, sender.Status);
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler);
        }

        void Default_SeedCompleted(ISeed sender)
        {
            //change to 100%
            //this.StatusBar.Items.Add(new status)
            Task.Factory.StartNew(
                () =>
                {
                    this._seeds[sender.Chapter][sender.Episode].Header = string.Format("{0}-{1}", sender.Title, sender.Status);
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler);

        }

        void Default_StatusChanged(object sender, string message)
        {
            //this.StatusBar.Items.Add(new status)
            Task.Factory.StartNew(
                () =>
                {
                    this._label.Content = message;
                },CancellationToken.None,TaskCreationOptions.None, this._scheduler);
        }

        private void Default_Ready(object sender, IList<ISeed> seeds)
        {
            Task.Factory.StartNew(
                () =>
                {
                    //draw tree view
                    this.tvSeeds.Items.Clear();

                    this._headers=new SortedDictionary<int, TreeViewItem>();
                    this._seeds=new SortedDictionary<int, SortedList<int,TreeViewItem>>();

                    foreach (ISeed seed in seeds)
                    {
                        int chapter = seed.Chapter;
                        int episode = seed.Episode;
                        if (!this._headers.ContainsKey(chapter))
                        {
                            this._headers.Add(chapter,new TreeViewItem(){Header = (seed as Seed).GetChapterName()});
                        }

                        if (!this._seeds.ContainsKey(chapter))
                        {
                            this._seeds.Add(chapter,new SortedList<int, TreeViewItem>());
                        }

                        if (!this._seeds[chapter].ContainsKey(episode))
                        {
                            TreeViewItem s = new TreeViewItem() { Header = string.Format("{0}-{1}", seed.Title, seed.Status) };
                            this._seeds[chapter].Add(episode,s);
                            this._headers[chapter].Items.Add(s);
                        }
                    }

                    foreach (int chapter in this._headers.Keys)
                    {
                        this.tvSeeds.Items.Add(this._headers[chapter]);
                    }
                },CancellationToken.None,TaskCreationOptions.None, this._scheduler).ContinueWith(
                    (task) =>
                    {
                        //start downloading queue
                        DownloadingQueue.Default.Start();
                    });
        }

        private void Transaction_Click(object sender, RoutedEventArgs e)
        {
            this.btTransaction.IsEnabled = false;
            
            bool isStarted = string.Compare(
                "stop",
                this.btTransaction.Content.ToString(),
                StringComparison.OrdinalIgnoreCase) == 0;

            if (isStarted)
            {

                PendingQueue.Default.Stop();
                DownloadingQueue.Default.Stop();
                this.btTransaction.Content = "Start";
            }
            else
            {
                Task.Factory.StartNew(() => { PendingQueue.Default.Start(); });
                this.btTransaction.Content = "Stop";
            }

            this.btTransaction.IsEnabled = true;

        }
    }
}
