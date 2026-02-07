using System;
using AsmodeeNet.Utils.Extensions;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x02000945 RID: 2373
	public class Preferences
	{
		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x00050FA0 File Offset: 0x0004F1A0
		public float Aspect
		{
			get
			{
				this.UpdateAspect();
				return this._aspect;
			}
		}

		// Token: 0x14000152 RID: 338
		// (add) Token: 0x06003FD2 RID: 16338 RVA: 0x0015CCC0 File Offset: 0x0015AEC0
		// (remove) Token: 0x06003FD3 RID: 16339 RVA: 0x0015CCF8 File Offset: 0x0015AEF8
		public event Action AspectDidChange;

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06003FD4 RID: 16340 RVA: 0x00050FAE File Offset: 0x0004F1AE
		public Preferences.Orientation InterfaceOrientation
		{
			get
			{
				this.UpdateInterfaceOrientation();
				return this._interfaceOrientation;
			}
		}

		// Token: 0x14000153 RID: 339
		// (add) Token: 0x06003FD5 RID: 16341 RVA: 0x0015CD30 File Offset: 0x0015AF30
		// (remove) Token: 0x06003FD6 RID: 16342 RVA: 0x0015CD68 File Offset: 0x0015AF68
		public event Action InterfaceOrientationDidChange;

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x0015CDA0 File Offset: 0x0015AFA0
		// (set) Token: 0x06003FD8 RID: 16344 RVA: 0x00050FBC File Offset: 0x0004F1BC
		public Preferences.DisplayMode InterfaceDisplayMode
		{
			get
			{
				if (this._displayMode == Preferences.DisplayMode.Unknown)
				{
					if (KeyValueStore.HasKey("DisplayMode"))
					{
						try
						{
							string @string = KeyValueStore.GetString("DisplayMode", "");
							this._displayMode = (Preferences.DisplayMode)Enum.Parse(typeof(Preferences.DisplayMode), @string);
						}
						catch
						{
						}
					}
					if (this._displayMode == Preferences.DisplayMode.Unknown)
					{
						switch (SystemInfo.deviceType)
						{
						case DeviceType.Handheld:
							this._displayMode = ((ScreenExtension.DiagonalLengthInch < 8f) ? Preferences.DisplayMode.Small : Preferences.DisplayMode.Regular);
							break;
						case DeviceType.Console:
							this._displayMode = Preferences.DisplayMode.Small;
							break;
						case DeviceType.Desktop:
							this._displayMode = ((ScreenExtension.DiagonalLengthInch < 16f) ? Preferences.DisplayMode.Regular : Preferences.DisplayMode.Big);
							break;
						}
					}
				}
				return this._displayMode;
			}
			set
			{
				this._displayMode = value;
				KeyValueStore.SetString("DisplayMode", this._displayMode.ToString());
				if (this.InterfaceDisplayModeDidChange != null)
				{
					this.InterfaceDisplayModeDidChange();
				}
			}
		}

		// Token: 0x14000154 RID: 340
		// (add) Token: 0x06003FD9 RID: 16345 RVA: 0x0015CE68 File Offset: 0x0015B068
		// (remove) Token: 0x06003FDA RID: 16346 RVA: 0x0015CEA0 File Offset: 0x0015B0A0
		public event Action InterfaceDisplayModeDidChange;

		// Token: 0x06003FDB RID: 16347 RVA: 0x00050FF3 File Offset: 0x0004F1F3
		public void Update()
		{
			this.UpdateAspect();
			this.UpdateInterfaceOrientation();
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x0015CED8 File Offset: 0x0015B0D8
		private void UpdateAspect()
		{
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			float aspect = main.aspect;
			if (!Mathf.Approximately(aspect, this._aspect))
			{
				this._aspect = aspect;
				if (this.AspectDidChange != null)
				{
					this.AspectDidChange();
				}
			}
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x0015CF24 File Offset: 0x0015B124
		private void UpdateInterfaceOrientation()
		{
			Preferences.Orientation orientation = ((this.Aspect < 1f) ? Preferences.Orientation.Vertical : Preferences.Orientation.Horizontal);
			if (orientation != this._interfaceOrientation)
			{
				this._interfaceOrientation = orientation;
				if (this.InterfaceOrientationDidChange != null)
				{
					this.InterfaceOrientationDidChange();
				}
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06003FDE RID: 16350 RVA: 0x00051001 File Offset: 0x0004F201
		public bool IsDesktop
		{
			get
			{
				return SystemInfo.deviceType == DeviceType.Desktop;
			}
		}

		// Token: 0x040030B8 RID: 12472
		private float _aspect;

		// Token: 0x040030BA RID: 12474
		private Preferences.Orientation _interfaceOrientation;

		// Token: 0x040030BC RID: 12476
		private const string _kDisplayModeKey = "DisplayMode";

		// Token: 0x040030BD RID: 12477
		private Preferences.DisplayMode _displayMode;

		// Token: 0x02000946 RID: 2374
		public enum Orientation
		{
			// Token: 0x040030C0 RID: 12480
			Unknown,
			// Token: 0x040030C1 RID: 12481
			Horizontal,
			// Token: 0x040030C2 RID: 12482
			Vertical
		}

		// Token: 0x02000947 RID: 2375
		public enum DisplayMode
		{
			// Token: 0x040030C4 RID: 12484
			Unknown,
			// Token: 0x040030C5 RID: 12485
			Small,
			// Token: 0x040030C6 RID: 12486
			Regular,
			// Token: 0x040030C7 RID: 12487
			Big
		}
	}
}
