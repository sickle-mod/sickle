using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x0200065D RID: 1629
	public class ActivateTrigger : MonoBehaviour
	{
		// Token: 0x060033AC RID: 13228 RVA: 0x00132B14 File Offset: 0x00130D14
		private void DoActivateTrigger()
		{
			this.triggerCount--;
			if (this.triggerCount == 0 || this.repeatTrigger)
			{
				global::UnityEngine.Object @object = this.target ?? base.gameObject;
				Behaviour behaviour = @object as Behaviour;
				GameObject gameObject = @object as GameObject;
				if (behaviour != null)
				{
					gameObject = behaviour.gameObject;
				}
				switch (this.action)
				{
				case ActivateTrigger.Mode.Trigger:
					if (gameObject != null)
					{
						gameObject.BroadcastMessage("DoActivateTrigger");
						return;
					}
					break;
				case ActivateTrigger.Mode.Replace:
					if (this.source != null && gameObject != null)
					{
						global::UnityEngine.Object.Instantiate<GameObject>(this.source, gameObject.transform.position, gameObject.transform.rotation);
						global::UnityEngine.Object.Destroy(gameObject);
						return;
					}
					break;
				case ActivateTrigger.Mode.Activate:
					if (gameObject != null)
					{
						gameObject.SetActive(true);
						return;
					}
					break;
				case ActivateTrigger.Mode.Enable:
					if (behaviour != null)
					{
						behaviour.enabled = true;
						return;
					}
					break;
				case ActivateTrigger.Mode.Animate:
					if (gameObject != null)
					{
						gameObject.GetComponent<Animation>().Play();
						return;
					}
					break;
				case ActivateTrigger.Mode.Deactivate:
					if (gameObject != null)
					{
						gameObject.SetActive(false);
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x00048A3F File Offset: 0x00046C3F
		private void OnTriggerEnter(Collider other)
		{
			this.DoActivateTrigger();
		}

		// Token: 0x040023DE RID: 9182
		public ActivateTrigger.Mode action = ActivateTrigger.Mode.Activate;

		// Token: 0x040023DF RID: 9183
		public global::UnityEngine.Object target;

		// Token: 0x040023E0 RID: 9184
		public GameObject source;

		// Token: 0x040023E1 RID: 9185
		public int triggerCount = 1;

		// Token: 0x040023E2 RID: 9186
		public bool repeatTrigger;

		// Token: 0x0200065E RID: 1630
		public enum Mode
		{
			// Token: 0x040023E4 RID: 9188
			Trigger,
			// Token: 0x040023E5 RID: 9189
			Replace,
			// Token: 0x040023E6 RID: 9190
			Activate,
			// Token: 0x040023E7 RID: 9191
			Enable,
			// Token: 0x040023E8 RID: 9192
			Animate,
			// Token: 0x040023E9 RID: 9193
			Deactivate
		}
	}
}
