using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Night.Core.Client.Phone.Apps;
using Night.Core.Client.Phone.Images;
using static CitizenFX.Core.BaseScript;

namespace Night.Core.Client.Phone
{
	public class Phone
	{
		public Scaleform Scaleform;

		public bool IsActive { get; set; }

		public bool Visible = true;

		public List<Message> Messages = new List<Message>
		{

		};

		private App currentApp;

		private readonly List<string> days = new List<string>
		{
			"Sunday",
			"Monday",
			"Tuesday",
			"Wednesday",
			"Thursday",
			"Friday",
			"Saturday"
		};

		private int visibleAnimProgress;
		private int index;


		public async Task Open()
		{
			this.Scaleform = new Scaleform("CELLPHONE_IFRUIT");
			while (!this.Scaleform.IsLoaded) await Delay(0);

			var mainMenu = new HomeScreen(this, null);
			OpenApp(mainMenu);
			this.Scaleform.CallFunction("DISPLAY_VIEW", 1, 0);

			InitSettings();

			API.CreateMobilePhone(0);
			API.SetMobilePhonePosition(55, -27, -60);
			this.visibleAnimProgress = 21;

			API.SetPedConfigFlag(Game.PlayerPed.Handle, 242, false);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 243, false);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 244, true);
			API.N_0x83a169eabcdb10a2(Game.PlayerPed.Handle, API.GetResourceKvpInt("phone:theme") - 1);

			Game.PlaySound("PULL_OUT", "PHONE_SOUNDSET_MICHAEL");
			this.IsActive = true;
			this.Visible = true;
		}

		private void InitSettings()
		{
			this.Scaleform.CallFunction("SET_THEME", API.GetResourceKvpInt("phone:theme"));
			if (string.IsNullOrWhiteSpace(API.GetResourceKvpString("phone:wallpaper")))
			{
				API.SetResourceKvp("phone:wallpaper", Wallpaper.iFruit.Name);
			}
			this.Scaleform.CallFunction("SET_BACKGROUND_CREW_IMAGE", API.GetResourceKvpString("phone:wallpaper"));
			this.Scaleform.CallFunction("SET_SLEEP_MODE", false);
		}

		public async Task Update()
		{
			API.SetMobilePhonePosition(60f, -21f - this.visibleAnimProgress, -60f);
			API.SetMobilePhoneRotation(-90f, this.visibleAnimProgress * 2f, 0f, 0);
			if (this.visibleAnimProgress > 0) this.visibleAnimProgress -= 3;

			var time = World.CurrentDayTime;
			this.Scaleform.CallFunction("SET_TITLEBAR_TIME", time.Hours, time.Minutes, this.days[API.GetClockDayOfWeek()].Substring(0, 3));

			var scumminess = API.GetZoneScumminess(API.GetZoneAtCoords(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z));
			this.Scaleform.CallFunction("SET_SIGNAL_STRENGTH", scumminess);

			int signalStrength;
			switch (scumminess)
			{
				case 0:
				case 1:
					signalStrength = 5;
					break;
				case 2:
				case 3:
					signalStrength = 4;
					break;
				case 4:
					signalStrength = 3;
					break;
				case 5:
					signalStrength = 2;
					break;
				default:
					signalStrength = 3;
					break;
			}

			if (signalStrength < 3 && new Random().Next(8) < 2) signalStrength--;

			this.Scaleform.CallFunction("SET_PROVIDER_ICON", 1, signalStrength);

			HandleInput();

			await this.currentApp.Update();
			this.Scaleform.CallFunction("DISPLAY_VIEW", this.currentApp.DisplayId, this.currentApp.SelectedIndex);

			var renderId = 0;
			API.GetMobilePhoneRenderId(ref renderId);
			API.SetTextRenderId(renderId);
			if(this.Visible)
				API.DrawScaleformMovie(this.Scaleform.Handle, 0.0998f, 0.1775f, 0.1983f, 0.364f, 255, 255, 255, 255, 0); // must be direct api call to work
			API.SetTextRenderId(1);

			var scale = 0f;
			if (API.GetFollowPedCamViewMode() != 4 && this.Visible)
			{
				scale = 285f;
			}

			API.SetMobilePhoneScale(scale);
		}

		public void OpenApp(App app)
		{
			ClearSoftKey(1);
			ClearSoftKey(2);
			ClearSoftKey(3);
			this.currentApp?.Kill();
			this.currentApp = null;
			if (app == null)
			{
				Close();
				return;
			}

			this.currentApp = app;
			this.currentApp.Initialize();
			this.Scaleform.CallFunction("DISPLAY_VIEW", this.currentApp.DisplayId, this.currentApp.SelectedIndex);
		}

		private void HandleInput()
		{
			if (API.IsPauseMenuActive())
			{
				Close();
				return;
			}
			if (Game.IsControlJustPressed(0, Control.PhoneUp))
			{
				this.currentApp.HandleInput(PhoneInput.Up);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneDown))
			{
				this.currentApp.HandleInput(PhoneInput.Down);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneLeft))
			{
				this.currentApp.HandleInput(PhoneInput.Left);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneRight))
			{
				this.currentApp.HandleInput(PhoneInput.Right);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneCancel))
			{
				this.currentApp.HandleInput(PhoneInput.Back);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneSelect))
			{
				this.currentApp.HandleInput(PhoneInput.Select);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneOption))
			{
				this.currentApp.HandleInput(PhoneInput.Option);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneExtraOption))
			{
				this.currentApp.HandleInput(PhoneInput.SpecialOption);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneScrollBackward))
			{
				this.currentApp.HandleInput(PhoneInput.ScrollBackward);
			}

			if (Game.IsControlJustPressed(0, Control.PhoneScrollForward))
			{
				this.currentApp.HandleInput(PhoneInput.ScrollForward);
			}
		}

		public void SetSoftKey(int slot, SoftKeyIcon icon, PhoneColor color)
		{
			this.Scaleform.CallFunction("SET_SOFT_KEYS", slot, true, (int)icon);
			this.Scaleform.CallFunction("SET_SOFT_KEYS_COLOUR", slot, color.R, color.G, color.B); // yuck. colo'u'r
		}

		public void SetSoftKey(int slot, SoftKeyIcon icon)
		{
			this.Scaleform.CallFunction("SET_SOFT_KEYS", slot, true, (int)icon);
			this.Scaleform.CallFunction("SET_SOFT_KEYS_COLOUR", slot, 255, 255, 255); // yuck. colo'u'r
		}

		public void ClearSoftKey(int slot)
		{
			this.Scaleform.CallFunction("SET_SOFT_KEYS", slot, false, 0);
		}

		public void Close()
		{
			this.Scaleform.CallFunction("SHUTDOWN_MOVIE");
			Game.PlaySound("Put_Away", "Phone_SoundSet_Michael");
			API.DestroyMobilePhone();
			this.IsActive = false;
		}
	}
}
