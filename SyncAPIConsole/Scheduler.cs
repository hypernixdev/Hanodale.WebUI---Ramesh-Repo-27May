using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncAPIConsole
{
    internal class Scheduler
    {
        internal int Id { get; set; }
        internal string SyncModule { get; set; }
        internal string TimeSlot { get; set; }
        internal string LastProcessTime { get; set; }
    }

}
