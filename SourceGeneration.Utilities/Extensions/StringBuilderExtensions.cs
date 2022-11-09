using System.Text;

namespace CodeChops.SourceGeneration.Utilities.Extensions;

public static class StringBuilderExtensions
{
	public static StringBuilder TrimEnd(this StringBuilder sb)
	{
		if (sb.Length == 0) return sb;

		var i = sb.Length - 1;

		for (; i >= 0; i--)
			if (!Char.IsWhiteSpace(sb[i]))
				break;

		if (i < sb.Length - 1)
			sb.Length = i + 1;

		return sb;
	}

	public static StringBuilder AppendLineIfNotNull(this StringBuilder sb, Func<string?> textRetriever)
	{
		var text = textRetriever();

		if (text is not null) 
			sb.AppendLine(text);

		return sb;
	}
}
