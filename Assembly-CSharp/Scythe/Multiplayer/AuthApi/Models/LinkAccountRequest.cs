using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000366 RID: 870
	[DataContract]
	public class LinkAccountRequest : IEquatable<LinkAccountRequest>
	{
		// Token: 0x060018FD RID: 6397 RVA: 0x00038CF2 File Offset: 0x00036EF2
		[Preserve]
		public LinkAccountRequest(LoginPlatform Platform = LoginPlatform.Undefined, LinkAccountParams LinkAccountParams = null)
		{
			this.Platform = Platform;
			if (LinkAccountParams == null)
			{
				throw new InvalidDataException("LinkAccountParams is a required property for LinkAccountRequest and cannot be null");
			}
			this.LinkAccountParams = LinkAccountParams;
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x00038D16 File Offset: 0x00036F16
		// (set) Token: 0x060018FF RID: 6399 RVA: 0x00038D1E File Offset: 0x00036F1E
		[DataMember(Name = "platform", EmitDefaultValue = true)]
		public LoginPlatform Platform { get; set; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x00038D27 File Offset: 0x00036F27
		// (set) Token: 0x06001901 RID: 6401 RVA: 0x00038D2F File Offset: 0x00036F2F
		[DataMember(Name = "linkAccountParams", EmitDefaultValue = true)]
		public LinkAccountParams LinkAccountParams { get; set; }

		// Token: 0x06001902 RID: 6402 RVA: 0x000A11A0 File Offset: 0x0009F3A0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LinkAccountRequest {\n");
			stringBuilder.Append("  Platform: ").Append(this.Platform).Append("\n");
			stringBuilder.Append("  LinkAccountParams: ").Append(this.LinkAccountParams).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x00038D38 File Offset: 0x00036F38
		public override bool Equals(object input)
		{
			return this.Equals(input as LinkAccountRequest);
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x000A1218 File Offset: 0x0009F418
		public bool Equals(LinkAccountRequest input)
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
			return this.LinkAccountParams == input.LinkAccountParams || (this.LinkAccountParams != null && this.LinkAccountParams.Equals(input.LinkAccountParams));
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x000A1290 File Offset: 0x0009F490
		public override int GetHashCode()
		{
			int num = 41;
			LoginPlatform platform = this.Platform;
			num = num * 59 + this.Platform.GetHashCode();
			if (this.LinkAccountParams != null)
			{
				num = num * 59 + this.LinkAccountParams.GetHashCode();
			}
			return num;
		}
	}
}
