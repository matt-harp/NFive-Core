using System;
using NFive.SDK.Core.Models;

namespace Night.Core.Shared.Models
{
	public interface ICharacter
	{
		/// <summary>
		/// Name of the character that the player has set.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gender. 0 = Male, 1 = Female
		/// </summary>
		short Gender { get; set; }

		/// <summary>
		/// The persistent armor value of the character.
		/// </summary>
		int Armor { get; set; }

		/// <summary>
		/// The position of the character, where the player last logged out.
		/// </summary>
		Position Position { get; set; }

		/// <summary>
		/// Character model
		/// </summary>
		string Model { get; set; }

		/// <summary>
		/// Character walking style.
		/// </summary>
		string WalkingStyle { get; set; }

		/// <summary>
		/// The last logout time of the character.
		/// </summary>
		DateTime? LastActive { get; set; }
	}
}
