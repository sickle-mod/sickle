using System;
using System.Collections.Generic;
using cakeslice;
using DG.Tweening;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000419 RID: 1049
	public class ResourcePresenter : MonoBehaviour, ITooltipInfo, IDraggableObject, ISeismograph
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x0003C885 File Offset: 0x0003AA85
		public global::cakeslice.Outline ResourceOutlineStandalone
		{
			get
			{
				return base.GetComponentInChildren<global::cakeslice.Outline>(false);
			}
		}

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06002006 RID: 8198 RVA: 0x000C362C File Offset: 0x000C182C
		// (remove) Token: 0x06002007 RID: 8199 RVA: 0x000C3660 File Offset: 0x000C1860
		public static event ResourcePresenter.OnResourceDown ResourceDown;

		// Token: 0x06002008 RID: 8200 RVA: 0x0003C88E File Offset: 0x0003AA8E
		private void Awake()
		{
			base.GetComponentInChildren<global::cakeslice.Outline>(false).enabled = false;
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0003C89D File Offset: 0x0003AA9D
		private void OnEnable()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0003C8AA File Offset: 0x0003AAAA
		private void OnMouseDown()
		{
			if (ResourcePresenter.ResourceDown != null && GameController.Instance.cameraControler.MouseHitTestUI())
			{
				ResourcePresenter.ResourceDown(this);
			}
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0003C8CF File Offset: 0x0003AACF
		public void SetFocus(bool hasFocus)
		{
			this.highlight.enabled = hasFocus;
			this.ResourceOutlineActivation(hasFocus, 0);
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x000C3694 File Offset: 0x000C1894
		public void ResourceOutlineActivation(bool on, int color)
		{
			if (PlatformManager.IsMobile)
			{
				global::cakeslice.Outline componentInChildren = base.GetComponentInChildren<global::cakeslice.Outline>(false);
				if (componentInChildren != null)
				{
					componentInChildren.enabled = on;
					componentInChildren.color = color;
					return;
				}
			}
			else
			{
				foreach (Renderer renderer in this.resourceRenderer)
				{
					if (this.materialPropertyBlock != null)
					{
						renderer.GetPropertyBlock(this.materialPropertyBlock);
						if (on)
						{
							this.materialPropertyBlock.SetFloat("_FresnelIntensivity", 11f);
							this.materialPropertyBlock.SetColor("_FresnelColor", this.fresnelColors[color]);
							if (this.resourceType == ResourceType.oil)
							{
								this.materialPropertyBlock.SetFloat("_NormalSource", 1f);
							}
						}
						else
						{
							this.materialPropertyBlock.SetFloat("_FresnelIntensivity", 0f);
						}
						renderer.SetPropertyBlock(this.materialPropertyBlock);
					}
				}
			}
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x0003C8E5 File Offset: 0x0003AAE5
		public int GetTemporaryResourceValue()
		{
			return this.temporaryResourceValue;
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x000C379C File Offset: 0x000C199C
		public string InfoBasic()
		{
			return "Resource" + this.resourceType.ToString().Substring(0, 1).ToUpper() + this.resourceType.ToString().Substring(1) + "Basic";
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x000C37EC File Offset: 0x000C19EC
		public string InfoAdv()
		{
			return "Resource" + this.resourceType.ToString().Substring(0, 1).ToUpper() + this.resourceType.ToString().Substring(1) + "Adv";
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x000C2B84 File Offset: 0x000C0D84
		public void SetColliderEnabled(bool enabled)
		{
			Collider componentInChildren = base.GetComponentInChildren<Collider>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = enabled;
				return;
			}
			Debug.LogWarning("Collider not attached");
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000C383C File Offset: 0x000C1A3C
		private void PlayPickUpSound()
		{
			switch (this.resourceType)
			{
			case ResourceType.oil:
				WorldSFXManager.PlaySound(SoundEnum.UseOil, AudioSourceType.WorldSfx);
				return;
			case ResourceType.metal:
				WorldSFXManager.PlaySound(SoundEnum.UseIron, AudioSourceType.WorldSfx);
				return;
			case ResourceType.food:
				WorldSFXManager.PlaySound(SoundEnum.UseGrain, AudioSourceType.WorldSfx);
				return;
			case ResourceType.wood:
				WorldSFXManager.PlaySound(SoundEnum.UseWood, AudioSourceType.WorldSfx);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000C3898 File Offset: 0x000C1A98
		private void PlayPutDownSound()
		{
			switch (this.resourceType)
			{
			case ResourceType.oil:
				WorldSFXManager.PlaySound(SoundEnum.ProduceOil, AudioSourceType.WorldSfx);
				return;
			case ResourceType.metal:
				WorldSFXManager.PlaySound(SoundEnum.ProduceIron, AudioSourceType.WorldSfx);
				return;
			case ResourceType.food:
				WorldSFXManager.PlaySound(SoundEnum.ProduceGrain, AudioSourceType.WorldSfx);
				return;
			case ResourceType.wood:
				WorldSFXManager.PlaySound(SoundEnum.ProduceWood, AudioSourceType.WorldSfx);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x0003C8ED File Offset: 0x0003AAED
		public void SetTemporaryResource(GameObject tempResource)
		{
			this.tempResource = tempResource;
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0003C8F6 File Offset: 0x0003AAF6
		public void OnDragBegin(Vector3 pivotPosition, float rodLength, float timeToSnap)
		{
			this.PlayPickUpSound();
			this.PickUpAnimation(pivotPosition, rodLength, timeToSnap);
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0003C907 File Offset: 0x0003AB07
		public void OnDragEnd(Vector3 position, float timeToLand, bool loadingToUnit)
		{
			if (this.tempResource == null)
			{
				Debug.LogWarning("Temp resource not found!");
				return;
			}
			this.PutDownAnimation(position, timeToLand, loadingToUnit);
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0003C92B File Offset: 0x0003AB2B
		public void PickUpAnimation(Vector3 pivotPosition, float rodLength, float timeToSnap)
		{
			this.tempResource.transform.position = base.transform.position;
			this.temporaryResourceValue--;
			this.UpdateTempResource(this.temporaryResourceValue);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000C38E8 File Offset: 0x000C1AE8
		public void PutDownAnimation(Vector3 position, float timeToLand, bool loadingToUnit)
		{
			Sequence sequence = DOTween.Sequence();
			GameObject res = this.tempResource;
			this.heightOnDrop = res.transform.position.y;
			sequence.Append(this.tempResource.transform.DOMove(position, timeToLand, false).SetEase(Ease.InExpo).OnComplete(delegate
			{
				this.OnPutDownFinished(ref res, loadingToUnit);
			}));
			sequence.Join(this.tempResource.transform.DORotate(Vector3.zero, timeToLand, RotateMode.Fast).SetEase(Ease.InExpo));
			sequence.Play<Sequence>();
			this.tempResource = null;
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x0003C962 File Offset: 0x0003AB62
		private void OnPutDownFinished(ref GameObject resource, bool loadingToUnit)
		{
			resource.SetActive(false);
			this.PlayPutDownSound();
			if (!loadingToUnit)
			{
				this.temporaryResourceValue++;
				this.UpdateTempResource(this.temporaryResourceValue);
				this.CreateQuakeOnDrop();
			}
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x000C399C File Offset: 0x000C1B9C
		public bool SnapsTo(GameObject otherObject)
		{
			if (otherObject == null)
			{
				return false;
			}
			UnitPresenter component = otherObject.GetComponent<UnitPresenter>();
			return otherObject.layer == LayerMask.NameToLayer("Units") && component != null && component.UnitLogic.MovesLeft != 0 && component.GetPosition() == this.GetPosition();
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0003C995 File Offset: 0x0003AB95
		public GameHexPresenter GetPosition()
		{
			return this.hex;
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0003C99D File Offset: 0x0003AB9D
		public Vector3 GetDefaultPosition()
		{
			return this.hex.resources[(int)this.resourceType].resourceObject.transform.position;
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x000C39F4 File Offset: 0x000C1BF4
		private void CreateQuakeOnDrop()
		{
			Vector3 position = base.transform.position;
			position.y = 0f;
			int mask = LayerMask.GetMask(new string[] { "Units", "Resources", "Structures" });
			foreach (Collider collider in Physics.OverlapSphere(position, this.quakeRadius, mask))
			{
				if (!(collider.name == base.name) && !(collider.name == "DropShadow"))
				{
					ISeismograph seismograph;
					if (collider.gameObject.layer == LayerMask.NameToLayer("Resources"))
					{
						seismograph = collider.transform.parent.parent.GetComponent<ISeismograph>();
					}
					else if (collider.gameObject.layer == LayerMask.NameToLayer("Structures"))
					{
						seismograph = collider.transform.parent.parent.parent.GetComponent<ISeismograph>();
					}
					else
					{
						seismograph = collider.GetComponent<ISeismograph>();
					}
					if (seismograph != null)
					{
						seismograph.OnQuakeDetected(position, this.mass * 9.81f * this.heightOnDrop, this.quakeRadius);
					}
				}
			}
			this.heightOnDrop = 0f;
			GameController.Instance.cameraControler.ShakeCamera(this.mass);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x000C3B48 File Offset: 0x000C1D48
		public void OnQuakeDetected(Vector3 epicenter, float force, float radius)
		{
			Vector3 position = base.transform.position;
			position.y = 0f;
			float magnitude = (epicenter - position).magnitude;
			float num = (radius - magnitude) / radius;
			if (num < 0f)
			{
				return;
			}
			float num2 = 0.5f * num;
			float num3 = force * num / (this.mass * this.forceAmortization);
			this.SetColliderEnabled(false);
			base.transform.DOJump(base.transform.position, num3, 1, num2, false).OnComplete(delegate
			{
				this.OnJumpComplete();
			});
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0003C9C0 File Offset: 0x0003ABC0
		private void OnJumpComplete()
		{
			this.SetColliderEnabled(true);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000C3BDC File Offset: 0x000C1DDC
		private GameObject PrepareResourceObject()
		{
			GameObject gameObject = null;
			switch (this.resourceType)
			{
			case ResourceType.oil:
				gameObject = ResourcesObjectPoolScript.Instance.GetPooledObject(2);
				break;
			case ResourceType.metal:
				gameObject = ResourcesObjectPoolScript.Instance.GetPooledObject(0);
				break;
			case ResourceType.food:
				gameObject = ResourcesObjectPoolScript.Instance.GetPooledObject(3);
				break;
			case ResourceType.wood:
				gameObject = ResourcesObjectPoolScript.Instance.GetPooledObject(1);
				break;
			}
			return gameObject;
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0003C99D File Offset: 0x0003AB9D
		private Vector3 GetFieldPosition()
		{
			return this.hex.resources[(int)this.resourceType].resourceObject.transform.position;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000C3C44 File Offset: 0x000C1E44
		public void MoveResource(int amount, bool toField, Vector3 secondPosition, bool lastAnimation)
		{
			Vector3 vector = ((!toField) ? secondPosition : this.GetFieldPosition());
			Vector3 vector2 = (toField ? secondPosition : this.GetFieldPosition());
			this.MoveResource(this.PrepareResourceObject(), vector2, vector, amount, toField, lastAnimation);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x000C3C80 File Offset: 0x000C1E80
		private void MoveResource(GameObject resourceModel, Vector3 moveFromPosition, Vector3 moveToPosition, int amount, bool fromUnit = false, bool lastAnimation = false)
		{
			this.resourcesOnMove.Add(resourceModel);
			this.SetStartingParameters(resourceModel, moveFromPosition, !fromUnit);
			if (!fromUnit)
			{
				this.temporaryResourceValue -= amount;
				this.UpdateTempResource(this.temporaryResourceValue);
			}
			this.RunMoveAnimation(resourceModel, moveFromPosition, moveToPosition, fromUnit, lastAnimation);
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000C3CD4 File Offset: 0x000C1ED4
		private void SetStartingParameters(GameObject resourceModel, Vector3 startPosition, bool toField)
		{
			resourceModel.transform.position = startPosition;
			if (!toField)
			{
				resourceModel.transform.localScale = new Vector3(0f, 0f, 0f);
			}
			else
			{
				resourceModel.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			resourceModel.SetActive(true);
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x000C3D38 File Offset: 0x000C1F38
		private void RunMoveAnimation(GameObject resourceModel, Vector3 moveFromPosition, Vector3 moveToPosition, bool fromUnit = false, bool lastAnimation = false)
		{
			float resourcesMoveSpeed = GameController.Instance.resourcesMoveSpeed;
			float num = resourceModel.transform.position.y + GameController.Instance.resourcesJumpHeight;
			resourceModel.transform.DOScale(fromUnit ? 1f : 0f, resourcesMoveSpeed).SetEase(Ease.Linear);
			resourceModel.gameObject.transform.DOMoveX(moveToPosition.x, resourcesMoveSpeed, false).SetEase(GameController.Instance.unitsHorizontalEase).SetId("Tween_LastUnit_MoveX");
			resourceModel.gameObject.transform.DOMoveZ(moveToPosition.z, resourcesMoveSpeed, false).SetEase(GameController.Instance.unitsHorizontalEase).SetId("Tween_LastUnit_MoveZ");
			resourceModel.transform.DOMoveY(num, resourcesMoveSpeed, false).SetEase(GameController.Instance.unitsVerticalEase).SetId("Tween_LastUnit_MoveY")
				.OnComplete(delegate
				{
					this.ReportFinishingAnimation(resourceModel, lastAnimation, fromUnit);
				});
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0003C9C9 File Offset: 0x0003ABC9
		private void ReportFinishingAnimation(GameObject resourceModel, bool lastAnimation = false, bool fromUnit = false)
		{
			this.resourcesOnMove.Remove(resourceModel);
			resourceModel.SetActive(false);
			if (lastAnimation)
			{
				ShowEnemyMoves.Instance.SetAnimationInProgress(false);
				ShowEnemyMoves.Instance.GetNextAnimation();
			}
			if (fromUnit)
			{
				GameController.Instance.gameBoardPresenter.UpdateStaticObjects();
			}
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x000C3E70 File Offset: 0x000C2070
		public void UpdateTempResource(int amount)
		{
			GameHexPresenter.HexResource hexResourceData = this.hex.GetHexResourceData(this.resourceType);
			this.temporaryResourceValue = amount;
			hexResourceData.resourceObject.SetActive(amount > 0);
			hexResourceData.resourceCountInfo.gameObject.SetActive(amount > 0);
			hexResourceData.resourceCountInfo.text = this.temporaryResourceValue.ToString();
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x000C3ED0 File Offset: 0x000C20D0
		public void UpdateTempResourceAfterMove(int amount)
		{
			int num = this.hex.GetGameHexLogic().resources[this.resourceType];
			this.UpdateTempResource(num);
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x000C3F00 File Offset: 0x000C2100
		public void ForceResourceModelToFinishMove()
		{
			foreach (GameObject gameObject in new List<GameObject>(this.resourcesOnMove))
			{
				gameObject.gameObject.transform.DOComplete(true);
				gameObject.transform.DOComplete(true);
			}
		}

		// Token: 0x0400167E RID: 5758
		public GameHexPresenter hex;

		// Token: 0x0400167F RID: 5759
		public Light highlight;

		// Token: 0x04001680 RID: 5760
		public ResourceType resourceType;

		// Token: 0x04001681 RID: 5761
		public Image[] iconVariants;

		// Token: 0x04001682 RID: 5762
		[SerializeField]
		private float quakeRadius = 4f;

		// Token: 0x04001683 RID: 5763
		private float mass = 0.2f;

		// Token: 0x04001684 RID: 5764
		private float forceAmortization = 50f;

		// Token: 0x04001685 RID: 5765
		private int temporaryResourceValue;

		// Token: 0x04001686 RID: 5766
		private float heightOnDrop;

		// Token: 0x04001687 RID: 5767
		public MeshRenderer resourceOutlineMobile;

		// Token: 0x04001688 RID: 5768
		[SerializeField]
		private List<Renderer> resourceRenderer;

		// Token: 0x04001689 RID: 5769
		private MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x0400168A RID: 5770
		private Color[] fresnelColors = new Color[]
		{
			Color.yellow,
			Color.green,
			Color.blue
		};

		// Token: 0x0400168C RID: 5772
		private GameObject tempResource;

		// Token: 0x0400168D RID: 5773
		private List<GameObject> resourcesOnMove = new List<GameObject>();

		// Token: 0x0200041A RID: 1050
		// (Invoke) Token: 0x0600202C RID: 8236
		public delegate void OnResourceDown(ResourcePresenter presenter);
	}
}
