using System;
using System.Collections.Generic;
using System.Text;

namespace GodCommon.Models
{
    public class ServerProperty
    {
        public virtual int Id { get; set; }
        public virtual string Ip { get; set; }
        public virtual string Name { get; set; }
        public virtual int Count { get; set; }
    }
}
