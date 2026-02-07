using System;

namespace Scythe.Multiplayer
{
	// Token: 0x02000297 RID: 663
	public static class ServerEndpoints
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0002FB18 File Offset: 0x0002DD18
		public static EndpointType EndpointType
		{
			get
			{
				return EndpointType.Production;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x0003627E File Offset: 0x0003447E
		public static string ServerEndpoint
		{
			get
			{
				return "https://game.scythedigitaledition.com/api/";
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x00036285 File Offset: 0x00034485
		public static string AzureFunctionsEndpoint
		{
			get
			{
				return "https://func-game.scythedigitaledition.com/api/";
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0003628C File Offset: 0x0003448C
		public static string AuthApiEndpoint
		{
			get
			{
				return "https://auth.scythedigitaledition.com/api";
			}
		}
	}
}
