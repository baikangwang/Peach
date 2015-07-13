namespace Peach.Downloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Peach.Downloader.Models;

    public class QueuePresenter
    {
        public string CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "seeds.db");

        public void SaveSeeds(IList<ISeed> seeds)
        {
            if(seeds.Count==0) return;

            StringBuilder sb = new StringBuilder();
            foreach (ISeed seed in seeds)
            {
                sb.AppendLine(seed.ToString());
            }

            string content = sb.ToString();

            if (File.Exists(this.CachePath))
            {
                File.Delete(this.CachePath);
            }

            using (FileStream fs=new FileStream(this.CachePath,FileMode.OpenOrCreate,FileAccess.ReadWrite))
            {
                byte[] stream = Encoding.Default.GetBytes(content);
                fs.Write(stream,0,stream.Length);
                fs.Flush(true);
            }
        }

        public void SaveSeed(ISeed seed)
        {
            //throw new System.NotImplementedException();
        }

        public IList<ISeed> ListAllSeeds()
        {
            if (File.Exists(this.CachePath))
            {
                string content;

                using (FileStream fs = new FileStream(this.CachePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] stream = new byte[fs.Length];
                    fs.Read(stream, 0, stream.Length);
                    content = Encoding.Default.GetString(stream);
                }

                string[] lines =
                    Regex.Split(content, "\\r|\\n", RegexOptions.IgnoreCase | RegexOptions.Singleline)
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();

                return lines.Select(line => new Seed(line)).Cast<ISeed>().ToList();
            }
            else
            {
                return new List<ISeed>();
            }
        }

        public string GetCachePath()
        {
            return this.CachePath;
        }
    }
}