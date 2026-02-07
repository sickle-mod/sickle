using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200036A RID: 874
	[JsonConverter(typeof(StringEnumConverter))]
	[Preserve]
	public enum LoginPlatform
	{
		// Token: 0x0400126A RID: 4714
		[EnumMember(Value = "Undefined")]
		Undefined,
		// Token: 0x0400126B RID: 4715
		[EnumMember(Value = "Steam")]
		Steam,
		// Token: 0x0400126C RID: 4716
		[EnumMember(Value = "GameCenter")]
		GameCenter,
		// Token: 0x0400126D RID: 4717
		[EnumMember(Value = "GooglePlay")]
		GooglePlay,
		// Token: 0x0400126E RID: 4718
		[EnumMember(Value = "Gog")]
		Gog
	}
}
