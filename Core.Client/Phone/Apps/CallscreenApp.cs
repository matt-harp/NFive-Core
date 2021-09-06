using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Night.Core.Client.Phone.Apps
{
	public class CallscreenApp : App
	{
		private int callTimer;
		private int dialSoundId;
		private bool hasAnswered;

		public override AppIcon Icon => AppIcon.None;

		public override int DisplayId => 4;

		public override string Name
		{
			get => "Callscreen";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		/// <summary>
		/// The contact currently on the line
		/// </summary>
		public Contact CurrentContact { get; set; }

		public CallscreenApp(Phone phone, App parent = null) : base(phone, parent) { }

		public override async Task Update()
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", this.DisplayId);
			if (Game.GameTime > this.callTimer)
			{
				if (!this.hasAnswered)
				{
					Audio.StopSound(this.dialSoundId);
					Audio.ReleaseSound(this.dialSoundId);
					this.dialSoundId = -1;
					this.hasAnswered = true;
					this.CurrentContact.OnAnswered();
				}

				SetStatus(true);
			}
			else
			{
				SetStatus(false);
			}
		}

		public override void Initialize()
		{
			this.Phone.SetSoftKey(3, SoftKeyIcon.Hangup, PhoneColor.Red);
			if (this.CurrentContact == null)
			{
				throw new Exception("Callscreen needs a Contact assigned before calling Initialize()");
			}

			this.callTimer = Game.GameTime + this.CurrentContact.CallTime;
			this.dialSoundId = API.GetSoundId();
			API.PlaySoundFrontend(this.dialSoundId, "Remote_Ring", "Phone_SoundSet_Michael", true);
			this.hasAnswered = false;
		}

		public override void HandleInput(PhoneInput input)
		{
			if (input == PhoneInput.Back)
			{
				Hangup();
				this.Phone.OpenApp(this.Parent);
			}
		}

		private void Hangup()
		{
			Audio.StopSound(this.dialSoundId);
			Audio.ReleaseSound(this.dialSoundId);
			Game.PlaySound("Hang_Up", "Phone_SoundSet_Michael");
			this.CurrentContact.OnHangUp();
		}

		private void SetStatus(bool connected)
		{
			// 1st param ignored, 2nd param contact name, 3rd param contact texture name, 4th param text to display next to icon (CONNECTED or DIALING)
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT", this.DisplayId, 0, "ignored", this.CurrentContact.Name, this.CurrentContact.Icon.Name, connected ? "CONNECTED" : "DIALING...");
		}
	}
}
