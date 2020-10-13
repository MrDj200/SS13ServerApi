using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CurseServerPackCreator
{
    class CursePack
    {
        public ushort PackID { get; set; }
        public string PackName { get; set; }
        public string ThumbnailUrl { get; set; }
        public CursePack(ushort ID)
        {
            PackID = ID;
        }
    }
}
