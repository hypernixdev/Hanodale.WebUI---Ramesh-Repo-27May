using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Logging.Elmah
{
    [Serializable]
    public class ErrorException : Exception
    {
        public ErrorException(string message)
            : base(message)
        {
        }
    }
}