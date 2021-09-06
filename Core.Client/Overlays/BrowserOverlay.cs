using CitizenFX.Core;
using NFive.SDK.Client.Interface;

namespace Night.Core.Client.Overlays
{
	public class BrowserOverlay : Overlay
	{
		public BrowserOverlay(IOverlayManager manager) : base(manager) { }

		protected override dynamic Ready()
		{
			On("play", () => { });
			On("playsound", (dynamic obj) => Audio.PlaySoundFrontend(obj.sound, obj.set));
			On("clickLink", () => Audio.PlaySoundFrontend("Click_Link", "WEB_NAVIGATION_SOUNDS_PHONE"));
			On("click", () => Audio.PlaySoundFrontend("Click_Generic", "WEB_NAVIGATION_SOUNDS_PHONE"));
			return new
			{
				// return config that JS can use with nfive.on('ready', config => {});
			};
		}
	}
}
