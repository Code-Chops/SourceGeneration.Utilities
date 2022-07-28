namespace CodeChops.SourceGeneration.Utilities.UnitTests;

public class AttributeTests : TestsBase
{
	protected override SyntaxTree SyntaxTree { get; } = CSharpSyntaxTree.ParseText(@"
using System;

namespace CodeChops.Test;

public class AttributeWithoutGenericTypeAttribute : Attribute { }
public class AttributeWithGenericTypeAttribute<T> : Attribute { }

[AttributeWithoutGenericType]
[AttributeWithGenericType<int>]
private partial struct TestClass
{
}
");
	
	private StructDeclarationSyntax StructDeclaration { get; }

	public AttributeTests()
	{
		this.StructDeclaration = this.SyntaxTree.GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>().Single();
	}

	[Fact]
	public void Attribute_IsFound_ViaSyntax()
	{
		var attribute = this.StructDeclaration.AttributeLists
			.SelectMany(attributeList => attributeList.Attributes)
			.SingleOrDefault(attribute => attribute.Name.HasAttributeName("AttributeWithoutGenericType", CancellationToken.None));
		
		Assert.NotNull(attribute);
	}
	
	[Fact]
	public void Attribute_WithGenericParameter_IsFound_ViaSyntax()
	{
		var attribute = this.StructDeclaration.AttributeLists
			.SelectMany(attributeList => attributeList.Attributes)
			.SingleOrDefault(attribute => attribute.Name.HasAttributeName("AttributeWithGenericType", CancellationToken.None));
		
		Assert.NotNull(attribute);
	}
	
	[Fact]
	public void Attribute_IsFound_ViaTypeSymbol()
	{
		var type = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(this.StructDeclaration)!;
		var hasAttribute = type.HasAttribute("AttributeWithoutGenericType", "CodeChops.Test", out _);
		
		Assert.True(hasAttribute);
	}
	
	[Fact]
	public void Attribute_IsFound_ViaTypeSymbol_BecauseOf_IncorrectTypeParamCount()
	{
		var type = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(this.StructDeclaration)!;
		var hasAttribute = type.HasAttribute("AttributeWithoutGenericType", "CodeChops.Test", out _, expectedGenericTypeParamCount: 1);
		
		Assert.False(hasAttribute);
	}

	[Fact]
	public void Attribute_WithGenericParameter_IsFound_ViaTypeSymbol()
	{
		var type = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(this.StructDeclaration)!;
		var hasAttribute = type.HasAttribute("AttributeWithGenericType", "CodeChops.Test", out _, expectedGenericTypeParamCount: 1);
		
		Assert.True(hasAttribute);
	}
	
	[Fact]
	public void Attribute_WithGenericParameter_IsNotFound_ViaTypeSymbol_BecauseOf_IncorrectTypeParamCount()
	{
		var type = (ITypeSymbol)this.SemanticModel.GetDeclaredSymbol(this.StructDeclaration)!;
		var hasAttribute = type.HasAttribute("AttributeWithGenericType", "CodeChops.Test", out _, expectedGenericTypeParamCount: 0);
		
		Assert.False(hasAttribute);
	}
}