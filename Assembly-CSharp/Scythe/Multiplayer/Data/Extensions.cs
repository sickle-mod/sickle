using System;
using Scythe.Multiplayer.AuthApi.Models;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000332 RID: 818
	public static class Extensions
	{
		// Token: 0x06001783 RID: 6019 RVA: 0x00038068 File Offset: 0x00036268
		public static AuthGrantType ToAuthGrantType(this LoginPlatform platform)
		{
			switch (platform)
			{
			case LoginPlatform.Steam:
				return AuthGrantType.Steam;
			case LoginPlatform.GameCenter:
				return AuthGrantType.GameCenter;
			case LoginPlatform.GooglePlay:
				return AuthGrantType.GooglePlay;
			case LoginPlatform.Gog:
				return AuthGrantType.Gog;
			default:
				return AuthGrantType.Undefined;
			}
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0003808D File Offset: 0x0003628D
		public static LoginPlatform ToLoginPlatform(this AuthGrantType type)
		{
			switch (type)
			{
			case AuthGrantType.Steam:
				return LoginPlatform.Steam;
			case AuthGrantType.GameCenter:
				return LoginPlatform.GameCenter;
			case AuthGrantType.GooglePlay:
				return LoginPlatform.GooglePlay;
			case AuthGrantType.Gog:
				return LoginPlatform.Gog;
			}
			return LoginPlatform.Undefined;
		}
	}
}
