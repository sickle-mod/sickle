using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000382 RID: 898
	[DataContract]
	public class UnlinkAccountRequest : IEquatable<UnlinkAccountRequest>
	{
		// Token: 0x06001A06 RID: 6662 RVA: 0x00039508 File Offset: 0x00037708
		[Preserve]
		public UnlinkAccountRequest(LoginPlatform Platform = LoginPlatform.Undefined)
		{
			this.Platform = Platform;
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x00039517 File Offset: 0x00037717
		// (set) Token: 0x06001A08 RID: 6664 RVA: 0x0003951F File Offset: 0x0003771F
		[DataMember(Name = "platform", EmitDefaultValue = true)]
		public LoginPlatform Platform { get; set; }

		// Token: 0x06001A09 RID: 6665 RVA: 0x000A3384 File Offset: 0x000A1584
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class UnlinkAccountRequest {\n");
			stringBuilder.Append("  Platform: ").Append(this.Platform).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00039528 File Offset: 0x00037728
		public override bool Equals(object input)
		{
			return this.Equals(input as UnlinkAccountRequest);
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000A33DC File Offset: 0x000A15DC
		public bool Equals(UnlinkAccountRequest input)
		{
			if (input == null)
			{
				return false;
			}
			if (this.Platform != input.Platform)
			{
				LoginPlatform platform = this.Platform;
				return this.Platform.Equals(input.Platform);
			}
			return true;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000A3424 File Offset: 0x000A1624
		public override int GetHashCode()
		{
			int num = 41;
			LoginPlatform platform = this.Platform;
			return num * 59 + this.Platform.GetHashCode();
		}
	}
}
