using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000786 RID: 1926
	public class AlertController : MonoBehaviour
	{
		// Token: 0x060037D2 RID: 14290 RVA: 0x001486A8 File Offset: 0x001468A8
		public static AlertController InstantiateAlertController(string title, string message)
		{
			AlertController alertController = (AlertController)global::UnityEngine.Object.FindObjectOfType(typeof(AlertController));
			if (alertController == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.alertControllerPrefab);
				global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
				alertController = gameObject.GetComponent<AlertController>();
				alertController._Init(title, message);
			}
			else
			{
				AsmoLogger.Error("AlertController", "Try to InstantiateAlertController twice", null);
			}
			return alertController;
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x00148710 File Offset: 0x00146910
		private void _Init(string title, string message)
		{
			if (!string.IsNullOrEmpty(title))
			{
				this._ui.titleLabel.text = title;
			}
			else
			{
				this._ui.header.gameObject.SetActive(false);
			}
			if (!string.IsNullOrEmpty(message))
			{
				this._ui.messageLabel.text = message;
			}
			else
			{
				this._ui.content.gameObject.SetActive(false);
			}
			Button[] buttons = this._ui.buttons;
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0004BCCD File Offset: 0x00049ECD
		private void Awake()
		{
			this._responsivePopUp = base.GetComponent<ResponsivePopUp>();
			if (this._responsivePopUp == null)
			{
				AsmoLogger.Error("AlertController", "Missing ResponsivePopUp behavior", null);
			}
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x001487A8 File Offset: 0x001469A8
		public void AddAction(string title, AlertController.ButtonStyle style = AlertController.ButtonStyle.Default, Action action = null)
		{
			if (this._actions.Count >= 2)
			{
				throw new ArgumentOutOfRangeException();
			}
			AlertController.ButtonAction buttonAction = default(AlertController.ButtonAction);
			buttonAction.title = title;
			buttonAction.style = style;
			buttonAction.action = action;
			this._actions.Add(buttonAction);
			Button button = this._ui.buttons[this._actions.Count];
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<TextMeshProUGUI>().text = buttonAction.title;
			button.onClick.AddListener(delegate
			{
				button.interactable = false;
				this.Dismiss();
				if (buttonAction.action != null)
				{
					buttonAction.action();
				}
			});
			AlertControllerButton component = button.GetComponent<AlertControllerButton>();
			if (component != null)
			{
				component.Style = buttonAction.style;
			}
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x0004BCF9 File Offset: 0x00049EF9
		public void Dismiss()
		{
			this._responsivePopUp.FadeOut(delegate
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			});
		}

		// Token: 0x040029E1 RID: 10721
		private const string _kModuleName = "AlertController";

		// Token: 0x040029E2 RID: 10722
		[SerializeField]
		private AlertController.UI _ui;

		// Token: 0x040029E3 RID: 10723
		private ResponsivePopUp _responsivePopUp;

		// Token: 0x040029E4 RID: 10724
		private List<AlertController.ButtonAction> _actions = new List<AlertController.ButtonAction>(2);

		// Token: 0x02000787 RID: 1927
		[Serializable]
		public class UI
		{
			// Token: 0x040029E5 RID: 10725
			public RectTransform header;

			// Token: 0x040029E6 RID: 10726
			public TextMeshProUGUI titleLabel;

			// Token: 0x040029E7 RID: 10727
			public RectTransform content;

			// Token: 0x040029E8 RID: 10728
			public TextMeshProUGUI messageLabel;

			// Token: 0x040029E9 RID: 10729
			public Button[] buttons;
		}

		// Token: 0x02000788 RID: 1928
		public enum ButtonStyle
		{
			// Token: 0x040029EB RID: 10731
			Default,
			// Token: 0x040029EC RID: 10732
			Cancel,
			// Token: 0x040029ED RID: 10733
			Destructive
		}

		// Token: 0x02000789 RID: 1929
		private struct ButtonAction
		{
			// Token: 0x040029EE RID: 10734
			public string title;

			// Token: 0x040029EF RID: 10735
			public AlertController.ButtonStyle style;

			// Token: 0x040029F0 RID: 10736
			public Action action;
		}
	}
}
