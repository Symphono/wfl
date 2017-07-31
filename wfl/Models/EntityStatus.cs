using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Symphono.Wfl.Models
{
    public class EntityStatus
    {
        public enum Status
        {
            Active = 1,
            Discarded,
            Completed
        }
    }
}