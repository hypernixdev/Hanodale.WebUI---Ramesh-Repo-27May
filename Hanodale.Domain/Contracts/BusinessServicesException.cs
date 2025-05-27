using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.Contracts
{
    [Serializable]
    public class BusinessServicesException : Exception
    {
        public BusinessServicesException()
            : base()
        {
        }

        public BusinessServicesException(string message)
            : base(message)
        {
        }

        public BusinessServicesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
