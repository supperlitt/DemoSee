using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConAOP
{
    public class LogHandlerAttribute : HandlerAttribute
    {
        public LogHandlerAttribute(string msg)
        {
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new LogHandler() { };
        }
    }
}
