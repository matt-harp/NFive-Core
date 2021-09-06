using System;
using System.Drawing;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Night.Core.Client
{
	public class SpawnManager
	{
		public enum VehicleNodeType
		{
			AsphaltRoad,
			AnyRoad,
			Offroad,
			Water
		}

		public SpawnManager(DamageManager damageManager)
		{
			damageManager.OnRespawn += Respawn;
		}

		/// <summary>
		/// Respawns the player at given location
		/// </summary>
		/// <param name="position"></param>
		private void Respawn(Vector3 position)
		{
			API.NetworkResurrectLocalPlayer(position.X, position.Y, position.Z, new Random().Next(0, 360), true, false);
			Game.PlayerPed.ResetVisibleDamage();
			Game.PlayerPed.Task.ClearAll();
		}

		/// <summary>
		/// Respawns the player at the nearest safe position from where they last were.
		/// </summary>
		public void Respawn()
		{
			var rand = new Random();
			var x = (float)Math.Cos(rand.NextDouble() * Math.PI * 2) * rand.Next(100, 500);
			var y = (float)Math.Sin(rand.NextDouble() * Math.PI * 2) * rand.Next(100, 500);
			var result = World.Raycast(new Vector3(x, y, 900f), -Vector3.UnitZ, IntersectOptions.Map);
			var respawnLocation = FindSafeSpawnLocation(result.HitPosition != Vector3.Zero ? result.HitPosition : Game.PlayerPed.Position, VehicleNodeType.AsphaltRoad);
			API.NetworkResurrectLocalPlayer(respawnLocation.X, respawnLocation.Y, respawnLocation.Z, rand.Next(0, 360), true, false);
			Game.PlayerPed.ResetVisibleDamage();
			Game.PlayerPed.Task.ClearAll();
		}

		public static Vector3 FindSafeSpawnLocation(Vector3 desiredPos, VehicleNodeType roadType)
		{
			var offRoad = false;
			var nodeType = 0;
			if (roadType == VehicleNodeType.Offroad)
			{
				offRoad = true;
				nodeType = 1;
			}
			else if (roadType == VehicleNodeType.AsphaltRoad)
			{
				nodeType = 0;
			}
			else if (roadType == VehicleNodeType.Water)
			{
				nodeType = 3;
			}
			else if (roadType == VehicleNodeType.AnyRoad) nodeType = 1;

			int firstNodeId = 0, secondNodeId = 0;
			if (offRoad)
			{
				for (var node = 1; node < 100; node++)
				{
					firstNodeId = API.GetNthClosestVehicleNodeId(desiredPos.X, desiredPos.Y, desiredPos.Z, node, nodeType, 0, 0);
					secondNodeId = API.GetNthClosestVehicleNodeId(desiredPos.X, desiredPos.Y, desiredPos.Z, node + 1, nodeType, 0, 0);
					if (API.GetVehicleNodeIsSwitchedOff(firstNodeId)) break;
				}
			}
			else
			{
				firstNodeId = API.GetNthClosestVehicleNodeId(desiredPos.X, desiredPos.Y, desiredPos.Z, 1, nodeType, 0, 0);
				secondNodeId = API.GetNthClosestVehicleNodeId(desiredPos.X, desiredPos.Y, desiredPos.Z, 2, nodeType, 0, 0);
			}

			var firstNodePosition = Vector3.Zero;
			var secondNodePosition = Vector3.Zero;
			API.GetVehicleNodePosition(firstNodeId, ref firstNodePosition);
			API.GetVehicleNodePosition(secondNodeId, ref secondNodePosition);

			// Calculates point to the right
			var right = Vector3.Cross(secondNodePosition - firstNodePosition, Vector3.UnitZ);
			right.Normalize();
			var position = firstNodePosition + right * 20f;
			var finalpos = World.GetNextPositionOnSidewalk(position);

			if (CoreService.Debug)
			{
				World.DrawMarker(MarkerType.Number1, firstNodePosition, Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(255, 0, 0), faceCamera: true);
				World.DrawMarker(MarkerType.Number2, secondNodePosition, Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(0, 255, 0), faceCamera: true);
				World.DrawMarker(MarkerType.DebugSphere, finalpos, Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(0, 155, 155));
				World.DrawLine(firstNodePosition + Vector3.UnitZ * 0.5f, position + Vector3.UnitZ * 0.5f, Color.FromArgb(0, 255, 0));
				World.DrawLine(firstNodePosition, firstNodePosition + Vector3.UnitZ * 3f, Color.FromArgb(255, 255, 0));
				World.DrawLine(firstNodePosition + Vector3.UnitZ * 0.5f, secondNodePosition + Vector3.UnitZ * 0.5f, Color.FromArgb(255, 255, 255));

				if (finalpos.Equals(Vector3.Zero)) World.DrawMarker(MarkerType.ChevronUpx3, firstNodePosition + Vector3.UnitZ * 1.5f, Vector3.Zero, Vector3.Zero, Vector3.One, Color.FromArgb(30, 50, 150), faceCamera: true);
			}

			return finalpos == Vector3.Zero ? desiredPos : finalpos;
		}
	}
}
