using System;
using System.Threading;

namespace ConsoleApplication2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var array = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var array2 = new[] {array, array, array, array, array, array, array, array, array, array};

            var start = DateTime.Now;

            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array2.Length; j++)
                {
                    var a=array2[j][i];
                }
            }

            Console.WriteLine("纵向遍历耗时："+(DateTime.Now-start).Ticks);//二维数组纵向遍历
            
            var start1 = DateTime.Now;

            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array2.Length; j++)
                {
                    var a = array2[i][j];
                }
            }

            Console.WriteLine("横向遍历耗时："+(DateTime.Now-start1).Ticks);//二维数组横向遍历
        }
    }
}