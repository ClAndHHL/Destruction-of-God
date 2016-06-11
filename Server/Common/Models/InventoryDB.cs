using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon.Models
{
    public class InventoryDB
    {
        public virtual int Id { get; set; }
        public virtual int InventoryId { get; set; }
        public virtual int Num { get; set; }
        public virtual int Level { get; set; }
        public virtual bool IsDress { get; set; }
        public virtual Role Role { get; set; }
    }
}
