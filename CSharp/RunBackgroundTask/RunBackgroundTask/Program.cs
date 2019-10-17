using System;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Threading;

namespace RunBackgroundTask
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            Console.WriteLine("主线程开始");
            RunBackgroundTask((obj) =>
            {
                using (FileStream fs=new FileStream("name1.txt",FileMode.Create,FileAccess.Write))
                {
                    using (StreamWriter sw=new StreamWriter(fs))
                    {
                        sw.WriteLine(obj);
                    }
                }
            }, new {a = 1, b = 2});
            RunBackgroundTask(Console.WriteLine, new {a = 2, b = 3});
            RunBackgroundTask(Console.WriteLine, new {a = 4, b = 5});
            RunBackgroundTask(Console.WriteLine, new {a = 6, b = 7});
            Console.WriteLine("主线程结束");
        }
        
        protected static void RunBackgroundTask(Action<object> action, object actionArgument)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                
            Thread thread=new Thread(() =>
            {
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                try
                {
                    action(actionArgument);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);//LogHelper.LogException(ex);
                }
            })
            {
                Name = "RunBackgroundTask"
            };
            thread.Start();
        }
    }
}