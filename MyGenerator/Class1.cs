using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace MyGenerator
{
    [Generator]
    public class Class1 : ISourceGenerator
    {
        internal const string AttribName = "RpcRequest";
        public const string RpcRequestAttribContent = @"
namespace Rpc
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public sealed class RpcRequestAttribute : System.Attribute {
        public RpcRequestAttribute() {}
    }
}
";
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            // while (!System.Diagnostics.Debugger.IsAttached)
            //     System.Threading.Thread.Sleep(500);
#endif

            // Register a factory that can create our custom syntax receiver
            context.RegisterForSyntaxNotifications(() => new MySyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
#if DEBUG
            // while (!System.Diagnostics.Debugger.IsAttached)
            //     System.Threading.Thread.Sleep(500);
#endif
            context.AddSource(nameof(RpcRequestAttribContent), RpcRequestAttribContent);

            MySyntaxReceiver rx = (MySyntaxReceiver)context.SyntaxContextReceiver!;
            foreach (var request in rx.Requests)
            {

                // TODO: template serialize/deserialize methods

                var generated =
@$"
partial class {request.ClassName}
{{
    byte[] RpcRequest_{request.Name}(byte[] request) 
    {{
        var data = System.BitConverter.ToString(request);
        return System.BitConverter.GetBytes({request.Name}(data));
    }}
}}";
                context.AddSource($"Mustache{request.Name}", generated);
            }
        }
    }

    record RpcRequest
    {
        public string Name = string.Empty;
        public string TIn = string.Empty;
        public string TOut = string.Empty;
        public string ClassName = string.Empty;
    }

    class MySyntaxReceiver : ISyntaxContextReceiver
    {
        public List<RpcRequest> Requests = new List<RpcRequest>();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            // find all valid RpcRequest attributes
            if (context.Node is AttributeSyntax attrib
                && context.SemanticModel.GetTypeInfo( attrib).Type?.ToDisplayString() == Class1.AttribName)
            {

            }

            if (context.Node is MethodDeclarationSyntax mds)
            {
                foreach (var attributeList in mds.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (attribute.Name.ToString() == Class1.AttribName)
                        {
                            if (mds.ParameterList.Parameters.Count == 1
                                && mds.ReturnType.ToString() != "void"
                            )
                            {
                                Requests.Add(new RpcRequest()
                                {
                                    Name = mds.Identifier.Text,
                                    TIn = mds.ParameterList.Parameters[0].Type?.ToString() ?? string.Empty,
                                    TOut = mds.ReturnType.ToString(),
                                    ClassName = (mds.Parent as ClassDeclarationSyntax)?.Identifier.Text ?? "",
                                });
                            }
                        }
                    }
                }
            }
        }
    }
}

