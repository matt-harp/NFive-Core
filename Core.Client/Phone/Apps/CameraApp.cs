using System;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using static CitizenFX.Core.BaseScript;

namespace Night.Core.Client.Phone.Apps
{
	public class CameraApp : App
	{
		public override AppIcon Icon => AppIcon.Camera;
		public override int DisplayId => -1;

		public override string Name
		{
			get => "Camera";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		private Scaleform buttons;
		private Scaleform camera;

		private bool gridShown = true;
		private bool dofEnabled;
		private bool focusLock;
		private bool selfieMode;

		public CameraApp(Phone phone, App parent = null) : base(phone, parent) { }

		public override async Task Update()
		{
			API.HideHudComponentThisFrame(6);
			API.HideHudComponentThisFrame(7);
			API.HideHudComponentThisFrame(8);
			API.HideHudComponentThisFrame(9);
			API.HideHudComponentThisFrame(19);
			API.HideHudAndRadarThisFrame();

			HandlePhoneInput();
			if (this.buttons.IsLoaded)
			{
				this.buttons.Render2D();
			}
			if (this.camera.IsLoaded)
			{
				this.camera.Render2D();
			}

			Function.Call(Hash.SET_FACIAL_IDLE_ANIM_OVERRIDE, Game.PlayerPed.Handle, "mood_Angry_1");
		}

		public override async void Initialize()
		{
			this.buttons = new Scaleform("INSTRUCTIONAL_BUTTONS");
			while (!this.buttons.IsLoaded) await Delay(0);
			SetupInstructionalButtons();

			this.camera = new Scaleform("CAMERA_GALLERY");
			while (!this.camera.IsLoaded) await Delay(0);
			this.camera.CallFunction("DISPLAY_VIEW", 2);

			API.CellCamActivate(true, true);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 242, true);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 243, true);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 244, false);

			this.Phone.Scaleform.CallFunction("DISPLAY_VIEW", 16);

			ShutterDelay(() => this.camera.CallFunction("SHOW_PHOTO_FRAME"));
		}

		public override void Kill()
		{
			base.Kill();
			API.CellCamActivate(false, false);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 242, false);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 243, false);
			API.SetPedConfigFlag(Game.PlayerPed.Handle, 244, true);
		}

		public override void HandleInput(PhoneInput input)
		{
			switch (input)
			{
				case PhoneInput.Select:
					TakePhoto();
					break;
				case PhoneInput.Back:
					Game.PlaySound("Put_Away", "Phone_SoundSet_Michael");
					this.Phone.OpenApp(this.Parent);
					break;
			}
		}

		private void HandlePhoneInput()
		{
			if (Game.IsControlJustPressed(2, Control.PhoneCameraSelfie)) // Change Mode
			{
				this.selfieMode = !this.selfieMode;
				ShutterDelay(() => SetFrontCam(this.selfieMode), 1000);
				Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
				SetupInstructionalButtons();
			}

			if (Game.IsControlJustPressed(2, Control.PhoneCameraExpression)) // Change Expression
			{
				// TODO expressions?
			}

			if (Game.IsControlJustPressed(2, Control.PhoneCameraGrid)) // Toggle Grid
			{
				this.gridShown = !this.gridShown;
				this.camera.CallFunction("SHOW_PHOTO_FRAME", this.gridShown);
				Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
			}

			if (Game.IsControlJustPressed(2, Control.PhoneCameraDOF)) // Adjust Depth of Field
			{
				this.dofEnabled = !this.dofEnabled;
				API.SetMobilePhoneUnk(this.dofEnabled);
				Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
			}

			this.focusLock = Game.IsControlPressed(2, Control.PhoneCameraFocusLock);
			this.camera.CallFunction("SET_FOCUS_LOCK", this.focusLock, "Focus Lock (Hold)", !this.focusLock);
		}

		private void SetupInstructionalButtons()
		{
			this.buttons.CallFunction("CLEAR_ALL");
			if (this.selfieMode)
			{
				this.buttons.CallFunction("SET_DATA_SLOT", 0, API.GetControlInstructionalButton(2, (int)Control.FrontendCancel,1), "Exit");
				this.buttons.CallFunction("SET_DATA_SLOT", 1, API.GetControlInstructionalButton(2, (int)Control.PhoneSelect,1), Game.GetGXTEntry("CELL_280"));
				this.buttons.CallFunction("SET_DATA_SLOT", 2, API.GetControlInstructionalButton(2, (int)Control.PhoneCameraSelfie,1), Game.GetGXTEntry("CELL_SP_2NP_XB"));
				this.buttons.CallFunction("SET_DATA_SLOT", 3, API.GetControlInstructionalButton(2, (int)Control.PhoneCameraGrid,1), Game.GetGXTEntry("CELL_GRID"));
				this.buttons.CallFunction("SET_DATA_SLOT", 4, API.GetControlInstructionalButton(0, 1,1), Game.GetGXTEntry("CELL_285"));
				// this.buttons.CallFunction("SET_DATA_SLOT", 5, API.GetControlInstructionalButton(2, (int)Control.PhoneDown,1), Game.GetGXTEntry("CELL_FILTER"));
				this.buttons.CallFunction("SET_DATA_SLOT", 5, API.GetControlInstructionalButton(2, (int)Control.PhoneCameraDOF,1), Game.GetGXTEntry("CELL_DEPTH"));
			}
			else
			{
				this.buttons.CallFunction("SET_DATA_SLOT", 0, API.GetControlInstructionalButton(2, (int)Control.FrontendCancel,1), "Exit");
				this.buttons.CallFunction("SET_DATA_SLOT", 1, API.GetControlInstructionalButton(2, (int)Control.PhoneSelect,1), Game.GetGXTEntry("CELL_280"));
				this.buttons.CallFunction("SET_DATA_SLOT", 2, API.GetControlInstructionalButton(2, (int)Control.PhoneCameraSelfie,1), Game.GetGXTEntry("CELL_SP_1NP_XB"));
				this.buttons.CallFunction("SET_DATA_SLOT", 3, API.GetControlInstructionalButton(2, (int)Control.PhoneCameraFocusLock,1), Game.GetGXTEntry("CELL_FOCUS"));
				this.buttons.CallFunction("SET_DATA_SLOT", 4, API.GetControlInstructionalButton(2, (int)Control.PhoneCameraGrid,1), Game.GetGXTEntry("CELL_GRID"));
				this.buttons.CallFunction("SET_DATA_SLOT", 5, API.GetControlInstructionalButton(0, 1,1), Game.GetGXTEntry("CELL_285"));
				this.buttons.CallFunction("SET_DATA_SLOT", 6, API.GetControlInstructionalButton(2, (int)Control.SniperZoom,1), Game.GetGXTEntry("CELL_284"));
				// this.buttons.CallFunction("SET_DATA_SLOT", 7, API.GetControlInstructionalButton(2, (int)Control.PhoneDown,1), Game.GetGXTEntry("CELL_FILTER"));
				this.buttons.CallFunction("SET_DATA_SLOT", 7, API.GetControlInstructionalButton(2, (int)Control.PhoneCameraDOF,1), Game.GetGXTEntry("CELL_DEPTH"));
			}
			this.buttons.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS");
			this.buttons.CallFunction("SET_BACKGROUND_COLOUR", 0, 0, 0, 80);
		}

		private async void ShutterDelay(Action action, int time = 400)
		{
			this.camera.CallFunction("CLOSE_SHUTTER");
			await Delay(time/2);
			action();
			await Delay(time/2);
			this.camera.CallFunction("OPEN_SHUTTER");
		}

		private async void TakePhoto()
		{
			ShutterDelay(() => Game.PlaySound("Camera_Shoot", "Phone_SoundSet_Michael"), 250);
			API.BeginTakeHighQualityPhoto();
			await Delay(250);
			if (API.GetStatusOfTakeHighQualityPhoto() == 0 && API.SaveHighQualityPhoto(-1))
			{
				API.SetLoadingPromptTextEntry("CELL_278");
				API.ShowLoadingPrompt(1);
				while (API.GetStatusOfTakeHighQualityPhoto() == 1) await Delay(0);
				API.RemoveLoadingPrompt();
				ClearPhoto();
			}
		}

		private void ClearPhoto()
		{
			Function.Call((Hash)15564946096525386737);
		}

		private void SetFrontCam(bool b)
		{
			Function.Call((Hash)2635073306796480568, b);
		}
	}
}
