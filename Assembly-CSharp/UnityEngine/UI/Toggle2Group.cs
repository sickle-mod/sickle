using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x02000707 RID: 1799
	[AddComponentMenu("UI/Toggle2 Group", 32)]
	[DisallowMultipleComponent]
	public class Toggle2Group : UIBehaviour
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06003636 RID: 13878 RVA: 0x0004AB8F File Offset: 0x00048D8F
		// (set) Token: 0x06003637 RID: 13879 RVA: 0x0004AB97 File Offset: 0x00048D97
		public bool allowSwitchOff
		{
			get
			{
				return this.m_AllowSwitchOff;
			}
			set
			{
				this.m_AllowSwitchOff = value;
			}
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x0004ABA0 File Offset: 0x00048DA0
		protected Toggle2Group()
		{
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x0004ABB3 File Offset: 0x00048DB3
		private void ValidateToggleIsInGroup(Toggle2 toggle)
		{
			if (toggle == null || !this.m_Toggles.Contains(toggle))
			{
				throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[] { toggle, this }));
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x00140D08 File Offset: 0x0013EF08
		public void NotifyToggleOn(Toggle2 toggle)
		{
			this.ValidateToggleIsInGroup(toggle);
			if (this.showWhenOff != null)
			{
				this.showWhenOff.SetActive(false);
			}
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				if (!(this.m_Toggles[i] == toggle))
				{
					this.m_Toggles[i].isOn = false;
				}
			}
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x0004ABEA File Offset: 0x00048DEA
		public void NotifyToggleOff(Toggle2 toggle)
		{
			this.ValidateToggleIsInGroup(toggle);
			if (this.showWhenOff != null)
			{
				this.showWhenOff.SetActive(!this.AnyTogglesOn());
			}
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x0004AC15 File Offset: 0x00048E15
		public void UnregisterToggle(Toggle2 toggle)
		{
			if (this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Remove(toggle);
			}
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x0004AC32 File Offset: 0x00048E32
		public void RegisterToggle(Toggle2 toggle)
		{
			if (!this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Add(toggle);
			}
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x0004AC4E File Offset: 0x00048E4E
		public bool AnyTogglesOn()
		{
			return this.m_Toggles.Find((Toggle2 x) => x.isOn) != null;
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x0004AC80 File Offset: 0x00048E80
		public IEnumerable<Toggle2> ActiveToggles()
		{
			return this.m_Toggles.Where((Toggle2 x) => x.isOn);
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x00140D74 File Offset: 0x0013EF74
		public void SetAllTogglesOff()
		{
			bool allowSwitchOff = this.m_AllowSwitchOff;
			this.m_AllowSwitchOff = true;
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				this.m_Toggles[i].isOn = false;
			}
			this.m_AllowSwitchOff = allowSwitchOff;
		}

		// Token: 0x040027A7 RID: 10151
		[SerializeField]
		private bool m_AllowSwitchOff;

		// Token: 0x040027A8 RID: 10152
		public GameObject showWhenOff;

		// Token: 0x040027A9 RID: 10153
		private List<Toggle2> m_Toggles = new List<Toggle2>();
	}
}
