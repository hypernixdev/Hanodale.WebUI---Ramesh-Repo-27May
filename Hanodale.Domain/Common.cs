using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain
{
    public class Common
    {
        public const int ArchivalYear = 2018;
        public enum RecordStatus
        {
            Active,
            InActive
        }

        public enum AdminStatus
        {
            Admin,
            User
        }

        public enum Visibility
        {
            True,
            False
        }
        public enum AllowToSelect
        {
            True,
            False
        }

        public enum DurationType
        {
            Minute,
            Hour,
            Day
        }
    }
}
