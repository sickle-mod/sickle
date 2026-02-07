using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000678 RID: 1656
	public class TimedObjectActivator : MonoBehaviour
	{
		// Token: 0x06003409 RID: 13321 RVA: 0x00133D50 File Offset: 0x00131F50
		private void Awake()
		{
			foreach (TimedObjectActivator.Entry entry in this.entries.entries)
			{
				switch (entry.action)
				{
				case TimedObjectActivator.Action.Activate:
					base.StartCoroutine(this.Activate(entry));
					break;
				case TimedObjectActivator.Action.Deactivate:
					base.StartCoroutine(this.Deactivate(entry));
					break;
				case TimedObjectActivator.Action.Destroy:
					global::UnityEngine.Object.Destroy(entry.target, entry.delay);
					break;
				case TimedObjectActivator.Action.ReloadLevel:
					base.StartCoroutine(this.ReloadLevel(entry));
					break;
				}
			}
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x00048D89 File Offset: 0x00046F89
		private IEnumerator Activate(TimedObjectActivator.Entry entry)
		{
			yield return new WaitForSeconds(entry.delay);
			entry.target.SetActive(true);
			yield break;
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x00048D98 File Offset: 0x00046F98
		private IEnumerator Deactivate(TimedObjectActivator.Entry entry)
		{
			yield return new WaitForSeconds(entry.delay);
			entry.target.SetActive(false);
			yield break;
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x00048DA7 File Offset: 0x00046FA7
		private IEnumerator ReloadLevel(TimedObjectActivator.Entry entry)
		{
			yield return new WaitForSeconds(entry.delay);
			SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
			yield break;
		}

		// Token: 0x0400245F RID: 9311
		public TimedObjectActivator.Entries entries = new TimedObjectActivator.Entries();

		// Token: 0x02000679 RID: 1657
		public enum Action
		{
			// Token: 0x04002461 RID: 9313
			Activate,
			// Token: 0x04002462 RID: 9314
			Deactivate,
			// Token: 0x04002463 RID: 9315
			Destroy,
			// Token: 0x04002464 RID: 9316
			ReloadLevel,
			// Token: 0x04002465 RID: 9317
			Call
		}

		// Token: 0x0200067A RID: 1658
		[Serializable]
		public class Entry
		{
			// Token: 0x04002466 RID: 9318
			public GameObject target;

			// Token: 0x04002467 RID: 9319
			public TimedObjectActivator.Action action;

			// Token: 0x04002468 RID: 9320
			public float delay;
		}

		// Token: 0x0200067B RID: 1659
		[Serializable]
		public class Entries
		{
			// Token: 0x04002469 RID: 9321
			public TimedObjectActivator.Entry[] entries;
		}
	}
}
