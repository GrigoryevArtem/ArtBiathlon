namespace ArtBiathlon.DataEntity
{
    public class Mailing
    {
        public int Id { get; set; }
        public int MailingTopicId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

    }
}
