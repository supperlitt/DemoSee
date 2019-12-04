namespace FiddlerCoreConsoleApplication
{
    using System;

    class Program
    {
        static int Main(string[] args)
        {
            // 使用 Fiddler的例子
            // 程序在Windows下运行会修改IE指定成代理。。。不自动改回。

            /*
             * 注意 注意 注意 注意
             * 注意 注意 注意 注意
             * 注意 注意 注意 注意
             * 注意 注意 注意 注意
             * 注意 注意 注意 注意
            // 恢复 -手动改回在 打开IE->工具->Internat选项->连接->局域网设置->取消代理勾选，确定即可。。恢复
             */
            string hostToSniff = "www.baidu.com"; // localhost
            if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                hostToSniff = args[0];
            }

            var fiddlerEngine = new FiddlerEngine(hostToSniff);
            fiddlerEngine.Start();
            Console.ReadKey();
            fiddlerEngine.Stop();
            return 0;
        }
    }
}
