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

namespace Pean.Supperzzle.WPF.Pages
{
    /// <summary>
    /// Interaction logic for ForeBgPage.xaml
    /// </summary>
    public partial class ForeBgPage : UserControl
    {
        private ImageSelector _selector;

        public ForeBgPage()
        {
            InitializeComponent();

            this._selector = new ImageSelector()
                                   {
                                       Text = "选择纸牌背面",
                                       Width = 160,
                                       Height = 120,
                                       HorizontalAlignment = HorizontalAlignment.Center,
                                       VerticalAlignment = VerticalAlignment.Center
                                   };

            this.panel.Children.Add(this._selector);
        }

        public byte[] ForeBg
        {
            get
            {
                return this._selector.Image;
            }
        }

        public void Init(byte[] foreBg)
        {
            this._selector.SetBackground(foreBg);
        }
    }
}
