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
    using System.Runtime.Remoting.Channels;
    using System.Threading;

    using Pean.Supperzzle.WPF.Pages;

    public class ChessFore
    {
        public byte[] ForeImage { get; private set; }

        public int Token { get; private set; }

        public int Id { get; set; }

        public ChessFore(byte[] foreImage, int token, int id)
        {
            this.ForeImage = foreImage;
            this.Token = token;
            this.Id = id;
        }

        public ChessFore(byte[] foreImage, int token)
        {
            this.ForeImage = foreImage;
            this.Token = token;
        }
    }

    /// <summary>
    /// Interaction logic for ChessMaxtrix.xaml
    /// </summary>
    public partial class ChessMaxtrix : UserControl
    {
        #region Event
        public event EventHandler ComparedFail;

        public event EventHandler ComparedSuccess;

        public event EventHandler Finished;
        #endregion

        public ChessMaxtrix()
        {
            this.InitializeComponent();
            this._chesses = new List<Chess>();
        }

        private IList<Chess> _chesses; 

        public void Init(int columns, int rows, IList<ChessFore> chessFores, byte[] chessbg,IList<byte[]> puzzleBg)
        {
            for (int i = 0; i < columns; i++)
            {
                this.panel.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < rows; i++)
            {
                this.panel.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    int index = i * rows + j;

                    Chess chess = new Chess();
                    ChessFore chessFore = chessFores.FirstOrDefault(cf => cf.Id == index);
                    if (chessFore == null) continue;
                    int id = index;
                    byte[] foreimg = chessFore.ForeImage;
                    int token = chessFore.Token;
                    byte[] forebg = chessbg;
                    byte[] puzzlebg = puzzleBg[index];
                    chess.Init(id,token,forebg,foreimg,puzzlebg);

                    chess.SearchingOtherChess += this.Maxtrix_SearchingOtherChess;
                    chess.CheckingFinished += this.Maxtrix_CheckingFinished;
                    chess.ComparedFail += (sender, e) => { this.OnComparedFail(); };
                    chess.ComparedSuccess += (sender, e) => { this.OnComparedSuccess(); };
                    chess.Finished += (sender, e) => { this.OnFinished(); };

                    this._chesses.Add(chess);
                    Grid.SetColumn(chess,i);
                    Grid.SetRow(chess, j);

                    this.panel.Children.Add(chess);
                }
            }
        }

        private bool Maxtrix_CheckingFinished(object sender, EventArgs e)
        {
            return this._chesses.All(c => c.State == State.Inverted);
        }

        private Chess Maxtrix_SearchingOtherChess(object sender, EventArgs e)
        {
            Chess selecting = sender as Chess;
            return this._chesses.FirstOrDefault(c => c.State == State.Inverting && c.Id != selecting.Id);
        }

        public void Preview()
        {
            foreach (Chess chess in this._chesses)
            {
                chess.Preview();
            }
        }

        public void Ready()
        {
            foreach (Chess chess in this._chesses)
            {
                chess.Ready();
            }
        }

        public void Rest(IList<ChessFore> chessFores)
        {
            for (int i = 0; i < this._chesses.Count; i++)
            {
                Chess chess = this._chesses[i];
                ChessFore chessf = chessFores.FirstOrDefault(cf => cf.Id == chess.Id);
                if (chessf == null) continue;
                int token = chessf.Token;
                byte[] img = chessf.ForeImage;
                chess.Rest(token, img);
            }
        }

        #region
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
