using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	// Token: 0x02000704 RID: 1796
	[AddComponentMenu("UI/Toggle2", 31)]
	[RequireComponent(typeof(RectTransform))]
	public class Toggle2 : Button, IPointerClickHandler, IEventSystemHandler, ISubmitHandler, ICanvasElement
	{
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06003621 RID: 13857 RVA: 0x0004AAAE File Offset: 0x00048CAE
		// (set) Token: 0x06003622 RID: 13858 RVA: 0x0004AAB6 File Offset: 0x00048CB6
		public Toggle2Group group
		{
			get
			{
				return this.m_Group;
			}
			set
			{
				this.m_Group = value;
				this.SetToggleGroup(this.m_Group, true);
				this.PlayEffect(true);
			}
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x0004AAD3 File Offset: 0x00048CD3
		protected Toggle2()
		{
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x0004AAED File Offset: 0x00048CED
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetToggleGroup(this.m_Group, false);
			this.PlayEffect(true);
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x0004AB09 File Offset: 0x00048D09
		protected override void OnDisable()
		{
			this.SetToggleGroup(null, false);
			base.OnDisable();
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x00140AC0 File Offset: 0x0013ECC0
		protected override void OnDidApplyAnimationProperties()
		{
			if (this.graphic != null)
			{
				bool flag = !Mathf.Approximately(this.graphic.canvasRenderer.GetColor().a, 0f);
				if (this.m_IsOn != flag)
				{
					this.m_IsOn = flag;
					this.Set(!flag);
				}
			}
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x00140B20 File Offset: 0x0013ED20
		private void SetToggleGroup(Toggle2Group newGroup, bool setMemberValue)
		{
			Toggle2Group group = this.m_Group;
			if (this.m_Group != null)
			{
				this.m_Group.UnregisterToggle(this);
			}
			if (setMemberValue)
			{
				this.m_Group = newGroup;
			}
			if (newGroup != null && this.IsActive())
			{
				newGroup.RegisterToggle(this);
			}
			if (newGroup != null && newGroup != group && this.isOn && this.IsActive())
			{
				newGroup.NotifyToggleOn(this);
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600362B RID: 13867 RVA: 0x0004AB19 File Offset: 0x00048D19
		// (set) Token: 0x0600362C RID: 13868 RVA: 0x0004AB21 File Offset: 0x00048D21
		public bool isOn
		{
			get
			{
				return this.m_IsOn;
			}
			set
			{
				this.Set(value);
			}
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x0004AB2A File Offset: 0x00048D2A
		private void Set(bool value)
		{
			this.Set(value, true);
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x00140B9C File Offset: 0x0013ED9C
		private void Set(bool value, bool sendCallback)
		{
			if (this.m_IsOn == value)
			{
				return;
			}
			this.m_IsOn = value;
			if (this.m_Group != null && this.IsActive())
			{
				if (this.m_IsOn || (!this.m_Group.AnyTogglesOn() && !this.m_Group.allowSwitchOff))
				{
					this.m_IsOn = true;
					this.m_Group.NotifyToggleOn(this);
				}
				else if (!this.m_IsOn)
				{
					this.m_Group.NotifyToggleOff(this);
				}
			}
			this.PlayEffect(this.toggleTransition == Toggle2.ToggleTransition.None);
			if (sendCallback)
			{
				this.onValueChanged.Invoke(this.m_IsOn);
			}
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x00140C40 File Offset: 0x0013EE40
		private void PlayEffect(bool instant)
		{
			if (base.targetGraphic != null)
			{
				base.targetGraphic.CrossFadeAlpha(this.m_IsOn ? 0f : 1f, instant ? 0f : 0.1f, true);
			}
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.CrossFadeAlpha(this.m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
			if (this.graphic2 == null)
			{
				return;
			}
			this.graphic2.CrossFadeAlpha(this.m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x0004AB34 File Offset: 0x00048D34
		protected override void Start()
		{
			this.PlayEffect(true);
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x0004AB3D File Offset: 0x00048D3D
		public void InternalToggle()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.isOn = !this.isOn;
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x0004AB5F File Offset: 0x00048D5F
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			base.OnPointerClick(eventData);
			this.InternalToggle();
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x0004AB77 File Offset: 0x00048D77
		public override void OnSubmit(BaseEventData eventData)
		{
			this.InternalToggle();
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x0004AB7F File Offset: 0x00048D7F
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x0400279E RID: 10142
		public Toggle2.ToggleTransition toggleTransition = Toggle2.ToggleTransition.Fade;

		// Token: 0x0400279F RID: 10143
		public Graphic graphic;

		// Token: 0x040027A0 RID: 10144
		public Graphic graphic2;

		// Token: 0x040027A1 RID: 10145
		[SerializeField]
		private Toggle2Group m_Group;

		// Token: 0x040027A2 RID: 10146
		public Toggle2.ToggleEvent onValueChanged = new Toggle2.ToggleEvent();

		// Token: 0x040027A3 RID: 10147
		[FormerlySerializedAs("m_IsActive")]
		[Tooltip("Is the toggle currently on or off?")]
		[SerializeField]
		private bool m_IsOn;

		// Token: 0x02000705 RID: 1797
		public enum ToggleTransition
		{
			// Token: 0x040027A5 RID: 10149
			None,
			// Token: 0x040027A6 RID: 10150
			Fade
		}

		// Token: 0x02000706 RID: 1798
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}
	}
}
