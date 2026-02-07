using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200035A RID: 858
	[DataContract]
	public class BuddyDto : IEquatable<BuddyDto>
	{
		// Token: 0x06001897 RID: 6295 RVA: 0x000389AE File Offset: 0x00036BAE
		[Preserve]
		public BuddyDto(Guid Id = default(Guid), string Name = null)
		{
			this.Id = Id;
			this.Name = Name;
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x000389C4 File Offset: 0x00036BC4
		// (set) Token: 0x06001899 RID: 6297 RVA: 0x000389CC File Offset: 0x00036BCC
		[DataMember(Name = "id", EmitDefaultValue = true)]
		public Guid Id { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600189A RID: 6298 RVA: 0x000389D5 File Offset: 0x00036BD5
		// (set) Token: 0x0600189B RID: 6299 RVA: 0x000389DD File Offset: 0x00036BDD
		[DataMember(Name = "name", EmitDefaultValue = true)]
		public string Name { get; set; }

		// Token: 0x0600189C RID: 6300 RVA: 0x000A0690 File Offset: 0x0009E890
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class BuddyDto {\n");
			stringBuilder.Append("  Id: ").Append(this.Id).Append("\n");
			stringBuilder.Append("  Name: ").Append(this.Name).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x000389E6 File Offset: 0x00036BE6
		public override bool Equals(object input)
		{
			return this.Equals(input as BuddyDto);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x000A0708 File Offset: 0x0009E908
		public bool Equals(BuddyDto input)
		{
			if (input == null)
			{
				return false;
			}
			if (!(this.Id == input.Id))
			{
				Guid id = this.Id;
				if (!this.Id.Equals(input.Id))
				{
					return false;
				}
			}
			return this.Name == input.Name || (this.Name != null && this.Name.Equals(input.Name));
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x000A077C File Offset: 0x0009E97C
		public override int GetHashCode()
		{
			int num = 41;
			Guid id = this.Id;
			num = num * 59 + this.Id.GetHashCode();
			if (this.Name != null)
			{
				num = num * 59 + this.Name.GetHashCode();
			}
			return num;
		}
	}
}
