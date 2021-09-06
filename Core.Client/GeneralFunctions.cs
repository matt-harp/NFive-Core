using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace Night.Core.Client
{
	public static class GeneralFunctions
	{
		public static async void Teleport(Vector3 position, bool useSwitchAnimation)
		{
			if (useSwitchAnimation)
			{
				var to = await World.CreatePed(Game.PlayerPed.Model, position, Game.PlayerPed.Heading);
				to.IsVisible = false;
				API.SwitchOutPlayer(Game.PlayerPed.Handle, 0, 1);
				while (API.GetPlayerSwitchState() != 5) await BaseScript.Delay(1);

				API.SwitchInPlayer(to.Handle);
				while (API.GetPlayerSwitchState() != 8) await BaseScript.Delay(1);

				API.SetEntityCoords(Game.PlayerPed.Handle, position.X, position.Y, position.Z, false, false, false, true);
				to.MarkAsNoLongerNeeded();
			}
			else
			{
				API.SetEntityCoords(Game.PlayerPed.Handle, position.X, position.Y, position.Z, false, false, false, true);
			}
		}

		public static void ShowNotification(string message, bool blinking)
		{
			Screen.ShowNotification(message, blinking);
		}
	}
}
