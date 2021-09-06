﻿using System;

namespace Night.Core.Shared.Models
{
	public interface ICharacterInventory
	{
		Guid CharacterId { get; set; }
		Guid ContainerId { get; set; }
	}
}
