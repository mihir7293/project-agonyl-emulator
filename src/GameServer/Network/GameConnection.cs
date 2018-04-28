﻿#region copyright
// Copyright (c) 2018 Project Agonyl
#endregion

using Agonyl.Shared.Network;

namespace Agonyl.Game.Network
{
	public class GameConnection: Connection
	{
		/// <summary>
		/// Handles game server packets.
		/// </summary>
		/// <param name="packet"></param>
		protected override void HandlePacket(Packet packet)
		{
			GamePacketHandler.Instance.Handle(this, packet);
		}

		/// <summary>
		/// Cleans up connection, incl. account and characters.
		/// </summary>
		protected override void CleanUp()
		{
			return;
		}
	}
}