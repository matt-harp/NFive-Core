using System;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using Night.Core.Client.Overlays;

namespace Night.Core.Client.Phone.Apps
{
	public class InternetApp : App
	{
		public InternetApp(Phone phone, App parent = null) : base(phone, parent) { }
		public override AppIcon Icon => AppIcon.Eyefind;
		public override int DisplayId => 999;
		public override string Name
		{
			get => "Internet";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		public override async Task Update()
		{

		}

		public override void Initialize()
		{
			CoreService.Instance.Browser.Show();
			API.SetNuiFocus(true, true);
		}

		public override void HandleInput(PhoneInput input) { }
	}
}
