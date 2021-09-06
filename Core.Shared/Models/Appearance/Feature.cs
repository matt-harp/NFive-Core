﻿namespace Night.Core.Shared.Models.Appearance
{
	public class Feature
	{
		public FeatureType Type { get; set; }

		public int Index { get; set; }

		public float Opacity { get; set; }

		public FeatureColorType ColorType { get; set; }

		public int ColorId { get; set; }

		public int SecondColorId { get; set; }
	}
}
