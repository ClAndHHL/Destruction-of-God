using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon.Models
{
    public class SkillDB
    {
        public virtual int Id { get; set; }
        public virtual int SkillId { get; set; }
        public virtual int Level { get; set; }
        public virtual Role Role { get; set; }
    }
}
