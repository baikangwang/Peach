using System;
using System.IO;

namespace Peach.Entity
{
    public interface IImage : IDisposable
    {
        /// <summary>
        /// Gets the file name.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets the owner gallery.
        /// </summary>
        Gallery OwnerGallery { get; set; }

        /// <summary>
        ///     Gets the title.
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     Gets the url.
        /// </summary>
        string Url { get; }

        /// <summary>
        ///     The get content.
        /// </summary>
        /// <returns>
        ///     The <see cref="Stream" />.
        /// </returns>
        Stream GetContent();

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        void Load();
    }
}