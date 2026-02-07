using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004F6 RID: 1270
	public class PlaySoundButton : MonoBehaviour
	{
		// Token: 0x060028CA RID: 10442 RVA: 0x000426F9 File Offset: 0x000408F9
		public void Awake()
		{
			this._button = base.GetComponent<Button>();
			this._button.onClick.AddListener(new UnityAction(this.PlaySound));
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x00042723 File Offset: 0x00040923
		private void PlaySound()
		{
			ButtonsSFXManager.Instance.PlaySound(this._effect);
		}

		// Token: 0x04001D40 RID: 7488
		[SerializeField]
		private SoundEnum _effect;

		// Token: 0x04001D41 RID: 7489
		private Button _button;
	}
}
