﻿namespace Peach.Downloader.TingShu520
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

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
            PendingQueue.TingShu520.Ready += this.Default_Ready;
            PendingQueue.TingShu520.StatusChanged += this.Default_StatusChanged;
            DownloadingQueue.TingShu520.SeedCompleted += this.Default_SeedCompleted;
            DownloadingQueue.TingShu520.SeedFail += this.Default_SeedFail;
            DownloadingQueue.TingShu520.SeedStatusChanged += this.Default_SeedStatusChanged;
            DownloadingQueue.TingShu520.Finished += this.TingShu520_Finished;
            this._scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            this._label = new Label();
            this.StatusBar.Items.Add(this._label);
        }

        private void TingShu520_Finished(object sender, EventArgs e)
        {
            Task.Factory.StartNew(
                () =>
                {
                    DownloadingQueue.TingShu520.Stop();
                    PendingQueue.TingShu520.Stop();
                }).ContinueWith(
                    (task) =>
                    {
                        this.btTransaction.Content = "Start";
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.None,
                    this._scheduler);
        }

        void Default_SeedStatusChanged(ISeed sender, int changed)
        {
            //change progress
            Task.Factory.StartNew(
                () =>
                {
                    TreeViewItem item = this._seeds[sender.Chapter][sender.Episode];
                    item.Foreground = new SolidColorBrush(Colors.Blue);
                    item.Header = string.Format("{0}-{1}-{2}%", sender.Title, sender.Status, changed);
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler);
        }

        void Default_SeedFail(ISeed sender)
        {
            // change to fail
            Task.Factory.StartNew(
                () =>
                {
                    TreeViewItem item = this._seeds[sender.Chapter][sender.Episode];
                    item.Foreground = new SolidColorBrush(Colors.Red);
                    item.Header = string.Format("{0}-{1}", sender.Title, sender.Status);
                    Task.Factory.StartNew(() => { PendingQueue.TingShu520.RefreshCache(); });
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler);
        }

        void Default_SeedCompleted(ISeed sender)
        {
            //change to 100%
            //this.StatusBar.Items.Add(new status)
            Task.Factory.StartNew(
                () =>
                {
                    TreeViewItem item = this._seeds[sender.Chapter][sender.Episode];
                    item.Foreground = new SolidColorBrush(Colors.Green);
                    item.Header = string.Format("{0}-{1}", sender.Title, sender.Status);
                    Task.Factory.StartNew(() => { PendingQueue.TingShu520.RefreshCache(); });
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler);

        }

        void Default_StatusChanged(object sender, string message)
        {
            //this.StatusBar.Items.Add(new status)
            Task.Factory.StartNew(
                () =>
                {
                    this._label.Content = message;
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler);
        }

        private void Default_Ready(object sender, IList<ISeed> seeds)
        {
            Task.Factory.StartNew(
                () =>
                {
                    //draw tree view
                    this.tvSeeds.Items.Clear();

                    this._headers = new SortedDictionary<int, TreeViewItem>();
                    this._seeds = new SortedDictionary<int, SortedList<int, TreeViewItem>>();

                    foreach (ISeed seed in seeds)
                    {
                        int chapter = seed.Chapter;
                        int episode = seed.Episode;
                        if (!this._headers.ContainsKey(chapter))
                        {
                            this._headers.Add(chapter, new TreeViewItem() { Header = (seed as Seed).GetChapterName() });
                        }

                        if (!this._seeds.ContainsKey(chapter))
                        {
                            this._seeds.Add(chapter, new SortedList<int, TreeViewItem>());
                        }

                        if (!this._seeds[chapter].ContainsKey(episode))
                        {
                            TreeViewItem s = new TreeViewItem() { Header = string.Format("{0}-{1}", seed.Title, seed.Status) };
                            switch (seed.Status)
                            {
                                case Status.Complete:
                                    s.Foreground = new SolidColorBrush(Colors.Green);
                                    break;
                                case Status.Fail:
                                    s.Foreground = new SolidColorBrush(Colors.Red);
                                    break;
                                case Status.Waiting:
                                    s.Foreground = new SolidColorBrush(Colors.Silver);
                                    break;
                                case Status.Downloading:
                                    s.Foreground = new SolidColorBrush(Colors.Blue);
                                    break;
                                default:
                                    s.Foreground = new SolidColorBrush(Colors.Silver);
                                    break;
                            }
                            this._seeds[chapter].Add(episode, s);
                            this._headers[chapter].Items.Add(s);
                        }
                    }

                    foreach (int chapter in this._headers.Keys)
                    {
                        this.tvSeeds.Items.Add(this._headers[chapter]);
                    }
                }, CancellationToken.None, TaskCreationOptions.None, this._scheduler)
                .ContinueWith(
                    (task) =>
                    {
                        if (MessageBox.Show(
                            this,
                            "The seeds are ready, would you like to download immediately?\n\r\n\r\"Yes\" to download immediately\n\r\"No\" to have a review",
                            "520 TingShu",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question,
                            MessageBoxResult.Yes) == MessageBoxResult.No)
                        {
                            this.btTransaction.Content = "Start";
                            return false;
                        }
                        return true;
                    },CancellationToken.None, TaskContinuationOptions.None, this._scheduler)
                .ContinueWith(
                    (task) =>
                    {
                        if (task.Result)
                            //start downloading queue
                            DownloadingQueue.TingShu520.Start();
                        else
                        {
                            Process.Start(PendingQueue.TingShu520.GetCachePath());
                        }
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
                this.Cursor = Cursors.Wait;
                PendingQueue.TingShu520.Stop();
                DownloadingQueue.TingShu520.Stop();
                this.btTransaction.Content = "Start";
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                this.Cursor = Cursors.Wait;
                Task.Factory.StartNew(() => { PendingQueue.TingShu520.Start(); });
                this.btTransaction.Content = "Stop";
                this.Cursor = Cursors.Arrow;
            }

            this.btTransaction.IsEnabled = true;
        }
    }
}
