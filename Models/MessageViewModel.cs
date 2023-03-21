using ArtBiathlon.DataEntity;

namespace ArtBiathlon.Models
{
    public class MessageViewModel
    {
        public uint Id { get; set; }

        public uint? ParentId { get; set; }

        public string Fullname { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public List<User?> LikedUsers { get; set; }

        public List<User?> DislikedUsers { get; set; }

        public uint NestingLevel { get; set; }

    }
}
