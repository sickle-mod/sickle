using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000812 RID: 2066
	[CreateAssetMenu]
	public class CrossPromoSkin : ScriptableObject
	{
		// Token: 0x04002C5D RID: 11357
		[Header("Popup")]
		public Sprite PopupShadow;

		// Token: 0x04002C5E RID: 11358
		public int PopupShadowSize;

		// Token: 0x04002C5F RID: 11359
		public Sprite PopupWindow;

		// Token: 0x04002C60 RID: 11360
		public Sprite PopupHeaderBackground;

		// Token: 0x04002C61 RID: 11361
		public TextSkin PopupHeaderText;

		// Token: 0x04002C62 RID: 11362
		public Sprite PopupCloseButtonBackground;

		// Token: 0x04002C63 RID: 11363
		public Sprite PopupCloseButtonForeground;

		// Token: 0x04002C64 RID: 11364
		public Color PopupCloseButtonNormalColor;

		// Token: 0x04002C65 RID: 11365
		public Color PopupCloseButtonHighlightedColor;

		// Token: 0x04002C66 RID: 11366
		public Color PopupCloseButtonPressedColor;

		// Token: 0x04002C67 RID: 11367
		public Color PopupCloseButtonDisabledColor;

		// Token: 0x04002C68 RID: 11368
		[Header("Scroll bar")]
		public Sprite ScrollbarLine;

		// Token: 0x04002C69 RID: 11369
		public Sprite ScrollbarButton;

		// Token: 0x04002C6A RID: 11370
		public Sprite ScrollbarButtonGrip;

		// Token: 0x04002C6B RID: 11371
		public Color ScrollbarNormalColor;

		// Token: 0x04002C6C RID: 11372
		public Color ScrollbarHighlightedColor;

		// Token: 0x04002C6D RID: 11373
		public Color ScrollbarPressedColor;

		// Token: 0x04002C6E RID: 11374
		public Color ScrollbarDisabledColor;

		// Token: 0x04002C6F RID: 11375
		[Header("More games")]
		public Sprite FilterHighlighted;

		// Token: 0x04002C70 RID: 11376
		public Sprite FilterPressed;

		// Token: 0x04002C71 RID: 11377
		public Sprite FilterDisabled;

		// Token: 0x04002C72 RID: 11378
		public Color FilterHighlightedTextColor;

		// Token: 0x04002C73 RID: 11379
		public Color FilterNormalTextColor;

		// Token: 0x04002C74 RID: 11380
		[Header("More games - Tile")]
		public int TileBorderSize = 2;

		// Token: 0x04002C75 RID: 11381
		public Color TileBorderColor = Color.white;

		// Token: 0x04002C76 RID: 11382
		public Sprite TileDetailButton;

		// Token: 0x04002C77 RID: 11383
		public Color TileDetailButtonNormalColor;

		// Token: 0x04002C78 RID: 11384
		public Color TileDetailButtonHighlightedColor;

		// Token: 0x04002C79 RID: 11385
		public Color TileDetailButtonPressedColor;

		// Token: 0x04002C7A RID: 11386
		public Color TileDetailButtonDisabledColor;

		// Token: 0x04002C7B RID: 11387
		public Sprite TileLoading;

		// Token: 0x04002C7C RID: 11388
		public Color TileLoadingColor;

		// Token: 0x04002C7D RID: 11389
		public float TileLoadingSpeed;

		// Token: 0x04002C7E RID: 11390
		[Header("More games - Tile - Button")]
		public Color TileButtonColor;

		// Token: 0x04002C7F RID: 11391
		public Sprite TileButton;

		// Token: 0x04002C80 RID: 11392
		[Range(0f, 1f)]
		public float TileButtonAlpha;

		// Token: 0x04002C81 RID: 11393
		public TextSkin TileButtonText;
	}
}
