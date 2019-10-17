using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<Guid, Guid> dic = new Dictionary<Guid, Guid>();
            for (int i = 0; i < 5000; i++)
            {
                dic.Add(Guid.NewGuid(), Guid.NewGuid());
            }

            dic.Clear();
        }
    }
}