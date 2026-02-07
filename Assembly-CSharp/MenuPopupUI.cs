using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F7 RID: 247
public class MenuPopupUI : MonoBehaviour
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600080E RID: 2062 RVA: 0x0002D608 File Offset: 0x0002B808
	private Button[] Buttons
	{
		get
		{
			if (this.buttons == null)
			{
				this.buttons = new Button[] { this.button0, this.button1, this.button2 };
			}
			return this.buttons;
		}
	}

	// Token: 0x14000036 RID: 54
	// (add) Token: 0x0600080F RID: 2063 RVA: 0x000777A8 File Offset: 0x000759A8
	// (remove) Token: 0x06000810 RID: 2064 RVA: 0x000777E0 File Offset: 0x000759E0
	public event Action OnClickButton0;

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000811 RID: 2065 RVA: 0x00077818 File Offset: 0x00075A18
	// (remove) Token: 0x06000812 RID: 2066 RVA: 0x00077850 File Offset: 0x00075A50
	public event Action OnClickButton1;

	// Token: 0x06000813 RID: 2067 RVA: 0x00077888 File Offset: 0x00075A88
	private void Awake()
	{
		if (this.button0 != null)
		{
			this.button0.onClick.AddListener(delegate
			{
				UniversalInvocator.Event_Invocator(this.OnClickButton0);
			});
		}
		if (this.button1 != null)
		{
			this.button1.onClick.AddListener(delegate
			{
				UniversalInvocator.Event_Invocator(this.OnClickButton1);
			});
		}
		if (this.button2 != null)
		{
			this.button2.onClick.AddListener(delegate
			{
				UniversalInvocator.Event_Invocator(this.OnClickButton2);
			});
		}
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00077914 File Offset: 0x00075B14
	public void Reset()
	{
		if (this.textTitle)
		{
			this.textTitle.text = string.Empty;
		}
		if (this.textContent)
		{
			this.textContent.text = string.Empty;
		}
		if (this.textDescription)
		{
			this.textDescription.text = string.Empty;
		}
		foreach (Button button in this.Buttons)
		{
			if (button)
			{
				button.GetComponentInChildren<TextMeshProUGUI>(true).text = "";
				button.gameObject.SetActive(false);
			}
		}
		this.OnClickButton0 = null;
		this.OnClickButton1 = null;
		this.OnClickButton2 = null;
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0002D63F File Offset: 0x0002B83F
	public void Initialize(string title, string content, int numberOfButtons)
	{
		this.Initialize(title, content, string.Empty, numberOfButtons);
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x000779CC File Offset: 0x00075BCC
	public void Initialize(string title, string content, string description, int numberOfButtons)
	{
		if (this.textTitle)
		{
			this.textTitle.text = title;
		}
		if (this.textContent)
		{
			this.textContent.text = content;
		}
		if (this.textDescription)
		{
			this.textDescription.text = description;
		}
		for (int i = 0; i < this.Buttons.Length; i++)
		{
			if (this.Buttons[i])
			{
				this.Buttons[i].gameObject.SetActive(i < numberOfButtons);
			}
		}
		this.scrollContent.SetLayoutVertical();
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x0002D64F File Offset: 0x0002B84F
	public void InitializeButton(int buttonIndex, string buttonText, MenuPopupUI.ButtonColor color)
	{
		if (this.Buttons[buttonIndex])
		{
			this.Buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>(true).text = buttonText;
			this.Buttons[buttonIndex].image.sprite = this.GetButtonSprite(color);
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00077A68 File Offset: 0x00075C68
	private Sprite GetButtonSprite(MenuPopupUI.ButtonColor buttonColor)
	{
		switch (buttonColor)
		{
		case MenuPopupUI.ButtonColor.Green:
			return this.greenButtonSprite;
		case MenuPopupUI.ButtonColor.Red:
			return this.redButtonSprite;
		case MenuPopupUI.ButtonColor.Brown:
			return this.brownButtonSprite;
		default:
			throw new ArgumentOutOfRangeException("There is no defined sprite for " + buttonColor.ToString());
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0002D68D File Offset: 0x0002B88D
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0002D69B File Offset: 0x0002B89B
	public void Hide()
	{
		base.gameObject.SetActive(false);
		this.Reset();
	}

	// Token: 0x040006D1 RID: 1745
	[SerializeField]
	private TextMeshProUGUI textTitle;

	// Token: 0x040006D2 RID: 1746
	[SerializeField]
	private ContentSizeFitter scrollContent;

	// Token: 0x040006D3 RID: 1747
	[SerializeField]
	private TextMeshProUGUI textContent;

	// Token: 0x040006D4 RID: 1748
	[SerializeField]
	private TextMeshProUGUI textDescription;

	// Token: 0x040006D5 RID: 1749
	[SerializeField]
	private Button button0;

	// Token: 0x040006D6 RID: 1750
	[SerializeField]
	private Button button1;

	// Token: 0x040006D7 RID: 1751
	[SerializeField]
	private Button button2;

	// Token: 0x040006D8 RID: 1752
	private Button[] buttons;

	// Token: 0x040006D9 RID: 1753
	[SerializeField]
	private Sprite greenButtonSprite;

	// Token: 0x040006DA RID: 1754
	[SerializeField]
	private Sprite redButtonSprite;

	// Token: 0x040006DB RID: 1755
	[SerializeField]
	private Sprite brownButtonSprite;

	// Token: 0x040006DE RID: 1758
	public Action OnClickButton2;

	// Token: 0x020000F8 RID: 248
	public enum ButtonColor
	{
		// Token: 0x040006E0 RID: 1760
		Green,
		// Token: 0x040006E1 RID: 1761
		Red,
		// Token: 0x040006E2 RID: 1762
		Brown
	}
}
