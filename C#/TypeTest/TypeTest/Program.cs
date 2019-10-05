using System;

namespace TypeTest
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      object o=new MyClass();
      Console.WriteLine(typeof (DomainService).IsAssignableFrom(o.GetType()));
    }
  }

  class MyClass:DomainService,IInterface2
  {
    
  }
  abstract class  DomainService:IInterface1
  {
    
  }
  interface IInterface1
  {
    
  }
  interface IInterface2
  {
    
  }
  interface IInterface
  {
    
  }
}