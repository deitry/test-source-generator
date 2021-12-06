using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MyGenerator
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    class MySyntaxDiagnoster : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RpcRequest";
        private static readonly DiagnosticDescriptor RuleParameter = new DiagnosticDescriptor("RpcRequest01", "Title RpcRequest", "Количество аргументов больше одного", "Arguments", DiagnosticSeverity.Error, isEnabledByDefault: true, description: "RpcRequest must have 1 parameter.");
        private static readonly DiagnosticDescriptor RuleReturn = new DiagnosticDescriptor("RpcRequest02", "Title RpcRequest", "Возвращаемый тип должен быть не void", "Return type", DiagnosticSeverity.Error, isEnabledByDefault: true, description: "RpcRequest must have non-void return type.");


        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is MethodDeclarationSyntax mds)
            {
                foreach (var attributeList in mds.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (attribute.Name.ToString() == RpcRequestGenerator.AttribName)
                        {
                            if (mds.ParameterList.Parameters.Count > 1)
                            {
                                context.ReportDiagnostic(Diagnostic.Create(RuleParameter, context.Node.GetLocation()));
                            }

                            if (mds.ReturnType.ToString() == "void")
                            {
                                context.ReportDiagnostic(Diagnostic.Create(RuleReturn, context.Node.GetLocation()));
                            }
                        }
                    }
                }
            }
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(RuleParameter, RuleReturn); } }
    }

}