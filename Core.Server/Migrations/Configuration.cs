using JetBrains.Annotations;
using NFive.SDK.Server.Migrations;
using Night.Core.Server.Storage;

namespace Night.Core.Server.Migrations
{
	[UsedImplicitly]
	public sealed class Configuration : MigrationConfiguration<StorageContext> { }
}
