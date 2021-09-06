using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using JetBrains.Annotations;
using NFive.SDK.Client.Commands;
using NFive.SDK.Client.Communications;
using NFive.SDK.Client.Events;
using NFive.SDK.Client.Interface;
using NFive.SDK.Client.Services;
using NFive.SDK.Core.Diagnostics;
using NFive.SDK.Core.Models.Player;
using Night.Core.Client.Overlays;
using Night.Core.Client.Phone;
using Night.Core.Shared;

namespace Night.Core.Client
{
	[PublicAPI]
	public class CoreService : Service
	{
		public static CoreService Instance;
		public static bool Debug = true;

		private Configuration config;

		public BrowserOverlay Browser { get; private set; }

		public ITickManager TickManager => this.Ticks;

		public DamageManager Damage { get; private set; }
		public SpawnManager Spawn { get; private set; }
		public MapManager Map { get; private set; }
		public CharacterManager Character { get; private set; }
		public PhoneController Phone { get; private set; }

		public CoreService(ILogger logger, ITickManager ticks, ICommunicationManager comms, ICommandManager commands, IOverlayManager overlay, User user) : base(logger, ticks, comms, commands, overlay, user) { }

		public override async Task Started()
		{
			Instance = this;

			this.Browser = new BrowserOverlay(this.OverlayManager);

			this.Damage = new DamageManager();
			this.Spawn = new SpawnManager(this.Damage);
			this.Map = new MapManager();
			this.Character = new CharacterManager();
			this.Phone = new PhoneController();

			this.Comms.Event(CoreEvents.Teleport).FromServer().On<Vector3, bool>((e, p, c) => GeneralFunctions.Teleport(p, c));

			this.Ticks.On(OnTick);
			this.Ticks.On(this.Damage.OnTick);
			this.Ticks.On(this.Map.OnTick);
			this.Ticks.On(this.Phone.OnTick);

			this.Ticks.On(this.Character.ShowMenu);

			//change to initial spawn at last saved location or player preference
			this.Spawn.Respawn();

			API.RegisterCommand("suicide", new Action(this.Damage.Suicide), false);
			API.RegisterCommand("pos", new Action(() => { CitizenFX.Core.Debug.WriteLine(Game.PlayerPed.Position.ToString()); }), false);
		}

		private async Task OnTick()
		{

		}
	}
}
