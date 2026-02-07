using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000355 RID: 853
	[DataContract]
	public class AddBuddyByNameRequest : IEquatable<AddBuddyByNameRequest>
	{
		// Token: 0x06001873 RID: 6259 RVA: 0x00038864 File Offset: 0x00036A64
		[Preserve]
		public AddBuddyByNameRequest(string Name = null)
		{
			if (Name == null)
			{
				throw new InvalidDataException("Name is a required property for AddBuddyByNameRequest and cannot be null");
			}
			this.Name = Name;
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x00038881 File Offset: 0x00036A81
		// (set) Token: 0x06001875 RID: 6261 RVA: 0x00038889 File Offset: 0x00036A89
		[DataMember(Name = "name", EmitDefaultValue = true)]
		public string Name { get; set; }

		// Token: 0x06001876 RID: 6262 RVA: 0x000A01C8 File Offset: 0x0009E3C8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AddBuddyByNameRequest {\n");
			stringBuilder.Append("  Name: ").Append(this.Name).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0003889B File Offset: 0x00036A9B
		public override bool Equals(object input)
		{
			return this.Equals(input as AddBuddyByNameRequest);
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x000388A9 File Offset: 0x00036AA9
		public bool Equals(AddBuddyByNameRequest input)
		{
			return input != null && (this.Name == input.Name || (this.Name != null && this.Name.Equals(input.Name)));
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x000A0218 File Offset: 0x0009E418
		public override int GetHashCode()
		{
			int num = 41;
			if (this.Name != null)
			{
				num = num * 59 + this.Name.GetHashCode();
			}
			return num;
		}
	}
}
