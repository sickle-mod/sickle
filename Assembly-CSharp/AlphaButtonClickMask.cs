using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
public class AlphaButtonClickMask : MonoBehaviour, ICanvasRaycastFilter
{
	// Token: 0x06000019 RID: 25 RVA: 0x00052A50 File Offset: 0x00050C50
	public void Start()
	{
		this._image = base.GetComponent<Image>();
		Texture2D texture = this._image.sprite.texture;
		bool flag = false;
		if (texture != null)
		{
			try
			{
				texture.GetPixels32();
				goto IL_0041;
			}
			catch (UnityException ex)
			{
				Debug.LogError(ex.Message);
				flag = true;
				goto IL_0041;
			}
		}
		flag = true;
		IL_0041:
		if (flag)
		{
			Debug.LogError("This script need an Image with a readbale Texture2D to work.");
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00052ABC File Offset: 0x00050CBC
	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this._image.rectTransform, sp, eventCamera, out vector);
		Vector2 pivot = this._image.rectTransform.pivot;
		Vector2 vector2 = new Vector2(pivot.x + vector.x / this._image.rectTransform.rect.width, pivot.y + vector.y / this._image.rectTransform.rect.height);
		Vector2 vector3 = new Vector2(this._image.sprite.rect.x + vector2.x * this._image.sprite.rect.width, this._image.sprite.rect.y + vector2.y * this._image.sprite.rect.height);
		vector3.x /= (float)this._image.sprite.texture.width;
		vector3.y /= (float)this._image.sprite.texture.height;
		return this._image.sprite.texture.GetPixelBilinear(vector3.x, vector3.y).a > 0.1f;
	}

	// Token: 0x0400000A RID: 10
	protected Image _image;
}
