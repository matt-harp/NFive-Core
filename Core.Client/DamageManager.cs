using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.BaseScript;

namespace Night.Core.Client
{
	public class DamageManager
	{
		public delegate void DeathEvent(int killer);

		public delegate void DamageEvent();

		public delegate void RespawnEvent();

		private bool respawning;
		public event DeathEvent OnDead;
		public event DamageEvent OnDamaged;
		public event RespawnEvent OnRespawn;

		public async void OnTick()
		{
			if (Game.PlayerPed.IsDead && !this.respawning)
			{
				var handle = Game.PlayerPed.GetKiller()?.Handle;
				PlayerDeath(handle ?? 0);
			}
		}

		public async void PlayerDeath(int killerHandle)
		{
			this.OnDead?.Invoke(killerHandle);
			this.respawning = true;
			var scaleform = new Scaleform("mp_big_message_freemode");
			while (!scaleform.IsLoaded) await Delay(0);
			API.ShakeGameplayCam("DEATH_FAIL_IN_EFFECT_SHAKE", 1.0f);
			API.StartScreenEffect("DeathFailOut", 0, true);
			var underMessage = new List<string>
			{
				"You died."
			};

			if (killerHandle == 0)
			{
				underMessage.Add("You committed suicide.");
				underMessage.Add("You said goodbye to the world.");
				underMessage.Add("You took the easy way out.");
			}
			else
			{
				if (!new Ped(killerHandle).IsPlayer)
				{
					var ped = new Ped(killerHandle);
					if (ped.CurrentVehicle != null)
					{
						underMessage.Add("Someone ran you down.");
					}
					else
					{
						underMessage.Add("Someone killed you.");
					}
				}

				var killer = new Player(killerHandle);
				if (killer.Character.CurrentVehicle != null)
				{
					underMessage.Add($"~r~{killer.Name} ~w~ran you down.");
					underMessage.Add($"~r~{killer.Name} ~w~flattened you.");
				}
				else
				{
					if (killer.Character.Weapons.Current == null)
					{
						underMessage.Add($"~r~{killer.Name} ~w~killed you.");
						underMessage.Add($"~r~{killer.Name} ~w~erased you.");
						underMessage.Add($"~r~{killer.Name} ~w~executed you.");
					}
					else if (killer.Character.Weapons.Current.Group == WeaponGroup.Melee)
					{
						underMessage.Add($"~r~{killer.Name} ~w~beat the shit out of you.");
						underMessage.Add($"~r~{killer.Name} ~w~bonked you.");
						underMessage.Add($"~r~{killer.Name} ~w~crushed you.");
					}
					else if (killer.Character.Weapons.Current.Group == WeaponGroup.Sniper)
					{
						underMessage.Add($"~r~{killer.Name} ~w~sniped you.");
					}
					else if (killer.Character.Weapons.Current.Group == WeaponGroup.Shotgun)
					{
						underMessage.Add($"~r~{killer.Name} ~w~put a hole in you.");
						underMessage.Add($"~r~{killer.Name} ~w~annihilated you.");
						underMessage.Add($"~r~{killer.Name} ~w~blasted you.");
					}
					else if (killer.Character.Weapons.Current.Group == WeaponGroup.SMG)
					{
						underMessage.Add($"~r~{killer.Name} ~w~turned you into swiss cheese.");
						underMessage.Add($"~r~{killer.Name} ~w~shot you up.");
					}
					else if (killer.Character.Weapons.Current.Group == WeaponGroup.Unarmed)
					{
						underMessage.Add($"~r~{killer.Name} ~w~bested you.");
					}
				}
			}

			scaleform.CallFunction("SHOW_SHARD_WASTED_MP_MESSAGE", "wasted", underMessage[new Random().Next(underMessage.Count)], 27, false, true);
			var showScaleform = new Func<Task>(() =>
			{
				scaleform.Render2D();
				return null;
			});

			CoreService.Instance.TickManager.On(showScaleform);
			API.PlaySoundFrontend(-1, "ScreenFlash", "WastedSounds", false);
			API.PlaySoundFrontend(-1, "Bed", "WastedSounds", false);
			await Delay(3000);
			API.DoScreenFadeOut(2000);
			while (!API.IsScreenFadedOut()) await Delay(0);
			CoreService.Instance.TickManager.Off(showScaleform);
			this.OnRespawn?.Invoke();
			API.StopScreenEffect("DeathFailOut");
			API.DoScreenFadeIn(1000);
			while (!API.IsScreenFadedIn()) await Delay(0);
			this.respawning = false;
		}

		public async void Suicide()
		{
			API.RequestAnimDict("mp_suicide");
			while (!API.HasAnimDictLoaded("mp_suicide")) await Delay(1);
			string anim;
			float animTime;
			if (Game.PlayerPed.Weapons.HasWeapon(WeaponHash.Pistol) && Game.PlayerPed.Weapons[WeaponHash.Pistol].Ammo > 0)
			{
				if (Game.PlayerPed.Weapons.Current.Hash != WeaponHash.Pistol)
				{
					Game.PlayerPed.Weapons.Select(WeaponHash.Pistol);
				}

				await Game.PlayerPed.Task.PlayAnimation("mp_suicide", "pistol", 8, -8, -1, AnimationFlags.StayInEndFrame, 0);
				anim = "pistol";
				animTime = .365f;
				while (!API.HasAnimEventFired(Game.PlayerPed.Handle, (uint)API.GetHashKey("Fire"))) await Delay(1);
				API.SetPedShootsAtCoord(Game.PlayerPed.Handle, 0, 0, 0, false);
			}
			else if (Game.PlayerPed.Weapons.HasWeapon(WeaponHash.PistolMk2) && Game.PlayerPed.Weapons[WeaponHash.PistolMk2].Ammo > 0)
			{
				if (Game.PlayerPed.Weapons.Current.Hash != WeaponHash.PistolMk2)
				{
					Game.PlayerPed.Weapons.Select(WeaponHash.PistolMk2);
				}

				await Game.PlayerPed.Task.PlayAnimation("mp_suicide", "pistol", 8, -8, -1, AnimationFlags.StayInEndFrame, 0);
				anim = "pistol";
				animTime = .365f;
				while (!API.HasAnimEventFired(Game.PlayerPed.Handle, (uint)API.GetHashKey("Fire"))) await Delay(1);
				API.SetPedShootsAtCoord(Game.PlayerPed.Handle, 0, 0, 0, false);
			}
			else
			{
				await Game.PlayerPed.Task.PlayAnimation("mp_suicide", "pill", 8, -8, -1, AnimationFlags.StayInEndFrame, 0);
				anim = "pill";
				animTime = .536f;
			}

			while (API.GetEntityAnimCurrentTime(Game.PlayerPed.Handle, "mp_suicide", anim) < animTime) await Delay(1);
			PlayerDeath(0);
			API.RemoveAnimDict("mp_suicide");
		}
	}
}
