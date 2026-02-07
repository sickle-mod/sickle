using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007BB RID: 1979
	[RequireComponent(typeof(Image))]
	public class ImageModifier : MonoBehaviour
	{
		// Token: 0x060038F2 RID: 14578 RVA: 0x0004C9F1 File Offset: 0x0004ABF1
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x0004C9FF File Offset: 0x0004ABFF
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this._Display;
			this._Display();
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x0004CA22 File Offset: 0x0004AC22
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this._Display;
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0014C354 File Offset: 0x0014A554
		private void _Display()
		{
			Preferences.DisplayMode currentDisplayMode = CoreApplication.Instance.Preferences.InterfaceDisplayMode;
			if (!this.displayModeToSprite.Any((ImageModifier.DisplayModeToSprite x) => x.displayMode == currentDisplayMode))
			{
				currentDisplayMode = Preferences.DisplayMode.Unknown;
			}
			this._image.sprite = this.displayModeToSprite.Single((ImageModifier.DisplayModeToSprite x) => x.displayMode == currentDisplayMode).sprite;
		}

		// Token: 0x04002AE5 RID: 10981
		private const string _documentation = "Allow you to set a different sprite for the image which will be display according to the current display mode (small, regular, big).\nIf none is specified for a specific display mode then default will be used instead";

		// Token: 0x04002AE6 RID: 10982
		private Image _image;

		// Token: 0x04002AE7 RID: 10983
		public List<ImageModifier.DisplayModeToSprite> displayModeToSprite = new List<ImageModifier.DisplayModeToSprite>();

		// Token: 0x020007BC RID: 1980
		[Serializable]
		public class DisplayModeToSprite
		{
			// Token: 0x04002AE8 RID: 10984
			public Preferences.DisplayMode displayMode;

			// Token: 0x04002AE9 RID: 10985
			public Sprite sprite;
		}
	}
}
