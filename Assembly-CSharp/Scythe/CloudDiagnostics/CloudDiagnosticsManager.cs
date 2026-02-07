using System;
using System.Collections.Generic;
using System.Text;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.CloudDiagnostics
{
	// Token: 0x02000388 RID: 904
	public class CloudDiagnosticsManager : SingletonMono<CloudDiagnosticsManager>
	{
		// Token: 0x06001A22 RID: 6690 RVA: 0x00039629 File Offset: 0x00037829
		public void Awake()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x000A3684 File Offset: 0x000A1884
		public void RegisterDataProvider(ICloudDiagnosticsDataProvider dataProvider)
		{
			if (this.dataProvidersHashSet.Contains(dataProvider))
			{
				Debug.LogError(string.Format("{0} is already registered!", dataProvider.GetType()));
				return;
			}
			this.dataProvidersHashSet.Add(dataProvider);
			this.dataProvidersList.Add(dataProvider);
			dataProvider.OnRegister();
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00039636 File Offset: 0x00037836
		public void UnregisterDataProvider(ICloudDiagnosticsDataProvider dataProvider)
		{
			if (!this.dataProvidersHashSet.Remove(dataProvider))
			{
				Debug.LogError(string.Format("Failed to remove data provider: {0}. Was it added?", dataProvider.GetType()));
				return;
			}
			this.dataProvidersList.Remove(dataProvider);
			dataProvider.OnUnregister();
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0003966F File Offset: 0x0003786F
		[ContextMenu("Test diagnostics")]
		public void DebugTestCloudDiagnostics()
		{
			Debug.LogException(new Exception("Cloud Diagnostics exception 3 test!"));
			Debug.Log("Testing manually caused exception.");
			(new object[0])[1] = null;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x000A36D4 File Offset: 0x000A18D4
		[ContextMenu("List registered data providers")]
		public void DebugListRegisteredDataProviders()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("There are {0} data providers:", this.dataProvidersList.Count));
			for (int i = 0; i < this.dataProvidersList.Count; i++)
			{
				stringBuilder.AppendLine(string.Format("[{0}] {1}", i, this.dataProvidersList[i].GetType()));
			}
			Debug.Log(stringBuilder.ToString());
		}

		// Token: 0x040012C6 RID: 4806
		private List<ICloudDiagnosticsDataProvider> dataProvidersList = new List<ICloudDiagnosticsDataProvider>();

		// Token: 0x040012C7 RID: 4807
		private HashSet<ICloudDiagnosticsDataProvider> dataProvidersHashSet = new HashSet<ICloudDiagnosticsDataProvider>();
	}
}
