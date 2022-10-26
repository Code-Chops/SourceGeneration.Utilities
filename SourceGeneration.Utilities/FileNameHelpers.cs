using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeChops.SourceGeneration.Utilities;

public static class FileNameHelpers
{
	/// <summary>
	/// <list type="bullet">
	/// <item>Removes 'global::' from the start of the name if it exists.</item>
	/// <item>Replaces invalid file name characters to '_'.</item>
	/// <item>Removes the root namespace of the source assembly from the start file name.</item>
	/// <item>Adds suffix '.g.cs' to the file name, if needed.</item>
	/// </list>
	/// <para>
	/// For this to work, add: &lt;CompilerVisibleProperty Include="RootNamespace"/&gt; to an ItemGroup in the project file of the source generator.
	/// </para>
	/// </summary>
	/// <param name="fileName">The file name without the extension.</param>
	public static string GetFileName(string fileName, AnalyzerConfigOptionsProvider configOptionsProvider)
	{
		fileName = fileName.StartsWith("global::") 
			? fileName.Substring(8) 
			: fileName;
		
		var buffer = new char[fileName.Length];
		var i = 0;
		var invalidCharacters = Path.GetInvalidFileNameChars();

		foreach (var c in fileName)
		{
			var newCharacter = c;
			if (invalidCharacters.Contains(c)) newCharacter = '_';
			buffer[i] = newCharacter;
			i++;
		}

		fileName = new String(buffer);
		
		configOptionsProvider.GlobalOptions.TryGetValue("build_property.RootNamespace", out var rootNamespace);
		
		if (rootNamespace is not null && fileName.Length > rootNamespace.Length && fileName.Substring(0, rootNamespace.Length + 1) == $"{rootNamespace}.")
			fileName = fileName.Substring(rootNamespace.Length + 1);

		return fileName.EndsWith(".cs")
			? fileName 
			: $"{fileName}.g.cs";
	}
}
