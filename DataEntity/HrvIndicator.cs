using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArtBiathlon.DataEntity
{
    [PrimaryKey(nameof(Date), nameof(IdBiathlete))]
    public class HrvIndicator
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime Date { get; set; }
        public uint IdBiathlete { get; set;}
        public double READINESS { get; set; }
        public int RMSSD { get; set; }
        public int RR { get; set; }
        public int SDNN { get; set; }
        public int SD1 { get; set; }
        public int TP { get; set; }
        public int HF { get; set; }
        public int LF { get; set; }
        public double SI { get; set; }
        public double LFHF { get; set; }
    }
}
