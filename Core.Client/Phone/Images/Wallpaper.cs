using System.Collections.Generic;

namespace Night.Core.Client.Phone.Images
{
	public sealed class Wallpaper : PhoneImage
	{
		public static readonly List<Wallpaper> Wallpapers = new List<Wallpaper>
		{
			iFruit,
			Badger,
			Bittersweet,
			PurpleGlow,
			GreenSquares,
			OrangeHerringBone,
			OrangeHalftone,
			GreenTriangles,
			GreenShards,
			BlueAngles,
			BlueShards,
			BlueTriangles,
			BlueCircles,
			Diamonds,
			GreenGlow,
			Orange8Bit,
			OrangeTriangles,
			PurpleTartan
		};

		public int Index { get; }
		public string DisplayName { get; }

		public static Wallpaper iFruit => new Wallpaper("iFruit", "Phone_Wallpaper_ifruitdefault");
		public static Wallpaper Badger => new Wallpaper("Badger", "Phone_Wallpaper_badgerdefault");
		public static Wallpaper Bittersweet => new Wallpaper("Bittersweet", "Phone_Wallpaper_bittersweet_b");
		public static Wallpaper PurpleGlow => new Wallpaper("PurpleGlow", "Phone_Wallpaper_purpleglow");
		public static Wallpaper GreenSquares => new Wallpaper("GreenSquares", "Phone_Wallpaper_greensquares");
		public static Wallpaper OrangeHerringBone => new Wallpaper("OrangeHerringBone", "Phone_Wallpaper_orangeherringbone");
		public static Wallpaper OrangeHalftone => new Wallpaper("OrangeHalftone", "Phone_Wallpaper_orangehalftone");
		public static Wallpaper GreenTriangles => new Wallpaper("GreenTriangles", "Phone_Wallpaper_greentriangles");
		public static Wallpaper GreenShards => new Wallpaper("GreenShards", "Phone_Wallpaper_greenshards");
		public static Wallpaper BlueAngles => new Wallpaper("BlueAngles", "Phone_Wallpaper_blueangles");
		public static Wallpaper BlueShards => new Wallpaper("BlueShards", "Phone_Wallpaper_blueshards");
		public static Wallpaper BlueTriangles => new Wallpaper("BlueTriangles", "Phone_Wallpaper_bluetriangles");
		public static Wallpaper BlueCircles => new Wallpaper("BlueCircles", "Phone_Wallpaper_bluecircles");
		public static Wallpaper Diamonds => new Wallpaper("Diamonds", "Phone_Wallpaper_diamonds");
		public static Wallpaper GreenGlow => new Wallpaper("GreenGlow", "Phone_Wallpaper_greenglow");
		public static Wallpaper Orange8Bit => new Wallpaper("Orange8Bit", "Phone_Wallpaper_orange8bit");
		public static Wallpaper OrangeTriangles => new Wallpaper("OrangeTriangles", "Phone_Wallpaper_orangetriangles");
		public static Wallpaper PurpleTartan => new Wallpaper("PurpleTartan", "Phone_Wallpaper_purpletartan");

		private Wallpaper(string displayName, string txd) : base(txd)
		{
			this.DisplayName = displayName;
		}
	}
}
