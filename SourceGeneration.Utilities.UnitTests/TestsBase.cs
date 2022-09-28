namespace CodeChops.SourceGeneration.Utilities.UnitTests;

public abstract class TestsBase
{
	private SyntaxTree SyntaxTree { get; }
	
	protected SemanticModel SemanticModel { get; } 
	
	protected TestsBase(SyntaxTree syntaxTree)
	{
		this.SyntaxTree = syntaxTree;
		var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
		var compilation = CSharpCompilation.Create(
			assemblyName: nameof(TestsBase),
			syntaxTrees: new[] { this.SyntaxTree }, 
			references: new[] { mscorlib });

		this.SemanticModel = compilation.GetSemanticModel(this.SyntaxTree);
	}
}