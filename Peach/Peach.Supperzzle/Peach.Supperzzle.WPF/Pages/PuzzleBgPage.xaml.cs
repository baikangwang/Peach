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

namespace Pean.Supperzzle.WPF.Pages
{
    using System.Drawing;
    using System.IO;

    using Pean.Supperzzle.WPF.Controls;

    /// <summary>
    /// Interaction logic for PuzzleBgPage.xaml
    /// </summary>
    public partial class PuzzleBgPage : UserControl
    {
        private ImageMaxtrix maxtrix;

        public PuzzleBgPage()
        {
            this.InitializeComponent();

            this.maxtrix = new ImageMaxtrix();
            this.panel.Children.Add(this.maxtrix);
        }

        public IList<byte[]> PuzzleBg
        {
            get
            {
                return this.maxtrix.Images;
            }
        }

        public void Init(int columns, int rows, IList<byte[]> images)
        {
            this.maxtrix.Init(columns, rows, images);
        }

        public void Init(int columns, int rows)
        {
            this.maxtrix.Init(columns, rows);
        }
    }
}
