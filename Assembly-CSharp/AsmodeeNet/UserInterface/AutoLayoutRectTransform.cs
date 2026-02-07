using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200078D RID: 1933
	[RequireComponent(typeof(RectTransform))]
	public class AutoLayoutRectTransform : MonoBehaviour
	{
		// Token: 0x060037E3 RID: 14307 RVA: 0x0004BD88 File Offset: 0x00049F88
		private void Awake()
		{
			this._rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x0004BD96 File Offset: 0x00049F96
		private void OnEnable()
		{
			CoreApplication.Instance.CommunityHubLauncher.communityHubDidStart += this._LinkCommunityHub;
			this._LinkCommunityHub();
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x0004BDB9 File Offset: 0x00049FB9
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.CommunityHubLauncher.communityHubDidStart -= this._LinkCommunityHub;
			CoreApplication.Instance.CommunityHub.RemoveTransformToAutoLayout(this._rectTransform);
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x0004BDF3 File Offset: 0x00049FF3
		private void _LinkCommunityHub()
		{
			if (!CoreApplication.Instance.CommunityHubLauncher.IsCommunityHubLaunched)
			{
				return;
			}
			CoreApplication.Instance.CommunityHub.AddTransformToAutoLayout(this._rectTransform);
		}

		// Token: 0x040029FC RID: 10748
		private const string _documentation = "<b>AutoLayoutRectTransform</b> automatically registers its <b>RectTransform</b> to <b>CommunityHub.TransformsToAutoLayout</b>.";

		// Token: 0x040029FD RID: 10749
		private RectTransform _rectTransform;
	}
}
