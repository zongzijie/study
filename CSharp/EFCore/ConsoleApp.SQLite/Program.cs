using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleApp.SQLite
{
    public class Program
    {
        public static void Main()
        {
//            using (var db = new BloggingContext())
//            {
//                db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet",Posts = new List<Post>{new Post
//                {
//                 Content = "12",Title = "sdf"
//                }}});
//                var count = db.SaveChanges();
//                Console.WriteLine("{0} records saved to database", count);
//
//                Console.WriteLine();
//                Console.WriteLine("All blogs in database:");
//                foreach (var blog in db.Blogs)
//                {
//                    Console.WriteLine(" - {0}", blog.Url);
//                }
//                foreach (var post in db.Posts)
//                {
//                    Console.WriteLine(" url- {0},Blog.blogid-{1},blogid-{2}", post.Blog.Url,post.Blog.BlogId,post.BlogId);
//                }
//            }
//            int? i = 8;
//            object a = i;
//            a = 4;
//            object b = a;
//            b = 5;
//            Console.WriteLine(i.GetType());
//            Console.WriteLine(i);
//            Console.WriteLine(a.GetType());
//            Console.WriteLine(a);
//            Console.WriteLine(b.GetType());
//            Console.WriteLine(b);
//            var name = "www.w3cschool.cn"; 
//            var age = 25; 
//            var isRabbit = true; 
//
//            Type nameType = name.GetType(); 
//            Type ageType = age.GetType(); 
//            Type isRabbitType = isRabbit.GetType(); 
//
//            Console.WriteLine("name is type " + nameType.ToString()); 
//            Console.WriteLine("age is type " + ageType.ToString()); 
//            Console.WriteLine("isRabbit is type " + isRabbitType.ToString()); 
//            int? i = 3;
//            var type=i.GetType();
////            Console.WriteLine(type);
//            string color = "F5545A";
//            int colorint = 0xF5545A; //int.Parse(color);
//            Console.WriteLine(colorint.ToString());
            
            Dictionary<ExtendObject,ExtendObject> dic=new Dictionary<ExtendObject, ExtendObject>();
            ExtendObject a=new ExtendObject();
            dic[a] = a;
            dic[a] = a;

        }
    }

    public  class ExtendObject
    {
        
        public override bool Equals(object obj)
        {
            return false;
        }
    }
}323