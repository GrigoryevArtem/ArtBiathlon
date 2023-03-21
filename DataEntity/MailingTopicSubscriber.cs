namespace ArtBiathlon.DataEntity
{
    public class MailingTopicSubscriber
    {
        public int Id { get; set; }
        public int MailingTopicId { get; set; }
        public int UserId { get; set; }
    }
}
