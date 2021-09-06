using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Night.Core.Client.Phone.Apps.Settings;

namespace Night.Core.Client.Phone.Apps
{
	public class HomeScreen : App
	{
		public List<App> Apps;

		private int virtualIndex;
		private int topLeftIndex;

		public override AppIcon Icon => AppIcon.None;
		public override int DisplayId => 1;

		// todo Localization using Game.GetGXTEntry()
		public override string Name
		{
			get => "HomeScreen";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		public override int SelectedIndex => this.virtualIndex - this.topLeftIndex;

		public HomeScreen(Phone phone, App parent) : base(phone, parent) { }

		public override async Task Update()
		{
			ScrollIfNeeded();
			for (var index = 0; index < 9 && index < this.Apps.Count; index++)
			{
				var realIndex = index + this.topLeftIndex;
				SetHomepageIcon(index, this.Apps[realIndex].Icon, 99, this.Apps[realIndex].Name);
			}
		}

		public override void Initialize()
		{
			this.Apps = new List<App>
			{
				new ContactsApp(this.Phone, this),
				new SettingsApp(this.Phone, this),
				new MessagesApp(this.Phone, this),
				new CameraApp(this.Phone, this),
				new InternetApp(this.Phone, this)
			};
		}

		public override void HandleInput(PhoneInput input)
		{
			switch (input)
			{
				case PhoneInput.Up:
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					if (this.virtualIndex - 3 >= 0) this.virtualIndex -= 3;
					API.MoveFinger((int)FingerDirection.Up);
					break;
				case PhoneInput.Down:
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					if (this.virtualIndex + 3 <= this.Apps.Count - 1) this.virtualIndex += 3;
					API.MoveFinger((int)FingerDirection.Down);
					break;
				case PhoneInput.Left:
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					this.virtualIndex--;
					API.MoveFinger((int)FingerDirection.Left);
					break;
				case PhoneInput.Right:
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					this.virtualIndex++;
					API.MoveFinger((int)FingerDirection.Right);
					break;
				case PhoneInput.Back:
					this.Phone.OpenApp(this.Parent);
					break;
				case PhoneInput.Select:
					API.MoveFinger((int)FingerDirection.Select);
					Game.PlaySound("Menu_Accept", "Phone_SoundSet_Michael");
					this.Phone.OpenApp(this.Apps[this.virtualIndex]);
					break;
				case PhoneInput.Option:
				case PhoneInput.SpecialOption:
					break;
				case PhoneInput.ScrollForward:
					HandleInput(PhoneInput.Right);
					break;
				case PhoneInput.ScrollBackward:
					HandleInput(PhoneInput.Left);
					break;
			}

			this.virtualIndex = MathUtil.Clamp(this.virtualIndex, 0, this.Apps.Count - 1);
		}

		private void SetHomepageIcon(int viewSlot, AppIcon iconId, int notifications, string appName, int opacity = 100)
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT", 1, viewSlot, (int)iconId, notifications, appName, opacity);
		}

		private void ScrollIfNeeded()
		{
			if (this.virtualIndex < this.topLeftIndex)
			{
				this.topLeftIndex -= 3;
			}
			else if (this.virtualIndex >= this.topLeftIndex + 9)
			{
				this.topLeftIndex += 3;
			}
		}
	}
}
