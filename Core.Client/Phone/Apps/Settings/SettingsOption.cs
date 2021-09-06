using System;

namespace Night.Core.Client.Phone.Apps.Settings
{
	public class SettingsOption
	{
		public Action OnSelected;
		public string Name { get; set; }
		public SettingsIcon Icon { get; set; }
	}
}
