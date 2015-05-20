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
    using System.Drawing;
    using System.IO;

    using Pean.Supperzzle.WPF.Pages;

    using Xceed.Wpf.Toolkit;
    using Xceed.Wpf.Toolkit.Core;
    using Xceed.Wpf.Toolkit.Core.Converters;

    public class Setting
    {
        public static string INFO=System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "supperzzle.ini");
    }

    /// <summary>
    /// Interaction logic for fmConfig.xaml
    /// </summary>
    public partial class fmConfig : Window
    {
        private Options _options;

        private MaxtrixPage _maxtrixPage;

        private ForeImagesPage _foreimagesPage;

        private ForeBgPage _foreBgPage;

        private PuzzleBgPage _puzzleBgPage;

        private bool _isMaxtrixChanged;

        public fmConfig()
        {
            InitializeComponent();

            this.Loaded += this.Config_Load;
            this._isMaxtrixChanged = false;

            //wizard.Next+= (sender, e) => { e.}
        }

        private void Config_Load(object sender, RoutedEventArgs e)
        {
            this.Load();

            this._maxtrixPage = new MaxtrixPage();
            this._foreimagesPage = new ForeImagesPage();
            this._foreBgPage = new ForeBgPage();
            this._puzzleBgPage = new PuzzleBgPage();

            this.InitPages();

            this.InitWarzid();
        }

        private void InitPages()
        {
            int columns = (int)this._options.Size.Width;
            int rows = (int)this._options.Size.Height;
            int preoid = this._options.PreparingPreoid == 0 ? 20 : this._options.PreparingPreoid;
            bool isValidMaxtrix = this.ValidateMaxtrix(columns, rows);
            if (isValidMaxtrix)
            {
                this._maxtrixPage.Init(columns, rows,preoid);
            }
            else
            {
                this._maxtrixPage.Init(0, 0, preoid);
            }

            bool isValidForeImages = this.ValidateForeImages(columns, rows, this._options.ForeImages);
            if (isValidMaxtrix && isValidForeImages)
            {
                this._foreimagesPage.Init(this._options.ForeImages);
            }
            else if (isValidMaxtrix)
            {
                this._foreimagesPage.Init(columns * rows / 2);
            }

            bool isValidForeBg = this.ValidateForeBg(this._options.ForeBackground);
            if(isValidForeBg)
                this._foreBgPage.Init(this._options.ForeBackground);

            bool isValidPuzzleBg = this.ValidatePuzzleBg(columns, rows, this._options.PuzzleBackground);
            if (isValidMaxtrix && isValidPuzzleBg) 
                this._puzzleBgPage.Init(columns, rows, this._options.PuzzleBackground);
            else if (isValidMaxtrix)
                this._puzzleBgPage.Init(columns, rows);

        }

        private void InitWarzid()
        {
            Wizard wizard = new Wizard() { FinishButtonClosesWindow = true};

            WizardPage welcome = new WizardPage()
                                 {
                                     Title = "欢迎",
                                 };

            WizardPage maxtrixPage = new WizardPage()
                                     {
                                         PageType = WizardPageType.Interior,
                                         Title = "设置游戏格数",
                                         Content = this._maxtrixPage,
                                     };

            WizardPage foreimagesPage = new WizardPage()
                                        {
                                            PageType = WizardPageType.Interior,
                                            Title = "选择纸牌正面",
                                            Content = this._foreimagesPage
                                        };
            WizardPage forebgPage = new WizardPage()
                                    {
                                        PageType = WizardPageType.Interior,
                                        Title = "选择纸牌背面",
                                        Content = this._foreBgPage
                                    };
            WizardPage puzzlebgPage = new WizardPage()
                                      {
                                          PageType = WizardPageType.Interior,
                                          Title = "选择游戏背景",
                                          Content = this._puzzleBgPage,
                                          CanFinish = true,
                                      };

            welcome.NextPage = maxtrixPage;
            maxtrixPage.NextPage = foreimagesPage;
            maxtrixPage.PreviousPage = welcome;
            foreimagesPage.PreviousPage = maxtrixPage;
            foreimagesPage.NextPage = forebgPage;
            forebgPage.PreviousPage = foreimagesPage;
            forebgPage.NextPage = puzzlebgPage;
            puzzlebgPage.PreviousPage = forebgPage;

            wizard.Items.Add(welcome);
            wizard.Items.Add(maxtrixPage);
            wizard.Items.Add(foreimagesPage);
            wizard.Items.Add(forebgPage);
            wizard.Items.Add(puzzlebgPage);

            this.panel.Children.Add(wizard);

            wizard.Finish += this.Wizard_Finish; //(@object, args) => { this.Save(); };

            wizard.Next += this.Wizard_Next;
        }

        private void Wizard_Finish(object sender, RoutedEventArgs e)
        {
            Wizard wizard = sender as Wizard;
            object content = wizard.CurrentPage.Content;
            if (content is PuzzleBgPage)
            {
                PuzzleBgPage page = content as PuzzleBgPage;

                int columns = (int)this._options.Size.Width;
                int rows = (int)this._options.Size.Height;
                IList<byte[]> images = page.PuzzleBg;
                MethodResult isValid = this.ValidatePuzzleBg(columns, rows, images);

                if (!isValid)
                {
                    if (!string.IsNullOrEmpty(isValid.Message)) MessageBox.Show(isValid.Message);
                    e.Handled = false;
                    return;
                }

                this._options.PuzzleBackground = images;
            }

            this.Save();
        }

        private void Wizard_Next(object sender, CancelRoutedEventArgs e)
        {
            Wizard wizard = sender as Wizard;
            object content = wizard.CurrentPage.Content;
            if (content is MaxtrixPage)
            {
                MaxtrixPage page = content as MaxtrixPage;
                int columns = page.Columns;
                int rows = page.Rows;
                int preoid = page.PreparingPreoid;

                // validation
                MethodResult isValid = this.ValidateMaxtrix(columns, rows);
                if (!isValid)
                {
                    if(!string.IsNullOrEmpty(isValid.Message))
                    MessageBox.Show(isValid.Message);
                    e.Cancel = true;
                    return;
                }

                this._isMaxtrixChanged = this.IsMaxtrixChanged(
                    (int)this._options.Size.Width,
                    (int)this._options.Size.Height,
                    columns,
                    rows);

                // init
                this._options.Size = new System.Windows.Size(columns, rows);
                this._options.PreparingPreoid = preoid;

                if (this._isMaxtrixChanged)
                {
                    // init ForeImages
                    ForeImagesPage next = wizard.CurrentPage.NextPage.Content as ForeImagesPage;
                    int total = columns * rows / 2;
                    next.Init(total);
                }
            }
            else if (content is ForeImagesPage)
            {
                ForeImagesPage page = content as ForeImagesPage;
                int columns = (int)this._options.Size.Width;
                int rows = (int)this._options.Size.Height;
                IList<byte[]> images = page.ForeImages;
                MethodResult isValid = this.ValidateForeImages(columns, rows, images);

                if (!isValid)
                {
                    if (!string.IsNullOrEmpty(isValid.Message)) MessageBox.Show(isValid.Message);
                    e.Cancel = true;
                    return;
                }

                this._options.ForeImages = images;
            }
            else if (content is ForeBgPage)
            {
                ForeBgPage page = content as ForeBgPage;

                byte[] image = page.ForeBg;

                MethodResult isValid = this.ValidateForeBg(image);

                if (!isValid)
                {
                    if (!string.IsNullOrEmpty(isValid.Message)) MessageBox.Show(isValid.Message);
                    e.Cancel = true;
                    return;
                }

                this._options.ForeBackground = image;

                if (this._isMaxtrixChanged)
                {
                    // init ForeImages
                    PuzzleBgPage next = wizard.CurrentPage.NextPage.Content as PuzzleBgPage;
                    int columns = (int)this._options.Size.Width;
                    int rows = (int)this._options.Size.Height;
                    next.Init(columns,rows);
                }
            }
            else if (content is PuzzleBgPage)
            {
            }
        }

        private bool IsMaxtrixChanged(int oColumns, int oRows, int nColumns, int nRows)
        {
            return oColumns != nColumns || oRows != nRows;
        }

        #region Validation

        private MethodResult ValidateMaxtrix(int columns, int rows)
        {
            if (columns == 0)
            {
                return new MethodResult(false, "错误：列数必须大于0");
            }
            if (rows == 0)
            {
                return new MethodResult(false, "错误：行数必须大于0");
            }

            bool isValid = (columns * rows) % 2 == 0;
            if (!isValid)
            {
                return new MethodResult(false, "错误：行数与列数乘积必须是偶数");
            }

            return new MethodResult();
        }

        private MethodResult ValidatePuzzleBg(int columns, int rows, IList<byte[]> puzzleBackground)
        {
            if (puzzleBackground == null || puzzleBackground.Count == 0) return new MethodResult(false, "错误：请选择游戏背景");

            return puzzleBackground.Count == (columns * rows) ? new MethodResult() : new MethodResult(false, "错误：游戏背景的格数与设置的游戏格数不符");
        }

        private MethodResult ValidateForeBg(byte[] foreBackground)
        {
            return foreBackground != null && foreBackground.Length != 0 ? new MethodResult() : new MethodResult(false, "错误：请选择纸牌背面图片");
        }

        private MethodResult ValidateForeImages(int columns, int rows, IList<byte[]> foreImages)
        {
            if (foreImages == null || foreImages.Count == 0) return new MethodResult(false, "错误：请选择纸牌正面图片");

            return foreImages.Count == (columns * rows / 2) ? new MethodResult() : new MethodResult(false, "错误：纸牌数量与设置的游戏格数不符");
        }

        #endregion

        public void Load()
        {
            this._options = new Options(Setting.INFO);
        }

        public void Save()
        {
            string backup = Setting.INFO + ".bk";
            if (File.Exists(Setting.INFO))
            {
                File.Move(Setting.INFO, backup);
            }

            using (FileStream fs = new FileStream(Setting.INFO, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                string options = this._options.ToString();
                byte[] content = Encoding.Default.GetBytes(options);
                fs.Write(content, 0, content.Length);
                fs.Flush(true);
            }

            if (File.Exists(backup)) File.Delete(backup);

            this._options.Dispose();
        }
    }

    public class MethodResult
    {
        public bool Value { get; private set; }

        public string Message { get; private set; }

        public MethodResult()
        {
            this.Value = true;
            this.Message = string.Empty;
        }

        public MethodResult(bool value, string message)
        {
            this.Value = value;
            this.Message = message;
        }

        public static implicit operator bool(MethodResult result)
        {
            return result.Value;
        }
    }
}
