using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Night.Core.Client.Phone.Apps.Settings
{
	public class SettingsSubMenuApp : App
	{
		public List<SettingsOption> Options = new List<SettingsOption>();
		public override AppIcon Icon => AppIcon.None;
		public override int DisplayId => 22;
		public override string Name { get; set; } = "Settings";
		public int SelectedOption { get; set; }

		public SettingsSubMenuApp(Phone phone, App parent = null) : base(phone, parent) { }

		public override async Task Update()
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", this.DisplayId);
			for (var index = 0; index < this.Options.Count; index++)
			{
				var option = this.Options[index];
				this.Phone.Scaleform.CallFunction("SET_DATA_SLOT", this.DisplayId, index, index == this.SelectedOption ? (int)SettingsIcon.Ticked : (int)option.Icon, (this.SelectedIndex == index ? "" : "~l~") + option.Name);
			}
		}

		public override void Initialize()
		{
			this.Phone.SetSoftKey(2, SoftKeyIcon.Yes, PhoneColor.Green);
			this.Phone.SetSoftKey(3, SoftKeyIcon.Back, PhoneColor.Red);
		}

		public override void HandleInput(PhoneInput input)
		{
			switch (input)
			{
				case PhoneInput.Up:
				case PhoneInput.ScrollBackward:
					this.SelectedIndex--;
					if (this.SelectedIndex < 0) this.SelectedIndex = this.Options.Count - 1;
					API.MoveFinger((int)FingerDirection.Up);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.Down:
				case PhoneInput.ScrollForward:
					this.SelectedIndex++;
					if (this.SelectedIndex > this.Options.Count - 1) this.SelectedIndex = 0;
					API.MoveFinger((int)FingerDirection.Down);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.Select:
					this.Options[this.SelectedIndex].OnSelected();
					this.SelectedOption = this.SelectedIndex;
					Game.PlaySound("Menu_Accept", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.Back:
					Game.PlaySound("Put_Away", "Phone_SoundSet_Michael");
					this.Phone.OpenApp(this.Parent);
					break;
			}
		}
	}
}
