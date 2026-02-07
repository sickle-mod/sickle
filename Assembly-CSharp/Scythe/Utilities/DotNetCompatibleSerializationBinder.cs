using System;
using Newtonsoft.Json.Serialization;

namespace Scythe.Utilities
{
	// Token: 0x020001D1 RID: 465
	internal sealed class DotNetCompatibleSerializationBinder : DefaultSerializationBinder
	{
		// Token: 0x06000D76 RID: 3446 RVA: 0x00030EC4 File Offset: 0x0002F0C4
		public override Type BindToType(string assemblyName, string typeName)
		{
			if (assemblyName != "System.Private.CoreLib")
			{
				return base.BindToType(assemblyName, typeName);
			}
			assemblyName = "mscorlib";
			typeName = typeName.Replace("System.Private.CoreLib", "mscorlib");
			return base.BindToType(assemblyName, typeName);
		}

		// Token: 0x04000AC9 RID: 2761
		private const string CoreLibAssembly = "System.Private.CoreLib";

		// Token: 0x04000ACA RID: 2762
		private const string MscorlibAssembly = "mscorlib";
	}
}
