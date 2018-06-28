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

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create<DiagnosticDescriptor>(); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeControllerPostfix, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeAuthorizedController, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeEntities, SymbolKind.NamedType);
        }

        private static void AnalyzeControllerPostfix(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var mvcController = context.Compilation.GetTypeByMetadataName("System.Web.Mvc.Controller");

            if (!IsClassNameInheritedFromMvcControllerEndsWithController(namedTypeSymbol,mvcController))
            {
                var diagnosticRule = GetDiagnosticRuleForControllerThatNotEndsWithControllerPostfix(namedTypeSymbol);
                context.ReportDiagnostic(diagnosticRule);
            }
        }

        private static bool IsClassNameInheritedFromMvcControllerEndsWithController(INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol mvcController)
        {
            return namedTypeSymbol.BaseType.Equals(mvcController) && namedTypeSymbol.Name.EndsWith("Controller");
        }

        private static Diagnostic GetDiagnosticRuleForControllerThatNotEndsWithControllerPostfix(INamedTypeSymbol namedTypeSymbol)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(
                        DiagnosticId,
                        "Controller inherited from System.Web.Mvc.Controller must contain suffix 'Controller'",
                        "Controller inherited from System.Web.Mvc.Controller must contain suffix 'Controller'",
                        Category,
                        DiagnosticSeverity.Warning,
                         true,
                         "Add suffix 'Controller' to class name"), 
                     namedTypeSymbol.Locations[0],
                     namedTypeSymbol.Name
            );
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
                    var diagnosticRule = GetDiagnosticRuleForControllerThatNotAuthorizedCompletely(namedTypeSymbol);
                    context.ReportDiagnostic(diagnosticRule);
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
                if (controllerMethods.Any(method => !IsSymbolHasAttribute(method, attribute)))
                    return false;
            }
            return true;
        }

        private static Diagnostic GetDiagnosticRuleForControllerThatNotAuthorizedCompletely(INamedTypeSymbol namedTypeSymbol)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                        "Controller or all controller methods must marked with attribute [Authorize]",
                        "Controller or all controller methods must marked with attribute [Authorize]",
                        Category,
                        DiagnosticSeverity.Warning,
                         true
                        ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
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
            if (IsEntitiesNamespace(namedTypeSymbol))
            {
                if (!IsPublicEntity(namedTypeSymbol))
                {
                    var diagnosticRule = GetDiagnosticRuleForNonPublicEntity(namedTypeSymbol);
                    context.ReportDiagnostic(diagnosticRule);
                }
                
                if (!IsEntityContainIdAndNameProperties(namedTypeSymbol))
                {
                    var diagnosticRule = GetDiagnosticRuleForEntityWithoutIdAndName(namedTypeSymbol);
                    context.ReportDiagnostic(diagnosticRule);
                }

                var dataContractAttribute = context.Compilation.GetTypeByMetadataName("System.Runtime.Serialization.DataContractAttribute");
                if (!IsSymbolHasAttribute(namedTypeSymbol, dataContractAttribute))
                {
                    var diagnosticRule = GetDiagnosticRuleForEntityWithoutDataContractAttribute(namedTypeSymbol);
                    context.ReportDiagnostic(diagnosticRule);
                }
            }
        }

        private static bool IsEntitiesNamespace(INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol.ContainingNamespace.Name.Equals("Entities");
        }

        private static bool IsPublicEntity(INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol.DeclaredAccessibility == Accessibility.Public;
        }

        private static Diagnostic GetDiagnosticRuleForNonPublicEntity(INamedTypeSymbol namedTypeSymbol)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                           "Entity type must be public",
                           "Entity type must be public",
                           Category,
                           DiagnosticSeverity.Warning,
                            true
                           ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
        }

        private static IEnumerable<string> GetEntityProperties(INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol.GetMembers()
                                  .Where(member => member.Kind == SymbolKind.Property)
                                  .Select(property => property.Name);
        }

        private static bool IsEntityContainIdAndNameProperties(INamedTypeSymbol namedTypeSymbol)
        {
            var entityProperties = GetEntityProperties(namedTypeSymbol);
            return entityProperties.Contains("Id") && entityProperties.Contains("Name");
        }

        private static Diagnostic GetDiagnosticRuleForEntityWithoutIdAndName(INamedTypeSymbol namedTypeSymbol)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                          "Entity must has Id and Name properties",
                          "Entity must has Id and Name properties",
                          Category,
                          DiagnosticSeverity.Warning,
                           true
                          ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
        }

        private static Diagnostic GetDiagnosticRuleForEntityWithoutDataContractAttribute(INamedTypeSymbol namedTypeSymbol)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(DiagnosticId,
                          "Entity must has attribute [DataContractAttribute]",
                          "Entity must has attribute [DataContractAttribute]",
                          Category,
                          DiagnosticSeverity.Warning,
                           true
                          ), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
        }
    }
}
