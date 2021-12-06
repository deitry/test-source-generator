// using Rpc;

using System;
using System.Text;
using Rpc;

namespace TestFrameworkApp
{

    internal class Program
    {
        public static void Main(string[] args)
        {
            HelloWorldGenerated.HelloWorld.SayHello(); // calls Console.WriteLine("Hello World!") and then prints out syntax trees
            var my = new MyClass();
            var result = my.RpcRequest_abc(Encoding.UTF8.GetBytes("da"));

            Console.WriteLine(result);
        }
    }


}

public partial class MyClass
{
    [RpcRequest]
    public int abc(string s)
    {
        return 5;
    }
}
