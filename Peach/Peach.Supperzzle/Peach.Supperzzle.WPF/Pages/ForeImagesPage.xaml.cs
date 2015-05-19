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
    /// Interaction logic for ForeImagesPage.xaml
    /// </summary>
    public partial class ForeImagesPage : UserControl
    {
        public ForeImagesPage()
        {
            InitializeComponent();
            this._ImageSelectors = new List<ImageSelector>();
        }

        private IList<ImageSelector> _ImageSelectors; 

        public IList<byte[]> ForeImages
        {
            get
            {
              return  this._ImageSelectors.Where(s => s.Image != null && s.Image.Length != 0).Select(s=>s.Image).ToList();
            }
        }

        public void Init(int total)
        {
            this._ImageSelectors.Clear();
            this.panel.Children.Clear();

            for (int i = 0; i < total; i++)
            {
                ImageSelector selector = new ImageSelector()
                                         {
                                             Name = "btSelector" + i,
                                             Text = string.Format("选择纸牌 {0} 正面", i + 1),
                                             Width = 160,
                                             Height = 120,
                                             Margin = new Thickness(10,10,10,10)
                                         };
                this._ImageSelectors.Add(selector);

                this.panel.Children.Add(selector);
            }
        }

        public void Init(IList<byte[]> images)
        {
            this._ImageSelectors.Clear();
            this.panel.Children.Clear();

            for (int i = 0; i < images.Count; i++)
            {
                ImageSelector selector = new ImageSelector()
                {
                    Name = "btSelector" + i,
                    Text = string.Format("选择纸牌 {0} 正面", i + 1),
                    Width = 160,
                    Height = 120,
                    Margin = new Thickness(10, 10, 10, 10)
                };

                selector.SetBackground(images[i]);

                this._ImageSelectors.Add(selector);

                this.panel.Children.Add(selector);
            }
        }
    }
}
