using System;                 // Importing namespace


class Test                    // Class declaration
{
  static void Main()          // Method declaration
  {
    var i=0;
    try  {
        var a=  1/i;
        Console.WriteLine(a);
    }catch{
        throw;
    }
    //int x = 12 * 30;          // Statement 1
    Console.WriteLine ("Hello form mono!");    // Statement 2
  }                           // End of method
}
