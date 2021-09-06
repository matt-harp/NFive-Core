using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace Night.Core.Client
{
	public class MapManager
	{
		public delegate void MapCycle(int index);

		private DateTime holdStart;
		private bool canHold = true;
		private bool enabled;

		public event MapCycle OnMapCycle;

		public async void OnTick()
		{
			Minimap();
			if (Game.IsControlJustPressed(0, Control.MultiplayerInfo))
			{
				PlayerBlips();
			}

			if (Game.IsControlJustPressed(0, Control.Detonate))
			{
				Screen.ShowNotification(Game.PlayerPed.AttachedBlips[0].Sprite.ToString());
			}
		}

		private void PlayerBlips() { }

		private void Minimap()
		{
			if (API.IsControlPressed(0, (int)Control.MultiplayerInfo))
			{
				if (this.holdStart == default) this.holdStart = DateTime.UtcNow;
				if (this.canHold)
				{
					if ((DateTime.UtcNow - this.holdStart).TotalMilliseconds >= 500)
					{
						API.PlaySoundFrontend(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
						if (API.IsBigmapActive())
						{
							API.SetBigmapActive(false, false);
							this.OnMapCycle?.Invoke(0);
						}
						else
						{
							API.SetBigmapActive(true, false);
							this.OnMapCycle?.Invoke(1);
						}

						this.enabled = !this.enabled;
						this.canHold = false;
					}
				}
			}
			else
			{
				this.holdStart = default;
				this.canHold = true;
			}
		}
	}
}
