using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon.Models
{
    public class TaskDB
    {
        public virtual int Id { get; set; }
        public virtual int TaskId { get; set; }
        public virtual int Type { get; set; }
        public virtual int State { get; set; }
        public virtual DateTime LastUpdateTime { get; set; }
        public virtual Role Role { get; set; }
    }
}
