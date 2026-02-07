using System;
using System.Collections.Generic;
using DG.Tweening;
using I2.Loc;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace HoneyFramework
{
	// Token: 0x020001B7 RID: 439
	public class CameraControler : MonoBehaviour
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0003070E File Offset: 0x0002E90E
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x00030715 File Offset: 0x0002E915
		public static bool CameraMovementBlocked { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0003071D File Offset: 0x0002E91D
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x00030724 File Offset: 0x0002E924
		public static bool CameraDragMovementBlocked { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0003072C File Offset: 0x0002E92C
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x00030733 File Offset: 0x0002E933
		public static bool BlockEverything { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00080FD8 File Offset: 0x0007F1D8
		private bool TooltipsEnabled
		{
			get
			{
				for (int i = 0; i < this.tooltipBlockers.Length; i++)
				{
					if (this.tooltipBlockers[i].activeInHierarchy)
					{
						return false;
					}
				}
				return PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_TOOLTIPS_ACTIVE, OptionsManager.TOOLTIPS_DEFAULT) != 0;
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0008101C File Offset: 0x0007F21C
		private void Awake()
		{
			if (PlatformManager.IsMobile)
			{
				this.mouseDragOffset = 10f;
			}
			CameraControler.Instance = this;
			CameraControler.BlockEverything = false;
			this.RotationAdjustment = new Vector3(-150f, 90f, -90f);
			this.encounterRotationAdjustment = Vector3.zero;
			this.RotateTracksToCamera();
			this.swipeMinDistanceTreshold = 200;
			this.swipeAngleTreshold = 10;
			this.swipeSpeedDetectionTreshold = 500;
			if (this.isGame)
			{
				this.stick = base.transform.parent;
				this.swivel = this.stick.parent;
				this.globalFogMask = base.GetComponent<GlobalFogMask>();
			}
			this.screenRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
			if (QualitySettings.GetQualityLevel() == 5)
			{
				DepthOfField component = base.GetComponent<DepthOfField>();
				if (component != null)
				{
					component.enabled = true;
				}
				PostProcessingBehaviour component2 = base.GetComponent<PostProcessingBehaviour>();
				if (component2 != null)
				{
					component2.enabled = true;
				}
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00081120 File Offset: 0x0007F320
		private void Start()
		{
			this.posXMin = -15f;
			this.posXMax = 15f;
			this.posYMin = -15f;
			this.posYMax = 15f;
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
			string name = SceneManager.GetActiveScene().name;
			CameraControler.CameraMovementBlocked = false;
			if (this.swivel != null)
			{
				this.rotation = this.swivel.transform.rotation.eulerAngles;
				this.initialCamRotation = this.swivel.transform.rotation.eulerAngles;
			}
			else
			{
				this.rotation = Vector3.zero;
				this.initialCamRotation = Vector3.zero;
			}
			this.fixedRotationPos = 0f;
			this.RotateFactionLogos();
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000811EC File Offset: 0x0007F3EC
		public bool MouseHitTestUI()
		{
			List<RaycastResult> list = new List<RaycastResult>();
			Canvas[] ui = this.UI;
			for (int i = 0; i < ui.Length; i++)
			{
				ui[i].GetComponent<GraphicRaycaster>().Raycast(new PointerEventData(null)
				{
					position = Input.mousePosition
				}, list);
			}
			return list.Count == 0;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00081244 File Offset: 0x0007F444
		private void UpdateTooltip()
		{
			if (Input.mousePosition != this.mouselastPos)
			{
				this.mouselastPos = Input.mousePosition;
				this.mouseLastMoved = Time.realtimeSinceStartup;
			}
			if (Time.realtimeSinceStartup - this.mouseLastMoved > 0.5f)
			{
				List<RaycastResult> list = new List<RaycastResult>();
				Canvas[] ui = this.UI;
				for (int i = 0; i < ui.Length; i++)
				{
					ui[i].GetComponent<GraphicRaycaster>().Raycast(new PointerEventData(null)
					{
						position = Input.mousePosition
					}, list);
				}
				TooltipInfo tooltipInfo = null;
				bool flag = false;
				foreach (RaycastResult raycastResult in list)
				{
					if (raycastResult.gameObject != null)
					{
						TooltipInfo component = raycastResult.gameObject.GetComponent<TooltipInfo>();
						if (component != null)
						{
							if (component.block)
							{
								flag = true;
							}
							tooltipInfo = component;
						}
					}
				}
				if (!this.TooltipsEnabled)
				{
					this.tooltip.gameObject.SetActive(false);
					return;
				}
				if (tooltipInfo != null)
				{
					if (!flag && !this.isAnyDropdownDown())
					{
						this.UpdateTooltip(((ITooltipInfo)tooltipInfo).InfoBasic(), ((ITooltipInfo)tooltipInfo).InfoAdv());
						return;
					}
				}
				else
				{
					if (list.Count > 0)
					{
						this.tooltip.gameObject.SetActive(false);
						return;
					}
					RaycastHit raycastHit;
					if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, LayerMask.GetMask(new string[] { "Tokens" }) | LayerMask.GetMask(new string[] { "Hex2d" }) | LayerMask.GetMask(new string[] { "Units" }) | LayerMask.GetMask(new string[] { "Default" }) | LayerMask.GetMask(new string[] { "CameraPlane" }) | LayerMask.GetMask(new string[] { "Resources" })))
					{
						this.tooltip.gameObject.SetActive(false);
						return;
					}
					GameObject gameObject = raycastHit.transform.gameObject;
					if (gameObject.GetComponent<ITooltipInfo>() != null)
					{
						ITooltipInfo component2 = gameObject.GetComponent<ITooltipInfo>();
						string text = component2.InfoBasic();
						string text2 = component2.InfoAdv();
						if (text2.Length < 20)
						{
							if (!text.Contains("/"))
							{
								text = "Tooltips/" + text;
							}
							if (!text2.Contains("/"))
							{
								text2 = "Tooltips/" + text2;
							}
							this.UpdateTooltip(ScriptLocalization.Get(text), ScriptLocalization.Get(text2));
							return;
						}
						this.UpdateTooltip(text, text2);
						return;
					}
					else
					{
						if (gameObject.gameObject.name.Length <= 4)
						{
							this.tooltip.gameObject.SetActive(false);
							return;
						}
						int num = 0;
						int num2 = 0;
						bool flag2 = int.TryParse(gameObject.gameObject.name[3].ToString(), out num) & int.TryParse(gameObject.gameObject.name[4].ToString(), out num2);
						ITooltipInfo gameHexPresenter = GameController.Instance.GetGameHexPresenter(num, num2);
						if (flag2 && gameHexPresenter != null)
						{
							this.UpdateTooltip(ScriptLocalization.Get("Tooltips/" + gameHexPresenter.InfoBasic()), ScriptLocalization.Get("Tooltips/" + gameHexPresenter.InfoAdv()));
							return;
						}
						this.tooltip.gameObject.SetActive(false);
						return;
					}
				}
			}
			else
			{
				this.tooltip.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x000815C8 File Offset: 0x0007F7C8
		private void UpdateTooltip(string basicText, string advancedText)
		{
			this.tooltipTextBasic.text = basicText;
			this.tooltipTextAdvanced.text = advancedText;
			this.tooltip.transform.position = Input.mousePosition + 8f * Vector3.right;
			if (this.tooltip.rect.xMax + this.tooltip.localPosition.x > this.tooltipContainer.rect.xMax)
			{
				Vector3 localPosition = this.tooltip.transform.localPosition;
				localPosition.x = this.tooltipContainer.rect.xMax - this.tooltip.rect.xMax;
				this.tooltip.transform.localPosition = localPosition;
			}
			if (this.tooltip.rect.yMin + this.tooltip.localPosition.y < this.tooltipContainer.rect.yMin)
			{
				Vector3 localPosition2 = this.tooltip.transform.localPosition;
				localPosition2.y = this.tooltipContainer.rect.yMin - this.tooltip.rect.yMin;
				this.tooltip.transform.localPosition = localPosition2;
			}
			this.tooltipTextAdvanced.gameObject.SetActive(Time.realtimeSinceStartup - this.mouseLastMoved > 1.5f);
			this.tooltip.gameObject.SetActive(true);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0008175C File Offset: 0x0007F95C
		public bool isAnyDropdownDown()
		{
			if (PlatformManager.IsMobile)
			{
				return false;
			}
			foreach (DropdownChecker dropdownChecker in this.dropdowns)
			{
				if ((dropdownChecker != null) & dropdownChecker.isDropdownDown)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x000817C8 File Offset: 0x0007F9C8
		private void LateUpdate()
		{
			this.mouseHitTestNoUI = this.MouseHitTestUI();
			this.UpdateCursor();
			if (SceneManager.GetActiveScene().name != "showroom")
			{
				if (PlatformManager.IsStandalone)
				{
					this.UpdateTooltip();
				}
				if (CameraControler.BlockEverything)
				{
					return;
				}
				if (Input.GetMouseButtonDown(0))
				{
					this.mouseLastDownOnMap = this.mouseHitTestNoUI;
				}
				if (this.isGame)
				{
					this.UpdateCameraPosition();
					if (PlatformManager.IsStandalone)
					{
						this.UpdateCameraRotation();
					}
					this.OnMouseClick();
					if (!PlatformManager.IsStandalone)
					{
						this.UpdateCameraRotationBasedOnZoom();
					}
				}
				if (PlatformManager.IsStandalone)
				{
					this.UpdateHoover();
				}
				this.SwipeGesture();
				this.UpdateLayers();
			}
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00081870 File Offset: 0x0007FA70
		private void UpdateCursor()
		{
			if (this.isGame)
			{
				if (this.drag)
				{
					this.cursor = CameraControler.CursorType.Grab;
				}
				else if (GameController.Instance.hookController.CursorSnapsToObject())
				{
					this.cursor = CameraControler.CursorType.Load;
				}
				else if (this.mouseHitTestNoUI)
				{
					RaycastHit raycastHit;
					if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, LayerMask.GetMask(new string[] { "Hex2d" }) | LayerMask.GetMask(new string[] { "Units" }) | LayerMask.GetMask(new string[] { "Default" }) | LayerMask.GetMask(new string[] { "CameraPlane" }) | LayerMask.GetMask(new string[] { "Resources" })) && raycastHit.collider.gameObject.GetComponent<BuildingPresenter>() == null && raycastHit.collider.gameObject.GetComponent<ResourcePresenter>() == null && raycastHit.collider.gameObject.GetComponent<UnitPresenter>() == null)
					{
						this.cursor = CameraControler.CursorType.None;
					}
					else
					{
						this.cursor = CameraControler.CursorType.Finger;
					}
				}
				else
				{
					this.cursor = CameraControler.CursorType.None;
				}
				if (this.cursor != this.cursorLast)
				{
					switch (this.cursor)
					{
					case CameraControler.CursorType.None:
						Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
						break;
					case CameraControler.CursorType.Finger:
						Cursor.SetCursor(this.handCursor, Vector2.one * (float)this.handCursor.width / 2f, CursorMode.Auto);
						break;
					case CameraControler.CursorType.Grab:
						Cursor.SetCursor(this.grabCursor, Vector2.one * 16f, CursorMode.Auto);
						break;
					case CameraControler.CursorType.Load:
						Cursor.SetCursor(this.dropCursor, Vector2.one * (float)this.dropCursor.width / 2f, CursorMode.Auto);
						break;
					}
					this.cursorLast = this.cursor;
				}
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00081A6C File Offset: 0x0007FC6C
		private void UpdateCameraRotation()
		{
			if (!PlatformManager.IsMobile || Input.touchCount == 0)
			{
				if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
				{
					this.rotationSnapStart = Time.time - 10f;
					this.mouseReference = Input.mousePosition;
					this.initialCamRotation = this.swivel.localRotation.eulerAngles;
					this.initialCamRotation.z = this.initialCamRotation.z - 360f;
				}
				if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
				{
					this.mouseOffset = Input.mousePosition - this.mouseReference;
					this.rotation.y = this.fixedRotationPos + this.mouseOffset.x * this.rotationSpeed;
					this.rotation.z = Mathf.Clamp(this.initialCamRotation.z + this.mouseOffset.y * this.rotationSpeed, -this.swivelMaxZoom, -this.swivelMinZoom);
					this.swivel.localRotation = Quaternion.Euler(this.swivel.localRotation.x, this.rotation.y, this.rotation.z);
				}
				if (Input.GetMouseButtonUp(1) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
				{
					this.SmoothRotationReturn();
				}
			}
			if (PlatformManager.IsStandalone)
			{
				if (KeyboardShortcuts.Instance.isCameraRotateLeft)
				{
					this.rotation.y = this.rotation.y + this.rotationSpeedKeyboard;
					this.rotation.z = Mathf.Clamp(this.initialCamRotation.z + this.mouseOffset.y * this.rotationSpeed, -this.swivelMaxZoom, -this.swivelMinZoom);
					this.initialCamRotation = this.swivel.transform.rotation.eulerAngles;
					this.swivel.localRotation = Quaternion.Euler(new Vector3(this.swivel.localRotation.x, this.rotation.y, this.initialCamRotation.z));
				}
				else if (KeyboardShortcuts.Instance.isCameraRotateRight)
				{
					this.rotation.y = this.rotation.y - this.rotationSpeedKeyboard;
					this.rotation.z = Mathf.Clamp(this.initialCamRotation.z + this.mouseOffset.y * this.rotationSpeed, -this.swivelMaxZoom, -this.swivelMinZoom);
					this.initialCamRotation = this.swivel.transform.rotation.eulerAngles;
					this.swivel.localRotation = Quaternion.Euler(new Vector3(this.swivel.localRotation.x, this.rotation.y, this.initialCamRotation.z));
				}
			}
			this.SmoothRotation();
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00081D64 File Offset: 0x0007FF64
		public void SwipeGesture()
		{
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				Vector2 position = Input.GetTouch(0).position;
				Vector2 vector = position - this.swipeStartPosition;
				float num = Mathf.Sqrt(Mathf.Pow(vector.x, 2f) + Mathf.Pow(vector.y, 2f));
				float num2 = Mathf.Atan(vector.y / vector.x) * 57.295776f;
				float num3 = Time.time - this.swipeStartTime;
				float num4 = num / num3;
				if (this.swipeStartPosition.y < position.y)
				{
					if (num2 < 0f)
					{
						num2 *= -1f;
					}
					if (num > (float)this.swipeMinDistanceTreshold && num2 < (float)this.swipeAngleTreshold && num4 > (float)this.swipeSpeedDetectionTreshold)
					{
					}
				}
				else if (this.swipeStartPosition.y > position.y)
				{
					if (num2 < 0f)
					{
						num2 *= -1f;
					}
					if (num > (float)this.swipeMinDistanceTreshold && num2 < (float)this.swipeAngleTreshold)
					{
						float num5 = (float)this.swipeSpeedDetectionTreshold;
					}
				}
			}
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				this.swipeStartPosition = Input.GetTouch(0).position;
				this.swipeStartTime = Time.time;
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00081EC8 File Offset: 0x000800C8
		public void SmoothRotation()
		{
			if (Time.time <= this.rotationSnapStart + this.rotationAnimDuration)
			{
				float num = this.rotationAnimCurve.Evaluate((Time.time - this.rotationSnapStart) / this.rotationAnimDuration);
				float num2 = this.fixedRotationPos + num * (this.rotation.y - this.fixedRotationPos);
				float num3 = this.swivel.transform.rotation.eulerAngles.z;
				if (PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_CAMERA_ROTATION, 0) == 0 && !this.rotationKeyUsed)
				{
					num3 += num * (this.rotation.z - this.initialCamRotation.z);
				}
				Vector3 vector = new Vector3(this.swivel.localRotation.x, num2, num3);
				this.rotation.y = num2;
				this.swivel.localRotation = Quaternion.Euler(vector);
				this.objectsNeedRotation = true;
				return;
			}
			this.rotationKeyUsed = false;
			if (this.objectsNeedRotation && this.isRotationUpdate)
			{
				this.UpdateObjectRotationToCamera();
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00081FD4 File Offset: 0x000801D4
		public void SmoothRotationReturn()
		{
			this.rotationSnapStart = Time.time;
			this.rotation.y = this.rotation.y % 360f;
			if (this.rotation.y < -180f)
			{
				this.rotation.y = this.rotation.y + 360f;
			}
			else if (this.rotation.y > 180f)
			{
				this.rotation.y = this.rotation.y - 360f;
			}
			if (this.rotation.y < 30f && this.rotation.y > -30f)
			{
				this.fixedRotationPos = 0f;
				this.RotationAdjustment.x = -150f;
				this.encounterRotationAdjustment.z = 0f;
			}
			else if (this.rotation.y >= 30f && this.rotation.y < 75f)
			{
				this.fixedRotationPos = 60f;
				this.RotationAdjustment.x = -90f;
				this.encounterRotationAdjustment.z = -60f;
			}
			else if (this.rotation.y >= 75f && this.rotation.y < 105f)
			{
				this.fixedRotationPos = 90f;
				this.RotationAdjustment.x = -90f;
				this.encounterRotationAdjustment.z = -60f;
			}
			else if (this.rotation.y >= 105f && this.rotation.y < 150f)
			{
				this.fixedRotationPos = 120f;
				this.RotationAdjustment.x = -30f;
				this.encounterRotationAdjustment.z = 240f;
			}
			else if (this.rotation.y < -150f)
			{
				this.fixedRotationPos = -180f;
				this.RotationAdjustment.x = -330f;
				this.encounterRotationAdjustment.z = 180f;
			}
			else if (this.rotation.y > 150f)
			{
				this.fixedRotationPos = 180f;
				this.RotationAdjustment.x = -330f;
				this.encounterRotationAdjustment.z = 180f;
			}
			else if (this.rotation.y >= -150f && this.rotation.y < -105f)
			{
				this.fixedRotationPos = -120f;
				this.RotationAdjustment.x = -270f;
				this.encounterRotationAdjustment.z = 120f;
			}
			else if (this.rotation.y >= -105f && this.rotation.y < -60f)
			{
				this.fixedRotationPos = -90f;
				this.RotationAdjustment.x = -210f;
				this.encounterRotationAdjustment.z = 60f;
			}
			else if (this.rotation.y >= -60f && this.rotation.y < -30f)
			{
				this.fixedRotationPos = -60f;
				this.RotationAdjustment.x = -210f;
				this.encounterRotationAdjustment.z = 60f;
			}
			if (this.fixedRotationPos == 90f)
			{
				this.fieldTemplate.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(90f, 150f, 0f));
				return;
			}
			if (this.fixedRotationPos == -90f)
			{
				this.fieldTemplate.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(90f, 30f, 0f));
				return;
			}
			this.fieldTemplate.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(90f, 90f + this.fixedRotationPos, 0f));
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x000823CC File Offset: 0x000805CC
		public void UpdateObjectRotationToCamera()
		{
			foreach (GameHexPresenter gameHexPresenter in this.gameBoard.GetComponent<FlatWorld>().hexes.Values)
			{
				Vector3 vector = new Vector3(gameHexPresenter.GetWorldPosition().x, 0f, gameHexPresenter.GetWorldPosition().y);
				for (int i = 0; i < gameHexPresenter.resources.Length; i++)
				{
					Vector3 vector2 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.resources[i].transform.position;
					gameHexPresenter.resources[i].resourceObject.transform.position = vector2;
				}
				if (gameHexPresenter.hasTunnel)
				{
					gameHexPresenter.tunnelTransform.localRotation = Quaternion.Euler(0f, 90f, this.RotationAdjustment.x + 150f);
				}
				if (gameHexPresenter.token != null)
				{
					Vector3 vector3 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.token.transform.position;
					gameHexPresenter.token.transform.position = vector3;
				}
				if (gameHexPresenter.hasEncounter && gameHexPresenter.encounter != null)
				{
					Vector3 vector4 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.encounter.transform.position;
					gameHexPresenter.encounter.transform.position = vector4;
					gameHexPresenter.encounter.transform.localRotation = Quaternion.Euler(this.encounterRotationAdjustment - new Vector3(0f, 0f, 43f));
					gameHexPresenter.fadedEncounterMarker.transform.position = gameHexPresenter.encounter.transform.Find("EncounterModel").transform.position;
					gameHexPresenter.fadedEncounterMarker.transform.localPosition = new Vector3(-0.04f, gameHexPresenter.fadedEncounterMarker.transform.localPosition.y, gameHexPresenter.fadedEncounterMarker.transform.localPosition.z);
					gameHexPresenter.fadedEncounterMarker.transform.localRotation = Quaternion.Euler(0f, -90f, this.encounterRotationAdjustment.z - 43f);
				}
				if (gameHexPresenter.hexType == HexType.factory)
				{
					Vector3 vector5 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.factory.transform.position;
					gameHexPresenter.hexTemplate.transform.position = vector5;
					gameHexPresenter.hexTemplate.transform.localRotation = Quaternion.Euler(this.RotationAdjustment);
				}
				if (gameHexPresenter.hexType == HexType.village)
				{
					Vector3 vector6 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.village.transform.position;
					gameHexPresenter.hexTemplate.transform.position = vector6;
					gameHexPresenter.hexTemplate.transform.localRotation = Quaternion.Euler(this.RotationAdjustment);
				}
				if (gameHexPresenter.hexType == HexType.forest)
				{
					Vector3 vector7 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.forest.transform.position;
					gameHexPresenter.hexTemplate.transform.position = vector7;
					gameHexPresenter.hexTemplate.transform.localRotation = Quaternion.Euler(this.RotationAdjustment);
				}
				if (gameHexPresenter.hexType == HexType.tundra)
				{
					Vector3 vector8 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.tundra.transform.position;
					gameHexPresenter.hexTemplate.transform.position = vector8;
					gameHexPresenter.hexTemplate.transform.localRotation = Quaternion.Euler(this.RotationAdjustment);
				}
				if (gameHexPresenter.hexType == HexType.mountain)
				{
					Vector3 vector9 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.mountain.transform.position;
					gameHexPresenter.hexTemplate.transform.position = vector9;
					gameHexPresenter.hexTemplate.transform.localRotation = Quaternion.Euler(this.RotationAdjustment);
				}
				if (gameHexPresenter.hexType == HexType.farm)
				{
					Vector3 vector10 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.farm.transform.position;
					gameHexPresenter.hexTemplate.transform.position = vector10;
					gameHexPresenter.hexTemplate.transform.localRotation = Quaternion.Euler(this.RotationAdjustment);
				}
				if (gameHexPresenter.hexType == HexType.lake)
				{
					Vector3 vector11 = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.lake.transform.position;
					gameHexPresenter.hexTemplate.transform.position = vector11;
					gameHexPresenter.hexTemplate.transform.localRotation = Quaternion.Euler(this.RotationAdjustment);
				}
				if (gameHexPresenter.building != null)
				{
					gameHexPresenter.building.transform.position = vector + GameController.Instance.gameBoardPresenter.positionsTemplate.building.transform.position;
					gameHexPresenter.building.transform.localRotation = Quaternion.Euler(0f, 0f, this.encounterRotationAdjustment.z);
				}
				this.RotateTracksToCamera();
				this.RotateFactionLogos();
				if (PlatformManager.IsStandalone)
				{
					this.ShowAndRotatePlayerNames();
				}
			}
			foreach (PlayerUnits playerUnits2 in GameController.Instance.playerUnits)
			{
				if (!(playerUnits2 == null))
				{
					foreach (UnitPresenter unitPresenter in playerUnits2.mechObjects)
					{
						if (unitPresenter.gameObject.activeSelf)
						{
							unitPresenter.gameObject.transform.position = unitPresenter.hex.GetUnitPosition(unitPresenter.UnitLogic);
						}
					}
					foreach (UnitPresenter unitPresenter2 in playerUnits2.workerObjects)
					{
						if (unitPresenter2.gameObject.activeSelf)
						{
							unitPresenter2.gameObject.transform.position = unitPresenter2.hex.GetUnitPosition(unitPresenter2.UnitLogic);
							if (PlatformManager.IsMobile)
							{
								unitPresenter2.gameObject.transform.localRotation = Quaternion.Euler(0f, this.RotationAdjustment.x + 150f, 0f);
							}
							else
							{
								unitPresenter2.gameObject.transform.localRotation = Quaternion.Euler(-this.RotationAdjustment.x - 150f, -90f, 90f);
							}
						}
					}
					if (playerUnits2.characterObject.gameObject.activeSelf)
					{
						playerUnits2.characterObject.gameObject.transform.position = playerUnits2.characterObject.hex.GetUnitPosition(playerUnits2.characterObject.UnitLogic);
					}
				}
			}
			this.objectsNeedRotation = false;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00082B04 File Offset: 0x00080D04
		public void RotateTracksToCamera()
		{
			if (PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_TRADITIONAL_TRACKS) == 1 && this.popTrack != null && this.powerTrack != null && this.trumpTrack != null)
			{
				string text = this.fixedRotationPos.ToString();
				uint num = global::<PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 421209969U)
				{
					if (num <= 353113755U)
					{
						if (num != 201234636U)
						{
							if (num != 353113755U)
							{
								goto IL_041E;
							}
							if (!(text == "-180"))
							{
								goto IL_041E;
							}
							goto IL_026C;
						}
						else if (!(text == "90"))
						{
							goto IL_041E;
						}
					}
					else if (num != 367583803U)
					{
						if (num != 421209969U)
						{
							goto IL_041E;
						}
						if (!(text == "-120"))
						{
							goto IL_041E;
						}
						goto IL_0345;
					}
					else if (!(text == "60"))
					{
						goto IL_041E;
					}
				}
				else if (num <= 1933075630U)
				{
					if (num != 890022063U)
					{
						if (num != 1933075630U)
						{
							goto IL_041E;
						}
						if (!(text == "120"))
						{
							goto IL_041E;
						}
					}
					else
					{
						if (!(text == "0"))
						{
							goto IL_041E;
						}
						goto IL_041E;
					}
				}
				else if (num != 3383969689U)
				{
					if (num != 3620974926U)
					{
						if (num != 4146044052U)
						{
							goto IL_041E;
						}
						if (!(text == "180"))
						{
							goto IL_041E;
						}
						goto IL_026C;
					}
					else
					{
						if (!(text == "-60"))
						{
							goto IL_041E;
						}
						goto IL_0345;
					}
				}
				else
				{
					if (!(text == "-90"))
					{
						goto IL_041E;
					}
					goto IL_0345;
				}
				this.popTrack.transform.localPosition = new Vector3(3f, 18f, -0.01f);
				this.popTrack.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
				this.powerTrack.transform.localPosition = new Vector3(3f, -17.75f, -0.01f);
				this.powerTrack.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
				this.trumpTrack.transform.localPosition = new Vector3(18f, -2f, -0.01f);
				this.trumpTrack.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
				return;
				IL_026C:
				this.popTrack.transform.localPosition = new Vector3(18f, 3.8f, -0.01f);
				this.popTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
				this.powerTrack.transform.localPosition = new Vector3(-17.75f, 4f, -0.01f);
				this.powerTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
				this.trumpTrack.transform.localPosition = new Vector3(0f, -17.5f, -0.01f);
				this.trumpTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
				return;
				IL_0345:
				this.popTrack.transform.localPosition = new Vector3(-3f, -18f, -0.01f);
				this.popTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
				this.powerTrack.transform.localPosition = new Vector3(-3f, 17.75f, -0.01f);
				this.powerTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
				this.trumpTrack.transform.localPosition = new Vector3(-18f, 2f, -0.01f);
				this.trumpTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
				return;
				IL_041E:
				this.popTrack.transform.localPosition = new Vector3(-18f, 3.8f, -0.01f);
				this.popTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				this.powerTrack.transform.localPosition = new Vector3(17.75f, 4f, -0.01f);
				this.powerTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
				this.trumpTrack.transform.localPosition = new Vector3(0f, 17f, -0.01f);
				this.trumpTrack.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00083008 File Offset: 0x00081208
		public void RotateFactionLogos()
		{
			if (this.factionLogos == null)
			{
				return;
			}
			foreach (object obj in this.factionLogos)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeSelf)
				{
					if (this.fixedRotationPos == 90f)
					{
						Vector3 vector = new Vector3(90f, 0f, -180f);
						transform.transform.rotation = Quaternion.Euler(vector);
					}
					else if (this.fixedRotationPos == -90f)
					{
						Vector3 vector2 = new Vector3(90f, 0f, 0f);
						transform.transform.rotation = Quaternion.Euler(vector2);
					}
					else
					{
						Vector3 vector3 = new Vector3(90f, 0f, this.encounterRotationAdjustment.z - 90f);
						transform.transform.rotation = Quaternion.Euler(vector3);
					}
				}
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00083124 File Offset: 0x00081324
		public void ShowAndRotatePlayerNames()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				using (List<PlayerData>.Enumerator enumerator = MultiplayerController.Instance.playersInGame.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlayerData playerData = enumerator.Current;
					}
					return;
				}
			}
			foreach (Player player in GameController.GameManager.players)
			{
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x000831C0 File Offset: 0x000813C0
		private void UpdateCameraRotationBasedOnZoom()
		{
			float num = Mathf.Lerp(this.swivelMinZoom, this.swivelMaxZoom, this.zoom);
			this.initialCamRotation.z = -num;
			this.swivel.localRotation = Quaternion.Euler(0f, this.swivel.localRotation.y + this.fixedRotationPos, this.initialCamRotation.z);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0008322C File Offset: 0x0008142C
		private void UpdateCameraPosition()
		{
			Vector3 vector = this.swivel.transform.position;
			if (this.zoomInToMouse)
			{
				this.CameraZoomCorrectionFirstRaycast();
			}
			if (this.mouseHitTestNoUI && !CameraControler.CameraMovementBlocked)
			{
				this.verticalDistanceAccumulator -= this.GetZoomInput() * this.scrollSpeedMultipler;
				float num = 0f;
				if (Mathf.Abs(this.verticalDistanceAccumulator) > 0.02f)
				{
					num += 0.02f * (float)((this.verticalDistanceAccumulator >= 0f) ? 1 : (-1));
				}
				else
				{
					num = this.verticalDistanceAccumulator;
				}
				this.verticalDistanceAccumulator -= num;
				if (PlatformManager.IsStandalone)
				{
					if (KeyboardShortcuts.Instance.isCameraZoomIn)
					{
						num += 0.02f;
					}
					else if (KeyboardShortcuts.Instance.isCameraZoomOut)
					{
						num -= 0.02f;
					}
				}
				num *= (1f + 50f / (this.stickMaxZoom - this.stickMinZoom)) / 2f;
				this.zoom = Mathf.Clamp01(this.zoom + num);
			}
			float num2 = Mathf.Lerp(this.stickMinZoom, this.stickMaxZoom, this.zoom);
			this.stick.localPosition = new Vector3(-num2, 0f, 0f);
			this.AdjustFog(num2);
			if (PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_CAMERA_ROTATION, 0) == 0)
			{
				this.UpdateCameraRotationBasedOnZoom();
			}
			if (PlatformManager.IsMobile)
			{
				if (Input.touchCount > 1)
				{
					return;
				}
				if (Input.touchCount == 1 && Input.touches[0].fingerId != this.lastFingerUsedForDrag)
				{
					this.lastFingerUsedForDrag = Input.touches[0].fingerId;
					this.mouseDragStart = Vector3.zero;
					return;
				}
			}
			if (!CameraControler.CameraMovementBlocked)
			{
				float num3 = this.CalculateVerticalMovement(num2);
				float num4 = this.CalculateHorizontalMovement(num2);
				if (Input.GetMouseButton(0) && !CameraControler.CameraDragMovementBlocked && this.mouseDragStart != Vector3.zero && this.mouseDragStart != Input.mousePosition)
				{
					RaycastHit raycastHit;
					if (Physics.Raycast(base.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out raycastHit))
					{
						this.cameraToTerrainDistance = raycastHit.distance;
					}
					if ((Input.mousePosition - this.mouseDragStart).magnitude > this.mouseDragOffset)
					{
						if (PlatformManager.IsMobile)
						{
							float num5 = (Input.mousePosition.x - this.mouseDragStart.x) * 1000f / (float)Screen.width;
							float num6 = (-Input.mousePosition.y + this.mouseDragStart.y) * 600f / (float)Screen.height;
							float num7 = (this.startCameraToTerrainDistance + this.cameraToTerrainDistance) / 2f;
							vector -= base.gameObject.transform.right * this.moveCameraSmooth * num7 * num5;
							vector += Quaternion.Euler(0f, this.swivel.transform.rotation.eulerAngles.y, 0f) * Vector3.right * this.moveCameraSmooth * num7 * num6 * 2f;
							vector.y = this.swivel.transform.position.y;
						}
						else
						{
							vector = vector - base.gameObject.transform.right * this.moveCameraSmooth * ((this.startCameraToTerrainDistance + this.cameraToTerrainDistance) / 2f) * (Input.mousePosition.x - this.mouseDragStart.x) + Quaternion.Euler(0f, this.swivel.transform.rotation.eulerAngles.y, 0f) * Vector3.right * this.moveCameraSmooth * ((this.startCameraToTerrainDistance + this.cameraToTerrainDistance) / 2f) * (-Input.mousePosition.y + this.mouseDragStart.y) * 1f / (1f - 0.5f * Mathf.Sin(0.017453292f * (90f - this.angle)));
							vector.y = this.swivel.transform.position.y;
						}
						this.drag = true;
					}
					this.mouseDragStart = Input.mousePosition;
					this.startCameraToTerrainDistance = this.cameraToTerrainDistance;
				}
				if (Input.GetMouseButton(0) && this.mouseLastDownOnMap && !CameraControler.CameraDragMovementBlocked)
				{
					if (this.drag || this.mouseHitTestNoUI)
					{
						this.mouseDragStart = Input.mousePosition;
						RaycastHit raycastHit2;
						if (Physics.Raycast(base.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out raycastHit2))
						{
							this.startCameraToTerrainDistance = raycastHit2.distance;
						}
						if (this.statsPresenter != null)
						{
							this.endTurnHint.enabled = false;
							this.endTurnShade.enabled = false;
						}
					}
				}
				else
				{
					this.mouseDragStart = Vector3.zero;
				}
				if (GameController.Instance.combatPresenter != null && GameController.Instance.combatPresenter.preperationPanel != null && GameController.Instance.combatPresenter.preperationPanel.activeInHierarchy)
				{
					GameController.Instance.combatPresenter.ChangePreperationWindowVisibility(!this.drag);
				}
				num3 *= Time.unscaledDeltaTime;
				num4 *= Time.unscaledDeltaTime;
				Vector3 vector2 = Quaternion.Euler(0f, this.swivel.transform.rotation.eulerAngles.y, 0f) * Vector3.right * num3;
				vector += vector2 + base.gameObject.transform.right * num4;
				vector.y = this.swivel.transform.position.y;
			}
			if (vector.x < this.posXMin)
			{
				vector.x = this.posXMin;
			}
			if (vector.x > this.posXMax)
			{
				vector.x = this.posXMax;
			}
			if (vector.z < this.posYMin)
			{
				vector.z = this.posYMin;
			}
			if (vector.z > this.posYMax)
			{
				vector.z = this.posYMax;
			}
			this.swivel.transform.position = vector;
			if (this.zoomOutToCenter)
			{
				this.MoveCameraToCenterOnZoomOut();
			}
			if (this.zoomInToMouse)
			{
				this.CameraZoomCorrectionSecondRaycast();
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000838EC File Offset: 0x00081AEC
		private float GetZoomInput()
		{
			if (PlatformManager.IsStandalone)
			{
				return Input.GetAxis("Mouse ScrollWheel");
			}
			if (Input.touchCount >= 2)
			{
				this.allZoomTouchesReleased = false;
				Input.GetTouch(0);
				Input.GetTouch(1);
				float num = 1000f;
				float num2 = Vector2.Distance(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition, Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
				return (Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - num2) / num;
			}
			if (!this.allZoomTouchesReleased && Input.touchCount == 0)
			{
				this.allZoomTouchesReleased = true;
			}
			return 0f;
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000839BC File Offset: 0x00081BBC
		private float CalculateVerticalMovement(float distance)
		{
			float num = 5f;
			return this.AccumulateVerticalMovement() * this.speed * distance / num;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000839E0 File Offset: 0x00081BE0
		private float CalculateHorizontalMovement(float distance)
		{
			float num = 5f;
			return this.AccumulateHorizontalMovement() * this.speed * distance / num;
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00083A04 File Offset: 0x00081C04
		private float AccumulateVerticalMovement()
		{
			float num = 0f;
			if (PlatformManager.IsStandalone)
			{
				num = KeyboardShortcuts.Instance.verticalMovement;
			}
			if (PlatformManager.IsMobile && Input.touchCount == 1 && !this.edgeScrolling)
			{
				Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
				num += -deltaPosition.y / 1000f;
			}
			return num + (this.edgeScrolling ? this.GetScreenEdgeVerticalMovement() : 0f);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00083A78 File Offset: 0x00081C78
		private float AccumulateHorizontalMovement()
		{
			float num = 0f;
			if (PlatformManager.IsStandalone)
			{
				num = KeyboardShortcuts.Instance.horizontalMovement;
			}
			if (PlatformManager.IsMobile && Input.touchCount == 1 && !this.edgeScrolling)
			{
				Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
				num += -deltaPosition.x / 1000f;
			}
			return num + (this.edgeScrolling ? this.GetScreenEdgeHorizontalMovement() : 0f);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00083AEC File Offset: 0x00081CEC
		private float GetScreenEdgeVerticalMovement()
		{
			if (!this.screenRect.Contains(Input.mousePosition))
			{
				return 0f;
			}
			if (PlatformManager.IsStandalone || Application.isEditor)
			{
				if (Input.mousePosition.y > (float)Screen.height - this.screenEdgeSize)
				{
					return this.movementOnScreenEdge;
				}
				if (Input.mousePosition.y < this.screenEdgeSize)
				{
					return -this.movementOnScreenEdge;
				}
			}
			else
			{
				if (Input.GetTouch(0).position.y > (float)Screen.height - this.screenEdgeSize)
				{
					return this.movementOnScreenEdge;
				}
				if (Input.GetTouch(0).position.y < this.screenEdgeSize)
				{
					return -this.movementOnScreenEdge;
				}
			}
			return 0f;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00083BAC File Offset: 0x00081DAC
		private float GetScreenEdgeHorizontalMovement()
		{
			if (!this.screenRect.Contains(Input.mousePosition))
			{
				return 0f;
			}
			if (PlatformManager.IsStandalone || Application.isEditor)
			{
				if (Input.mousePosition.x > (float)Screen.width - this.screenEdgeSize)
				{
					return this.movementOnScreenEdge;
				}
				if (Input.mousePosition.x < this.screenEdgeSize)
				{
					return -this.movementOnScreenEdge;
				}
			}
			else
			{
				if (Input.GetTouch(0).position.x > (float)Screen.width - this.screenEdgeSize)
				{
					return this.movementOnScreenEdge;
				}
				if (Input.GetTouch(0).position.x < this.screenEdgeSize)
				{
					return -this.movementOnScreenEdge;
				}
			}
			return 0f;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0003073B File Offset: 0x0002E93B
		public void SetEdgeScrollingEnabled(bool enabled)
		{
			this.edgeScrolling = enabled;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00083C6C File Offset: 0x00081E6C
		private void CameraZoomCorrectionFirstRaycast()
		{
			RaycastHit raycastHit;
			if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && this.zoom > 0f && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1000f, this.mapLayerMask.value))
			{
				this.firstCorrectionRaycast = raycastHit.point;
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00083CCC File Offset: 0x00081ECC
		private void CameraZoomCorrectionSecondRaycast()
		{
			if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f && this.zoom > 0f)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1000f, this.mapLayerMask.value))
				{
					this.secondCorrectionRaycast = raycastHit.point;
				}
				this.cameraCorrectionVector = this.firstCorrectionRaycast - this.secondCorrectionRaycast;
				Vector3 position = this.swivel.transform.position;
				position.z += this.cameraCorrectionVector.z;
				position.x += this.cameraCorrectionVector.x;
				this.swivel.transform.position = position;
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00083D98 File Offset: 0x00081F98
		private void MoveCameraToCenterOnZoomOut()
		{
			if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
			{
				Vector3 position = this.swivel.position;
				if (this.swivel.position.x > -2.5f)
				{
					float num = Mathf.Lerp(this.swivel.position.x, -2.5f, this.zoom);
					position.x = num;
				}
				else
				{
					float num2 = Mathf.Lerp(this.swivel.position.x, -2.5f, this.zoom);
					position.x = num2;
				}
				if (this.swivel.position.z > 0.25f)
				{
					float num3 = Mathf.Lerp(this.swivel.position.z, 0.25f, this.zoom);
					position.z = num3;
				}
				else
				{
					float num4 = Mathf.Lerp(this.swivel.position.z, 0.25f, this.zoom);
					position.z = num4;
				}
				this.swivel.transform.DOMove(position, 0.1f, false);
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00030744 File Offset: 0x0002E944
		public void ShakeCamera(float power = 1f)
		{
			base.GetComponent<Camera>().DOShakeRotation(0.5f, new Vector3(1f, 0f, 1f) * power, 10, 90f, true);
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00083EB8 File Offset: 0x000820B8
		private void AdjustFog(float distance)
		{
			base.GetComponent<DepthOfField>().focalLength = distance + Mathf.Lerp(this.dofDistanceCorrection, 0f, this.zoom);
			base.GetComponent<DepthOfField>().focalSize = Mathf.Lerp(this.focalSizeClose, this.focalSizeFar, this.zoom);
			RenderSettings.fogDensity = this.fogDensityChangeCurve.Evaluate(this.zoom);
			this.globalFogMask.startDistance = this.fogStartDistanceChangeCurve.Evaluate(this.zoom);
			RenderSettings.fogColor = new Color(Mathf.Lerp(this.fogColorClose.r, this.fogColorFar.r, this.fogColorChangeCurve.Evaluate(this.zoom)), Mathf.Lerp(this.fogColorClose.g, this.fogColorFar.g, this.fogColorChangeCurve.Evaluate(this.zoom)), Mathf.Lerp(this.fogColorClose.b, this.fogColorFar.b, this.fogColorChangeCurve.Evaluate(this.zoom)));
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00083FCC File Offset: 0x000821CC
		private void OnMouseClick()
		{
			if (PlatformManager.IsMobile && Input.touchCount > 1)
			{
				return;
			}
			if (Input.GetMouseButtonUp(0))
			{
				if (GameController.Instance.gameBoardPresenter != null && GameController.Instance.gameBoardPresenter.status == GameBoardPresenter.Status.Ready && !this.drag && (PlatformManager.IsStandalone || this.allZoomTouchesReleased))
				{
					if (GameController.HexSelectionMode != GameController.SelectionMode.Normal && this.mouseLastDownOnMap && this.MouseHitTestUI())
					{
						GameHexPresenter gameHexPresenter = this.PointedHex();
						if (gameHexPresenter != null)
						{
							GameController.SetFocusHex(gameHexPresenter);
							return;
						}
					}
				}
				else if (this.drag)
				{
					this.drag = false;
				}
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00084064 File Offset: 0x00082264
		private void UpdateHoover()
		{
			GameController.SelectionMode hexSelectionMode = GameController.HexSelectionMode;
			if (hexSelectionMode == GameController.SelectionMode.MoveAction)
			{
				this.HooverForMove();
				return;
			}
			switch (hexSelectionMode)
			{
			case GameController.SelectionMode.Deploy:
				this.UniversalHoover(((DeployPresenter)HumanInputHandler.Instance.deployPresenter).GetAvaliableHexes());
				return;
			case GameController.SelectionMode.Build:
				this.UniversalHoover(((BuildPresenter)HumanInputHandler.Instance.buildPresenter).GetAvaliableHexes());
				return;
			case GameController.SelectionMode.PayResource:
				this.UniversalHoover(((PayResourcePresenter)HumanInputHandler.Instance.payDownActionPresenter).GetAvaliableHexes());
				return;
			default:
				return;
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00030779 File Offset: 0x0002E979
		public void HooverReset()
		{
			if (this.hooverHex != null)
			{
				this.hooverHex.SetFocus(false, HexMarkers.MarkerType.Hoover, 0f, false);
				this.hooverHex = null;
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x000840E8 File Offset: 0x000822E8
		private void HooverForMove()
		{
			MovePresenter movePresenter = (MovePresenter)HumanInputHandler.Instance.movePresenter;
			if (this.mouseHitTestNoUI)
			{
				if ((GameController.GameManager.moveManager.DoesUnitCannotMoveBecauseOfTheLakeCaseClick() && GameController.GameManager.moveManager.GetSelectedUnit() is Worker) || (GameController.Instance.DragAndDrop && (GameController.Instance.hookController.IsResourceDragged() || !GameController.Instance.hookController.IsUnitDragged())))
				{
					return;
				}
				GameHexPresenter gameHexPresenter = this.PointedHex();
				this.HooverReset();
				if (gameHexPresenter != null)
				{
					if (movePresenter != null && movePresenter.IsMovePossible(gameHexPresenter.GetGameHexLogic()))
					{
						movePresenter.ShowPath(gameHexPresenter.GetGameHexLogic());
						gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.Hoover, 0f, false);
						this.hooverHex = gameHexPresenter;
						return;
					}
					movePresenter.ShowPath(null);
					return;
				}
				else if (movePresenter != null)
				{
					movePresenter.ShowPath(null);
					return;
				}
			}
			else
			{
				this.HooverReset();
				movePresenter.ShowPath(null);
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000841D8 File Offset: 0x000823D8
		private void UniversalHoover(HashSet<GameHexPresenter> avaliableHexes)
		{
			if (this.mouseHitTestNoUI)
			{
				GameHexPresenter gameHexPresenter = this.PointedHex();
				if (gameHexPresenter != this.hooverHex)
				{
					this.HooverReset();
				}
				if (gameHexPresenter != null && avaliableHexes != null && avaliableHexes.Contains(gameHexPresenter))
				{
					gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.Hoover, 0f, false);
					this.hooverHex = gameHexPresenter;
					return;
				}
			}
			else
			{
				this.HooverReset();
			}
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x00084230 File Offset: 0x00082430
		public GameHexPresenter PointedHex()
		{
			GameHexPresenter gameHexPresenter = null;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask(new string[] { "Hex2d" }));
			RaycastHit raycastHit2;
			bool flag2 = Physics.Raycast(ray, out raycastHit2, 100f, LayerMask.GetMask(new string[] { "Units" }));
			if (flag && !flag2)
			{
				GameObject gameObject = raycastHit.transform.gameObject;
				int num = (int)char.GetNumericValue(gameObject.gameObject.name[3]);
				int num2 = (int)char.GetNumericValue(gameObject.gameObject.name[4]);
				gameHexPresenter = GameController.Instance.GetGameHexPresenter(num, num2);
			}
			else if (flag2 && PlatformManager.IsMobile && !Application.isEditor)
			{
				UnitPresenter component = raycastHit2.collider.gameObject.GetComponent<UnitPresenter>();
				if (component != null)
				{
					component.OnMouseDown();
					component.OnMouseUpAsButton();
				}
			}
			return gameHexPresenter;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0003079E File Offset: 0x0002E99E
		private void UpdateLayers()
		{
			if (this.battlefieldLayer != null)
			{
				this.battlefieldLayer.UpdateLayer();
			}
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00084328 File Offset: 0x00082528
		public Vector3 Serialize()
		{
			Vector3 position = this.swivel.transform.position;
			position.y = this.zoom;
			return position;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000307B9 File Offset: 0x0002E9B9
		public void Load(Vector3 v)
		{
			this.zoom = v.y;
			v.y = 0f;
			this.swivel.transform.position = v;
		}

		// Token: 0x040009EE RID: 2542
		public static CameraControler Instance;

		// Token: 0x040009EF RID: 2543
		public bool isRotationUpdate;

		// Token: 0x040009F0 RID: 2544
		public GameObject gameBoard;

		// Token: 0x040009F1 RID: 2545
		public Texture2D handCursor;

		// Token: 0x040009F2 RID: 2546
		public Texture2D grabCursor;

		// Token: 0x040009F3 RID: 2547
		public Texture2D dropCursor;

		// Token: 0x040009F4 RID: 2548
		public bool isGame = true;

		// Token: 0x040009F5 RID: 2549
		public float speed = 1f;

		// Token: 0x040009F6 RID: 2550
		public Canvas[] UI;

		// Token: 0x040009F7 RID: 2551
		public GameObject[] tooltipBlockers;

		// Token: 0x040009F8 RID: 2552
		public RectTransform tooltipContainer;

		// Token: 0x040009F9 RID: 2553
		public RectTransform tooltip;

		// Token: 0x040009FA RID: 2554
		public TextMeshProUGUI tooltipTextBasic;

		// Token: 0x040009FB RID: 2555
		public TextMeshProUGUI tooltipTextAdvanced;

		// Token: 0x040009FC RID: 2556
		public ResourceTypeLayer resourceTypeLayer;

		// Token: 0x040009FD RID: 2557
		public BattlefieldsLayer battlefieldLayer;

		// Token: 0x040009FE RID: 2558
		public Scythe.UI.PlayerStatsPresenter statsPresenter;

		// Token: 0x040009FF RID: 2559
		public Text endTurnHint;

		// Token: 0x04000A00 RID: 2560
		public Image endTurnShade;

		// Token: 0x04000A04 RID: 2564
		private Vector3 mouseDragStart = Vector3.zero;

		// Token: 0x04000A05 RID: 2565
		private float mouseDragOffset = 1f;

		// Token: 0x04000A06 RID: 2566
		public Vector3 mouselastPos = Vector3.zero;

		// Token: 0x04000A07 RID: 2567
		public float mouseLastMoved;

		// Token: 0x04000A08 RID: 2568
		private bool drag;

		// Token: 0x04000A09 RID: 2569
		private bool allZoomTouchesReleased = true;

		// Token: 0x04000A0A RID: 2570
		private float verticalDistanceAccumulator;

		// Token: 0x04000A0B RID: 2571
		public float stickMinZoom;

		// Token: 0x04000A0C RID: 2572
		public float stickMaxZoom;

		// Token: 0x04000A0D RID: 2573
		public float swivelMinZoom;

		// Token: 0x04000A0E RID: 2574
		public float swivelMaxZoom;

		// Token: 0x04000A0F RID: 2575
		public float posXMin = -15f;

		// Token: 0x04000A10 RID: 2576
		public float posXMax = 15f;

		// Token: 0x04000A11 RID: 2577
		public float posYMin = -15f;

		// Token: 0x04000A12 RID: 2578
		public float posYMax = 15f;

		// Token: 0x04000A13 RID: 2579
		public float dofDistanceCorrection;

		// Token: 0x04000A14 RID: 2580
		public float focalSizeClose = 2f;

		// Token: 0x04000A15 RID: 2581
		public float focalSizeFar = 0.05f;

		// Token: 0x04000A16 RID: 2582
		public AnimationCurve fogDensityChangeCurve;

		// Token: 0x04000A17 RID: 2583
		public AnimationCurve fogStartDistanceChangeCurve;

		// Token: 0x04000A18 RID: 2584
		public Color fogColorClose;

		// Token: 0x04000A19 RID: 2585
		public Color fogColorFar;

		// Token: 0x04000A1A RID: 2586
		public AnimationCurve fogColorChangeCurve;

		// Token: 0x04000A1B RID: 2587
		public bool zoomBlocked;

		// Token: 0x04000A1C RID: 2588
		public float zoom = 0.5f;

		// Token: 0x04000A1D RID: 2589
		public float angle;

		// Token: 0x04000A1E RID: 2590
		public bool rotationKeyUsed;

		// Token: 0x04000A1F RID: 2591
		public Transform swivel;

		// Token: 0x04000A20 RID: 2592
		public Transform stick;

		// Token: 0x04000A21 RID: 2593
		private GlobalFogMask globalFogMask;

		// Token: 0x04000A22 RID: 2594
		private GameHexPresenter hooverHex;

		// Token: 0x04000A23 RID: 2595
		private bool mouseLastDownOnMap;

		// Token: 0x04000A24 RID: 2596
		private bool mouseHitTestNoUI;

		// Token: 0x04000A25 RID: 2597
		private CameraControler.CursorType cursorLast;

		// Token: 0x04000A26 RID: 2598
		private CameraControler.CursorType cursor;

		// Token: 0x04000A27 RID: 2599
		[SerializeField]
		private float cameraToTerrainDistance;

		// Token: 0x04000A28 RID: 2600
		public float startCameraToTerrainDistance;

		// Token: 0x04000A29 RID: 2601
		public float moveCameraSmooth = 0.01f;

		// Token: 0x04000A2A RID: 2602
		[SerializeField]
		private bool edgeScrolling;

		// Token: 0x04000A2B RID: 2603
		[SerializeField]
		private float screenEdgeSize = 0.1f;

		// Token: 0x04000A2C RID: 2604
		[SerializeField]
		private float movementOnScreenEdge = 0.8f;

		// Token: 0x04000A2D RID: 2605
		public LayerMask mapLayerMask;

		// Token: 0x04000A2E RID: 2606
		private Vector3 cameraCorrectionVector;

		// Token: 0x04000A2F RID: 2607
		private Vector3 firstCorrectionRaycast;

		// Token: 0x04000A30 RID: 2608
		private Vector3 secondCorrectionRaycast;

		// Token: 0x04000A31 RID: 2609
		public float scrollSpeedMultipler = 16f;

		// Token: 0x04000A32 RID: 2610
		public bool zoomOutToCenter;

		// Token: 0x04000A33 RID: 2611
		public bool zoomInToMouse;

		// Token: 0x04000A34 RID: 2612
		public float rotationSpeed;

		// Token: 0x04000A35 RID: 2613
		public float rotationSpeedKeyboard;

		// Token: 0x04000A36 RID: 2614
		private Vector3 mouseReference;

		// Token: 0x04000A37 RID: 2615
		private Vector3 mouseOffset;

		// Token: 0x04000A38 RID: 2616
		private Vector3 rotation;

		// Token: 0x04000A39 RID: 2617
		private Vector3 initialCamRotation;

		// Token: 0x04000A3A RID: 2618
		public float rotationSnapStart;

		// Token: 0x04000A3B RID: 2619
		public AnimationCurve rotationAnimCurve;

		// Token: 0x04000A3C RID: 2620
		public float rotationAnimDuration;

		// Token: 0x04000A3D RID: 2621
		public float fixedRotationPos;

		// Token: 0x04000A3E RID: 2622
		public GameObject fieldTemplate;

		// Token: 0x04000A3F RID: 2623
		public bool objectsNeedRotation;

		// Token: 0x04000A40 RID: 2624
		public Vector3 RotationAdjustment;

		// Token: 0x04000A41 RID: 2625
		public Vector3 encounterRotationAdjustment;

		// Token: 0x04000A42 RID: 2626
		private int lastFingerUsedForDrag;

		// Token: 0x04000A43 RID: 2627
		public GameObject trumpTrack;

		// Token: 0x04000A44 RID: 2628
		public GameObject powerTrack;

		// Token: 0x04000A45 RID: 2629
		public GameObject popTrack;

		// Token: 0x04000A46 RID: 2630
		public Transform factionLogos;

		// Token: 0x04000A47 RID: 2631
		private Rect screenRect;

		// Token: 0x04000A48 RID: 2632
		public Vector2 swipeStartPosition;

		// Token: 0x04000A49 RID: 2633
		public float swipeStartTime;

		// Token: 0x04000A4A RID: 2634
		public int swipeMinDistanceTreshold;

		// Token: 0x04000A4B RID: 2635
		public int swipeAngleTreshold;

		// Token: 0x04000A4C RID: 2636
		public int swipeSpeedDetectionTreshold;

		// Token: 0x04000A4D RID: 2637
		public List<DropdownChecker> dropdowns = new List<DropdownChecker>();

		// Token: 0x020001B8 RID: 440
		private enum CursorType
		{
			// Token: 0x04000A4F RID: 2639
			None,
			// Token: 0x04000A50 RID: 2640
			Finger,
			// Token: 0x04000A51 RID: 2641
			Grab,
			// Token: 0x04000A52 RID: 2642
			Load
		}
	}
}
