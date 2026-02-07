using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004F7 RID: 1271
	public class PlaySoundToggle : MonoBehaviour
	{
		// Token: 0x060028CD RID: 10445 RVA: 0x00042735 File Offset: 0x00040935
		private void Awake()
		{
			this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.PlaySound));
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x00042753 File Offset: 0x00040953
		private void PlaySound(bool isOn)
		{
			ButtonsSFXManager.Instance.PlaySound(this._effect);
		}

		// Token: 0x04001D42 RID: 7490
		[SerializeField]
		private SoundEnum _effect;

		// Token: 0x04001D43 RID: 7491
		private Toggle _toggle;
	}
}
