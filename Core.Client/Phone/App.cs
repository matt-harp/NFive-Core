using System.Threading.Tasks;

namespace Night.Core.Client.Phone
{
	public abstract class App
	{
		/// <summary>
		/// Icon for the current app (only displayed on homescreen)
		/// </summary>
		public abstract AppIcon Icon { get; }

		/// <summary>
		/// Internal ID of the current screen in the iFruit Scaleform
		/// </summary>
		public abstract int DisplayId { get; }

		/// <summary>
		/// Reference to Phone object that owns this app
		/// </summary>
		protected Phone Phone { get; }

		/// <summary>
		/// Parent app of this one, will return to this when the current app is closed, or close the phone if null
		/// </summary>
		protected App Parent { get; }

		/// <summary>
		/// "Highlighted" choice from current list of options
		/// </summary>
		public virtual int SelectedIndex { get; set; }

		/// <summary>
		/// Name of this app displayed on top and below the app icon
		/// </summary>
		public abstract string Name { get; set; }

		public App(Phone phone, App parent = null)
		{
			this.Phone = phone;
			this.Parent = parent;
		}

		/// <summary>
		/// For displaying the app's info each frame
		/// </summary>
		/// <returns></returns>
		public abstract Task Update();

		/// <summary>
		/// For initialization of app data and soft keys
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		/// For any extra cleanup that needs to be done when closing
		/// </summary>
		public virtual void Kill()
		{
			this.Phone.Scaleform.CallFunction("SET_DATA_SLOT_EMPTY", this.DisplayId);
		}

		public abstract void HandleInput(PhoneInput input);
	}
}
