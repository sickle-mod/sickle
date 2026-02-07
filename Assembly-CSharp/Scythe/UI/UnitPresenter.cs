using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using DG.Tweening;
using HoneyFramework;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x0200041F RID: 1055
	public class UnitPresenter : MonoBehaviour, IDraggableObject, ISeismograph
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06002039 RID: 8249 RVA: 0x0003CAB1 File Offset: 0x0003ACB1
		// (set) Token: 0x0600203A RID: 8250 RVA: 0x0003CAB9 File Offset: 0x0003ACB9
		public Unit UnitLogic { get; private set; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x0600203B RID: 8251 RVA: 0x0003CAC2 File Offset: 0x0003ACC2
		// (set) Token: 0x0600203C RID: 8252 RVA: 0x0003CACA File Offset: 0x0003ACCA
		public Scythe.BoardPresenter.GameHexPresenter hex { get; private set; }

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x0600203D RID: 8253 RVA: 0x000C4214 File Offset: 0x000C2414
		// (remove) Token: 0x0600203E RID: 8254 RVA: 0x000C4248 File Offset: 0x000C2448
		public static event UnitPresenter.UnitStatus UnitStatusChanged;

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x0600203F RID: 8255 RVA: 0x000C427C File Offset: 0x000C247C
		// (remove) Token: 0x06002040 RID: 8256 RVA: 0x000C42B0 File Offset: 0x000C24B0
		public static event UnitPresenter.UnitLoaded UnitGetLoaded;

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06002041 RID: 8257 RVA: 0x000C42E4 File Offset: 0x000C24E4
		// (remove) Token: 0x06002042 RID: 8258 RVA: 0x000C4318 File Offset: 0x000C2518
		public static event UnitPresenter.UnitMoveStart MoveStart;

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06002043 RID: 8259 RVA: 0x000C434C File Offset: 0x000C254C
		// (remove) Token: 0x06002044 RID: 8260 RVA: 0x000C4380 File Offset: 0x000C2580
		public static event UnitPresenter.UnitMoveStop MoveStop;

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06002045 RID: 8261 RVA: 0x000C43B4 File Offset: 0x000C25B4
		// (remove) Token: 0x06002046 RID: 8262 RVA: 0x000C43E8 File Offset: 0x000C25E8
		public static event UnitPresenter.UnitDragStart UnitDragged;

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06002047 RID: 8263 RVA: 0x000C441C File Offset: 0x000C261C
		// (remove) Token: 0x06002048 RID: 8264 RVA: 0x000C4450 File Offset: 0x000C2650
		public static event UnitPresenter.UnitDragEnd UnitDroped;

		// Token: 0x06002049 RID: 8265 RVA: 0x000C4484 File Offset: 0x000C2684
		private void OnEnable()
		{
			if (GameController.Instance != null && !GameController.Instance.GameIsLoaded && (this.UnitLogic is Mech || (this.UnitLogic is Worker && !(this.UnitLogic as Worker).OnMech)) && ((!GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent.IsHuman) || (GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner == GameController.GameManager.PlayerCurrent) || !GameController.GameManager.showEnemyActions) && (double)base.gameObject.transform.position.y > 0.2)
			{
				this.UnitLogic.spawnAnimation = true;
				this.UnitSpawnAnimation(false, false);
			}
			if (PlatformManager.IsStandalone)
			{
				this.fresnelPropertyBlock = new MaterialPropertyBlock();
				this.highlightPropertyBlock = new MaterialPropertyBlock();
				if (this.UnitLogic is Worker)
				{
					this.GroundHighlightShaderPropertyOnStart(0.8f);
				}
				this.unitRenderer.Add(this.unitMeshRenderer.gameObject.GetComponent<Renderer>());
				if (this.secondaryMeshRenderer != null)
				{
					this.unitRenderer.Add(this.secondaryMeshRenderer.gameObject.GetComponent<Renderer>());
				}
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000C45DC File Offset: 0x000C27DC
		private void Start()
		{
			this.movePresenter = (MovePresenter)HumanInputHandler.Instance.movePresenter;
			if (base.GetComponentInChildren<Animator>() != null)
			{
				base.GetComponentInChildren<Animator>().enabled = false;
			}
			this.unitCollider = base.GetComponent<Collider>();
			this.lastPosition = base.transform.position;
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x0003CAD3 File Offset: 0x0003ACD3
		public void SetLogicUnit(Unit unit)
		{
			this.UnitLogic = unit;
			this.UpdateMass();
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000C4638 File Offset: 0x000C2838
		public void ReadRotationFromLogicUnit()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			eulerAngles.y = this.UnitLogic.GetRotation();
			base.transform.eulerAngles = eulerAngles;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000C4670 File Offset: 0x000C2870
		private void UpdateMass()
		{
			if (this.UnitLogic == null)
			{
				return;
			}
			switch (this.UnitLogic.UnitType)
			{
			case UnitType.Character:
				this.mass = 0.5f;
				return;
			case UnitType.Mech:
				this.mass = 0.6f;
				return;
			case UnitType.Worker:
				this.mass = 0.2f;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000C46C8 File Offset: 0x000C28C8
		public void OnMouseUpAsButton()
		{
			if (!GameController.Instance.darkenUI.enabled && !CameraControler.BlockEverything && CameraControler.Instance.MouseHitTestUI())
			{
				if (Input.GetKey(KeyCode.LeftControl))
				{
					if (UnitPresenter.UnitGetLoaded != null)
					{
						UnitPresenter.UnitGetLoaded(this);
						return;
					}
				}
				else if (!GameController.Instance.DragAndDrop)
				{
					GameController.SetFocusUnit(this);
					if (!GameController.GameManager.PlayerCurrent.ActionInProgress)
					{
						this.ShowDistanceRange();
					}
				}
			}
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000C4744 File Offset: 0x000C2944
		public void OnMouseDown()
		{
			if (!GameController.Instance.darkenUI.enabled && !CameraControler.BlockEverything && CameraControler.Instance.MouseHitTestUI() && GameController.Instance.DragAndDrop)
			{
				GameController.SetFocusUnit(this);
				switch (this.UnitLogic.UnitType)
				{
				case UnitType.Character:
					WorldSFXManager.PlaySound(SoundEnum.ClickMainCharracter, AudioSourceType.WorldSfx);
					break;
				case UnitType.Mech:
					WorldSFXManager.PlaySound(SoundEnum.ClickMech, AudioSourceType.WorldSfx);
					break;
				case UnitType.Worker:
					WorldSFXManager.PlaySound(SoundEnum.ClickWorker, AudioSourceType.WorldSfx);
					break;
				}
				if (!GameController.GameManager.PlayerCurrent.ActionInProgress)
				{
					this.ShowDistanceRange();
				}
			}
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x0003CAE2 File Offset: 0x0003ACE2
		public void SetFocus(bool hasFocus, int color = 0)
		{
			if (this.outlineComponents != null)
			{
				this.OutlineActivation(hasFocus, color, false);
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x0003CAF5 File Offset: 0x0003ACF5
		public void SetHex(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			this.hex = hex;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000C47DC File Offset: 0x000C29DC
		private void AddOutlineRecursive(GameObject obj)
		{
			foreach (object obj2 in obj.transform)
			{
				Transform transform = (Transform)obj2;
				if (!(null == transform) && (transform.gameObject.GetComponent<MeshRenderer>() != null || transform.gameObject.GetComponent<SkinnedMeshRenderer>() != null) && transform.gameObject.GetComponent<Outline>() != null)
				{
					transform.gameObject.GetComponent<Outline>().enabled = false;
				}
			}
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000C4884 File Offset: 0x000C2A84
		public void OutlineActivation(bool on, int color = 0, bool force = false)
		{
			if (!GameController.GameManager.PlayerCurrent.IsHuman || GameController.GameManager.PlayerCurrent.ActionInProgress)
			{
				this.SetOutlineEnabled(on, color);
				return;
			}
			if (on)
			{
				this.SetOutlineEnabled(on, color);
				return;
			}
			if (force)
			{
				this.SetOutlineEnabled(false, color);
				return;
			}
			this.outlineCoroutine = base.StartCoroutine(this.OffOutlineComponents(false, color));
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000C48E8 File Offset: 0x000C2AE8
		private void SetOutlineEnabled(bool enabled, int color)
		{
			if (PlatformManager.IsMobile)
			{
				for (int i = 0; i < this.outlineComponents.Count; i++)
				{
					this.outlineComponents[i].enabled = enabled;
					switch (color)
					{
					case 0:
						this.outlineComponents[i].materials[0].SetColor("_TintColor", Color.green);
						break;
					case 1:
						this.outlineComponents[i].materials[0].SetColor("_TintColor", Color.red);
						break;
					case 2:
						this.outlineComponents[i].materials[0].SetColor("_TintColor", Color.blue);
						break;
					}
				}
				return;
			}
			foreach (Renderer renderer in this.unitRenderer)
			{
				if (this.fresnelPropertyBlock != null)
				{
					renderer.GetPropertyBlock(this.fresnelPropertyBlock);
				}
				if (enabled)
				{
					this.fresnelPropertyBlock.SetFloat("_FresnelIntensivity", 11f);
					this.fresnelPropertyBlock.SetColor("_FresnelColor", this.fresnelColors[color]);
				}
				else
				{
					this.fresnelPropertyBlock.SetFloat("_FresnelIntensivity", 0f);
				}
				renderer.SetPropertyBlock(this.fresnelPropertyBlock);
			}
			for (int j = 0; j < this.outlineComponents.Count; j++)
			{
				this.outlineComponents[j].enabled = enabled;
				if (this.highlightPropertyBlock != null)
				{
					this.outlineComponents[j].GetPropertyBlock(this.highlightPropertyBlock);
				}
				this.highlightPropertyBlock.SetColor("_Color", this.fresnelColors[color]);
				this.outlineComponents[j].SetPropertyBlock(this.highlightPropertyBlock);
			}
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x0003CAFE File Offset: 0x0003ACFE
		private IEnumerator OffOutlineComponents(bool on, int color = 0)
		{
			yield return new WaitForSeconds(GameController.Instance.rangeVisibilityTime);
			this.SetOutlineEnabled(false, 0);
			yield break;
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000C4AD0 File Offset: 0x000C2CD0
		private void ForceOutlineOff()
		{
			if (PlatformManager.IsMobile)
			{
				if (this.outlineCoroutine != null)
				{
					base.StopCoroutine(this.outlineCoroutine);
					this.outlineCoroutine = null;
				}
				Camera.main.GetComponent<OutlineEffect>().FadeOutOutline(false);
				this.SetOutlineEnabled(false, 0);
				return;
			}
			foreach (Renderer renderer in this.unitRenderer)
			{
				renderer.GetPropertyBlock(this.fresnelPropertyBlock);
				this.fresnelPropertyBlock.SetFloat("_FresnelIntensivity", 0f);
				renderer.SetPropertyBlock(this.fresnelPropertyBlock);
			}
			for (int i = 0; i < this.outlineComponents.Count; i++)
			{
				if (this.outlineComponents[i] != null)
				{
					this.outlineComponents[i].enabled = false;
				}
			}
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000C4BC0 File Offset: 0x000C2DC0
		private void GroundHighlightShaderPropertyOnStart(float inOffsetValue)
		{
			for (int i = 0; i < this.outlineComponents.Count; i++)
			{
				this.outlineComponents[i].GetPropertyBlock(this.highlightPropertyBlock);
				this.highlightPropertyBlock.SetFloat("_In_Offset", inOffsetValue);
				this.outlineComponents[i].SetPropertyBlock(this.highlightPropertyBlock);
			}
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000C4C24 File Offset: 0x000C2E24
		private void ShowDistanceRange()
		{
			this.StopFadeMovesHighlightCoroutine();
			this.movePresenter.ShowPossibleMoves(this.UnitLogic, false, false);
			Camera.main.GetComponent<OutlineEffect>().clickedOutlineActive = true;
			Camera.main.GetComponent<OutlineEffect>().lineColor0.a = 1f;
			if (this.outlineComponents != null)
			{
				this.OutlineActivation(true, 0, false);
			}
			if (GameController.GameManager.moveManager.GetActualAction() == null)
			{
				this.StartFadeMovesHighlight();
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x0003CB0D File Offset: 0x0003AD0D
		public void StartFadeMovesHighlight()
		{
			this.lastFadeHighlightMoveCoroutine = base.StartCoroutine(this.FadeMovesHighlight());
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x0003CB21 File Offset: 0x0003AD21
		private IEnumerator FadeMovesHighlight()
		{
			Camera.main.GetComponent<OutlineEffect>().FadeOutOutline(true);
			yield return new WaitForSeconds(GameController.Instance.rangeVisibilityTime);
			this.movePresenter.HideMoveHexesHighlight(this.UnitLogic, false);
			this.ForceOutlineOff();
			yield break;
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0003CB30 File Offset: 0x0003AD30
		private void StopFadeMovesHighlightCoroutine()
		{
			if (this.lastFadeHighlightMoveCoroutine != null)
			{
				base.StopCoroutine(this.lastFadeHighlightMoveCoroutine);
			}
			this.lastFadeHighlightMoveCoroutine = null;
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x0003CB4D File Offset: 0x0003AD4D
		public void ForceToHideMovesHighlight()
		{
			this.StopFadeMovesHighlightCoroutine();
			this.ForceOutlineOff();
			this.movePresenter.HideMoveHexesHighlight(this.UnitLogic, true);
			Camera.main.GetComponent<OutlineEffect>().StopHideHighlightCoroutine();
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x0003CB7C File Offset: 0x0003AD7C
		public void SetColliderEnabled(bool enabled)
		{
			if (this.unitCollider == null)
			{
				this.unitCollider = base.GetComponent<Collider>();
			}
			if (this.unitCollider != null)
			{
				this.unitCollider.enabled = enabled;
			}
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x0003CBB2 File Offset: 0x0003ADB2
		public void RunTheMoveAnimation(Dictionary<GameHex, GameHex> rawPathData, bool isEnemyAction = false, bool isRetreat = false, bool isBotBattlefieldMove = false, GameHex start = null, GameHex end = null)
		{
			this.ForceFinishMoveAnimation();
			this.UpdateUnitState(UnitState.PreparingToMove);
			this.CalculatePath(rawPathData, start, end, isBotBattlefieldMove);
			this.SetNextMovePosition();
			this.UpdateUnitState(UnitState.Moving);
			this.Move(isEnemyAction, isRetreat, false);
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000C4C9C File Offset: 0x000C2E9C
		public void RunMoveToOwnerPosition(GameHex position, bool isEnemyRunToOwner = false)
		{
			this.moveToPosition = GameController.Instance.GetGameHexPresenter(position).GetUnitPosition(this.UnitLogic);
			this.actualDestination = this.UnitLogic.position;
			this.UnitLogic.IncreaseMoveAnimationAmount();
			this.UpdateUnitState(UnitState.Moving);
			this.Move(isEnemyRunToOwner, false, true);
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000C4CF4 File Offset: 0x000C2EF4
		private void CalculatePath(Dictionary<GameHex, GameHex> movesRegister, GameHex start, GameHex end, bool isEnemyBattleMove = false)
		{
			this.actualDestination = this.hex.GetGameHexLogic();
			this.path = new List<GameHex>();
			this.positionPath = new List<Vector3>();
			GameHex gameHex = end ?? this.UnitLogic.position;
			this.path.Add(gameHex);
			while (gameHex != null)
			{
				if (movesRegister.ContainsKey(gameHex))
				{
					gameHex = movesRegister[gameHex];
					if (gameHex != null && this.actualDestination != gameHex)
					{
						this.path.Add(gameHex);
					}
				}
			}
			this.path.Reverse();
			this.actualDestination = this.path[0];
			for (int i = 0; i < this.path.Count; i++)
			{
				this.positionPath.Add(isEnemyBattleMove ? this.GetEnemyMovePosition(this.path[i]) : this.GetNormalMovePosition(this.path[i]));
			}
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000C4DDC File Offset: 0x000C2FDC
		private void SetNextMovePosition()
		{
			if (this.positionPathIterator < this.positionPath.Count)
			{
				this.moveToPosition = this.positionPath[this.positionPathIterator];
				this.actualDestination = this.path[this.positionPathIterator];
			}
			this.positionPathIterator++;
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0003CBE4 File Offset: 0x0003ADE4
		private void UpdateUnitState(UnitState state)
		{
			this.actualState = state;
			if (UnitPresenter.UnitStatusChanged != null)
			{
				UnitPresenter.UnitStatusChanged(this.actualState, this);
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x0003CC05 File Offset: 0x0003AE05
		private void Move(bool isEnemyAction = false, bool isRetreat = false, bool takeOwnerPosition = false)
		{
			this.MoveOnceAnimation(base.transform.position, this.moveToPosition, isEnemyAction, isRetreat, takeOwnerPosition);
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000C4E38 File Offset: 0x000C3038
		private void MoveOnceAnimation(Vector3 from, Vector3 to, bool isEnemyAction, bool isRetreat, bool takeOwnerPosition)
		{
			if (this.actualState == UnitState.MovingFromMech)
			{
				from = new Vector3(from.x, -0.06f, from.z);
			}
			if (this.UnitLogic.UnitType != UnitType.Worker)
			{
				Vector3 vector = to - from;
				vector.y = 0f;
				vector = vector.normalized;
				float num = Vector3.SignedAngle(-base.transform.right, vector, Vector3.up);
				if (!float.IsNaN(num))
				{
					base.transform.Rotate(new Vector3(0f, num, 0f));
					this.UnitLogic.SetRotation(base.transform.eulerAngles.y);
				}
			}
			float num2 = Vector3.Distance(to, from) / GameController.Instance.unitsMoveSpeed;
			float num3 = to.x - from.x;
			float num4 = to.z - from.z;
			UnitState stateBeforeAnimation = this.actualState;
			float num5 = this.Map(num4, 1f, -1f, -GameController.Instance.maxRotation, GameController.Instance.maxRotation);
			float num6 = this.Map(num3, -1f, 1f, -GameController.Instance.maxRotation, GameController.Instance.maxRotation);
			if (this.moveSequence != null && this.moveSequence.IsPlaying())
			{
				this.moveSequence.Complete(true);
			}
			this.moveSequence = DOTween.Sequence();
			this.moveSequence.Append(base.transform.DOMoveX(to.x, num2, false).SetEase(GameController.Instance.unitsHorizontalEase).SetId("Tween_LastUnit_MoveX"));
			this.moveSequence.Join(base.transform.DOMoveZ(to.z, num2, false).SetEase(GameController.Instance.unitsHorizontalEase).SetId("Tween_LastUnit_MoveZ"));
			this.moveSequence.Join(base.transform.DOMoveY(from.y + GameController.Instance.jumpHeight, num2, false).SetEase(GameController.Instance.unitsVerticalEase).SetId("Tween_LastUnit_MoveY"));
			this.moveSequence.Join(base.transform.DORotate(new Vector3(num5, 0f, num6), num2, RotateMode.WorldAxisAdd).SetEase(GameController.Instance.unitsRotationEase).SetId("Tween_LastUnit_Rotate"));
			if (UnitPresenter.MoveStart != null)
			{
				UnitPresenter.MoveStart(this, this.actualState);
			}
			this.moveSequence.Play<Sequence>().OnComplete(delegate
			{
				this.ReportFinishingAnimation(isEnemyAction, isRetreat, stateBeforeAnimation, takeOwnerPosition);
			});
			if (PlatformManager.IsMobile && this.skipMove)
			{
				this.moveSequence.Complete();
				this.skipMove = false;
			}
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x00031315 File Offset: 0x0002F515
		private float Map(float value, float sourceMin, float sourceMax, float destinMin, float destinMax)
		{
			return (value - sourceMin) / (sourceMax - sourceMin) * (destinMax - destinMin) + destinMin;
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x000C5114 File Offset: 0x000C3314
		private void ReportFinishingAnimation(bool isEnemyAction = false, bool isRetreat = false, UnitState stateBeforeAnimation = UnitState.Moving, bool takeOwnerPosition = false)
		{
			this.PlayMoveSound();
			Scythe.BoardPresenter.GameHexPresenter hex = this.hex;
			if (this.actualDestination != null)
			{
				this.SetHex(GameController.Instance.GetGameHexPresenter(this.actualDestination));
			}
			hex.UpdateOwnership();
			this.hex.UpdateOwnership();
			this.hex.UpdateTokenState(this.UnitLogic);
			if (stateBeforeAnimation == UnitState.MovingToMech)
			{
				this.SetActive(false);
			}
			this.SetNextMovePosition();
			this.MoveAnimationInterpreter(isRetreat, false);
			this.hex.UpdateTokenState(this.UnitLogic);
			if (UnitPresenter.MoveStop != null)
			{
				UnitPresenter.MoveStop(this, takeOwnerPosition);
			}
			UnitType unitType = this.UnitLogic.UnitType;
			if (isEnemyAction)
			{
				this.OnEnemyAnimationFinished(stateBeforeAnimation, takeOwnerPosition);
			}
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x000C51C4 File Offset: 0x000C33C4
		private void OnEnemyAnimationFinished(UnitState stateBeforeAnimation, bool takeOwnerPosition)
		{
			if (stateBeforeAnimation == UnitState.MovingFromMech)
			{
				ShowEnemyMoves.Instance.WorkerUnloadedReport();
			}
			else if (stateBeforeAnimation == UnitState.MovingToMech)
			{
				ShowEnemyMoves.Instance.WorkerLoadedReport();
			}
			else
			{
				ShowEnemyMoves.Instance.MoveEndedReport(takeOwnerPosition);
				this.hex.UpdateOwnership();
			}
			if (this.UnitLogic.UnitType == UnitType.Character && this.UnitLogic.position.hasEncounter && !GameController.GameManager.IsMultiplayer)
			{
				GameController.Instance.GetGameHexPresenter(this.UnitLogic.position).ActivateEncounterEndAnimation();
			}
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000C524C File Offset: 0x000C344C
		private void PlayMoveSound()
		{
			switch (this.UnitLogic.UnitType)
			{
			case UnitType.Character:
				WorldSFXManager.PlaySound(SoundEnum.MovePlayer, AudioSourceType.WorldSfx);
				return;
			case UnitType.Mech:
				WorldSFXManager.PlaySound(SoundEnum.MechMove, AudioSourceType.WorldSfx);
				break;
			case UnitType.Worker:
				if (this.actualState != UnitState.MovingFromMech && this.actualState != UnitState.MovingToMech)
				{
					WorldSFXManager.PlaySound(SoundEnum.MoveWorker, AudioSourceType.WorldSfx);
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x000C52A4 File Offset: 0x000C34A4
		private void MoveAnimationInterpreter(bool isRetreat, bool immidiate = false)
		{
			if (this.positionPathIterator > this.positionPath.Count)
			{
				this.moveSequence = null;
				this.positionPathIterator = 0;
				this.positionPath.Clear();
				this.path.Clear();
				if (this.UnitIsMech() && ((this.UnitLogic.MovesLeft == 0 && this.UnitLogic.Owner == GameController.GameManager.PlayerMaster) || isRetreat))
				{
					this.UnloadAllWorkers(immidiate);
					this.movePresenter.UpdateWorkerButtons(this.UnitLogic);
				}
				if (PlatformManager.IsMobile)
				{
					this.skipMove = false;
				}
				this.UnitLogic.DecreaseMoveAnimationAmount();
				this.UpdateUnitState(UnitState.Standing);
			}
			if (this.actualState == UnitState.Moving)
			{
				this.Move(false, false, false);
			}
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x000C5368 File Offset: 0x000C3568
		private Vector3 GetNormalMovePosition(GameHex newDestination)
		{
			Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(newDestination);
			Vector3 vector = default(Vector3);
			if (newDestination != this.UnitLogic.position)
			{
				vector = HexCoordinates.HexToWorld3D(gameHexPresenter.position);
			}
			else if (this.actualState != UnitState.MovingFromMech || this.actualState != UnitState.MovingToMech)
			{
				vector = gameHexPresenter.GetUnitPosition(this.UnitLogic);
			}
			return vector;
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x000C53CC File Offset: 0x000C35CC
		private Vector3 GetEnemyMovePosition(GameHex newDestination)
		{
			Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(newDestination);
			Vector3 vector = default(Vector3);
			if (newDestination != this.UnitLogic.position)
			{
				vector = HexCoordinates.HexToWorld3D(gameHexPresenter.position);
			}
			else
			{
				vector = gameHexPresenter.GetEnemyUnitPosition(this.UnitLogic);
			}
			return vector;
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x000C541C File Offset: 0x000C361C
		public void LoadToMechMove(Vector3 destinationPoint, bool isEnemyAction = false)
		{
			this.ForceFinishMoveAnimation();
			this.positionPathIterator = 0;
			this.positionPath = new List<Vector3>();
			this.positionPath.Add(destinationPoint);
			this.moveToPosition = destinationPoint;
			this.path = new List<GameHex>();
			this.path.Add(this.UnitLogic.position);
			this.actualDestination = this.UnitLogic.position;
			this.SetNextMovePosition();
			this.UpdateUnitState(UnitState.MovingToMech);
			this.Move(isEnemyAction, false, false);
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000C549C File Offset: 0x000C369C
		public void UnloadFromMechMove(Vector3 destinationPoint, bool isEnemyAction = false)
		{
			this.ForceFinishMoveAnimation();
			this.positionPathIterator = 0;
			this.SetActive(true);
			this.positionPath = new List<Vector3>();
			this.positionPath.Add(destinationPoint);
			this.moveToPosition = destinationPoint;
			this.path = new List<GameHex>();
			this.path.Add(this.UnitLogic.position);
			this.actualDestination = this.UnitLogic.position;
			this.SetNextMovePosition();
			this.UpdateUnitState(UnitState.MovingFromMech);
			this.Move(isEnemyAction, false, false);
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x000315A8 File Offset: 0x0002F7A8
		public void ResetWorkerPositionAfterUnload(Vector3 mechPosition)
		{
			base.transform.position = mechPosition;
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x0003CC21 File Offset: 0x0003AE21
		public void ForceFinishMoveAnimation()
		{
			if (this.moveSequence != null)
			{
				if (PlatformManager.IsMobile)
				{
					this.skipMove = true;
				}
				this.moveSequence.Complete(true);
			}
			this.moveSequence = null;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x0003CC4C File Offset: 0x0003AE4C
		public bool UnitIsMech()
		{
			return this.UnitLogic.UnitType == UnitType.Mech;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x0003CC5C File Offset: 0x0003AE5C
		public bool CanLoadUnit(Unit worker)
		{
			return worker != null && (this.UnitIsMech() || worker.UnitType == UnitType.Worker);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x0003CC76 File Offset: 0x0003AE76
		public Unit GetWorker(int workerListId)
		{
			if (!this.UnitIsMech())
			{
				return null;
			}
			if (this.loadedUnits.Count > 0 && this.loadedUnits.Count > workerListId)
			{
				return this.loadedUnits[workerListId];
			}
			return null;
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000C5524 File Offset: 0x000C3724
		public Unit GetWorkerByUnitId(int workerId)
		{
			if (!this.UnitIsMech())
			{
				return null;
			}
			Unit unit = null;
			for (int i = 0; i < this.loadedUnits.Count; i++)
			{
				if (this.loadedUnits[i].Id == workerId)
				{
					unit = this.loadedUnits[i];
					break;
				}
			}
			return unit;
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0003CCAC File Offset: 0x0003AEAC
		public List<Unit> GetWorkersList()
		{
			return this.loadedUnits;
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x0003CCB4 File Offset: 0x0003AEB4
		public void AddWorker(Unit worker)
		{
			if (!this.CanLoadUnit(worker))
			{
				return;
			}
			if (!this.loadedUnits.Contains(worker))
			{
				this.loadedUnits.Add(worker);
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000C5578 File Offset: 0x000C3778
		public void AddWorkers(List<Unit> workers)
		{
			foreach (Unit unit in workers)
			{
				this.AddWorker(unit);
			}
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x0003CCDA File Offset: 0x0003AEDA
		public void RemoveWorker(Unit worker)
		{
			if (this.loadedUnits.Contains(worker))
			{
				this.loadedUnits.Remove(worker);
			}
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000C55C8 File Offset: 0x000C37C8
		public Unit PopWorker(int workerListId)
		{
			Unit worker = this.GetWorker(workerListId);
			this.RemoveWorker(worker);
			return worker;
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000C55E8 File Offset: 0x000C37E8
		public Unit PopWorkerByUnitID(int workerId)
		{
			Unit workerByUnitId = this.GetWorkerByUnitId(workerId);
			this.RemoveWorker(workerByUnitId);
			return workerByUnitId;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0003CCF7 File Offset: 0x0003AEF7
		public void LoadWorker(Unit worker, bool isEnemyAction = false)
		{
			this.AddWorker(worker);
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(worker);
			this.ForceFinishMoveAnimation();
			unitPresenter.LoadToMechMove(this.hex.GetUnitPosition(this.UnitLogic), isEnemyAction);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000C5608 File Offset: 0x000C3808
		public UnitPresenter UnloadWorker(int workerListId, bool isEnemyAction = false)
		{
			Unit unit = this.PopWorker(workerListId);
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
			if (unit == null)
			{
				return null;
			}
			this.UnloadWorkerProcedure(unitPresenter, isEnemyAction);
			return unitPresenter;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000C5630 File Offset: 0x000C3830
		public UnitPresenter UnloadWorkerByUnitId(int workerId, bool isEnemyAction = false)
		{
			Unit unit = this.PopWorkerByUnitID(workerId);
			UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
			if (unit == null)
			{
				return null;
			}
			this.UnloadWorkerProcedure(unitPresenter, isEnemyAction);
			return unitPresenter;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000C5658 File Offset: 0x000C3858
		private void UnloadWorkerProcedure(UnitPresenter workerPresenter, bool isEnemyAction = false)
		{
			if (!isEnemyAction)
			{
				this.movePresenter.UnloadWorker(workerPresenter.UnitLogic);
			}
			if (workerPresenter != null)
			{
				this.ForceFinishMoveAnimation();
				workerPresenter.ForceFinishMoveAnimation();
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.gameBoardPresenter.GetGameHexPresenter(this.UnitLogic.position);
				workerPresenter.ResetWorkerPositionAfterUnload(gameHexPresenter.GetUnitPosition(this.UnitLogic));
				workerPresenter.SetActive(true);
				workerPresenter.UnloadFromMechMove(gameHexPresenter.GetUnitPosition(workerPresenter.UnitLogic), isEnemyAction);
				workerPresenter.SetHex(gameHexPresenter);
				if (!isEnemyAction && (GameController.GameManager.moveManager.GetMovesLeft() > 1 || (GameController.GameManager.moveManager.GetMovesLeft() == 1 && (this.UnitLogic.MovesLeft >= this.UnitLogic.MaxMoveCount || this.UnitLogic.MovesLeft == 0))) && workerPresenter.UnitLogic.MovesLeft > 0)
				{
					workerPresenter.SetFocus(true, 0);
				}
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000C5744 File Offset: 0x000C3944
		public void UnloadAllWorkers(bool immidiate = false)
		{
			if (!this.UnitIsMech())
			{
				return;
			}
			while (this.loadedUnits.Count > 0)
			{
				UnitPresenter unitPresenter = this.UnloadWorker(0, false);
				if (immidiate && unitPresenter != null)
				{
					unitPresenter.ForceFinishMoveAnimation();
				}
			}
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000C5784 File Offset: 0x000C3984
		public void UnloadAllResourcesAnimation()
		{
			if (this.UnitLogic.position != this.hex.GetGameHexLogic())
			{
				return;
			}
			if (this.UnitLogic.resources[ResourceType.food] > 0)
			{
				this.hex.ShowResourceAnimAfterMove(ResourceType.food, base.gameObject.transform.position, this.UnitLogic.resources[ResourceType.food]);
			}
			if (this.UnitLogic.resources[ResourceType.oil] > 0)
			{
				this.hex.ShowResourceAnimAfterMove(ResourceType.oil, base.gameObject.transform.position, this.UnitLogic.resources[ResourceType.oil]);
			}
			if (this.UnitLogic.resources[ResourceType.metal] > 0)
			{
				this.hex.ShowResourceAnimAfterMove(ResourceType.metal, base.gameObject.transform.position, this.UnitLogic.resources[ResourceType.metal]);
			}
			if (this.UnitLogic.resources[ResourceType.wood] > 0)
			{
				this.hex.ShowResourceAnimAfterMove(ResourceType.wood, base.gameObject.transform.position, this.UnitLogic.resources[ResourceType.wood]);
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000C58B0 File Offset: 0x000C3AB0
		public void OnDragBegin(Vector3 pivotPosition, float rodLength, float timeToSnap)
		{
			this.PickUpAnimation(pivotPosition, rodLength, timeToSnap);
			this.SetColliderEnabled(false);
			if (UnitPresenter.UnitDragged != null)
			{
				UnitPresenter.UnitDragged(this);
			}
			(GameController.Instance.gameBoardPresenter as FlatWorld).SetCollidersEnabledOnUnits(false, this.UnitLogic.position, null);
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0003CD23 File Offset: 0x0003AF23
		public void OnDragEnd(Vector3 position, float timeToLand, bool loadingToUnit)
		{
			(GameController.Instance.gameBoardPresenter as FlatWorld).SetCollidersEnabledOnUnits(true, null, this.UnitLogic);
			this.PutDownAnimation(position, timeToLand, loadingToUnit);
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void PickUpAnimation(Vector3 pivotPosition, float rodLength, float timeToSnap)
		{
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x000C5900 File Offset: 0x000C3B00
		public void PutDownAnimation(Vector3 position, float timeToLand, bool loadingToUnit)
		{
			this.moveSequence = DOTween.Sequence();
			this.heightOnDrop = base.transform.position.y;
			Vector3 unitPosition = this.hex.GetUnitPosition(this.UnitLogic);
			Vector3 vector = ((this.UnitLogic.UnitType == UnitType.Worker) ? Vector3.zero : new Vector3(0f, Mathf.Atan2(position.x - unitPosition.x, position.z - unitPosition.z) * 57.29578f + 90f, 0f));
			this.UnitLogic.SetRotation(vector.y);
			this.moveSequence.Append(base.transform.DOMove(position, timeToLand, false).SetEase(Ease.InExpo));
			this.moveSequence.Join(base.transform.DORotate(vector, timeToLand, RotateMode.Fast).SetEase(Ease.InExpo));
			this.moveSequence.OnComplete(delegate
			{
				this.OnPutDownAnimationComplete(loadingToUnit);
			});
			this.moveSequence.Play<Sequence>();
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x000C5A1C File Offset: 0x000C3C1C
		private void OnPutDownAnimationComplete(bool loadingToUnit)
		{
			this.moveSequence = null;
			this.PlayMoveSound();
			this.UpdateUnitState(UnitState.Standing);
			this.SetColliderEnabled(true);
			this.SetHex(GameController.Instance.GetGameHexPresenter(this.UnitLogic.position));
			this.actualDestination = null;
			if (this.movePresenter.exchangePanel.DragAndDropBar.PreviousUnitEqualsUnit(this.UnitLogic) && this.movePresenter.exchangePanel.DragAndDropBar.AnythingLoaded() && this.UnitLogic.MovesLeft != 0)
			{
				this.movePresenter.exchangePanel.DragAndDropBar.SetActive(true, true);
				if (this.UnitIsMech())
				{
					this.movePresenter.UpdateWorkerButtons(this.UnitLogic);
				}
			}
			this.hex.UpdateTokenState(this.UnitLogic);
			if (UnitPresenter.UnitDroped != null)
			{
				UnitPresenter.UnitDroped(this);
			}
			if (loadingToUnit)
			{
				this.SetActive(false);
				return;
			}
			this.CreateQuakeOnDrop();
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000C5B0C File Offset: 0x000C3D0C
		public bool SnapsTo(GameObject otherObject)
		{
			if (this.UnitLogic.UnitType != UnitType.Worker || otherObject == null)
			{
				return false;
			}
			UnitPresenter component = otherObject.GetComponent<UnitPresenter>();
			return component != null && component.GetPosition() == this.GetPosition() && component.UnitLogic.MovesLeft != 0 && component.UnitLogic.UnitType == UnitType.Mech;
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0003CD4A File Offset: 0x0003AF4A
		public Scythe.BoardPresenter.GameHexPresenter GetPosition()
		{
			return this.hex;
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x0003CD52 File Offset: 0x0003AF52
		public Vector3 GetDefaultPosition()
		{
			return this.hex.GetUnitPosition(this.UnitLogic);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000C5B70 File Offset: 0x000C3D70
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

		// Token: 0x0600208A RID: 8330 RVA: 0x000C5CC4 File Offset: 0x000C3EC4
		public void OnQuakeDetected(Vector3 epicenter, float force, float radius)
		{
			if (this.actualState != UnitState.Standing || this.jump)
			{
				return;
			}
			Vector3 position = base.transform.position;
			position.y = 0f;
			float magnitude = (epicenter - position).magnitude;
			float num = (radius - magnitude) / radius;
			if (num <= 0f)
			{
				return;
			}
			float num2 = 0.5f * num;
			float num3 = force * num / (this.mass * this.forceAmortization);
			this.jump = true;
			base.transform.DOJump(base.transform.position, num3, 1, num2, false).OnComplete(delegate
			{
				this.OnJumpComplete();
			});
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x0003CD65 File Offset: 0x0003AF65
		private void OnJumpComplete()
		{
			this.jump = false;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000C5D6C File Offset: 0x000C3F6C
		public void UnitSpawnAnimation(bool enemyAction = false, bool lastSpawn = false)
		{
			base.transform.position = new Vector3(this.hex.GetUnitPosition(this.UnitLogic).x, this.hex.GetUnitPosition(this.UnitLogic).y + 2f, this.hex.GetUnitPosition(this.UnitLogic).z);
			if (this.UnitLogic is Worker)
			{
				base.gameObject.transform.DOMoveY(-0.06f, 1f, false).SetEase(Ease.OutBounce).OnComplete(delegate
				{
					this.SpawnAnimationEnd(enemyAction, lastSpawn);
				});
				base.StartCoroutine("SpawnSoundPlay");
				return;
			}
			base.gameObject.transform.DOMoveY(0f, 1f, false).SetEase(Ease.OutBounce).OnComplete(delegate
			{
				this.SpawnAnimationEnd(enemyAction, lastSpawn);
			});
			base.StartCoroutine("SpawnSoundPlay");
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x0003CD6E File Offset: 0x0003AF6E
		private IEnumerator SpawnSoundPlay()
		{
			yield return new WaitForSeconds(0.45f);
			if ((this.UnitLogic is Mech || this.UnitLogic is Worker) && GameController.Instance.cameraMovementEffects.presenationFinished)
			{
				WorldSFXManager.PlaySound(SoundEnum.ProduceWorker, AudioSourceType.WorldSfx);
			}
			yield break;
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0003CD7D File Offset: 0x0003AF7D
		private void SpawnAnimationEnd(bool enemyAction = false, bool lastSpawn = false)
		{
			this.UnitLogic.spawnAnimation = false;
			if (enemyAction && lastSpawn)
			{
				ShowEnemyMoves.Instance.SetAnimationInProgress(false);
				ShowEnemyMoves.Instance.GetNextAnimation();
			}
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000C5E7C File Offset: 0x000C407C
		public string InfoBasic()
		{
			return "Unit" + this.UnitLogic.UnitType.ToString() + this.UnitLogic.Owner.matFaction.faction.ToString() + "Basic";
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000C5ED4 File Offset: 0x000C40D4
		public string InfoAdv()
		{
			Faction faction = GameController.GameManager.PlayerCurrent.matFaction.faction;
			if (GameController.GameManager.IsMultiplayer)
			{
				faction = GameController.GameManager.PlayerOwner.matFaction.faction;
			}
			return "Unit" + this.UnitLogic.UnitType.ToString() + ((this.UnitLogic.Owner.matFaction.faction == faction) ? "Friend" : "Enemy") + "Adv";
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000C5F64 File Offset: 0x000C4164
		public void Clear()
		{
			this.moveSequence.Kill(false);
			this.isSpawnAnimation = false;
			this.positionPathIterator = 0;
			this.SetHex(null);
			this.actualDestination = null;
			this.moveToPosition = Vector3.zero;
			this.lastPosition = Vector3.zero;
			this.actualState = UnitState.Standing;
			if (PlatformManager.IsStandalone)
			{
				base.transform.rotation = Quaternion.Euler(Vector3.zero);
			}
			this.positionPath.Clear();
			this.path.Clear();
			this.UnitLogic.DecreaseMoveAnimationAmount();
			this.UnitLogic.spawnAnimation = false;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x0003CDA5 File Offset: 0x0003AFA5
		public void SetSkin(int value)
		{
			this.unitMeshRenderer.material = this.materialVariants[value];
			if (this.secondaryMeshRenderer != null)
			{
				this.secondaryMeshRenderer.material = this.secondaryMaterialVariants[value];
			}
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x0003CDE3 File Offset: 0x0003AFE3
		public void SetMesh(int value)
		{
			this.unitMeshFilter.mesh = this.meshVariants[value];
			if (this.secondaryMeshFilter != null)
			{
				this.secondaryMeshFilter.mesh = this.secondaryMeshVariants[value];
			}
		}

		// Token: 0x040016A7 RID: 5799
		public GameObject coasterObject;

		// Token: 0x040016A8 RID: 5800
		private Collider unitCollider;

		// Token: 0x040016A9 RID: 5801
		public MeshFilter unitMeshFilter;

		// Token: 0x040016AA RID: 5802
		public MeshRenderer unitMeshRenderer;

		// Token: 0x040016AB RID: 5803
		public MeshFilter secondaryMeshFilter;

		// Token: 0x040016AC RID: 5804
		public MeshRenderer secondaryMeshRenderer;

		// Token: 0x040016AD RID: 5805
		public List<Mesh> meshVariants;

		// Token: 0x040016AE RID: 5806
		public List<Material> materialVariants;

		// Token: 0x040016AF RID: 5807
		public List<Mesh> secondaryMeshVariants;

		// Token: 0x040016B0 RID: 5808
		public List<Material> secondaryMaterialVariants;

		// Token: 0x040016B1 RID: 5809
		[SerializeField]
		private List<MeshRenderer> outlineComponents;

		// Token: 0x040016B2 RID: 5810
		private MaterialPropertyBlock highlightPropertyBlock;

		// Token: 0x040016B3 RID: 5811
		[SerializeField]
		private List<Renderer> unitRenderer;

		// Token: 0x040016B4 RID: 5812
		private MaterialPropertyBlock fresnelPropertyBlock;

		// Token: 0x040016B5 RID: 5813
		private Color[] fresnelColors = new Color[]
		{
			Color.yellow,
			Color.green,
			Color.blue
		};

		// Token: 0x040016BA RID: 5818
		private List<Vector3> positionPath = new List<Vector3>();

		// Token: 0x040016BB RID: 5819
		private List<GameHex> path = new List<GameHex>();

		// Token: 0x040016BC RID: 5820
		private int positionPathIterator;

		// Token: 0x040016BD RID: 5821
		private List<Unit> loadedUnits = new List<Unit>();

		// Token: 0x040016BE RID: 5822
		public UnitState actualState;

		// Token: 0x040016BF RID: 5823
		private GameHex actualDestination;

		// Token: 0x040016C0 RID: 5824
		private Vector3 moveToPosition;

		// Token: 0x040016C1 RID: 5825
		private Sequence moveSequence;

		// Token: 0x040016C2 RID: 5826
		public bool isSpawnAnimation;

		// Token: 0x040016C3 RID: 5827
		private Vector3 newPosition;

		// Token: 0x040016C4 RID: 5828
		private bool skipMove;

		// Token: 0x040016C5 RID: 5829
		private Vector3 lastPosition;

		// Token: 0x040016C6 RID: 5830
		private GameHex enemyBattleHex;

		// Token: 0x040016C7 RID: 5831
		private Coroutine lastFadeHighlightMoveCoroutine;

		// Token: 0x040016C8 RID: 5832
		private Coroutine outlineCoroutine;

		// Token: 0x040016C9 RID: 5833
		private MovePresenter movePresenter;

		// Token: 0x040016CA RID: 5834
		[SerializeField]
		private float quakeRadius = 4f;

		// Token: 0x040016CB RID: 5835
		[SerializeField]
		private float mass = 0.5f;

		// Token: 0x040016CC RID: 5836
		private float heightOnDrop;

		// Token: 0x040016CD RID: 5837
		private float forceAmortization = 50f;

		// Token: 0x040016CE RID: 5838
		private bool jump;

		// Token: 0x040016D1 RID: 5841
		private const string tintColorProperty = "_TintColor";

		// Token: 0x040016D2 RID: 5842
		private const string inOffsetProperty = "_In_Offset";

		// Token: 0x040016D3 RID: 5843
		private const string fresnelIntensivityProperty = "_FresnelIntensivity";

		// Token: 0x040016D4 RID: 5844
		private const string fresnelColorProperty = "_FresnelColor";

		// Token: 0x040016D5 RID: 5845
		private const string colorProperty = "_Color";

		// Token: 0x02000420 RID: 1056
		// (Invoke) Token: 0x06002097 RID: 8343
		public delegate void UnitStatus(UnitState unitStatus, UnitPresenter unitPresenter);

		// Token: 0x02000421 RID: 1057
		// (Invoke) Token: 0x0600209B RID: 8347
		public delegate void UnitLoaded(UnitPresenter presenter);

		// Token: 0x02000422 RID: 1058
		// (Invoke) Token: 0x0600209F RID: 8351
		public delegate void UnitMoveStart(UnitPresenter presenter, UnitState moveType);

		// Token: 0x02000423 RID: 1059
		// (Invoke) Token: 0x060020A3 RID: 8355
		public delegate void UnitMoveStop(UnitPresenter presenter, bool takeOwnerPosition);

		// Token: 0x02000424 RID: 1060
		// (Invoke) Token: 0x060020A7 RID: 8359
		public delegate void UnitDragStart(UnitPresenter presenter);

		// Token: 0x02000425 RID: 1061
		// (Invoke) Token: 0x060020AB RID: 8363
		public delegate void UnitDragEnd(UnitPresenter presenter);
	}
}
