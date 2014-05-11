namespace Peach.Entity
{
    public interface IThumbnail:IImage
    {
        /// <summary>
        /// Gets the full url.
        /// </summary>
        string FullUrl { get; }
    }
}