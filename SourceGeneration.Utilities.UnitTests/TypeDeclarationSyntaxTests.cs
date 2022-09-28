namespace CodeChops.SourceGeneration.Utilities.UnitTests;

public class TypeDeclarationSyntaxTests
{
	[Fact]
	public void ClassGenericConstraints_IsRetrieved_Correctly()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(@"
using System;

namespace CodeChops.Test;

public record RecordWithConstraints<T1, T2>
	where T1 : class,
	where T2 : notnull
{
}

public class ClassWithoutConstraints
{
}
");
		var recordCreationSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<RecordDeclarationSyntax>().Single();	
		var classCreationSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();

		var recordConstraint = recordCreationSyntax.GetClassGenericConstraints();
		var classConstraint = classCreationSyntax.GetClassGenericConstraints();
		
		Assert.Equal(
@"	where T1 : class,
	where T2 : notnull
", recordConstraint);
		
		Assert.Null(classConstraint);
	}
	
	[Fact]
	public void ClassUsings_IsRetrieved_Correctly()
	{
		var syntaxTree = CSharpSyntaxTree.ParseText(@"
using System;
using System.Buffers;

namespace CodeChops.Test;

public class ClassWithoutConstraints
{
}
");
		
		var classCreationSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
		
		var usings = classCreationSyntax.GetUsings().ToList();
		
		Assert.Contains("using System.Buffers;", usings);
		Assert.Contains("using System;", usings);
	}
}