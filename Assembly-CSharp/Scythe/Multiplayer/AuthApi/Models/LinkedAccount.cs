using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000368 RID: 872
	[DataContract]
	public class LinkedAccount : IEquatable<LinkedAccount>
	{
		// Token: 0x0600190F RID: 6415 RVA: 0x00038D74 File Offset: 0x00036F74
		[Preserve]
		public LinkedAccount(LoginPlatform Platform = LoginPlatform.Undefined, string PlatformUserId = null)
		{
			this.Platform = Platform;
			this.PlatformUserId = PlatformUserId;
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001910 RID: 6416 RVA: 0x00038D8A File Offset: 0x00036F8A
		// (set) Token: 0x06001911 RID: 6417 RVA: 0x00038D92 File Offset: 0x00036F92
		[DataMember(Name = "platform", EmitDefaultValue = true)]
		public LoginPlatform Platform { get; set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001912 RID: 6418 RVA: 0x00038D9B File Offset: 0x00036F9B
		// (set) Token: 0x06001913 RID: 6419 RVA: 0x00038DA3 File Offset: 0x00036FA3
		[DataMember(Name = "platformUserId", EmitDefaultValue = true)]
		public string PlatformUserId { get; set; }

		// Token: 0x06001914 RID: 6420 RVA: 0x000A13B0 File Offset: 0x0009F5B0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LinkedAccount {\n");
			stringBuilder.Append("  Platform: ").Append(this.Platform).Append("\n");
			stringBuilder.Append("  PlatformUserId: ").Append(this.PlatformUserId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x00038DAC File Offset: 0x00036FAC
		public override bool Equals(object input)
		{
			return this.Equals(input as LinkedAccount);
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x000A1428 File Offset: 0x0009F628
		public bool Equals(LinkedAccount input)
		{
			if (input == null)
			{
				return false;
			}
			if (this.Platform != input.Platform)
			{
				LoginPlatform platform = this.Platform;
				if (!this.Platform.Equals(input.Platform))
				{
					return false;
				}
			}
			return this.PlatformUserId == input.PlatformUserId || (this.PlatformUserId != null && this.PlatformUserId.Equals(input.PlatformUserId));
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x000A14A4 File Offset: 0x0009F6A4
		public override int GetHashCode()
		{
			int num = 41;
			LoginPlatform platform = this.Platform;
			num = num * 59 + this.Platform.GetHashCode();
			if (this.PlatformUserId != null)
			{
				num = num * 59 + this.PlatformUserId.GetHashCode();
			}
			return num;
		}
	}
}
