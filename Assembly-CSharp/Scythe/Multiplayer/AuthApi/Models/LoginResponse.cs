using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200036C RID: 876
	[DataContract]
	public class LoginResponse : IEquatable<LoginResponse>
	{
		// Token: 0x0600193B RID: 6459 RVA: 0x000A1AFC File Offset: 0x0009FCFC
		[Preserve]
		public LoginResponse(Result Result = Result.Success, Guid? Id = null, string Login = null, string Email = null, bool? TransferNeeded = null, bool? ActivationNeeded = null, string Token = null, string AccessToken = null, string RefreshToken = null, string ActivationToken = null, List<LinkedAccount> LinkedAccounts = null)
		{
			this.Result = Result;
			this.Id = Id;
			this.Login = Login;
			this.Email = Email;
			this.TransferNeeded = TransferNeeded;
			this.ActivationNeeded = ActivationNeeded;
			this.Token = Token;
			this.AccessToken = AccessToken;
			this.RefreshToken = RefreshToken;
			this.ActivationToken = ActivationToken;
			this.LinkedAccounts = LinkedAccounts;
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x00027E56 File Offset: 0x00026056
		public LoginResponse()
		{
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x00038EB5 File Offset: 0x000370B5
		// (set) Token: 0x0600193E RID: 6462 RVA: 0x00038EBD File Offset: 0x000370BD
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x00038EC6 File Offset: 0x000370C6
		// (set) Token: 0x06001940 RID: 6464 RVA: 0x00038ECE File Offset: 0x000370CE
		[DataMember(Name = "id", EmitDefaultValue = true)]
		public Guid? Id { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x00038ED7 File Offset: 0x000370D7
		// (set) Token: 0x06001942 RID: 6466 RVA: 0x00038EDF File Offset: 0x000370DF
		[DataMember(Name = "login", EmitDefaultValue = true)]
		public string Login { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x00038EE8 File Offset: 0x000370E8
		// (set) Token: 0x06001944 RID: 6468 RVA: 0x00038EF0 File Offset: 0x000370F0
		[DataMember(Name = "email", EmitDefaultValue = true)]
		public string Email { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001945 RID: 6469 RVA: 0x00038EF9 File Offset: 0x000370F9
		// (set) Token: 0x06001946 RID: 6470 RVA: 0x00038F01 File Offset: 0x00037101
		[DataMember(Name = "transferNeeded", EmitDefaultValue = true)]
		public bool? TransferNeeded { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x00038F0A File Offset: 0x0003710A
		// (set) Token: 0x06001948 RID: 6472 RVA: 0x00038F12 File Offset: 0x00037112
		[DataMember(Name = "activationNeeded", EmitDefaultValue = true)]
		public bool? ActivationNeeded { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001949 RID: 6473 RVA: 0x00038F1B File Offset: 0x0003711B
		// (set) Token: 0x0600194A RID: 6474 RVA: 0x00038F23 File Offset: 0x00037123
		[DataMember(Name = "token", EmitDefaultValue = true)]
		public string Token { get; set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x00038F2C File Offset: 0x0003712C
		// (set) Token: 0x0600194C RID: 6476 RVA: 0x00038F34 File Offset: 0x00037134
		[DataMember(Name = "accessToken", EmitDefaultValue = true)]
		public string AccessToken { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600194D RID: 6477 RVA: 0x00038F3D File Offset: 0x0003713D
		// (set) Token: 0x0600194E RID: 6478 RVA: 0x00038F45 File Offset: 0x00037145
		[DataMember(Name = "refreshToken", EmitDefaultValue = true)]
		public string RefreshToken { get; set; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600194F RID: 6479 RVA: 0x00038F4E File Offset: 0x0003714E
		// (set) Token: 0x06001950 RID: 6480 RVA: 0x00038F56 File Offset: 0x00037156
		[DataMember(Name = "activationToken", EmitDefaultValue = true)]
		public string ActivationToken { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x00038F5F File Offset: 0x0003715F
		// (set) Token: 0x06001952 RID: 6482 RVA: 0x00038F67 File Offset: 0x00037167
		[DataMember(Name = "linkedAccounts", EmitDefaultValue = true)]
		public List<LinkedAccount> LinkedAccounts { get; set; }

		// Token: 0x06001953 RID: 6483 RVA: 0x000A1B64 File Offset: 0x0009FD64
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LoginResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Id: ").Append(this.Id).Append("\n");
			stringBuilder.Append("  Login: ").Append(this.Login).Append("\n");
			stringBuilder.Append("  Email: ").Append(this.Email).Append("\n");
			stringBuilder.Append("  TransferNeeded: ").Append(this.TransferNeeded).Append("\n");
			stringBuilder.Append("  ActivationNeeded: ").Append(this.ActivationNeeded).Append("\n");
			stringBuilder.Append("  Token: ").Append(this.Token).Append("\n");
			stringBuilder.Append("  AccessToken: ").Append(this.AccessToken).Append("\n");
			stringBuilder.Append("  RefreshToken: ").Append(this.RefreshToken).Append("\n");
			stringBuilder.Append("  ActivationToken: ").Append(this.ActivationToken).Append("\n");
			stringBuilder.Append("  LinkedAccounts: ").Append(this.LinkedAccounts).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00038F70 File Offset: 0x00037170
		public override bool Equals(object input)
		{
			return this.Equals(input as LoginResponse);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x000A1D14 File Offset: 0x0009FF14
		public bool Equals(LoginResponse input)
		{
			if (input == null)
			{
				return false;
			}
			if (this.Result != input.Result)
			{
				Result result = this.Result;
				if (!this.Result.Equals(input.Result))
				{
					return false;
				}
			}
			if ((this.Id == input.Id || (this.Id != null && this.Id.Equals(input.Id))) && (this.Login == input.Login || (this.Login != null && this.Login.Equals(input.Login))) && (this.Email == input.Email || (this.Email != null && this.Email.Equals(input.Email))))
			{
				bool? flag = this.TransferNeeded;
				bool? flag2 = input.TransferNeeded;
				if (((flag.GetValueOrDefault() == flag2.GetValueOrDefault()) & (flag != null == (flag2 != null))) || (this.TransferNeeded != null && this.TransferNeeded.Equals(input.TransferNeeded)))
				{
					flag2 = this.ActivationNeeded;
					flag = input.ActivationNeeded;
					if ((((flag2.GetValueOrDefault() == flag.GetValueOrDefault()) & (flag2 != null == (flag != null))) || (this.ActivationNeeded != null && this.ActivationNeeded.Equals(input.ActivationNeeded))) && (this.Token == input.Token || (this.Token != null && this.Token.Equals(input.Token))) && (this.AccessToken == input.AccessToken || (this.AccessToken != null && this.AccessToken.Equals(input.AccessToken))) && (this.RefreshToken == input.RefreshToken || (this.RefreshToken != null && this.RefreshToken.Equals(input.RefreshToken))) && (this.ActivationToken == input.ActivationToken || (this.ActivationToken != null && this.ActivationToken.Equals(input.ActivationToken))))
					{
						return this.LinkedAccounts == input.LinkedAccounts || (this.LinkedAccounts != null && this.LinkedAccounts.SequenceEqual(input.LinkedAccounts));
					}
				}
			}
			return false;
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x000A2008 File Offset: 0x000A0208
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			if (this.Id != null)
			{
				num = num * 59 + this.Id.GetHashCode();
			}
			if (this.Login != null)
			{
				num = num * 59 + this.Login.GetHashCode();
			}
			if (this.Email != null)
			{
				num = num * 59 + this.Email.GetHashCode();
			}
			if (this.TransferNeeded != null)
			{
				num = num * 59 + this.TransferNeeded.GetHashCode();
			}
			if (this.ActivationNeeded != null)
			{
				num = num * 59 + this.ActivationNeeded.GetHashCode();
			}
			if (this.Token != null)
			{
				num = num * 59 + this.Token.GetHashCode();
			}
			if (this.AccessToken != null)
			{
				num = num * 59 + this.AccessToken.GetHashCode();
			}
			if (this.RefreshToken != null)
			{
				num = num * 59 + this.RefreshToken.GetHashCode();
			}
			if (this.ActivationToken != null)
			{
				num = num * 59 + this.ActivationToken.GetHashCode();
			}
			if (this.LinkedAccounts != null)
			{
				num = num * 59 + this.LinkedAccounts.GetHashCode();
			}
			return num;
		}
	}
}
