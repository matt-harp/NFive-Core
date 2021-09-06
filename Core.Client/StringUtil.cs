using System.Text;

namespace Night.Core.Client
{
	public static class StringUtil
	{
		public static string ToUpperBeforeCapitals(this string text)
		{
			var newText = new StringBuilder(text.Length * 2);
			newText.Append(text[0]);
			for (var i = 1; i < text.Length; i++)
			{
				if (char.IsUpper(text[i]) && text[i - 1] != ' ')
				{
					newText.Append(' ');
				}

				newText.Append(text[i]);
			}

			return newText.ToString();
		}
	}
}
