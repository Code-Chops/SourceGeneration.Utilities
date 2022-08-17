using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeChops.SourceGeneration.Utilities;

public static class FileNameHelpers
{
	/// <summary>
	/// Removes the root namespace of the source assembly from the file name.
	/// <para>
	/// For this to work, add: &lt;CompilerVisibleProperty Include="RootNamespace"/&gt; to an ItemGroup in the source assembly.
	/// </para>
	/// </summary>
	/// <param name="fileName">The file name without the extension.</param>
	public static string GetFileName(string fileName, AnalyzerConfigOptionsProvider configOptionsProvider)
	{
		configOptionsProvider.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);
		
		if (rootNamespace is not null && fileName.Substring(0, rootNamespace.Length) == rootNamespace)
			fileName = fileName.Substring(rootNamespace.Length + 1);

		return $"{fileName}.g.cs";
	}
}