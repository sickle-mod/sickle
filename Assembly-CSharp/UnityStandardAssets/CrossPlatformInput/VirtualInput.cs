using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x02000701 RID: 1793
	public abstract class VirtualInput
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060035EF RID: 13807 RVA: 0x0004A757 File Offset: 0x00048957
		// (set) Token: 0x060035F0 RID: 13808 RVA: 0x0004A75F File Offset: 0x0004895F
		public Vector3 virtualMousePosition { get; private set; }

		// Token: 0x060035F1 RID: 13809 RVA: 0x0004A768 File Offset: 0x00048968
		public bool AxisExists(string name)
		{
			return this.m_VirtualAxes.ContainsKey(name);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x0004A776 File Offset: 0x00048976
		public bool ButtonExists(string name)
		{
			return this.m_VirtualButtons.ContainsKey(name);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001409F0 File Offset: 0x0013EBF0
		public void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis)
		{
			if (this.m_VirtualAxes.ContainsKey(axis.name))
			{
				Debug.LogError("There is already a virtual axis named " + axis.name + " registered.");
				return;
			}
			this.m_VirtualAxes.Add(axis.name, axis);
			if (!axis.matchWithInputManager)
			{
				this.m_AlwaysUseVirtual.Add(axis.name);
			}
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x00140A58 File Offset: 0x0013EC58
		public void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button)
		{
			if (this.m_VirtualButtons.ContainsKey(button.name))
			{
				Debug.LogError("There is already a virtual button named " + button.name + " registered.");
				return;
			}
			this.m_VirtualButtons.Add(button.name, button);
			if (!button.matchWithInputManager)
			{
				this.m_AlwaysUseVirtual.Add(button.name);
			}
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x0004A784 File Offset: 0x00048984
		public void UnRegisterVirtualAxis(string name)
		{
			if (this.m_VirtualAxes.ContainsKey(name))
			{
				this.m_VirtualAxes.Remove(name);
			}
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x0004A7A1 File Offset: 0x000489A1
		public void UnRegisterVirtualButton(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				this.m_VirtualButtons.Remove(name);
			}
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x0004A7BE File Offset: 0x000489BE
		public CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				return null;
			}
			return this.m_VirtualAxes[name];
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x0004A7DC File Offset: 0x000489DC
		public void SetVirtualMousePositionX(float f)
		{
			this.virtualMousePosition = new Vector3(f, this.virtualMousePosition.y, this.virtualMousePosition.z);
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x0004A800 File Offset: 0x00048A00
		public void SetVirtualMousePositionY(float f)
		{
			this.virtualMousePosition = new Vector3(this.virtualMousePosition.x, f, this.virtualMousePosition.z);
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x0004A824 File Offset: 0x00048A24
		public void SetVirtualMousePositionZ(float f)
		{
			this.virtualMousePosition = new Vector3(this.virtualMousePosition.x, this.virtualMousePosition.y, f);
		}

		// Token: 0x060035FB RID: 13819
		public abstract float GetAxis(string name, bool raw);

		// Token: 0x060035FC RID: 13820
		public abstract bool GetButton(string name);

		// Token: 0x060035FD RID: 13821
		public abstract bool GetButtonDown(string name);

		// Token: 0x060035FE RID: 13822
		public abstract bool GetButtonUp(string name);

		// Token: 0x060035FF RID: 13823
		public abstract void SetButtonDown(string name);

		// Token: 0x06003600 RID: 13824
		public abstract void SetButtonUp(string name);

		// Token: 0x06003601 RID: 13825
		public abstract void SetAxisPositive(string name);

		// Token: 0x06003602 RID: 13826
		public abstract void SetAxisNegative(string name);

		// Token: 0x06003603 RID: 13827
		public abstract void SetAxisZero(string name);

		// Token: 0x06003604 RID: 13828
		public abstract void SetAxis(string name, float value);

		// Token: 0x06003605 RID: 13829
		public abstract Vector3 MousePosition();

		// Token: 0x0400279B RID: 10139
		protected Dictionary<string, CrossPlatformInputManager.VirtualAxis> m_VirtualAxes = new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();

		// Token: 0x0400279C RID: 10140
		protected Dictionary<string, CrossPlatformInputManager.VirtualButton> m_VirtualButtons = new Dictionary<string, CrossPlatformInputManager.VirtualButton>();

		// Token: 0x0400279D RID: 10141
		protected List<string> m_AlwaysUseVirtual = new List<string>();
	}
}
