using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConAOP
{
    public class TestTool : MarshalByRefObject
    {
        private static TestTool oUserOpertion = null;

        // 定义单例模式将PolicyInjection.Create<UserOperation>()产生的这个对象传出去，这样就避免了在调用处写这些东西
        public static TestTool GetInstance()
        {
            if (oUserOpertion == null)
                oUserOpertion = PolicyInjection.Create<TestTool>();

            return oUserOpertion;
        }

        [LogHandler("HOOK")]
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
