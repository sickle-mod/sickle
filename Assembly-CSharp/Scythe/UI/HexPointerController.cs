using System;
using System.Collections.Generic;
using Scythe.BoardPresenter;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020003ED RID: 1005
	public class HexPointerController : MonoBehaviour
	{
		// Token: 0x06001E20 RID: 7712 RVA: 0x000B9848 File Offset: 0x000B7A48
		private void Awake()
		{
			if (PlatformManager.IsMobile)
			{
				Canvas componentInParent = base.GetComponentInParent<Canvas>();
				this.canvasCamera = ((componentInParent.renderMode == RenderMode.ScreenSpaceCamera) ? componentInParent.worldCamera : null);
				if (GameController.GameManager.IsCampaign)
				{
					base.gameObject.SetActive(false);
				}
			}
			this.InitializeResourceFieldPointerPool(1);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0003B843 File Offset: 0x00039A43
		private void Update()
		{
			this.UpdateResourceFieldPointers();
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0003B84B File Offset: 0x00039A4B
		public void SetHexesWithResource(List<GameHexPresenter> hexesWithResource)
		{
			this.DisableResourceFieldPointers();
			this.hexesWithResource = hexesWithResource;
			this.UpdateResourceFieldPointers();
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0003B860 File Offset: 0x00039A60
		public void Clear()
		{
			this.DisableResourceFieldPointers();
			this.hexesWithResource.Clear();
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000B989C File Offset: 0x000B7A9C
		private void InitializeResourceFieldPointerPool(int size)
		{
			if (this.resourceFieldPointerPool == null)
			{
				this.resourceFieldPointerPool = new List<GameObject>();
			}
			for (int i = 0; i < size; i++)
			{
				this.CreateResourceFieldPointer();
			}
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x000B98D0 File Offset: 0x000B7AD0
		private GameObject CreateResourceFieldPointer()
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.resourceFieldPointer, this.PointerArea.transform);
			gameObject.SetActive(false);
			this.resourceFieldPointerPool.Add(gameObject);
			return gameObject;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000B9908 File Offset: 0x000B7B08
		private GameObject GetResourceFieldPointer()
		{
			GameObject gameObject = this.resourceFieldPointerPool.Find((GameObject pointer) => !pointer.activeInHierarchy);
			if (gameObject == null)
			{
				gameObject = this.CreateResourceFieldPointer();
			}
			return gameObject;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000B9954 File Offset: 0x000B7B54
		private void DisableResourceFieldPointers()
		{
			foreach (GameObject gameObject in this.resourceFieldPointerPool)
			{
				gameObject.SetActive(false);
			}
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000B99A8 File Offset: 0x000B7BA8
		private void UpdateResourceFieldPointers()
		{
			if (this.hexesWithResource.Count > 0)
			{
				this.DisableResourceFieldPointers();
				foreach (GameHexPresenter gameHexPresenter in this.hexesWithResource)
				{
					Vector3 vector = gameHexPresenter.GetWorldPosition();
					vector.z = vector.y;
					vector.y = 0f;
					if (!this.PositionIsVisibleInPointerArea(vector))
					{
						GameObject gameObject = this.GetResourceFieldPointer();
						this.PointArrowToPosition(ref gameObject, vector);
					}
				}
			}
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000B9A44 File Offset: 0x000B7C44
		private bool PositionIsVisibleInPointerArea(Vector3 position)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(position);
			if (vector.z < 0f)
			{
				vector = -vector;
			}
			Vector2 vector2;
			if (PlatformManager.IsMobile)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.PointerArea, vector, this.canvasCamera, out vector2);
			}
			else
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.PointerArea, vector, null, out vector2);
			}
			return this.PointerArea.rect.xMin < vector2.x && vector2.x < this.PointerArea.rect.xMax && this.PointerArea.rect.yMin < vector2.y && vector2.y < this.PointerArea.rect.yMax;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000B9B18 File Offset: 0x000B7D18
		private void PointArrowToPosition(ref GameObject resourceFieldPointer, Vector3 position)
		{
			float num = 0f;
			Vector3 vector = Camera.main.WorldToScreenPoint(position);
			Vector2 vector2 = default(Vector2);
			float height = resourceFieldPointer.GetComponent<RectTransform>().rect.height;
			if (vector.z < 0f)
			{
				vector = -vector;
			}
			if (PlatformManager.IsMobile)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.PointerArea, vector, this.canvasCamera, out vector2);
			}
			else
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.PointerArea, vector, null, out vector2);
			}
			float num2 = vector2.x;
			float num3 = vector2.y;
			if (vector2.x > this.PointerArea.rect.xMax - height)
			{
				num2 = this.PointerArea.rect.xMax - height;
				num = 90f;
			}
			if (vector2.x < this.PointerArea.rect.xMin + height)
			{
				num2 = this.PointerArea.rect.xMin + height;
				num = -90f;
			}
			if (vector2.y > this.PointerArea.rect.yMax - height)
			{
				num3 = this.PointerArea.rect.yMax - height;
				if (num == 90f)
				{
					num += 45f;
				}
				else if (num == -90f)
				{
					num -= 45f;
				}
				else
				{
					num = 180f;
				}
			}
			if (vector2.y < this.PointerArea.rect.yMin + height)
			{
				num3 = this.PointerArea.rect.yMin + height;
				if (num == 90f)
				{
					num -= 45f;
				}
				else if (num == -90f)
				{
					num += 45f;
				}
				else
				{
					num = 0f;
				}
			}
			resourceFieldPointer.GetComponent<RectTransform>().localPosition = new Vector3(num2, num3, 0f);
			Vector3 vector3 = new Vector3(0f, 0f, num);
			resourceFieldPointer.GetComponent<RectTransform>().localEulerAngles = vector3;
			resourceFieldPointer.SetActive(true);
		}

		// Token: 0x04001571 RID: 5489
		public RectTransform PointerArea;

		// Token: 0x04001572 RID: 5490
		public GameObject resourceFieldPointer;

		// Token: 0x04001573 RID: 5491
		private List<GameObject> resourceFieldPointerPool = new List<GameObject>();

		// Token: 0x04001574 RID: 5492
		private List<GameHexPresenter> hexesWithResource = new List<GameHexPresenter>();

		// Token: 0x04001575 RID: 5493
		private Camera canvasCamera;
	}
}
