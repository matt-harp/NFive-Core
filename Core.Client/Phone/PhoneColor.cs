namespace Night.Core.Client.Phone
{
	public struct PhoneColor
	{
		public int R { get; }
		public int G { get; }
		public int B { get; }

		public PhoneColor(int r, int g, int b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}

		public static PhoneColor Red = new PhoneColor(240, 63, 63);
		public static PhoneColor Green = new PhoneColor(34, 139, 34);
		public static PhoneColor Blue = new PhoneColor(102, 165, 229);
	}
}
