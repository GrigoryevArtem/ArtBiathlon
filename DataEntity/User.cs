using ArtBiathlon.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtBiathlon.DataEntity
{
    public class User
    {
        public uint Id { get; set; }
        public string FIO { get; set; }
        public DateTime Date { get; set; }
        public Gender Gender { get; set; }
        public Rank Rank { get; set; }
        public Role Status { get; set; }
        public string Email { get; set; }
        public string EmailAccessPassword { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }     
    }
}
