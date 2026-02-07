using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006F2 RID: 1778
	public static class CrossPlatformInputManager
	{
		// Token: 0x060035A2 RID: 13730 RVA: 0x0004A2A4 File Offset: 0x000484A4
		public static void SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod activeInputMethod)
		{
			if (activeInputMethod == CrossPlatformInputManager.ActiveInputMethod.Hardware)
			{
				CrossPlatformInputManager.activeInput = CrossPlatformInputManager.s_HardwareInput;
				return;
			}
			if (activeInputMethod != CrossPlatformInputManager.ActiveInputMethod.Touch)
			{
				return;
			}
			CrossPlatformInputManager.activeInput = CrossPlatformInputManager.s_TouchInput;
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x0004A2C3 File Offset: 0x000484C3
		public static bool AxisExists(string name)
		{
			return CrossPlatformInputManager.activeInput.AxisExists(name);
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x0004A2D0 File Offset: 0x000484D0
		public static bool ButtonExists(string name)
		{
			return CrossPlatformInputManager.activeInput.ButtonExists(name);
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x0004A2DD File Offset: 0x000484DD
		public static void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis)
		{
			CrossPlatformInputManager.activeInput.RegisterVirtualAxis(axis);
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x0004A2EA File Offset: 0x000484EA
		public static void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button)
		{
			CrossPlatformInputManager.activeInput.RegisterVirtualButton(button);
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x0004A2F7 File Offset: 0x000484F7
		public static void UnRegisterVirtualAxis(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			CrossPlatformInputManager.activeInput.UnRegisterVirtualAxis(name);
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x0004A312 File Offset: 0x00048512
		public static void UnRegisterVirtualButton(string name)
		{
			CrossPlatformInputManager.activeInput.UnRegisterVirtualButton(name);
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x0004A31F File Offset: 0x0004851F
		public static CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name)
		{
			return CrossPlatformInputManager.activeInput.VirtualAxisReference(name);
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x0004A32C File Offset: 0x0004852C
		public static float GetAxis(string name)
		{
			return CrossPlatformInputManager.GetAxis(name, false);
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x0004A335 File Offset: 0x00048535
		public static float GetAxisRaw(string name)
		{
			return CrossPlatformInputManager.GetAxis(name, true);
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x0004A33E File Offset: 0x0004853E
		private static float GetAxis(string name, bool raw)
		{
			return CrossPlatformInputManager.activeInput.GetAxis(name, raw);
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x0004A34C File Offset: 0x0004854C
		public static bool GetButton(string name)
		{
			return CrossPlatformInputManager.activeInput.GetButton(name);
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x0004A359 File Offset: 0x00048559
		public static bool GetButtonDown(string name)
		{
			return CrossPlatformInputManager.activeInput.GetButtonDown(name);
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x0004A366 File Offset: 0x00048566
		public static bool GetButtonUp(string name)
		{
			return CrossPlatformInputManager.activeInput.GetButtonUp(name);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x0004A373 File Offset: 0x00048573
		public static void SetButtonDown(string name)
		{
			CrossPlatformInputManager.activeInput.SetButtonDown(name);
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0004A380 File Offset: 0x00048580
		public static void SetButtonUp(string name)
		{
			CrossPlatformInputManager.activeInput.SetButtonUp(name);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0004A38D File Offset: 0x0004858D
		public static void SetAxisPositive(string name)
		{
			CrossPlatformInputManager.activeInput.SetAxisPositive(name);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x0004A39A File Offset: 0x0004859A
		public static void SetAxisNegative(string name)
		{
			CrossPlatformInputManager.activeInput.SetAxisNegative(name);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x0004A3A7 File Offset: 0x000485A7
		public static void SetAxisZero(string name)
		{
			CrossPlatformInputManager.activeInput.SetAxisZero(name);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x0004A3B4 File Offset: 0x000485B4
		public static void SetAxis(string name, float value)
		{
			CrossPlatformInputManager.activeInput.SetAxis(name, value);
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060035B6 RID: 13750 RVA: 0x0004A3C2 File Offset: 0x000485C2
		public static Vector3 mousePosition
		{
			get
			{
				return CrossPlatformInputManager.activeInput.MousePosition();
			}
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x0004A3CE File Offset: 0x000485CE
		public static void SetVirtualMousePositionX(float f)
		{
			CrossPlatformInputManager.activeInput.SetVirtualMousePositionX(f);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x0004A3DB File Offset: 0x000485DB
		public static void SetVirtualMousePositionY(float f)
		{
			CrossPlatformInputManager.activeInput.SetVirtualMousePositionY(f);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x0004A3E8 File Offset: 0x000485E8
		public static void SetVirtualMousePositionZ(float f)
		{
			CrossPlatformInputManager.activeInput.SetVirtualMousePositionZ(f);
		}

		// Token: 0x04002755 RID: 10069
		private static VirtualInput activeInput = CrossPlatformInputManager.s_HardwareInput;

		// Token: 0x04002756 RID: 10070
		private static VirtualInput s_TouchInput = new MobileInput();

		// Token: 0x04002757 RID: 10071
		private static VirtualInput s_HardwareInput = new StandaloneInput();

		// Token: 0x020006F3 RID: 1779
		public enum ActiveInputMethod
		{
			// Token: 0x04002759 RID: 10073
			Hardware,
			// Token: 0x0400275A RID: 10074
			Touch
		}

		// Token: 0x020006F4 RID: 1780
		public class VirtualAxis
		{
			// Token: 0x170003F1 RID: 1009
			// (get) Token: 0x060035BA RID: 13754 RVA: 0x0004A3F5 File Offset: 0x000485F5
			// (set) Token: 0x060035BB RID: 13755 RVA: 0x0004A3FD File Offset: 0x000485FD
			public string name { get; private set; }

			// Token: 0x170003F2 RID: 1010
			// (get) Token: 0x060035BC RID: 13756 RVA: 0x0004A406 File Offset: 0x00048606
			// (set) Token: 0x060035BD RID: 13757 RVA: 0x0004A40E File Offset: 0x0004860E
			public bool matchWithInputManager { get; private set; }

			// Token: 0x060035BE RID: 13758 RVA: 0x0004A417 File Offset: 0x00048617
			public VirtualAxis(string name)
				: this(name, true)
			{
			}

			// Token: 0x060035BF RID: 13759 RVA: 0x0004A421 File Offset: 0x00048621
			public VirtualAxis(string name, bool matchToInputSettings)
			{
				this.name = name;
				this.matchWithInputManager = matchToInputSettings;
			}

			// Token: 0x060035C0 RID: 13760 RVA: 0x0004A437 File Offset: 0x00048637
			public void Remove()
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.name);
			}

			// Token: 0x060035C1 RID: 13761 RVA: 0x0004A444 File Offset: 0x00048644
			public void Update(float value)
			{
				this.m_Value = value;
			}

			// Token: 0x170003F3 RID: 1011
			// (get) Token: 0x060035C2 RID: 13762 RVA: 0x0004A44D File Offset: 0x0004864D
			public float GetValue
			{
				get
				{
					return this.m_Value;
				}
			}

			// Token: 0x170003F4 RID: 1012
			// (get) Token: 0x060035C3 RID: 13763 RVA: 0x0004A44D File Offset: 0x0004864D
			public float GetValueRaw
			{
				get
				{
					return this.m_Value;
				}
			}

			// Token: 0x0400275C RID: 10076
			private float m_Value;
		}

		// Token: 0x020006F5 RID: 1781
		public class VirtualButton
		{
			// Token: 0x170003F5 RID: 1013
			// (get) Token: 0x060035C4 RID: 13764 RVA: 0x0004A455 File Offset: 0x00048655
			// (set) Token: 0x060035C5 RID: 13765 RVA: 0x0004A45D File Offset: 0x0004865D
			public string name { get; private set; }

			// Token: 0x170003F6 RID: 1014
			// (get) Token: 0x060035C6 RID: 13766 RVA: 0x0004A466 File Offset: 0x00048666
			// (set) Token: 0x060035C7 RID: 13767 RVA: 0x0004A46E File Offset: 0x0004866E
			public bool matchWithInputManager { get; private set; }

			// Token: 0x060035C8 RID: 13768 RVA: 0x0004A477 File Offset: 0x00048677
			public VirtualButton(string name)
				: this(name, true)
			{
			}

			// Token: 0x060035C9 RID: 13769 RVA: 0x0004A481 File Offset: 0x00048681
			public VirtualButton(string name, bool matchToInputSettings)
			{
				this.name = name;
				this.matchWithInputManager = matchToInputSettings;
			}

			// Token: 0x060035CA RID: 13770 RVA: 0x0004A4A7 File Offset: 0x000486A7
			public void Pressed()
			{
				if (this.m_Pressed)
				{
					return;
				}
				this.m_Pressed = true;
				this.m_LastPressedFrame = Time.frameCount;
			}

			// Token: 0x060035CB RID: 13771 RVA: 0x0004A4C4 File Offset: 0x000486C4
			public void Released()
			{
				this.m_Pressed = false;
				this.m_ReleasedFrame = Time.frameCount;
			}

			// Token: 0x060035CC RID: 13772 RVA: 0x0004A4D8 File Offset: 0x000486D8
			public void Remove()
			{
				CrossPlatformInputManager.UnRegisterVirtualButton(this.name);
			}

			// Token: 0x170003F7 RID: 1015
			// (get) Token: 0x060035CD RID: 13773 RVA: 0x0004A4E5 File Offset: 0x000486E5
			public bool GetButton
			{
				get
				{
					return this.m_Pressed;
				}
			}

			// Token: 0x170003F8 RID: 1016
			// (get) Token: 0x060035CE RID: 13774 RVA: 0x0004A4ED File Offset: 0x000486ED
			public bool GetButtonDown
			{
				get
				{
					return this.m_LastPressedFrame - Time.frameCount == -1;
				}
			}

			// Token: 0x170003F9 RID: 1017
			// (get) Token: 0x060035CF RID: 13775 RVA: 0x0004A4FE File Offset: 0x000486FE
			public bool GetButtonUp
			{
				get
				{
					return this.m_ReleasedFrame == Time.frameCount - 1;
				}
			}

			// Token: 0x04002760 RID: 10080
			private int m_LastPressedFrame = -5;

			// Token: 0x04002761 RID: 10081
			private int m_ReleasedFrame = -5;

			// Token: 0x04002762 RID: 10082
			private bool m_Pressed;
		}
	}
}
