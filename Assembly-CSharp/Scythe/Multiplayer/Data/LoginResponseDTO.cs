using System;
using Newtonsoft.Json;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000337 RID: 823
	[JsonObject(MemberSerialization.Fields)]
	public class LoginResponseDTO
	{
		// Token: 0x06001795 RID: 6037 RVA: 0x0003818C File Offset: 0x0003638C
		public Guid PlayerId()
		{
			return this.playerId;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00038194 File Offset: 0x00036394
		public string Token()
		{
			return this.token;
		}

		// Token: 0x04001158 RID: 4440
		private Guid playerId;

		// Token: 0x04001159 RID: 4441
		private string token;
	}
}
