using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007DE RID: 2014
	public class UINavigationManager : MonoBehaviour
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600399C RID: 14748 RVA: 0x0014E79C File Offset: 0x0014C99C
		public FocusableLayer RootFocusableLayer
		{
			get
			{
				if (this._rootFocusableLayer == null)
				{
					if (this._focusableLayers.Count > 0)
					{
						this._rootFocusableLayer = this._focusableLayers.First<FocusableLayer>();
					}
					else
					{
						AsmoLogger.Error("UINavigationManager", "Missing RootFocusableLayer", null);
					}
				}
				return this._rootFocusableLayer;
			}
		}

		// Token: 0x14000151 RID: 337
		// (add) Token: 0x0600399D RID: 14749 RVA: 0x0014E7F0 File Offset: 0x0014C9F0
		// (remove) Token: 0x0600399E RID: 14750 RVA: 0x0014E828 File Offset: 0x0014CA28
		public event Action OnBackAction;

		// Token: 0x0600399F RID: 14751 RVA: 0x0004D026 File Offset: 0x0004B226
		private void Awake()
		{
			this.forceToGiveFocus = () => Input.GetJoystickNames().Length != 0;
			this.highlightFocusable = delegate(Focusable focusable)
			{
				focusable.StopCoroutine(this._Bounce(focusable));
				focusable.StartCoroutine(this._Bounce(focusable));
			};
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x0004D05F File Offset: 0x0004B25F
		private void Start()
		{
			if (this.navigationInput == null)
			{
				AsmoLogger.Error("UINavigationManager", "\"navigationInput\" is null. #define UINAVIGATION, provide your own version or deactivate UINavigationManager in CoreApplication", null);
			}
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x0004D079 File Offset: 0x0004B279
		private IEnumerator _Bounce(Focusable focusable)
		{
			Transform t = focusable.transform;
			Vector3 originalScale = Vector3.one;
			Vector3 bouncedScale = originalScale * 1.1f;
			t.localScale = bouncedScale;
			float currentTime = 0f;
			do
			{
				t.localScale = Vector3.Lerp(bouncedScale, originalScale, currentTime * 3.3333333f);
				currentTime += Time.deltaTime;
				yield return null;
			}
			while (currentTime <= 0.3f);
			yield break;
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x0004D088 File Offset: 0x0004B288
		public void RegisterFocusableLayer(FocusableLayer focusableLayer)
		{
			AsmoLogger.Trace("UINavigationManager", "RegisterFocusableLayer " + focusableLayer.gameObject.name, null);
			this._focusableLayers.Add(focusableLayer);
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x0004D0B6 File Offset: 0x0004B2B6
		public void UnRegisterFocusableLayer(FocusableLayer focusableLayer)
		{
			AsmoLogger.Trace("UINavigationManager", "UnRegisterFocusableLayer " + focusableLayer.gameObject.name, null);
			this._focusableLayers.Remove(focusableLayer);
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x0014E860 File Offset: 0x0014CA60
		public void MoveFocus(UINavigationManager.Direction direction)
		{
			if (this._focusables.Count == 0)
			{
				return;
			}
			Focusable focusable = null;
			if (this._currentFocusable == null)
			{
				foreach (Focusable focusable2 in this._focusables)
				{
					if (focusable2.firstFocusable)
					{
						focusable = focusable2;
						break;
					}
				}
				if (focusable == null)
				{
					focusable = this._focusables.First<Focusable>();
				}
			}
			else
			{
				if (direction == UINavigationManager.Direction.Next)
				{
					focusable = this._FindNextFocusableOf(this._currentFocusable);
				}
				else if (direction == UINavigationManager.Direction.Previous)
				{
					focusable = this._FindPreviousFocusableOf(this._currentFocusable);
				}
				if (focusable == null && !this._isEditingInputField)
				{
					focusable = this._FindClosestFocusable(this._currentFocusable, direction);
					if (focusable == null)
					{
						focusable = this._currentFocusable;
					}
				}
			}
			if (focusable != null)
			{
				if (focusable != this._currentFocusable)
				{
					this._UpdateFocus(focusable);
					return;
				}
				this.highlightFocusable(focusable);
			}
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x0014E96C File Offset: 0x0014CB6C
		private Focusable _FindNextFocusableOf(Focusable currentFocusable)
		{
			if (currentFocusable == null)
			{
				return null;
			}
			Focusable focusable = currentFocusable.next;
			while (focusable != null && focusable != currentFocusable)
			{
				if (focusable.isActiveAndEnabled && focusable.Selectable.IsInteractable())
				{
					return focusable;
				}
				focusable = focusable.next;
			}
			return focusable;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x0014E9C0 File Offset: 0x0014CBC0
		private Focusable _FindPreviousFocusableOf(Focusable currentFocusable)
		{
			if (currentFocusable == null)
			{
				return null;
			}
			Focusable focusable = currentFocusable.previous;
			while (focusable != null && focusable != currentFocusable)
			{
				if (focusable.isActiveAndEnabled && focusable.Selectable.IsInteractable())
				{
					return focusable;
				}
				focusable = focusable.previous;
			}
			return focusable;
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x0014EA14 File Offset: 0x0014CC14
		public void FindFocusableInputFieldAndEnterEditMode()
		{
			if (this._IsInputField(this._currentFocusable))
			{
				if (!this._isEditingInputField)
				{
					this._ActivateInputField();
				}
				return;
			}
			foreach (Focusable focusable in this._focusables)
			{
				if (this._IsInputField(focusable))
				{
					this._isEditingInputField = true;
					this._UpdateFocus(focusable);
					break;
				}
			}
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x0004D0E5 File Offset: 0x0004B2E5
		public void HandleInputString(string inputString)
		{
			if (this._IsInputField(this._currentFocusable) && !this._isEditingInputField)
			{
				this._ActivateInputField();
			}
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x0014EA98 File Offset: 0x0014CC98
		public void CancelCurrentContext()
		{
			if (!this.HasFocus)
			{
				for (int i = this._focusableLayers.Count - 1; i >= 0; i--)
				{
					FocusableLayer focusableLayer = this._focusableLayers[i];
					if (focusableLayer.OnBackAction != null)
					{
						focusableLayer.OnBackAction.Invoke();
						break;
					}
					if (focusableLayer.modal)
					{
						break;
					}
				}
				if (this.OnBackAction != null)
				{
					this.OnBackAction();
				}
				return;
			}
			if (this._isEditingInputField)
			{
				this._DeactivateInputField();
				return;
			}
			this.LoseFocus();
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x0014EB18 File Offset: 0x0014CD18
		public void ValidateFocusable()
		{
			if (!this.HasFocus)
			{
				this.FindFocusableInputFieldAndEnterEditMode();
				return;
			}
			if (!this._IsInputField(this._currentFocusable))
			{
				ExecuteEvents.Execute<ISubmitHandler>(this._currentFocusable.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
				return;
			}
			if (!this._isEditingInputField)
			{
				this._ActivateInputField();
				return;
			}
			this._DeactivateInputField();
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060039AB RID: 14763 RVA: 0x0004D103 File Offset: 0x0004B303
		public bool HasFocus
		{
			get
			{
				return this._currentFocusable != null;
			}
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x0004D111 File Offset: 0x0004B311
		public void LoseFocus()
		{
			AsmoLogger.Trace("UINavigationManager", "LoseFocus", null);
			this._isEditingInputField = false;
			this._UpdateFocus(null);
			EventSystem.current.SetSelectedGameObject(null);
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x0014EB78 File Offset: 0x0014CD78
		private void Update()
		{
			EventSystem current = EventSystem.current;
			if (current == null)
			{
				return;
			}
			current.sendNavigationEvents = false;
			this._focusables.Clear();
			for (int i = this._focusableLayers.Count - 1; i >= 0; i--)
			{
				FocusableLayer focusableLayer = this._focusableLayers[i];
				this._focusables.AddRange(focusableLayer.Focusables.ToList<Focusable>());
				if (focusableLayer.modal)
				{
					break;
				}
			}
			if (this.HasFocus && !this._focusables.Contains(this._currentFocusable))
			{
				this.LoseFocus();
				this.MoveFocus(UINavigationManager.Direction.Next);
			}
			Focusable focusable = ((current.currentSelectedGameObject != null) ? current.currentSelectedGameObject.GetComponent<Focusable>() : null);
			if (focusable != this._currentFocusable)
			{
				if (focusable == null)
				{
					this._UpdateFocus(this._currentFocusable);
				}
				else
				{
					if (this._IsInputField(focusable))
					{
						this._isEditingInputField = true;
					}
					this._UpdateFocus(focusable);
				}
			}
			if (this.navigationInput != null)
			{
				this.navigationInput.ProcessInput(this);
			}
			if (!this.HasFocus && this.forceToGiveFocus())
			{
				this.MoveFocus(UINavigationManager.Direction.Next);
			}
			current.SetSelectedGameObject((this._currentFocusable != null) ? this._currentFocusable.gameObject : null);
			if (this._IsInputField(this._currentFocusable))
			{
				if (!this._isEditingInputField)
				{
					this._DeactivateInputField();
					return;
				}
			}
			else
			{
				this._isEditingInputField = false;
			}
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0004D13C File Offset: 0x0004B33C
		private bool _IsInputField(Focusable focusable)
		{
			return !(focusable == null) && focusable.IsInputField;
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x0014ECE4 File Offset: 0x0014CEE4
		private void _ActivateInputField()
		{
			this._isEditingInputField = true;
			if (this._currentFocusable.InputField != null)
			{
				if (!this._currentFocusable.InputField.isFocused)
				{
					this._currentFocusable.InputField.ActivateInputField();
					return;
				}
			}
			else if (this._currentFocusable.TMP_InputField != null && !this._currentFocusable.TMP_InputField.isFocused)
			{
				this._currentFocusable.TMP_InputField.ActivateInputField();
			}
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x0014ED64 File Offset: 0x0014CF64
		private void _DeactivateInputField()
		{
			this._isEditingInputField = false;
			if (this._currentFocusable.InputField != null)
			{
				if (this._currentFocusable.InputField.isFocused)
				{
					this._currentFocusable.InputField.DeactivateInputField();
					return;
				}
			}
			else if (this._currentFocusable.TMP_InputField != null && this._currentFocusable.TMP_InputField.isFocused)
			{
				this._currentFocusable.TMP_InputField.DeactivateInputField(false);
			}
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x0004D14F File Offset: 0x0004B34F
		private void _UpdateFocus(Focusable focusable)
		{
			this._currentFocusable = focusable;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0014EDE4 File Offset: 0x0014CFE4
		private Focusable _FindClosestFocusable(Focusable origin, UINavigationManager.Direction direction)
		{
			List<UINavigationManager.FocusablePosition> list = this._ComputeFocusablePositions(origin);
			List<UINavigationManager.FocusablePosition> list2 = this._TrimAndSortPositions(list, direction);
			if (list2.Count <= 0)
			{
				return null;
			}
			return list2.First<UINavigationManager.FocusablePosition>().focusable;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x0014EE18 File Offset: 0x0014D018
		private List<UINavigationManager.FocusablePosition> _TrimAndSortPositions(List<UINavigationManager.FocusablePosition> positions, UINavigationManager.Direction direction)
		{
			List<UINavigationManager.FocusablePosition> list = null;
			if (direction == UINavigationManager.Direction.Left)
			{
				list = (from p in positions
					where p.deltaPosition.x < -0.001f
					orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.y / p.deltaPosition.x))
					select p).ToList<UINavigationManager.FocusablePosition>();
			}
			else if (direction == UINavigationManager.Direction.Up || direction == UINavigationManager.Direction.Previous)
			{
				list = (from p in positions
					where p.deltaPosition.y > 0.001f
					orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.x / p.deltaPosition.y))
					select p).ToList<UINavigationManager.FocusablePosition>();
			}
			else if (direction == UINavigationManager.Direction.Right)
			{
				list = (from p in positions
					where p.deltaPosition.x > 0.001f
					orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.y / p.deltaPosition.x))
					select p).ToList<UINavigationManager.FocusablePosition>();
			}
			else if (direction == UINavigationManager.Direction.Down || direction == UINavigationManager.Direction.Next)
			{
				list = (from p in positions
					where p.deltaPosition.y < -0.001f
					orderby p.squaredDistance * Mathf.Max(1f, Mathf.Abs(this.directionalWeight * p.deltaPosition.x / p.deltaPosition.y))
					select p).ToList<UINavigationManager.FocusablePosition>();
			}
			return list;
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x0014EF3C File Offset: 0x0014D13C
		private List<UINavigationManager.FocusablePosition> _ComputeFocusablePositions(Focusable origin)
		{
			List<UINavigationManager.FocusablePosition> list = new List<UINavigationManager.FocusablePosition>();
			Vector2 viewportPosition = origin.ViewportPosition;
			foreach (Focusable focusable in this._focusables)
			{
				if (focusable.isActiveAndEnabled && focusable.Selectable.IsInteractable() && focusable != origin)
				{
					Vector2 vector = focusable.ViewportPosition - viewportPosition;
					list.Add(new UINavigationManager.FocusablePosition(focusable, vector, vector.sqrMagnitude));
				}
			}
			return list;
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x0014EFD8 File Offset: 0x0014D1D8
		private void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this._currentFocusable == null || !this._currentFocusable.isActiveAndEnabled || !this._currentFocusable.Selectable.IsInteractable())
			{
				return;
			}
			Focusable focusable = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Left);
			Focusable focusable2 = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Right);
			Focusable focusable3 = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Up);
			Focusable focusable4 = this._FindClosestFocusable(this._currentFocusable, UINavigationManager.Direction.Down);
			Focusable focusable5 = this._FindNextFocusableOf(this._currentFocusable);
			Focusable focusable6 = this._FindPreviousFocusableOf(this._currentFocusable);
			if (this.gizmosLevel != UINavigationManager.GizmosLevel.Minimalist)
			{
				List<UINavigationManager.FocusablePosition> list = this._ComputeFocusablePositions(this._currentFocusable);
				UINavigationManager.Direction[] array2;
				if (this.gizmosLevel != UINavigationManager.GizmosLevel.HorizontalGradient)
				{
					UINavigationManager.Direction[] array = new UINavigationManager.Direction[2];
					array[0] = UINavigationManager.Direction.Up;
					array2 = array;
					array[1] = UINavigationManager.Direction.Down;
				}
				else
				{
					(array2 = new UINavigationManager.Direction[2])[1] = UINavigationManager.Direction.Right;
				}
				foreach (UINavigationManager.Direction direction in array2)
				{
					List<UINavigationManager.FocusablePosition> list2 = this._TrimAndSortPositions(list, direction);
					float num = 0f;
					float num2 = 0.67f / (float)list2.Count;
					foreach (ref UINavigationManager.FocusablePosition ptr in list2)
					{
						Gizmos.color = Color.HSVToRGB(num, 1f, 1f);
						Gizmos.DrawSphere(ptr.focusable.transform.position, 5f);
						num += num2;
					}
				}
			}
			Gizmos.color = Color.red;
			uint num3 = ((this.gizmosLevel != UINavigationManager.GizmosLevel.Minimalist) ? 3U : 2U);
			if (focusable5 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable5.transform.position, num3);
			}
			if (focusable6 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable6.transform.position, num3);
			}
			Gizmos.color = Color.blue;
			num3 = ((this.gizmosLevel != UINavigationManager.GizmosLevel.Minimalist) ? 2U : 1U);
			if (focusable != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable.transform.position, num3);
			}
			if (focusable2 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable2.transform.position, num3);
			}
			if (focusable3 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable3.transform.position, num3);
			}
			if (focusable4 != null)
			{
				UINavigationManager._GizmosDrawLine(this._currentFocusable.transform.position, focusable4.transform.position, num3);
			}
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x0014F298 File Offset: 0x0014D498
		private static void _GizmosDrawLine(Vector3 from, Vector3 to, uint width)
		{
			if (width == 1U)
			{
				Gizmos.DrawLine(from, to);
				return;
			}
			Camera current = Camera.current;
			if (current == null)
			{
				return;
			}
			Vector3 normalized = (to - from).normalized;
			Vector3 normalized2 = (current.transform.position - from).normalized;
			Vector3 vector = Vector3.Cross(normalized, normalized2);
			float num = (1f - width) * 0.5f;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)width))
			{
				Vector3 vector2 = vector * num;
				Gizmos.DrawLine(from + vector2, to + vector2);
				num += 0.5f;
				num2++;
			}
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0014F33C File Offset: 0x0014D53C
		public void BeginIgnoringInteractionEvents(string requester)
		{
			this._ignoringInteractionEventsRequestCount++;
			AsmoLogger.Trace("UINavigationManager", "Request to IGNORE interaction events", new Hashtable
			{
				{ "requester", requester },
				{ "request count", this._ignoringInteractionEventsRequestCount }
			});
			this._AllowOrIgnoreInteractionEvents();
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0014F394 File Offset: 0x0014D594
		public void EndIgnoringInteractionEvents(string requester)
		{
			this._ignoringInteractionEventsRequestCount--;
			AsmoLogger.Trace("UINavigationManager", "Request to ALLOW interaction events", new Hashtable
			{
				{ "requester", requester },
				{ "request count", this._ignoringInteractionEventsRequestCount }
			});
			this._AllowOrIgnoreInteractionEvents();
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x0014F3EC File Offset: 0x0014D5EC
		private void _AllowOrIgnoreInteractionEvents()
		{
			bool flag = this._ignoringInteractionEventsRequestCount == 0;
			EventSystem[] eventSystems = global::UnityEngine.Object.FindObjectsOfType(typeof(EventSystem)) as EventSystem[];
			int idx;
			int idx2;
			for (idx = 0; idx < eventSystems.Length; idx = idx2 + 1)
			{
				EventSystem eventSystem = eventSystems[idx];
				if (eventSystem.enabled != flag)
				{
					string msg = (flag ? "Allow Interaction Events [{0}/{1}]" : "Ignore Interaction Events [{0}/{1}]");
					AsmoLogger.Debug("UINavigationManager", () => string.Format(msg, idx + 1, eventSystems.Length), null);
					eventSystem.enabled = flag;
				}
				idx2 = idx;
			}
		}

		// Token: 0x04002B7D RID: 11133
		private const string _documentation = "<b>UINavigationManager</b> works with <b>Focusable</b> and <b>Selectable</b> to provide relevant UI Navigation on all platforms";

		// Token: 0x04002B7E RID: 11134
		private const string _kModuleName = "UINavigationManager";

		// Token: 0x04002B7F RID: 11135
		private FocusableLayer _rootFocusableLayer;

		// Token: 0x04002B80 RID: 11136
		private List<FocusableLayer> _focusableLayers = new List<FocusableLayer>();

		// Token: 0x04002B81 RID: 11137
		private Focusable _currentFocusable;

		// Token: 0x04002B82 RID: 11138
		private List<Focusable> _focusables = new List<Focusable>();

		// Token: 0x04002B83 RID: 11139
		private bool _isEditingInputField;

		// Token: 0x04002B85 RID: 11141
		public UINavigationManager.ForceToGiveFocus forceToGiveFocus;

		// Token: 0x04002B86 RID: 11142
		public UINavigationManager.HighlightFocusable highlightFocusable;

		// Token: 0x04002B87 RID: 11143
		public UINavigationInput navigationInput;

		// Token: 0x04002B88 RID: 11144
		[Range(0f, 10f)]
		public float directionalWeight = 2f;

		// Token: 0x04002B89 RID: 11145
		public UINavigationManager.GizmosLevel gizmosLevel;

		// Token: 0x04002B8A RID: 11146
		private int _ignoringInteractionEventsRequestCount;

		// Token: 0x020007DF RID: 2015
		// (Invoke) Token: 0x060039C1 RID: 14785
		public delegate bool ForceToGiveFocus();

		// Token: 0x020007E0 RID: 2016
		// (Invoke) Token: 0x060039C5 RID: 14789
		public delegate void HighlightFocusable(Focusable focusable);

		// Token: 0x020007E1 RID: 2017
		public enum Direction
		{
			// Token: 0x04002B8C RID: 11148
			Left,
			// Token: 0x04002B8D RID: 11149
			Up,
			// Token: 0x04002B8E RID: 11150
			Right,
			// Token: 0x04002B8F RID: 11151
			Down,
			// Token: 0x04002B90 RID: 11152
			Next,
			// Token: 0x04002B91 RID: 11153
			Previous
		}

		// Token: 0x020007E2 RID: 2018
		private struct FocusablePosition
		{
			// Token: 0x060039C8 RID: 14792 RVA: 0x0004D20A File Offset: 0x0004B40A
			public FocusablePosition(Focusable focusable, Vector2 deltaPosition, float squaredDistance)
			{
				this.focusable = focusable;
				this.deltaPosition = deltaPosition;
				this.squaredDistance = squaredDistance;
			}

			// Token: 0x04002B92 RID: 11154
			public Focusable focusable;

			// Token: 0x04002B93 RID: 11155
			public Vector2 deltaPosition;

			// Token: 0x04002B94 RID: 11156
			public float squaredDistance;
		}

		// Token: 0x020007E3 RID: 2019
		public enum GizmosLevel
		{
			// Token: 0x04002B96 RID: 11158
			Minimalist,
			// Token: 0x04002B97 RID: 11159
			HorizontalGradient,
			// Token: 0x04002B98 RID: 11160
			VerticalGradient
		}
	}
}
