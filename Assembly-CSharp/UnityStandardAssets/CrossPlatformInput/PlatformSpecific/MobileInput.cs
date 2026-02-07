using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
	// Token: 0x02000702 RID: 1794
	public class MobileInput : VirtualInput
	{
		// Token: 0x06003607 RID: 13831 RVA: 0x0004A871 File Offset: 0x00048A71
		private void AddButton(string name)
		{
			CrossPlatformInputManager.RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton(name));
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x0004A87E File Offset: 0x00048A7E
		private void AddAxes(string name)
		{
			CrossPlatformInputManager.RegisterVirtualAxis(new CrossPlatformInputManager.VirtualAxis(name));
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x0004A88B File Offset: 0x00048A8B
		public override float GetAxis(string name, bool raw)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			return this.m_VirtualAxes[name].GetValue;
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x0004A8B3 File Offset: 0x00048AB3
		public override void SetButtonDown(string name)
		{
			if (!this.m_VirtualButtons.ContainsKey(name))
			{
				this.AddButton(name);
			}
			this.m_VirtualButtons[name].Pressed();
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x0004A8DB File Offset: 0x00048ADB
		public override void SetButtonUp(string name)
		{
			if (!this.m_VirtualButtons.ContainsKey(name))
			{
				this.AddButton(name);
			}
			this.m_VirtualButtons[name].Released();
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x0004A903 File Offset: 0x00048B03
		public override void SetAxisPositive(string name)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(1f);
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x0004A930 File Offset: 0x00048B30
		public override void SetAxisNegative(string name)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(-1f);
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x0004A95D File Offset: 0x00048B5D
		public override void SetAxisZero(string name)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(0f);
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x0004A98A File Offset: 0x00048B8A
		public override void SetAxis(string name, float value)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(value);
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x0004A9B3 File Offset: 0x00048BB3
		public override bool GetButtonDown(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				return this.m_VirtualButtons[name].GetButtonDown;
			}
			this.AddButton(name);
			return this.m_VirtualButtons[name].GetButtonDown;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x0004A9ED File Offset: 0x00048BED
		public override bool GetButtonUp(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				return this.m_VirtualButtons[name].GetButtonUp;
			}
			this.AddButton(name);
			return this.m_VirtualButtons[name].GetButtonUp;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x0004AA27 File Offset: 0x00048C27
		public override bool GetButton(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				return this.m_VirtualButtons[name].GetButton;
			}
			this.AddButton(name);
			return this.m_VirtualButtons[name].GetButton;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x0004AA61 File Offset: 0x00048C61
		public override Vector3 MousePosition()
		{
			return base.virtualMousePosition;
		}
	}
}
