using System;
using NFive.SDK.Core.Models;
using Night.Core.Shared.Models;

namespace Night.Core.Server.Models
{
	public class CharacterInventory : IdentityModel, ICharacterInventory
	{
		public Guid CharacterId { get; set; }

		public virtual Character Character { get; set; }

		public Guid ContainerId { get; set; }

		//todo public virtual Container { get; set; }
	}
}
