using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArtBiathlon.DataEntity
{
    public class Training
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }
        public string NameTraining { get; set; }
        public uint IdType { get; set; }
    }
}
