﻿#region copyright

// Copyright (c) 2020 Project Agonyl

#endregion copyright

using Agonyl.Shared.Network;
using Agonyl.Shared.Util;

namespace Agonyl.Login.Network
{
    public class LoginPacketHandler : PacketHandler<LoginConnection>
    {
        public static readonly LoginPacketHandler Instance = new LoginPacketHandler();

        /// <summary>
        /// Sent when user tries to log in using the client.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="packet"></param>
        [PacketHandler(Op.C2S_LOGIN)]
        public void C2S_LOGIN(LoginConnection conn, Packet packet)
        {
            packet.SetReadPointer(10);
            var username = packet.GetString(20);
            packet.SetReadPointer(31);
            var password = packet.GetString(20);
            if (!LoginServer.Instance.ASDDatabase.AccountExists(username, password))
                Send.S2C_LOGIN_MESSAGE(conn, "Invalid ID/password");
            else
            {
                if (LoginServer.Instance.Redis.IsLoggedIn(username))
                    Send.S2C_LOGIN_MESSAGE(conn, "Account already logged in");
                else
                {
                    Log.Info(username + " account successfully logged in");
                    conn.Username = username;
                    Send.S2C_LOGIN_OK(conn);
                    Send.S2C_SERVER_LIST(conn, LoginServer.Instance.Conf.ServerName);
                }
            }
        }

        /// <summary>
        /// Sent when user selects a server.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="packet"></param>
        [PacketHandler(Op.C2S_SERVER_DETAILS)]
        public void C2S_SERVER_DETAILS(LoginConnection conn, Packet packet)
        {
            LoginServer.Instance.Redis.AddLoggedInAccount(conn.Username);
            Send.S2C_SERVER_DETAILS(conn, LoginServer.Instance.Conf.GameServerHost, LoginServer.Instance.Conf.GameServerPort);
        }
    }
}
