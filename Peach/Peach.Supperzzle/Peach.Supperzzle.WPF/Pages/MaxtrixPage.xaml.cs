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
    /// Interaction logic for MaxtrixPage.xaml
    /// </summary>
    public partial class MaxtrixPage : UserControl
    {
        public MaxtrixPage()
        {
            InitializeComponent();
        }

        public int Columns
        {
            get
            {
                string str = this.txtColumns.Text;
                int val;
                if (!int.TryParse(str, out val)) val = 0;
                return val;
            }
        }

        public int Rows
        {
            get
            {
                string str = this.txtRows.Text;
                int val;
                if (!int.TryParse(str, out val)) val = 0;
                return val;
            }
        }

        public int PreparingPreoid
        {
            get
            {
                string str = this.txtPreoid.Text;
                int val;
                if (!int.TryParse(str, out val)) val = 0;
                return val;
            }
        }

        public void Init(int columns, int rows,int preoid)
        {
            this.txtColumns.Text = columns.ToString();
            this.txtRows.Text = rows.ToString();
            this.txtPreoid.Text = preoid.ToString();
        }
    }
}
