using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.BaseScript;

namespace Night.Core.Client.Phone.Apps
{
	public class EyefindApp : App
	{
		public override AppIcon Icon => AppIcon.Eyefind;
		public override int DisplayId => 999;
		public override string Name
		{
			get => "Eyefind";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		private Scaleform browser;

		public EyefindApp(Phone phone, App parent = null) : base(phone, parent) { }

		public override async Task Update()
		{
			if (this.browser.IsLoaded)
			{
				this.browser.Render2D();
			}
			Game.DisableAllControlsThisFrame(0);
			HandleFrontendInput();
		}

		public override async void Initialize()
		{
			this.Phone.Visible = false;
			this.browser = new Scaleform("web_browser");
			while (!this.browser.IsLoaded) await Delay(0);
			this.browser.CallFunction("SET_IS_WIDESCREEN", true);
			this.browser.CallFunction("SET_MULTIPLAYER", true);
			this.browser.CallFunction("SET_BROWSER_SKIN", 1);
			this.browser.CallFunction("GO_TO_WEBPAGE", "WWW_EYEFIND_INFO");
			this.browser.CallFunction("SET_TICKERTAPE", 0.5);
		}

		public override void HandleInput(PhoneInput input) { }

		private void HandleFrontendInput()
		{
			// mouse mode
			this.browser.CallFunction("SET_MOUSE_INPUT", Game.GetDisabledControlNormal(2, Control.CursorX), Game.GetDisabledControlNormal(2, Control.CursorY));
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendLb))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 4);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendLt))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 5);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendRb))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 6);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendRt))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 7);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendUp))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 8);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendDown))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 9);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendLeft))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 10);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendRight))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 11);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.FrontendAccept))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 16);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.CursorAccept))
			{
				this.browser.CallFunction("SET_INPUT_EVENT", 1001);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.CursorCancel) || Game.IsDisabledControlJustPressed(2, Control.FrontendCancel))
			{
				this.browser.CallFunction("GO_BACK");
				Game.PlaySound("CLICK_BACK", "WEB_NAVIGATION_SOUNDS_PHONE");
			}
			if (Game.IsDisabledControlJustPressed(2, Control.CursorScrollUp))
			{
				this.browser.CallFunction("SET_ANALOG_STICK_INPUT", 0, 0, -200, true);
			}
			if (Game.IsDisabledControlJustPressed(2, Control.CursorScrollDown))
			{
				this.browser.CallFunction("SET_ANALOG_STICK_INPUT", 0, 0, 200, true);
			}

			if (Game.IsDisabledControlJustReleased(2, Control.CursorAccept))
			{
				this.browser.CallFunction("SET_INPUT_RELEASE_EVENT", 237);
			}
		}
	}
}
