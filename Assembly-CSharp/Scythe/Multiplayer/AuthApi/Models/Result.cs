using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200037D RID: 893
	[JsonConverter(typeof(StringEnumConverter))]
	[Preserve]
	public enum Result
	{
		// Token: 0x04001295 RID: 4757
		[EnumMember(Value = "Success")]
		Success,
		// Token: 0x04001296 RID: 4758
		[EnumMember(Value = "Error")]
		Error,
		// Token: 0x04001297 RID: 4759
		[EnumMember(Value = "ServerError")]
		ServerError,
		// Token: 0x04001298 RID: 4760
		[EnumMember(Value = "Unauthorized")]
		Unauthorized
	}
}
