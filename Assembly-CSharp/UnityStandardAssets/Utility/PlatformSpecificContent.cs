using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000674 RID: 1652
	public class PlatformSpecificContent : MonoBehaviour
	{
		// Token: 0x060033FF RID: 13311 RVA: 0x00048D16 File Offset: 0x00046F16
		private void OnEnable()
		{
			this.CheckEnableContent();
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x00048D1E File Offset: 0x00046F1E
		private void CheckEnableContent()
		{
			if (this.m_BuildTargetGroup == PlatformSpecificContent.BuildTargetGroup.Mobile)
			{
				this.EnableContent(false);
				return;
			}
			this.EnableContent(true);
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x00133854 File Offset: 0x00131A54
		private void EnableContent(bool enabled)
		{
			if (this.m_Content.Length != 0)
			{
				foreach (GameObject gameObject in this.m_Content)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(enabled);
					}
				}
			}
			if (this.m_ChildrenOfThisObject)
			{
				foreach (object obj in base.transform)
				{
					((Transform)obj).gameObject.SetActive(enabled);
				}
			}
			if (this.m_MonoBehaviours.Length != 0)
			{
				MonoBehaviour[] monoBehaviours = this.m_MonoBehaviours;
				for (int i = 0; i < monoBehaviours.Length; i++)
				{
					monoBehaviours[i].enabled = enabled;
				}
			}
		}

		// Token: 0x04002449 RID: 9289
		[SerializeField]
		private PlatformSpecificContent.BuildTargetGroup m_BuildTargetGroup;

		// Token: 0x0400244A RID: 9290
		[SerializeField]
		private GameObject[] m_Content = new GameObject[0];

		// Token: 0x0400244B RID: 9291
		[SerializeField]
		private MonoBehaviour[] m_MonoBehaviours = new MonoBehaviour[0];

		// Token: 0x0400244C RID: 9292
		[SerializeField]
		private bool m_ChildrenOfThisObject;

		// Token: 0x02000675 RID: 1653
		private enum BuildTargetGroup
		{
			// Token: 0x0400244E RID: 9294
			Standalone,
			// Token: 0x0400244F RID: 9295
			Mobile
		}
	}
}
