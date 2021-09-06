using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Google.Protobuf.WellKnownTypes;

namespace Night.Core.Client.Phone.Apps
{
	public class MessagesApp : App
	{
		public override AppIcon Icon => AppIcon.TextMessage;
		public override int DisplayId => 6;

		public override string Name
		{
			get => "Messages";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		public MessagesApp(Phone phone, App parent = null) : base(phone, parent) { }

		public override async Task Update()
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", this.DisplayId);
			for (var i = 0; i < this.Phone.Messages.Count; i++)
			{
				var message = this.Phone.Messages[i];
				this.Phone.Scaleform.CallFunction("activate", i, this.SelectedIndex == i);
				SetMessage(i, message);
			}
		}

		public override void Initialize()
		{
			if(this.Phone.Messages.Count > 0)
				this.Phone.SetSoftKey(2, SoftKeyIcon.Select, PhoneColor.Green);
			this.Phone.SetSoftKey(3, SoftKeyIcon.Back, PhoneColor.Red);

			this.Phone.Scaleform.CallFunction("DISPLAY_VIEW", this.DisplayId, this.SelectedIndex);
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
					if (this.SelectedIndex > this.Phone.Messages.Count - 1) this.SelectedIndex = 0;
					API.MoveFinger((int)FingerDirection.Down);
					this.Phone.Scaleform.CallFunction("SET_INPUT_EVENT", (int)NavigationDirection.Down);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.ScrollBackward:
				case PhoneInput.Up:
					this.SelectedIndex--;
					if (this.SelectedIndex < 0) this.SelectedIndex = this.Phone.Messages.Count - 1;
					API.MoveFinger((int)FingerDirection.Up);
					this.Phone.Scaleform.CallFunction("SET_INPUT_EVENT", (int)NavigationDirection.Up);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.Select:
					this.Phone.OpenApp(new MessageViewApp(this.Phone, this)
					{
						DisplayedMessage = this.Phone.Messages[this.SelectedIndex]
					});
					this.Phone.Messages[this.SelectedIndex].IsRead = true;
					Game.PlaySound("Menu_Accept", "Phone_SoundSet_Michael");
					break;
			}
		}


		private void SetMessage(int index, Message message)
		{
			// hours, minutes, unread(33)/read(34), name, content
			// cellphone_ifruit.gfx hacked to display black/white text for message list, <SELECTED> needed for selected list item
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT", this.DisplayId, index, message.TimeReceived.Hour, message.TimeReceived.Minute, message.IsRead ? 34 : 33, message.From.Name, (this.SelectedIndex == index ? "<SELECTED>" : "") + message.Contents);
		}
	}

	public class Message
	{
		/// <summary>
		/// The name this message is from
		/// </summary>
		public Contact From { get; set; }

		/// <summary>
		/// The contents of the message
		/// </summary>
		public string Contents { get; set; }

		/// <summary>
		/// The time this message was received
		/// </summary>
		public DateTime TimeReceived { get; set; }

		/// <summary>
		/// Whether or not this message has been read
		/// </summary>
		public bool IsRead { get; set; }
	}
}
