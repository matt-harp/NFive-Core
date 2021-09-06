using JetBrains.Annotations;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Server.Communications;
using NFive.SDK.Server.Controllers;
using Night.Core.Shared;

namespace Night.Core.Server
{
	[PublicAPI]
	public class CoreController : ConfigurableController<Configuration>
	{
		public CoreController(ILogger logger, Configuration configuration, ICommunicationManager comms) : base(logger, configuration)
		{

			// Send configuration when requested
			comms.Event(CoreEvents.Configuration).FromClients().OnRequest(e => e.Reply(this.Configuration));

			//todo interface with database and return data based on user who sent the request
			comms.Event(CoreEvents.Customization).FromClients().OnRequest(e => e.Reply(null));
		}
	}
}
