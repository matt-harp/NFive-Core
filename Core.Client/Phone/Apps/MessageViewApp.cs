using System;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace Night.Core.Client.Phone.Apps
{
	public class MessageViewApp : App
	{
		public override AppIcon Icon => AppIcon.None;
		public override int DisplayId => 7;
		public Message DisplayedMessage { get; set; }

		public override string Name
		{
			get => "Message";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		public MessageViewApp(Phone phone, App parent = null) : base(phone, parent) { }

		public override async Task Update()
		{
			// contact name, message text (can include image), contact image txd string
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT", this.DisplayId, 0, this.DisplayedMessage.From.Name, this.DisplayedMessage.Contents, this.DisplayedMessage.From.Icon.Name);
		}

		public override void Initialize()
		{
			this.Phone.SetSoftKey(1, SoftKeyIcon.Call, PhoneColor.Green);
			this.Phone.SetSoftKey(2, SoftKeyIcon.Delete, PhoneColor.Blue);
			this.Phone.SetSoftKey(3, SoftKeyIcon.Back, PhoneColor.Red);
		}

		public override void HandleInput(PhoneInput input)
		{
			switch (input)
			{
				case PhoneInput.Back:
					Game.PlaySound("Put_Away", "Phone_SoundSet_Michael");
					this.Phone.OpenApp(this.Parent);
					break;
				case PhoneInput.Select:
					Game.PlaySound("Menu_Select", "Phone_SoundSet_Michael");
					// call
					break;
				case PhoneInput.SpecialOption:
					this.Phone.Messages.Remove(this.DisplayedMessage);
					this.Parent.SelectedIndex = 0;
					Game.PlaySound("Menu_Back", "Phone_SoundSet_Michael");
					this.Phone.OpenApp(this.Parent);
					break;
			}
		}
	}
}
