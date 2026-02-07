using System;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200078B RID: 1931
	public class AlertControllerButton : MonoBehaviour
	{
		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060037DC RID: 14300 RVA: 0x0004BD69 File Offset: 0x00049F69
		// (set) Token: 0x060037DD RID: 14301 RVA: 0x0004BD71 File Offset: 0x00049F71
		public AlertController.ButtonStyle Style
		{
			get
			{
				return this._style;
			}
			set
			{
				this._style = value;
				this._UpdateStyle();
			}
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x001488A4 File Offset: 0x00146AA4
		private void Awake()
		{
			this._button = base.GetComponent<Button>();
			if (this._button == null)
			{
				AsmoLogger.Error("AlertControllerButton", "Missing Button behavior", null);
			}
			if (this.cancelSprites == null)
			{
				this.cancelSprites = this.defaultSprites;
			}
			if (this.destructiveSprites == null)
			{
				this.destructiveSprites = this.defaultSprites;
			}
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x0004BD80 File Offset: 0x00049F80
		private void OnEnable()
		{
			this._UpdateStyle();
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x00148904 File Offset: 0x00146B04
		private void _UpdateStyle()
		{
			AlertController.ButtonStyle style = this._style;
			AlertControllerButton.FullSpriteState fullSpriteState;
			if (style != AlertController.ButtonStyle.Cancel)
			{
				if (style != AlertController.ButtonStyle.Destructive)
				{
					fullSpriteState = this.defaultSprites;
				}
				else
				{
					fullSpriteState = this.destructiveSprites;
				}
			}
			else
			{
				fullSpriteState = this.cancelSprites;
			}
			(this._button.targetGraphic as Image).sprite = fullSpriteState.idle;
			this._button.spriteState = fullSpriteState.spriteState;
		}

		// Token: 0x040029F4 RID: 10740
		private const string _kModuleName = "AlertControllerButton";

		// Token: 0x040029F5 RID: 10741
		public AlertControllerButton.FullSpriteState defaultSprites;

		// Token: 0x040029F6 RID: 10742
		public AlertControllerButton.FullSpriteState cancelSprites;

		// Token: 0x040029F7 RID: 10743
		public AlertControllerButton.FullSpriteState destructiveSprites;

		// Token: 0x040029F8 RID: 10744
		private AlertController.ButtonStyle _style;

		// Token: 0x040029F9 RID: 10745
		private Button _button;

		// Token: 0x0200078C RID: 1932
		[Serializable]
		public class FullSpriteState
		{
			// Token: 0x040029FA RID: 10746
			public Sprite idle;

			// Token: 0x040029FB RID: 10747
			public SpriteState spriteState;
		}
	}
}
