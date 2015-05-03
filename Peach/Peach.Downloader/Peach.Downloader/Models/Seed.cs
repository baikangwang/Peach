namespace Peach.Downloader.Models
{
    using System;
    using System.Text.RegularExpressions;

    public delegate void SeedStatusEvent(ISeed sender, int changed);

    public delegate void SeedDownloadEvent(ISeed sender);

    public interface ISeed
    {
        int Chapter { get; }

        int Episode { get; }

        Status Status { get; set; }

        string Title { get; }

        string Url { get; }

        string HttpUrl { get; }

        event SeedStatusEvent StatusChanged;

        event SeedDownloadEvent Completed;

        event SeedDownloadEvent Fail;

        void OnStatusChanged(int changed);

        void OnCompleted();

        void OnFail();
    }

    public class Seed:ISeed
    {
        public Seed(string title, int chapter, int episode, string url,string httpUrl)
        {
            this.Chapter = chapter;
            this.Title = title;
            this.Episode = episode;
            this.Url = url;
            this.Status = Status.Waiting;
            this.HttpUrl = httpUrl;
        }

        public Seed(string line)
        {
            Regex regex=new Regex("\\|",RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Singleline);
            string[] fields = regex.Split(line);
            this.Chapter = int.Parse(fields[0]);
            this.Episode = int.Parse(fields[1]);this.HttpUrl=fields[2];
            this.Status=(Status)int.Parse(fields[3]);
            this.Title=fields[4];
            this.Url = fields[5];
        }

        public int Chapter { get; private set; }

        public int Episode { get; private set; }

        public Status Status { get; set; }

        public string Title { get; private set; }

        public string Url { get; private set; }

        public string HttpUrl { get; private set; }

        public event SeedStatusEvent StatusChanged;

        public event SeedDownloadEvent Completed;

        public event SeedDownloadEvent Fail;

        public void OnStatusChanged(int changed)
        {
            var handler = this.StatusChanged;
            if (handler != null)
            {
                handler(this, changed);
            }
        }

        public void OnCompleted()
        {
            var handler = this.Completed;
            if (handler != null)
            {
                handler(this);
            }
        }

        public void OnFail()
        {
            var handler = this.Fail;
            if (handler != null)
            {
                handler(this);
            }
        }

        public override string ToString()
        {
            string str= string.Format("{0}|{1}|{2}|{3}|{4}|{5}",this.Chapter,this.Episode,this.HttpUrl,(int)this.Status,this.Title,this.Url);

            return str;
        }

        public string GetChapterName()
        {
            switch (this.Chapter)
            {
                case 1:
                    return "第一季";
                case 2:
                    return "第二季";
                case 3:
                    return "第三季";
                case 4:
                    return "第四季";
                case 5:
                    return "第五季";
                case 6:
                    return "第六季";
                case 7:
                    return "第七季";
                case 8:
                    return "第八季";
                case 9:
                    return "藏海花";
                default:
                    return DateTime.Now.ToString("yyyyMMddhhmmssfff");
            }
        }
    }
}
