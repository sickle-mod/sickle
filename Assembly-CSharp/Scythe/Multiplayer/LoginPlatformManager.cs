using System;
using Scythe.Multiplayer.AuthApi.Models;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200029D RID: 669
	public static class LoginPlatformManager
	{
		// Token: 0x06001525 RID: 5413 RVA: 0x0003651A File Offset: 0x0003471A
		public static LoginPlatform GetLoginPlatform()
		{
			if (PlatformManager.IsSteam)
			{
				return LoginPlatform.Steam;
			}
			if (PlatformManager.IsGOG)
			{
				return LoginPlatform.Gog;
			}
			if (PlatformManager.IsAndroid)
			{
				return LoginPlatform.GooglePlay;
			}
			if (PlatformManager.IsIOS)
			{
				return LoginPlatform.GameCenter;
			}
			Debug.Log("Unknown platform, returning Undefined");
			return LoginPlatform.Undefined;
		}
	}
}
