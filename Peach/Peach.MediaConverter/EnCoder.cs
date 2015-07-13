 //Code written by: Greg Chumney
//Using VS2010 IDE
//Encode class for encoding video files and generating thumbnails
//
//NOTES: This class seems to have a lot of redundant function calls, such as 
//calling the SourceExists function. It may not be needed, you may just delete 
//those lines if you wish. HOWEVER, I believe its better to be safe than sorry. 
//Everytime we try to get info, gen thumbnails, encode or anything like that we
//check JUST in case something happend to the source file so we can get out and 
//throw the exceptions.
//
//You can use this class for two scenarios. A: You have an app that has many files
//to be encoded, and you want to create an instance for each file. (ie, You may have 
//a custom control for each video file that displays a thumb and progress that you will
//queue.) or B: you have an app where you wish to only run ONE instance of the encoder,
//keep it open and just keep changing the source file property for each movie.
//
//Either way, A: OR B: it will keep track of how many thumbs it generates, and remove them
//upon you calling the dispose method.. 

//public delegate void ProgressEventHandler(int progressPercent, int timeRemaining);
//public delegate void StatusEventHandler(string status);
//public delegate void ConsoleOutputEventHandler(string stdOut);
//public delegate void MiscInfoEventHandler(int fps, int qfactor, int time, double bitrate, int frame, string size, string estimatedSize);

namespace Peach.MediaConverter
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    public class EncodeProgressEventArgs : EventArgs
    {
        public string RawOutputLine { get; set; }

        public short FPS { get; set; }

        public short Percentage { get; set; }

        public long CurrentFrame { get; set; }

        public long TotalFrames { get; set; }
    }

    public class EncodeFinishedEventArgs : EventArgs
    {
        public EncodedVideo EncodedVideoInfo { get; set; }
    }

    public class EncodedVideo
    {
        public string EncodingLog { get; set; }

        public string EncodedVideoPath { get; set; }

        public bool Success { get; set; }

        public string ThumbnailPath { get; set; }
    }

    public delegate void EncodeProgressEventHandler(object sender, EncodeProgressEventArgs e);

    public delegate void EncodeFinishedEventHandler(object sender, EncodeFinishedEventArgs e);

    public class Encoder
    {
        private const int PROGRESS_ERROR_LIMIT = 100;

        private int iProgressErrorCount;

        private EncodedVideo tempEncodedVideo;

        // System.Windows.Forms.Control tempCaller = null;
        private VideoFile tempVideoFile;

        public string FFmpegPath { get; set; }

        private string Params { get; set; }

        public event EncodeProgressEventHandler OnEncodeProgress;

        public event EncodeFinishedEventHandler OnEncodeFinished;

        protected virtual void DoEncodeProgress(EncodeProgressEventArgs e)
        {
            if (this.OnEncodeProgress != null)
            {
                this.OnEncodeProgress(this, e);
            }
        }

        protected virtual void DoEncodeFinished(EncodeFinishedEventArgs e)
        {
            if (this.OnEncodeFinished != null)
            {
                this.OnEncodeFinished(this, e);
            }
        }

        public EncodedVideo EncodeVideo(VideoFile input,string encodingCommand,string outputFile,bool getVideoThumbnail)
        {
            var encoded = new EncodedVideo();

            this.Params = string.Format("-i \"{0}\" {1} \"{2}\"", input.Path, encodingCommand, outputFile);
            var output = this.RunProcess(this.Params);
            encoded.EncodingLog = output;
            encoded.EncodedVideoPath = outputFile;

            if (File.Exists(outputFile))
            {
                encoded.Success = true;

                //get thumbnail?
                if (getVideoThumbnail)
                {
                    var saveThumbnailTo = outputFile + "_thumb.jpg";

                    if (this.GetVideoThumbnail(input, saveThumbnailTo))
                    {
                        encoded.ThumbnailPath = saveThumbnailTo;
                    }
                }
            }
            else
            {
                encoded.Success = false;
            }

            return encoded;
        }

        /// <summary>
        ///     Async for when secure threading is not necessary
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingCommand"></param>
        /// <param name="outputFile"></param>
        /// <param name="caller"></param>
        public void EncodeVideoAsync(VideoFile input, string encodingCommand, string outputFile, int threadCount)
        {
            //Gather info
            if (!input.infoGathered)
            {
                this.GetVideoInfo(input);
            }

            //Create new encoded video
            this.tempEncodedVideo = new EncodedVideo();
            this.tempEncodedVideo.EncodedVideoPath = outputFile;

            //Set input
            this.tempVideoFile = input;

            //Create parameters
            if (threadCount.Equals(1))
            {
                this.Params = string.Format("-i \"{0}\" {1} \"{2}\"", input.Path, encodingCommand, outputFile);
            }
            else
            {
                this.Params = string.Format(
                    "-i \"{0}\" -threads {1} {2} \"{3}\"",
                    input.Path,
                    threadCount,
                    encodingCommand,
                    outputFile);
            }

            //Execute ffmpeg async
            this.RunProcessAsync(this.Params);
        }

        /// <summary>
        ///     Async for when secure threading is not necessary
        ///     This method uses output filename to detect resulting type, same resolution as source and the same bitrate as source
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingCommand"></param>
        /// <param name="outputFile"></param>
        /// <param name="caller">The WinForm that makes the call</param>
        public void EncodeVideoAsyncAutoCommand(VideoFile input, string outputFile, int treadCount)
        {
            //Gather info
            if (!input.infoGathered)
            {
                this.GetVideoInfo(input);
            }

            //If input video bitrate 0, then the correct video bitrate has not been detected, generate a value
            if (input.VideoBitRate == 0)
            {
                //Use video height, guestimations, tweak these at ur own will
                var h = input.Height;

                if (h < 180)
                {
                    input.VideoBitRate = 400;
                }
                else if (h < 260)
                {
                    input.VideoBitRate = 1000;
                }
                else if (h < 400)
                {
                    input.VideoBitRate = 2000;
                }
                else if (h < 800)
                {
                    input.VideoBitRate = 5000;
                }
                else
                {
                    input.VideoBitRate = 8000;
                }
            }

            //If input audio bitrate is 0, then the correct audio bitrate has not been detected, set it to 128k
            if (input.AudioBitRate == 0) { input.AudioBitRate = 128; }

            //Build encoding command
            var encodingCommand = string.Format(
                "-threads {0} -y -b {1} -ab {2}",
                treadCount,
                input.VideoBitRate + "k",
                input.AudioBitRate + "k");

            //Create new encoded video
            this.tempEncodedVideo = new EncodedVideo();
            this.tempEncodedVideo.EncodedVideoPath = outputFile;

            //Set input
            this.tempVideoFile = input;

            //Create parameters
            this.Params = string.Format("-i \"{0}\" {1} \"{2}\"", input.Path, encodingCommand, outputFile);

            //Execute ffmpeg async
            this.RunProcessAsync(this.Params);
        }

        private void RunProcessAsync(string Parameters)
        {
            //Create process info
            var oInfo = new ProcessStartInfo(this.FFmpegPath, Parameters);

            //Set process properties
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = false;
            oInfo.RedirectStandardError = true;

            //Construct the process
            var proc = new Process();

            //Set start info
            proc.StartInfo = oInfo;

            //Hook up events
            proc.EnableRaisingEvents = true;
            proc.ErrorDataReceived += this.proc_ErrorDataReceived;
            proc.Exited += this.proc_Exited;

            //Start the process
            proc.Start();

            //Start async output reading
            proc.BeginErrorReadLine();
        }

        /// <summary>
        ///     Handles process exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proc_Exited(object sender, EventArgs e)
        {
            //Get process
            var proc = (Process)sender;

            //Get the process exit code
            var iExitCode = proc.ExitCode;

            //Check if file exists
            var blFileExists = File.Exists(this.tempEncodedVideo.EncodedVideoPath);

            //Set success
            this.tempEncodedVideo.Success = (iExitCode.Equals(0) && blFileExists);

            //Construct new finished event args
            var efe = new EncodeFinishedEventArgs();

            //Set the encode video finished event's VideoInfo object
            efe.EncodedVideoInfo = this.tempEncodedVideo;

            //Invoke/Fire real finished event
            this.DoEncodeFinished(efe);

            //Reset progress error count
            this.iProgressErrorCount = 0;

            //Release resourced from process
            proc.Close();
        }

        /// <summary>
        ///     Handles process progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.tempEncodedVideo.EncodingLog += e.Data + Environment.NewLine;

                if (e.Data.StartsWith("frame"))
                {
                    //Reset progress error count
                    this.iProgressErrorCount = 0;

                    //Create a new progress event object
                    var epe = new EncodeProgressEventArgs();

                    //Set raw data line
                    epe.RawOutputLine = e.Data;

                    //Set total frames
                    epe.TotalFrames = this.tempVideoFile.TotalFrames;

                    //Split the raw string
                    var parts = e.Data.Split(new[] { " ", "=" }, StringSplitOptions.RemoveEmptyEntries);

                    //Parse current frame
                    var lCurrentFrame = 0L;
                    long.TryParse(parts[1], out lCurrentFrame);
                    epe.CurrentFrame = lCurrentFrame;

                    //Parse FPS
                    short sFPS = 0;
                    short.TryParse(parts[3], out sFPS);
                    epe.FPS = sFPS;

                    //Calculate percentage
                    double dCurrentFrame = epe.CurrentFrame;
                    double dTotalFrames = epe.TotalFrames;
                    var sPercentage = (short)Math.Round(dCurrentFrame * 100 / dTotalFrames, 0);
                    epe.Percentage = sPercentage;

                    this.DoEncodeProgress(epe);
                }
                else
                {
                    //Increment progress error
                    this.iProgressErrorCount++;
                }
            }
            else
            {
                //Increment progress error
                this.iProgressErrorCount++;
            }

            //If ProgressErrorCount is more than limit, then kill the process
            if (this.iProgressErrorCount > PROGRESS_ERROR_LIMIT)
            {
                //Get the process
                var proc = (Process)sender;

                //Murder
                try
                {
                    proc.Kill();
                }
                catch
                {
                }
            }
        }

        public bool GetVideoThumbnail(VideoFile input, string saveThumbnailTo)
        {
            if (!input.infoGathered)
            {
                this.GetVideoInfo(input);
            }

            //divide the duration in 3 to get a preview image in the middle of the clip
            //instead of a black image from the beginning.
            int secs;
            secs = (int)Math.Round(TimeSpan.FromTicks(input.Duration.Ticks / 3).TotalSeconds, 0);
            if (secs.Equals(0)) { secs = 1; }

            var Params = string.Format(
                "-i \"{0}\" \"{1}\" -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo",
                input.Path,
                saveThumbnailTo,
                secs);
            var output = this.RunProcess(Params);

            if (File.Exists(saveThumbnailTo))
            {
                return true;
            }
            //try running again at frame 1 to get something
            Params = string.Format(
                "-i \"{0}\" \"{1}\" -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo",
                input.Path,
                saveThumbnailTo,
                1);
            output = this.RunProcess(Params);

            if (File.Exists(saveThumbnailTo))
            {
                return true;
            }
            return false;
        }

        private string RunProcess(string Parameters)
        {
            //create a process info
            var oInfo = new ProcessStartInfo(this.FFmpegPath, Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            //Create the output
            string output = null;

            //try the process
            try
            {
                //run the process
                var proc = Process.Start(oInfo);

                //now put it in a string
                //This needs to be before WaitForExit() to prevent deadlock, for details: http://msdn.microsoft.com/en-us/library/system.diagnostics.process.standardoutput%28v=VS.80%29.aspx
                output = proc.StandardError.ReadToEnd();

                //Wait for exit
                proc.WaitForExit();

                //Release resources
                proc.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }

            return output;
        }

        public void GetVideoInfo(VideoFile input)
        {
            var Params = string.Format("-i \"{0}\"", input.Path);
            var output = this.RunProcess(Params);

            input.RawInfo = output;
            input.Duration = this.ExtractDuration(input.RawInfo);
            input.BitRate = this.ExtractBitrate(input.RawInfo);
            input.RawAudioFormat = this.ExtractRawAudioFormat(input.RawInfo);
            input.AudioFormat = this.ExtractAudioFormat(input.RawAudioFormat);
            input.RawVideoFormat = this.ExtractRawVideoFormat(input.RawInfo);
            input.VideoFormat = this.ExtractVideoFormat(input.RawVideoFormat);
            input.Width = this.ExtractVideoWidth(input.RawInfo);
            input.Height = this.ExtractVideoHeight(input.RawInfo);
            input.FrameRate = this.ExtractFrameRate(input.RawVideoFormat);
            input.TotalFrames = this.ExtractTotalFrames(input.Duration, input.FrameRate);
            input.AudioBitRate = this.ExtractAudioBitRate(input.RawAudioFormat);
            input.VideoBitRate = this.ExtractVideoBitRate(input.RawVideoFormat);

            input.infoGathered = true;
        }

        #region Extraction methods

        private TimeSpan ExtractDuration(string rawInfo)
        {
            var t = new TimeSpan(0);
            var re = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
            var m = re.Match(rawInfo);

            if (m.Success)
            {
                var duration = m.Groups[1].Value;
                var timepieces = duration.Split(':', '.');
                if (timepieces.Length == 4)
                {
                    t = new TimeSpan(
                        0,
                        Convert.ToInt16(timepieces[0]),
                        Convert.ToInt16(timepieces[1]),
                        Convert.ToInt16(timepieces[2]),
                        Convert.ToInt16(timepieces[3]));
                }
            }

            return t;
        }

        private double ExtractBitrate(string rawInfo)
        {
            var re = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
            var m = re.Match(rawInfo);
            var kb = 0.0;
            if (m.Success)
            {
                double.TryParse(m.Groups[1].Value, out kb);
            }
            return kb;
        }

        private string ExtractRawAudioFormat(string rawInfo)
        {
            var a = string.Empty;
            var re = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
            var m = re.Match(rawInfo);
            if (m.Success)
            {
                a = m.Value;
            }
            return a.Replace("Audio: ", "");
        }

        private string ExtractAudioFormat(string rawAudioFormat)
        {
            var parts = rawAudioFormat.Split(new[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Audio: ", "");
        }

        private string ExtractRawVideoFormat(string rawInfo)
        {
            var v = string.Empty;
            var re = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
            var m = re.Match(rawInfo);
            if (m.Success)
            {
                v = m.Value;
            }
            return v.Replace("Video: ", "");
            ;
        }

        private string ExtractVideoFormat(string rawVideoFormat)
        {
            var parts = rawVideoFormat.Split(new[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Video: ", "");
        }

        private int ExtractVideoWidth(string rawInfo)
        {
            var width = 0;
            var re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            var m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out width);
            }
            return width;
        }

        private int ExtractVideoHeight(string rawInfo)
        {
            var height = 0;
            var re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            var m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[2].Value, out height);
            }
            return height;
        }

        private double ExtractFrameRate(string rawVideoFormat)
        {
            var parts = rawVideoFormat.Split(new[] { ", " }, StringSplitOptions.None);

            double dFPS = 0;

            foreach (var p in parts)
            {
                if (p.ToLower().Contains("fps"))
                {
                    double.TryParse(p.ToLower().Replace("fps", "").Replace(".", ",").Trim(), out dFPS);

                    break;
                }
                if (p.ToLower().Contains("tbr"))
                {
                    double.TryParse(p.ToLower().Replace("tbr", "").Replace(".", ",").Trim(), out dFPS);

                    break;
                }
            }

            //Audio: mp3, 44100 Hz, 2 channels, s16, 140 kb/s

            return dFPS;
        }

        private double ExtractAudioBitRate(string rawAudioFormat)
        {
            var parts = rawAudioFormat.Split(new[] { ", " }, StringSplitOptions.None);

            double dABR = 0;

            foreach (var p in parts)
            {
                if (p.ToLower().Contains("kb/s"))
                {
                    double.TryParse(p.ToLower().Replace("kb/s", "").Replace(".", ",").Trim(), out dABR);

                    break;
                }
            }

            return dABR;
        }

        private double ExtractVideoBitRate(string rawVideoFormat)
        {
            var parts = rawVideoFormat.Split(new[] { ", " }, StringSplitOptions.None);

            double dVBR = 0;

            foreach (var p in parts)
            {
                if (p.ToLower().Contains("kb/s"))
                {
                    double.TryParse(p.ToLower().Replace("kb/s", "").Replace(".", ",").Trim(), out dVBR);

                    break;
                }
            }

            return dVBR;
        }

        private long ExtractTotalFrames(TimeSpan duration, double frameRate)
        {
            return (long)Math.Round(duration.TotalSeconds * frameRate, 0);
        }

        #endregion
    }

    public class VideoFile
    {
        public bool infoGathered { get; set; }

        public string RawInfo { get; set; }

        public TimeSpan Duration { get; set; }

        public double BitRate { get; set; }

        public string RawAudioFormat { get; set; }

        public string AudioFormat { get; set; }

        public string RawVideoFormat { get; set; }

        public string VideoFormat { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public double FrameRate { get; set; }

        public long TotalFrames { get; set; }

        public double AudioBitRate { get; set; }

        public double VideoBitRate { get; set; }

        public string Path { get; set; }
    }
}