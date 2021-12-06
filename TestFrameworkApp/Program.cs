using Rpc;

namespace TestFrameworkApp
{

    internal class Program
    {
        public static void Main(string[] args)
        {

        }
    }


    class MyClass
    {
        [RpcRequest]
        int a(string s)
        {
            return 0;
        }
    }
}

namespace Rpc
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public sealed class RpcRequestAttribute : System.Attribute
    {
        public RpcRequestAttribute () {}
    }
}