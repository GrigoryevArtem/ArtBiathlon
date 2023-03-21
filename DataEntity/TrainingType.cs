using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArtBiathlon.DataEntity
{
    public class TrainingType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }
        public string NameType { get; set; }
    }
}
