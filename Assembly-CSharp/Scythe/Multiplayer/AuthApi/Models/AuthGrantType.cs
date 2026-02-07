using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000359 RID: 857
	[JsonConverter(typeof(StringEnumConverter))]
	[Preserve]
	public enum AuthGrantType
	{
		// Token: 0x04001218 RID: 4632
		[EnumMember(Value = "Undefined")]
		Undefined,
		// Token: 0x04001219 RID: 4633
		[EnumMember(Value = "Steam")]
		Steam,
		// Token: 0x0400121A RID: 4634
		[EnumMember(Value = "GameCenter")]
		GameCenter,
		// Token: 0x0400121B RID: 4635
		[EnumMember(Value = "GooglePlay")]
		GooglePlay,
		// Token: 0x0400121C RID: 4636
		[EnumMember(Value = "Gog")]
		Gog,
		// Token: 0x0400121D RID: 4637
		[EnumMember(Value = "RefreshToken")]
		RefreshToken,
		// Token: 0x0400121E RID: 4638
		[EnumMember(Value = "Password")]
		Password
	}
}
