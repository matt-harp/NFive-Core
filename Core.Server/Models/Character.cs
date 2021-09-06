using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NFive.SDK.Core.Models;
using NFive.SDK.Core.Models.Player;
using Night.Core.Shared.Models;

namespace Night.Core.Server.Models
{
	public class Character : IdentityModel, ICharacter
	{
		[Required]
		[StringLength((100))]
		public string Name { get; set; }

		[Required]
		[Range(0, 1)]
		public short Gender { get; set; }

		[Required]
		[Range(0, 100)]
		public int Armor { get; set; }

		[Required]
		public Position Position { get; set; }

		[Required]
		[StringLength(200)]
		public string Model { get; set; }

		[Required]
		[StringLength(200)]
		public string WalkingStyle { get; set; }

		[Required]
		[ForeignKey("Apparel")]
		public Guid ApparelId { get; set; }

		public virtual Apparel Apparel { get; set; }

		[Required]
		[ForeignKey("Appearance")]
		public Guid AppearanceId { get; set; }

		public virtual Appearance Appearance { get; set; }

		[Required]
		[ForeignKey("FaceShape")]
		public Guid FaceShapeId { get; set; }

		public virtual FaceShape FaceShape { get; set; }


		[Required]
		[ForeignKey("Heritage")]
		public Guid HeritageId { get; set; }

		public virtual Heritage Heritage { get; set; }

		public DateTime? LastActive { get; set; }

		[Required]
		[ForeignKey("User")]
		public Guid UserId { get; set; }

		public virtual User User { get; set; }
	}
}
