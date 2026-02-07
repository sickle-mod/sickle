using System;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.CrashReportHandler;

namespace Scythe.CloudDiagnostics
{
	// Token: 0x02000387 RID: 903
	public class CloudDiagnosticsDefaultDataProvider : MonoBehaviour, ICloudDiagnosticsDataProvider
	{
		// Token: 0x06001A1C RID: 6684 RVA: 0x000395A4 File Offset: 0x000377A4
		private void OnEnable()
		{
			if (!this.isRegistered)
			{
				SingletonMono<CloudDiagnosticsManager>.Instance.RegisterDataProvider(this);
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x000395B9 File Offset: 0x000377B9
		private void OnDisable()
		{
			if (this.isRegistered)
			{
				SingletonMono<CloudDiagnosticsManager>.Instance.UnregisterDataProvider(this);
			}
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x000395CE File Offset: 0x000377CE
		public void UpdateDiagnosticData()
		{
			if (this.isRegistered)
			{
				CrashReportHandler.SetUserMetadata("buildTag", this.buildTag);
				return;
			}
			CrashReportHandler.SetUserMetadata("buildTag", string.Empty);
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000395F8 File Offset: 0x000377F8
		public void OnRegister()
		{
			this.isRegistered = true;
			this.UpdateDiagnosticData();
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00039607 File Offset: 0x00037807
		public void OnUnregister()
		{
			this.isRegistered = false;
			this.UpdateDiagnosticData();
		}

		// Token: 0x040012C4 RID: 4804
		[SerializeField]
		private string buildTag = "Default";

		// Token: 0x040012C5 RID: 4805
		private bool isRegistered;
	}
}
