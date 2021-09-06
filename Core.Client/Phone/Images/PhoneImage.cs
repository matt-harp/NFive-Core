using CitizenFX.Core.Native;

namespace Night.Core.Client.Phone.Images
{
	public abstract class PhoneImage
	{
		/// <summary>
		/// Name of the image asset
		/// </summary>
		internal string Name { get; set; }

		/// <summary>
		/// Initialize the class.
		/// </summary>
		/// <param name="name">Name of the texture dictionary.</param>
		public PhoneImage(string name)
		{
			LoadTextureDict(name);
			this.Name = name;
		}

		/// <summary>
		/// Load a texture dictionary by name.
		/// </summary>
		/// <param name="txd">Name of the texture dictionary.</param>
		private void LoadTextureDict(string txd)
		{
			if (!API.HasStreamedTextureDictLoaded(txd))
			{
				API.RequestStreamedTextureDict(txd, false);
			}
		}
	}
}
