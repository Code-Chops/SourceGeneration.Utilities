namespace CodeChops.SourceGeneration.Utilities.UnitTests;

public class TypeSymbolTests : TestsBase
{
	private static readonly SyntaxTree SyntaxTree = CSharpSyntaxTree.ParseText(@"
using System;

namespace CodeChops.Test;

private abstract partial record TestRecord<T>
{
}

public struct TestStruct
{
}

internal enum TestEnum
{
} 

public static class TestClass
{
}
");

	public TypeSymbolTests()
		: base(SyntaxTree)
	{
	}
	
	[Fact]
	public void FullTypeName_IsRetrieved_Correctly()
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Single();
		var namedTypeSymbol = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;

		var fullTypeName = namedTypeSymbol.GetFullTypeNameWithGenericParameters();
		
		Assert.Equal("global::CodeChops.Test.TestRecord<T>", fullTypeName);
	}
	
	[Fact]
	public void FullTypeName_WithoutGenericParameters_IsRetrieved_Correctly()
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Single();
		var namedTypeSymbol = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;

		var fullTypeName = namedTypeSymbol.GetFullTypeNameWithoutGenericParameters();
		
		Assert.Equal("global::CodeChops.Test.TestRecord", fullTypeName);
	}
	
	[Fact]
	public void TypeName_WithGenericParameters_IsRetrieved_Correctly()
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Single();
		var namedTypeSymbol = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;

		var fullTypeName = namedTypeSymbol.GetTypeNameWithGenericParameters();
		
		Assert.Equal("TestRecord<T>", fullTypeName);
	}

	[Theory]
	[InlineData(typeof(RecordDeclarationSyntax), 	"private abstract record class",			false)]
	[InlineData(typeof(StructDeclarationSyntax), 	"public struct",							false)]
	[InlineData(typeof(EnumDeclarationSyntax),	 	"internal enum",							false)]
	[InlineData(typeof(ClassDeclarationSyntax),		"public static class",						false)]
	[InlineData(typeof(RecordDeclarationSyntax), 	"private abstract partial record class",	true)]
	[InlineData(typeof(StructDeclarationSyntax), 	"public partial struct",					true)]
	[InlineData(typeof(EnumDeclarationSyntax),	 	"internal enum",							true)]
	[InlineData(typeof(ClassDeclarationSyntax),		"public static partial class",				true)]
	public void ClassDefinition_IsRetrieved_Correctly(Type type, string expectedDefinition, bool makePartial)
	{
		var objectCreationSyntax = SyntaxTree.GetRoot().DescendantNodes().Single(node => node.GetType() == type);
		var namedTypeSymbol = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(objectCreationSyntax)!;
		
		var definition = namedTypeSymbol.GetObjectDeclaration(makePartial);
		
		Assert.Equal(expectedDefinition, definition);
	}
}