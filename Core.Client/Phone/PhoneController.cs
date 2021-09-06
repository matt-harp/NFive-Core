using System.Threading.Tasks;
using CitizenFX.Core;
using MenuAPI;

namespace Night.Core.Client.Phone
{
	public class PhoneController
	{
		public static Phone Phone = new Phone();

		public async Task OnTick()
		{
			if (Game.PlayerPed.IsDead && Phone.IsActive)
			{
				Phone.Close();
				return;
			}

			if (Game.IsControlJustPressed(0, Control.Phone) && !Phone.IsActive && !Game.PlayerPed.IsDead && !MenuController.IsAnyMenuOpen())
			{
				await Phone.Open();
			}

			if (Phone.IsActive)
			{
				await Phone.Update();
			}
		}
	}
}
