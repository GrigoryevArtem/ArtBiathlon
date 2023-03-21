using ArtBiathlon.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArtBiathlon.DataEntity
{
    public class TrainingSchedule
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }
        public uint IdTraining { get; set; }
        public DateTime Date { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public uint Duration { get; set; }
        public uint IdCamp { get; set; }

    }
}
