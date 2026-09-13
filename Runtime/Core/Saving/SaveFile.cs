using System;

namespace GameKit.Saving
{
    public sealed class SaveFile<T>
    {
        public int Version { get; set; }
        public DateTime SavedAtUtc { get; set; }
        public T Data { get; set; }
    }
}
