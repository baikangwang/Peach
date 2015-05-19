using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pean.Supperzzle.WPF
{
    using System.ComponentModel;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows;

    using Pean.Supperzzle.WPF.Annotations;

    public class Options:IDisposable
    {
        public Size Size { get; set; }

        public int PreparingPreoid { get; set; }

        public IList<byte[]> ForeImages { get; set; }

        public byte[] ForeBackground { get; set; }

        public IList<byte[]> PuzzleBackground { get; set; }

        public Options(string file):this()
        {
            if (!File.Exists(file)) return;

            string[] options = File.ReadAllLines(file);

            if (options.Length == 0) return;

            int i = 0;
            while (i < options.Length)
            {
                string option = options[i];
                Match m = Regex.Match(option, "(?<=^Size=).*(?=$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    if(string.IsNullOrEmpty(m.Value)) continue;

                    string[] parts = Regex.Split(m.Value, ",");
                    if (parts.Length > 1)
                    {
                        int width;
                        if (!int.TryParse(parts[0], out width)) width = 0;
                        int height;
                        if (!int.TryParse(parts[1], out height)) height = 0;
                        this.Size=new Size(width,height);
                    }

                    i++;
                    continue;
                }

                m = Regex.Match(option, "(?<=^PreparingPreoid=).*(?=$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    if (string.IsNullOrEmpty(m.Value)) continue;

                        int preoid;
                        if (!int.TryParse(m.Value, out preoid)) preoid = 20;
                        this.PreparingPreoid = preoid;

                    i++;
                    continue;
                }

                m = Regex.Match(option, "(?<=^ForeImgs=).*(?=$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    if (string.IsNullOrEmpty(m.Value)) continue;

                    string[] imgs = Regex.Split(m.Value, ",");

                    foreach (string img in imgs)
                    {
                        byte[] stream = Convert.FromBase64String(img); //Encoding.Default.GetBytes(img);
                        this.ForeImages.Add(stream);
                    }

                    i++;
                    continue;
                }

                m = Regex.Match(option, "(?<=^ForeBg=).*(?=$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    if (string.IsNullOrEmpty(m.Value)) continue;

                    byte[] stream = Convert.FromBase64String(m.Value); //Encoding.Default.GetBytes(m.Value);
                    this.ForeBackground = stream;
                    i++;
                    continue;
                }

                m = Regex.Match(option, "(?<=^PuzzleBg=).*(?=$)", RegexOptions.Singleline|RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    if (string.IsNullOrEmpty(m.Value)) continue;

                    string[] imgs = Regex.Split(m.Value, ",");

                    foreach (string img in imgs)
                    {
                        byte[] stream = Convert.FromBase64String(img); //Encoding.Default.GetBytes(img);
                        this.PuzzleBackground.Add(stream);
                    }
                    i++;
                }
            }
        }

        protected Options()
        {
            this.Size=new Size(1,1);
            this.PreparingPreoid = 0;
            this.ForeImages=new List<byte[]>();
            this.ForeBackground=new byte[0];
            this.PuzzleBackground = new List<byte[]>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Size={0},{1}", this.Size.Width, this.Size.Height));

            sb.AppendLine(string.Format("PreparingPreoid={0}", this.PreparingPreoid));

            string image=string.Join(",",this.ForeImages.Select(
                fi =>Convert.ToBase64String(fi, 0, fi.Length, Base64FormattingOptions.None)));
            sb.AppendLine(string.Format("ForeImgs={0}", image));

            image = Convert.ToBase64String(this.ForeBackground, 0, this.ForeBackground.Length, Base64FormattingOptions.None);
            sb.AppendLine(string.Format("ForeBg={0}", image));

            image = string.Join(",", this.PuzzleBackground.Select(fi => Convert.ToBase64String(fi, 0, fi.Length, Base64FormattingOptions.None)));
            sb.AppendLine(string.Format("PuzzleBg={0}", image));

            return sb.ToString();
        }

        protected virtual void Dispose(bool all)
        {
            if (all)
            {
                this.Size = new Size(0,0);
                this.PreparingPreoid = 0;
                this.ForeImages.Clear();
                this.ForeImages = null;
                this.ForeBackground = null;
                this.PuzzleBackground.Clear();
                this.PuzzleBackground = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
