namespace Night.Core.Client.Phone
{
	public enum PhoneTheme
	{
		Blue = 1,
		Green = 2,
		Red = 3,
		Orange = 4,
		Grey = 5,
		Purple = 6,
		Pink = 7
	}

	public static class PhoneThemeExtension
	{
		public static string ToDisplayName(this PhoneTheme theme)
		{
			var text = theme.ToString();
			return text.ToUpperBeforeCapitals();
		}
	}
}
