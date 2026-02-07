using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Scythe.Utilities
{
	// Token: 0x020001D0 RID: 464
	internal sealed class InnyBinder : DefaultSerializationBinder
	{
		// Token: 0x06000D74 RID: 3444 RVA: 0x0008587C File Offset: 0x00083A7C
		public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			base.BindToName(serializedType, out assemblyName, out typeName);
			TypeForwardedFromAttribute typeForwardedFromAttribute = Attribute.GetCustomAttribute(serializedType, typeof(TypeForwardedFromAttribute), false) as TypeForwardedFromAttribute;
			if (typeForwardedFromAttribute != null)
			{
				assemblyName = typeForwardedFromAttribute.AssemblyFullName;
			}
			if (assemblyName.StartsWith("System.Private.CoreLib"))
			{
				assemblyName = "mscorlib";
			}
		}
	}
}
