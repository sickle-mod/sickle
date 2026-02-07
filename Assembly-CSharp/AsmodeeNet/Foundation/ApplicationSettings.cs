using System;
using System.Collections.Generic;
using UnityEngine;

namespace AsmodeeNet.Foundation
{
	// Token: 0x0200094D RID: 2381
	[CreateAssetMenu]
	public class ApplicationSettings : ScriptableObject
	{
		// Token: 0x06004008 RID: 16392 RVA: 0x000512E5 File Offset: 0x0004F4E5
		private void OnEnable()
		{
			this._baseSettings = new List<IBaseSetting>();
			this._PopulateCoreSettingsList();
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0015D628 File Offset: 0x0015B828
		private void _PopulateCoreSettingsList()
		{
			this._music = this._music ?? new FloatSetting("Core.Music");
			this._baseSettings.Add(this._music);
			this._musicState = this._musicState ?? new BoolSetting("Core.MusicState");
			this._baseSettings.Add(this._musicState);
			this._sfx = this._sfx ?? new FloatSetting("Core.Sfx");
			this._baseSettings.Add(this._sfx);
			this._sfxState = this._sfxState ?? new BoolSetting("Core.SfxState");
			this._baseSettings.Add(this._sfxState);
			this._animationLevel = this._animationLevel ?? new IntSetting("Core.AnimationLevel");
			this._baseSettings.Add(this._animationLevel);
			this._fullScreen = this._fullScreen ?? new BoolSetting("Core.FullScreen");
			this._baseSettings.Add(this._fullScreen);
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0015D738 File Offset: 0x0015B938
		public void Clear()
		{
			foreach (IBaseSetting baseSetting in this._baseSettings)
			{
				baseSetting.Clear();
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600400B RID: 16395 RVA: 0x000512F8 File Offset: 0x0004F4F8
		public FloatSetting Music
		{
			get
			{
				return this._music;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x00051300 File Offset: 0x0004F500
		public BoolSetting MusicState
		{
			get
			{
				return this._musicState;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600400D RID: 16397 RVA: 0x00051308 File Offset: 0x0004F508
		public FloatSetting Sfx
		{
			get
			{
				return this._sfx;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600400E RID: 16398 RVA: 0x00051310 File Offset: 0x0004F510
		public BoolSetting SfxState
		{
			get
			{
				return this._sfxState;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600400F RID: 16399 RVA: 0x00051318 File Offset: 0x0004F518
		public IntSetting AnimationLevel
		{
			get
			{
				return this._animationLevel;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06004010 RID: 16400 RVA: 0x00051320 File Offset: 0x0004F520
		public BoolSetting FullScreen
		{
			get
			{
				return this._fullScreen;
			}
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0015D788 File Offset: 0x0015B988
		public override string ToString()
		{
			string text = "";
			foreach (IBaseSetting baseSetting in this._baseSettings)
			{
				text += string.Format("[{0}] ", baseSetting);
			}
			return text;
		}

		// Token: 0x040030E5 RID: 12517
		protected List<IBaseSetting> _baseSettings;

		// Token: 0x040030E6 RID: 12518
		[Header("Core Settings")]
		[SerializeField]
		private FloatSetting _music;

		// Token: 0x040030E7 RID: 12519
		[SerializeField]
		private BoolSetting _musicState;

		// Token: 0x040030E8 RID: 12520
		[SerializeField]
		private FloatSetting _sfx;

		// Token: 0x040030E9 RID: 12521
		[SerializeField]
		private BoolSetting _sfxState;

		// Token: 0x040030EA RID: 12522
		[SerializeField]
		private IntSetting _animationLevel;

		// Token: 0x040030EB RID: 12523
		[SerializeField]
		private BoolSetting _fullScreen;
	}
}
