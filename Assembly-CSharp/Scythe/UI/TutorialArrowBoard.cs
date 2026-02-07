using System;
using DG.Tweening;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000391 RID: 913
	public class TutorialArrowBoard : TutorialArrow
	{
		// Token: 0x06001B48 RID: 6984 RVA: 0x00039C4F File Offset: 0x00037E4F
		protected override void OnEnable()
		{
			base.OnEnable();
			this.animator.SetBool("isBoardJump", true);
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00039C68 File Offset: 0x00037E68
		public void SetPosition(Vector3 worldPosition)
		{
			if (this.baseRect != null)
			{
				this.baseRect.anchoredPosition3D = new Vector3(worldPosition.z, worldPosition.x, this.heightOffset);
			}
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000AC064 File Offset: 0x000AA264
		public void SetPosition(GameHex gameHex)
		{
			Vector2 worldPosition = GameController.Instance.GetGameHexPresenter(gameHex).GetWorldPosition();
			if (this.baseRect != null)
			{
				this.baseRect.anchoredPosition3D = new Vector3(worldPosition.y, worldPosition.x, this.heightOffset);
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000AC0B4 File Offset: 0x000AA2B4
		protected override void Animate()
		{
			this.imageRect.DOAnchorPos3D(new Vector3(0f, 0f, this.jumpHeight), this.jumpHeight / this.speed, false).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo)
				.SetId(this);
			this.imageRect.DOScale(this.squash, 0.1f);
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x00039C9A File Offset: 0x00037E9A
		protected override void StopAnimating()
		{
			DOTween.Kill(this, false);
			this.imageRect.anchoredPosition3D = Vector3.zero;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x00039CB4 File Offset: 0x00037EB4
		private Vector3 Squash()
		{
			return new Vector3(1f / (1f - this.squash), 1f - this.squash, 1f);
		}

		// Token: 0x04001339 RID: 4921
		public float heightOffset = 0.01f;

		// Token: 0x0400133A RID: 4922
		[Range(1E-05f, 1f)]
		public float squash;
	}
}
