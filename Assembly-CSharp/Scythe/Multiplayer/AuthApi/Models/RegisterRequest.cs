using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200036F RID: 879
	[DataContract]
	public class RegisterRequest : IEquatable<RegisterRequest>
	{
		// Token: 0x06001966 RID: 6502 RVA: 0x000A223C File Offset: 0x000A043C
		[Preserve]
		public RegisterRequest(string Login = null, string Password = null, string Email = null)
		{
			if (Login == null)
			{
				throw new InvalidDataException("Login is a required property for RegisterRequest and cannot be null");
			}
			this.Login = Login;
			if (Password == null)
			{
				throw new InvalidDataException("Password is a required property for RegisterRequest and cannot be null");
			}
			this.Password = Password;
			if (Email == null)
			{
				throw new InvalidDataException("Email is a required property for RegisterRequest and cannot be null");
			}
			this.Email = Email;
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x00038FDE File Offset: 0x000371DE
		// (set) Token: 0x06001968 RID: 6504 RVA: 0x00038FE6 File Offset: 0x000371E6
		[DataMember(Name = "login", EmitDefaultValue = true)]
		public string Login { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x00038FEF File Offset: 0x000371EF
		// (set) Token: 0x0600196A RID: 6506 RVA: 0x00038FF7 File Offset: 0x000371F7
		[DataMember(Name = "password", EmitDefaultValue = true)]
		public string Password { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x00039000 File Offset: 0x00037200
		// (set) Token: 0x0600196C RID: 6508 RVA: 0x00039008 File Offset: 0x00037208
		[DataMember(Name = "email", EmitDefaultValue = true)]
		public string Email { get; set; }

		// Token: 0x0600196D RID: 6509 RVA: 0x000A2290 File Offset: 0x000A0490
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class RegisterRequest {\n");
			stringBuilder.Append("  Login: ").Append(this.Login).Append("\n");
			stringBuilder.Append("  Password: ").Append(this.Password).Append("\n");
			stringBuilder.Append("  Email: ").Append(this.Email).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00039011 File Offset: 0x00037211
		public override bool Equals(object input)
		{
			return this.Equals(input as RegisterRequest);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x000A2324 File Offset: 0x000A0524
		public bool Equals(RegisterRequest input)
		{
			return input != null && ((this.Login == input.Login || (this.Login != null && this.Login.Equals(input.Login))) && (this.Password == input.Password || (this.Password != null && this.Password.Equals(input.Password)))) && (this.Email == input.Email || (this.Email != null && this.Email.Equals(input.Email)));
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x000A23C4 File Offset: 0x000A05C4
		public override int GetHashCode()
		{
			int num = 41;
			if (this.Login != null)
			{
				num = num * 59 + this.Login.GetHashCode();
			}
			if (this.Password != null)
			{
				num = num * 59 + this.Password.GetHashCode();
			}
			if (this.Email != null)
			{
				num = num * 59 + this.Email.GetHashCode();
			}
			return num;
		}
	}
}
