namespace ArtBiathlon.DataEntity
{
    public class ForumReaction
    {
        public uint Id { get; set; }
        public uint MessageId { get; set; }
        public uint UserId { get; set; }

        public bool IsLike { get; set; }
    }
}
