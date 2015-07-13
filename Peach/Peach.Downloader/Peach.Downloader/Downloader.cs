namespace Peach.Downloader
{
    using System;
    using System.IO;
    using System.Threading;

    using Peach.Downloader.Models;

    public abstract class Downloader
    {
        public string BaseSavePath = AppDomain.CurrentDomain.BaseDirectory;

        public abstract string GetContent(string url);

        public abstract void Download(ISeed seed, CancellationToken token);

        protected virtual string GetFileName(ISeed seed)
        {
            string fileName = string.Format("{0}.flv", seed.Title.Replace(" ", string.Empty).Replace("-", "_"));
            string chapterName = (seed as Seed).GetChapterName();
            string path = Path.Combine(this.BaseSavePath, chapterName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string file = Path.Combine(path, fileName);
            return file;
        }
    }
}
