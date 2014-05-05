using System;
using System.Collections.Generic;
using System.Text;

namespace Peach.Core
{
    public abstract class RecogtionExpcetion : ApplicationException
    {
        protected RecogtionExpcetion(string message)
            : base(message)
        {

        }

        protected RecogtionExpcetion(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    public class InputRecognitionException : RecogtionExpcetion
    {

        public InputRecognitionException(string message)
            : base(message)
        {

        }

        public InputRecognitionException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    public class GalleryRecogtionExpcetion:RecogtionExpcetion
    {
        public GalleryRecogtionExpcetion(string message) : base(message)
        {
        }

        public GalleryRecogtionExpcetion(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class ThumbnailRecogtionException:RecogtionExpcetion
    {
        public ThumbnailRecogtionException(string message) : base(message)
        {
        }

        public ThumbnailRecogtionException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class ViewRecogntionException:RecogtionExpcetion
    {
        public string View { get; private set; }
        public ViewRecogntionException(string message,string view)
            : base(message)
        {
            this.View = view;
        }

        public ViewRecogntionException(string message,string view, Exception inner) : base(message, inner)
        {
            this.View = view;
        }
    }

    public class PagerRecogntionException:RecogtionExpcetion
    {
        
        public PagerRecogntionException(string message) : base(message)
        {
        }

        public PagerRecogntionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
