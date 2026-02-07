using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200036B RID: 875
	[DataContract]
	public class LoginRequest : IEquatable<LoginRequest>
	{
		// Token: 0x06001931 RID: 6449 RVA: 0x00038E61 File Offset: 0x00037061
		[Preserve]
		public LoginRequest(AuthGrantType AuthGrantType = AuthGrantType.Undefined, LoginParams LoginParams = null)
		{
			this.AuthGrantType = AuthGrantType;
			if (LoginParams == null)
			{
				throw new InvalidDataException("LoginParams is a required property for LoginRequest and cannot be null");
			}
			this.LoginParams = LoginParams;
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001932 RID: 6450 RVA: 0x00038E85 File Offset: 0x00037085
		// (set) Token: 0x06001933 RID: 6451 RVA: 0x00038E8D File Offset: 0x0003708D
		[DataMember(Name = "authGrantType", EmitDefaultValue = true)]
		public AuthGrantType AuthGrantType { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001934 RID: 6452 RVA: 0x00038E96 File Offset: 0x00037096
		// (set) Token: 0x06001935 RID: 6453 RVA: 0x00038E9E File Offset: 0x0003709E
		[DataMember(Name = "loginParams", EmitDefaultValue = true)]
		public LoginParams LoginParams { get; set; }

		// Token: 0x06001936 RID: 6454 RVA: 0x000A19C0 File Offset: 0x0009FBC0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LoginRequest {\n");
			stringBuilder.Append("  AuthGrantType: ").Append(this.AuthGrantType).Append("\n");
			stringBuilder.Append("  LoginParams: ").Append(this.LoginParams).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x00038EA7 File Offset: 0x000370A7
		public override bool Equals(object input)
		{
			return this.Equals(input as LoginRequest);
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x000A1A38 File Offset: 0x0009FC38
		public bool Equals(LoginRequest input)
		{
			if (input == null)
			{
				return false;
			}
			if (this.AuthGrantType != input.AuthGrantType)
			{
				AuthGrantType authGrantType = this.AuthGrantType;
				if (!this.AuthGrantType.Equals(input.AuthGrantType))
				{
					return false;
				}
			}
			return this.LoginParams == input.LoginParams || (this.LoginParams != null && this.LoginParams.Equals(input.LoginParams));
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x000A1AB0 File Offset: 0x0009FCB0
		public override int GetHashCode()
		{
			int num = 41;
			AuthGrantType authGrantType = this.AuthGrantType;
			num = num * 59 + this.AuthGrantType.GetHashCode();
			if (this.LoginParams != null)
			{
				num = num * 59 + this.LoginParams.GetHashCode();
			}
			return num;
		}
	}
}
