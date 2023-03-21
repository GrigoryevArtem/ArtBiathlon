using System.Diagnostics.CodeAnalysis;

namespace ArtBiathlon.DataEntity
{
    public class ForumMessage
    {
        public uint Id { get; set; }

        public DateTime Date { get; set; }

        public uint UserId { get; set; }

        [MaybeNull]
        public uint? ParentId { get; set; }

        public string Message { get; set; }
    }
}
