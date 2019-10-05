using System;

namespace Algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            //循环队列
            var queue=new CircularQueue(15);
            for (int i = 0; i < 15; i++)
            {
                if (queue.EnQueue(i.ToString())==false)
                {
                    Console.WriteLine("队列满了");
                    break;
                }
                Console.WriteLine(i);
            }
        }
    }
}