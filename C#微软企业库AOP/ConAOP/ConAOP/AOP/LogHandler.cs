using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConAOP
{

    public class LogHandler : ICallHandler
    {
        // 这个方法就是拦截的方法，可以规定在执行方法之前和之后的拦截
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            string class_name = input.Target.ToString();
            string method_name = input.MethodBase.Name;

            Console.WriteLine("调用方法 " + class_name + " -> " + method_name + "(");
            string[] args = new string[input.Inputs.Count];
            for (int i = 0; i < input.Arguments.Count; i++)
            {
                string arg = (input.Arguments[i] == null ? "" : input.Arguments[i].ToString());
                Console.WriteLine("传入参数 " + class_name + " -> " + method_name + "( p" + i + " " + arg);
            }

            // 执行方法
            var messagereturn = getNext()(input, getNext);

            string result = messagereturn.ReturnValue == null ? "" : messagereturn.ReturnValue.ToString();
            Console.WriteLine("返回结果 " + class_name + " -> " + method_name + "( result " + result);

            return messagereturn;
        }

        public int Order
        {
            get;
            set;
        }
    }
}
