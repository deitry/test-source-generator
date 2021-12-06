// See https://aka.ms/new-console-template for more information

using System;
using Rpc;

Console.WriteLine("Hello, World!");
HelloWorldGenerated.HelloWorld.SayHello(); // calls Console.WriteLine("Hello World!") and then prints out syntax trees


partial class Test
{
    [RpcRequest]
    int StartExport(string path)
    {

        return 1;
    }
}