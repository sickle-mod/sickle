using System;
using DG.Tweening;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000390 RID: 912
	public abstract class TutorialArrow : MonoBehaviour
	{
		// Token: 0x06001B40 RID: 6976 RVA: 0x000AC014 File Offset: 0x000AA214
		protected void Awake()
		{
			this.baseRect = base.gameObject.GetComponent<RectTransform>();
			this.animator = base.gameObject.GetComponent<Animator>();
			if (this.imageRect == null)
			{
				this.imageRect = base.gameObject.GetComponentInChildren<RectTransform>();
			}
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00039C12 File Offset: 0x00037E12
		protected virtual void OnEnable()
		{
			this.animator.SetBool("animated", this.animated);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00027EF0 File Offset: 0x000260F0
		protected void OnDisable()
		{
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0002D68D File Offset: 0x0002B88D
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x00029172 File Offset: 0x00027372
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001B45 RID: 6981
		protected abstract void Animate();

		// Token: 0x06001B46 RID: 6982
		protected abstract void StopAnimating();

		// Token: 0x04001331 RID: 4913
		public bool animated = true;

		// Token: 0x04001332 RID: 4914
		[Tooltip("Speed of arrow vertical movemnet")]
		public float speed = 6f;

		// Token: 0x04001333 RID: 4915
		[Tooltip("")]
		public float jumpHeight = 1f;

		// Token: 0x04001334 RID: 4916
		public AnimationCurve jumpEase;

		// Token: 0x04001335 RID: 4917
		public RectTransform imageRect;

		// Token: 0x04001336 RID: 4918
		public RectTransform baseRect;

		// Token: 0x04001337 RID: 4919
		protected Animator animator;

		// Token: 0x04001338 RID: 4920
		protected Tweener tweener;
	}
}
