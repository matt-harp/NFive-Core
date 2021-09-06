using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Night.Core.Client.Phone.Images;

namespace Night.Core.Client.Phone.Apps
{
	public class ContactsApp : App
	{
		public override AppIcon Icon => AppIcon.Contacts;
		public override int DisplayId => 2;

		public override string Name
		{
			get => "Contacts";
			set => throw new NotSupportedException("The name of this app cannot be set.");
		}

		/// <summary>
		/// List of available contacts
		/// </summary>
		public List<Contact> ContactList { get; }
		//todo refactor data into phone

		/// <summary>
		/// Contact currently being called or null if none
		/// </summary>
		public Contact CurrentCaller { get; set; }

		public ContactsApp(Phone phone, App parent = null) : base(phone, parent)
		{
			this.ContactList = new List<Contact>();
		}

		public override async Task Update()
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", this.DisplayId);
			for (var i = 0; i < this.ContactList.Count; i++)
			{
				var contact = this.ContactList[i];
				SetContact(i, contact.Name, contact.Icon.Name);
			}
		}

		public override async void Initialize()
		{
			this.CurrentCaller = null;
			this.ContactList.Clear();

			this.ContactList.Add(Contact.Lester);

			this.ContactList.Add(new Contact
			{
				Icon = ContactIcon.LSCustoms,
				Name = "LS Customs"
			});

			this.Phone.Scaleform.CallFunction("SET_HEADER", "Contacts");
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
					if (this.SelectedIndex > this.ContactList.Count - 1) this.SelectedIndex = 0;
					API.MoveFinger((int)FingerDirection.Down);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.ScrollBackward:
				case PhoneInput.Up:
					this.SelectedIndex--;
					if (this.SelectedIndex < 0) this.SelectedIndex = this.ContactList.Count - 1;
					API.MoveFinger((int)FingerDirection.Up);
					Game.PlaySound("Menu_Navigate", "Phone_SoundSet_Michael");
					break;
				case PhoneInput.Select:
					this.CurrentCaller = this.ContactList[this.SelectedIndex];
					this.ContactList[this.SelectedIndex].OnCalled();
					this.Phone.OpenApp(new CallscreenApp(this.Phone, this)
					{
						CurrentContact = this.CurrentCaller
					});
					Game.PlaySound("Menu_Accept", "Phone_SoundSet_Michael");
					break;
			}
		}

		private long LoadImageIntoTxd(string txdToCreate, string textureName, string fileName)
		{
			var txd = API.CreateRuntimeTxd(txdToCreate);
			return API.CreateRuntimeTextureFromImage(txd, textureName, fileName);
		}

		private void SetContact(int index, string name, string iconTxd = "CHAR_HUMANDEFAULT")
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT", 2, index, 0, name, "", iconTxd);
		}
	}
}
