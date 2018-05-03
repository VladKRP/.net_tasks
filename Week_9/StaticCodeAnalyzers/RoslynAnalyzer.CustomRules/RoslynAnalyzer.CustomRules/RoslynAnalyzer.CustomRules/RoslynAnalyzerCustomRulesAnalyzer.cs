using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynAnalyzer.CustomRules
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RoslynAnalyzerCustomRulesAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RoslynAnalyzerCustomRules";
        private const string Category = "Naming";

        //private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        //private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        private static readonly string ControllerNamingTitle = "Controller inherited from System.Web.Mvc.Controller must contain suffix 'Controller'";
        private static DiagnosticDescriptor ControllerNamingRule = new DiagnosticDescriptor(
                    DiagnosticId,
                    ControllerNamingTitle,
                    ControllerNamingTitle,
                    Category,
                    DiagnosticSeverity.Warning,
                     true,
                     "Add suffix 'Controller' to class name"
            );


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(ControllerNamingRule); } }

        public override void Initialize(AnalysisContext context)
        {
            //context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeControllerSuffix, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeAuthorizedController, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeEntities, SymbolKind.NamedType);
        }

        //private static void AnalyzeSymbol(SymbolAnalysisContext context)
        //{
        //    // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
        //    var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

        //    // Find just those named type symbols with names containing lowercase letters.
        //    if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
        //    {
        //        // For all such symbols, produce a diagnostic.
        //        var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

        //        context.ReportDiagnostic(diagnostic);
        //    }
        //}


        private static void AnalyzeControllerSuffix(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var mvcController = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.Controller");

            if (namedTypeSymbol.BaseType.Equals(mvcController) && !namedTypeSymbol.Name.EndsWith("Controller"))
            {
                var diagnostic = Diagnostic.Create(ControllerNamingRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeAuthorizedController(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var authorizeAttribute = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.AuthorizeAttribute");
            var mvcController = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.Controller");

            if (namedTypeSymbol.BaseType.Equals(mvcController))
            {
                var isCompletelyAuthorizedController = IsControllerOrAllHisMethodsHasAttribute(namedTypeSymbol, authorizeAttribute);
                if (!isCompletelyAuthorizedController)
                {
                    var diagnostic = Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                        "Controller or all controller methods must marked with attribute [Authorize]",
                        "Controller or all controller methods must marked with attribute [Authorize]",
                        Category,
                        DiagnosticSeverity.Warning,
                         true
                        ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }



        }

        private static bool IsControllerOrAllHisMethodsHasAttribute(INamedTypeSymbol symbol, INamedTypeSymbol attribute)
        {
            if (IsSymbolHasAttribute(symbol, attribute))
                return true;
            else
            {
                var controllerMethods = symbol.GetMembers().Where(member => member.Kind == SymbolKind.Method && member.MetadataName != ".ctor");
                if (controllerMethods.All(method => IsSymbolHasAttribute(method, attribute)))
                    return true;
            }
            return false;
        }

        private static bool IsSymbolHasAttribute(ISymbol symbol, INamedTypeSymbol attribute)
        {
            var symbolAttributes = symbol.GetAttributes();
            if (symbolAttributes.Any(cattribute => cattribute.AttributeClass == attribute.OriginalDefinition))
                return true;
            return false;
        }

        private static void AnalyzeEntities(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            if (namedTypeSymbol.ContainingNamespace.Name.Equals("Entities"))
            {
                if (namedTypeSymbol.DeclaredAccessibility != Accessibility.Public)
                {
                    var diagnostic = Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                           "Entity type must be public",
                           "Entity type must be public",
                           Category,
                           DiagnosticSeverity.Warning,
                            true
                           ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }

                var entityProperties = namedTypeSymbol.GetMembers()
                                                      .Where(member => member.Kind == SymbolKind.Property).Select(property => property.Name);
                if (!entityProperties.Contains("Id") || !entityProperties.Contains("Name"))
                {
                    var diagnostic = Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                          "Entity must has Id and Name properties",
                          "Entity must has Id and Name properties",
                          Category,
                          DiagnosticSeverity.Warning,
                           true
                          ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }

                var dataContractAttribute = context.Compilation.GetTypeByMetadataName("System.Runtime.Serialization.DataContractAttribute");
                if (!IsSymbolHasAttribute(namedTypeSymbol, dataContractAttribute))
                {
                    var diagnostic = Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                          "Entity must has attribute [DataContractAttribute]",
                          "Entity must has attribute [DataContractAttribute]",
                          Category,
                          DiagnosticSeverity.Warning,
                           true
                          ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

    }
}
