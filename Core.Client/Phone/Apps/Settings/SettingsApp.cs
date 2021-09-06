using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Night.Core.Client.Phone.Images;

namespace Night.Core.Client.Phone.Apps.Settings
{
	public class SettingsApp : App
	{
		private readonly List<Setting> settings = new List<Setting>();

		public override AppIcon Icon => AppIcon.Settings;

		public override int DisplayId => 22;

		public override string Name
		{
			get => "Settings";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		public SettingsApp(Phone phone, App parent = null) : base(phone, parent) { }

		public override async Task Update()
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", this.DisplayId);
			for (var i = 0; i < this.settings.Count; i++) AddSetting(i, this.settings[i]);
		}

		public override void Initialize()
		{
			this.settings.Clear();
			this.settings.Add(new Setting
			{
				Name = "Theme",
				Icon = SettingsIcon.Edit,
				OnSelected = () =>
				{
					var sub = new SettingsSubMenuApp(this.Phone, this)
					{
						Name = "Theme",
						SelectedOption = API.GetResourceKvpInt("phone:theme") - 1,
						Options = new List<SettingsOption>()
					};

					foreach (PhoneTheme theme in Enum.GetValues(typeof(PhoneTheme)))
						sub.Options.Add(new SettingsOption
						{
							Name = theme.ToDisplayName(),
							Icon = SettingsIcon.Edit,
							OnSelected = () =>
							{
								API.SetResourceKvpInt("phone:theme", (int)theme);
								this.Phone.Scaleform.CallFunction("SET_THEME", (int)theme);
								API.N_0x83a169eabcdb10a2(Game.PlayerPed.Handle, (int)theme - 1);
							}
						});

					this.Phone.OpenApp(sub);
				}
			});

			this.settings.Add(new Setting
			{
				Name = "Background",
				Icon = SettingsIcon.Edit,
				OnSelected = () =>
				{
					var sub = new SettingsSubMenuApp(this.Phone, this)
					{
						Name = "Background",
						SelectedOption = Wallpaper.Wallpapers.FindIndex(w => w.Name == API.GetResourceKvpString("phone:wallpaper")),
						Options = new List<SettingsOption>()
					};

					foreach (var wallpaper in Wallpaper.Wallpapers)
						sub.Options.Add(new SettingsOption
						{
							Name = wallpaper.DisplayName,
							Icon = SettingsIcon.Edit,
							OnSelected = () =>
							{
								API.SetResourceKvp("phone:wallpaper", wallpaper.Name);
								this.Phone.Scaleform.CallFunction("SET_BACKGROUND_CREW_IMAGE", wallpaper.Name);
							}
						});

					this.Phone.OpenApp(sub);
				}
			});
		}

		public override void HandleInput(PhoneInput input)
		{
			switch (input)
			{
				case PhoneInput.Back:
					Game.PlaySound("Put_Away", "Phone_SoundSet_Michael");
					this.Phone.OpenApp(this.Parent);
					break;
				case PhoneInput.ScrollForward:
				case PhoneInput.Down:
					this.SelectedIndex++;
					if (this.SelectedIndex > this.settings.Count - 1) this.SelectedIndex = 0;
					API.MoveFinger((int)FingerDirection.Down);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.ScrollBackward:
				case PhoneInput.Up:
					this.SelectedIndex--;
					if (this.SelectedIndex < 0) this.SelectedIndex = this.settings.Count - 1;
					API.MoveFinger((int)FingerDirection.Up);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.Select:
					this.settings[this.SelectedIndex].OnSelected();
					Game.PlaySound("Menu_Accept", "Phone_SoundSet_Michael");
					break;
			}
		}

		private void AddSetting(int index, Setting setting)
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT", 22, index, (int)setting.Icon, (this.SelectedIndex == index ? "" : "~l~") + setting.Name);
		}
	}

	public class Setting
	{
		public Action OnSelected;
		public string Name { get; set; }
		public SettingsIcon Icon { get; set; }
	}
}
