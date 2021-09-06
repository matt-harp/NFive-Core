using System;
using Night.Core.Client.Phone.Images;

namespace Night.Core.Client.Phone
{
	public class Contact
	{
		public Action OnCalled = () => { };
		public Action OnAnswered = () => { };
		public Action OnHangUp = () => { };
		public string Name { get; set; } = "Unknown";
		public ContactIcon Icon { get; set; } = ContactIcon.Generic;
		public bool IsPlayer { get; set; } //todo
		public int CallTime { get; set; }


		public static Contact Lester = new Contact
		{
			CallTime = 2000,
			Icon = ContactIcon.Lester,
			Name = "Lester",
		};
	}
}
