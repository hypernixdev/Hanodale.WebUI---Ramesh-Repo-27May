using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs.Sync
{
    public class SyncReponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }

        public List<string> Errors { get; set; }
    }
}
