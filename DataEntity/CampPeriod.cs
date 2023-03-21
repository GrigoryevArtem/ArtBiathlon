using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArtBiathlon.DataEntity
{
    public class CampPeriod
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
