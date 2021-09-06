using NFive.SDK.Client.Interface;

namespace Night.Core.Client.Overlays
{
	public class CoreOverlay : Overlay
	{
		public CoreOverlay(IOverlayManager manager) : base(manager) { }

		protected override dynamic Ready()
		{
			On("play", () => { });
			return new
				{ };
		}
	}
}
