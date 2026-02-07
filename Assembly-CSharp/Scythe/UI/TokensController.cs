using System;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000450 RID: 1104
	public class TokensController : MonoBehaviour
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x0003E05F File Offset: 0x0003C25F
		private TokenManager tokenManager
		{
			get
			{
				if (GameController.GameManager == null)
				{
					return null;
				}
				return GameController.GameManager.tokenManager;
			}
		}

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x06002277 RID: 8823 RVA: 0x000CD1E4 File Offset: 0x000CB3E4
		// (remove) Token: 0x06002278 RID: 8824 RVA: 0x000CD218 File Offset: 0x000CB418
		public static event TokensController.PlaceToken OnTokenPlaced;

		// Token: 0x06002279 RID: 8825 RVA: 0x0003E074 File Offset: 0x0003C274
		private void OnEnable()
		{
			this.RemoveListeners();
			this.AttachListeners();
			this.flagsPanel.SetTokensMenuController(this);
			this.trapsPanel.SetTokensMenuController(this);
			this.armTrapPanel.SetTokensMenuController(this);
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x0003E0A6 File Offset: 0x0003C2A6
		private void OnDisable()
		{
			this.RemoveListeners();
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000CD24C File Offset: 0x000CB44C
		private void AttachListeners()
		{
			TokenManager tokenManager = this.tokenManager;
			tokenManager.TokenWasPlaced = (TokenManager.OnTokenPlaced)Delegate.Combine(tokenManager.TokenWasPlaced, new TokenManager.OnTokenPlaced(this.UpdateHexOnTokenPlaced));
			TokenManager tokenManager2 = this.tokenManager;
			tokenManager2.TrapWasArmed = (TokenManager.OnTrapTriggered)Delegate.Combine(tokenManager2.TrapWasArmed, new TokenManager.OnTrapTriggered(this.UpdateHexOnTrapArmed));
			TokenManager tokenManager3 = this.tokenManager;
			tokenManager3.CanPlaceTokenInfo = (TokenManager.CanUnitPlaceToken)Delegate.Combine(tokenManager3.CanPlaceTokenInfo, new TokenManager.CanUnitPlaceToken(this.ShowTokensPresenter));
			TokenPresenter.UnitTokenInteraction += this.EnemyTokenInteraction;
			UnitPresenter.MoveStart += this.UnitMoveAnimationStarted;
			UnitPresenter.MoveStop += this.UnitMoveAnimationStoped;
			UnitPresenter.UnitDragged += this.UnitDragStart;
			UnitPresenter.UnitDroped += this.UnitDragStop;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000CD324 File Offset: 0x000CB524
		private void RemoveListeners()
		{
			if (this.tokenManager != null)
			{
				TokenManager tokenManager = this.tokenManager;
				tokenManager.TokenWasPlaced = (TokenManager.OnTokenPlaced)Delegate.Remove(tokenManager.TokenWasPlaced, new TokenManager.OnTokenPlaced(this.UpdateHexOnTokenPlaced));
				TokenManager tokenManager2 = this.tokenManager;
				tokenManager2.TrapWasArmed = (TokenManager.OnTrapTriggered)Delegate.Remove(tokenManager2.TrapWasArmed, new TokenManager.OnTrapTriggered(this.UpdateHexOnTrapArmed));
				TokenManager tokenManager3 = this.tokenManager;
				tokenManager3.CanPlaceTokenInfo = (TokenManager.CanUnitPlaceToken)Delegate.Remove(tokenManager3.CanPlaceTokenInfo, new TokenManager.CanUnitPlaceToken(this.ShowTokensPresenter));
			}
			TokenPresenter.UnitTokenInteraction -= this.EnemyTokenInteraction;
			UnitPresenter.MoveStart -= this.UnitMoveAnimationStarted;
			UnitPresenter.MoveStop -= this.UnitMoveAnimationStoped;
			UnitPresenter.UnitDragged -= this.UnitDragStart;
			UnitPresenter.UnitDroped -= this.UnitDragStop;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x0003E0AE File Offset: 0x0003C2AE
		public void OnUndo()
		{
			this.tokenManager.OnUndo();
			this.RemoveListeners();
			this.AttachListeners();
			if (TokensController.OnTokenPlaced != null)
			{
				TokensController.OnTokenPlaced();
			}
			this.HidePanels();
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x0003E0DE File Offset: 0x0003C2DE
		public void AfterGameLoad()
		{
			this.ShowTokensPresenter(this.tokenManager.GetLastMovedUnit(), true);
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x0003E0F2 File Offset: 0x0003C2F2
		private void UnitMoveAnimationStarted(UnitPresenter unit, UnitState moveType)
		{
			if (this.DoNotShowPanels(unit.UnitLogic))
			{
				return;
			}
			if (moveType != UnitState.MovingFromMech && moveType != UnitState.MovingToMech)
			{
				this.HidePanels();
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x0003E111 File Offset: 0x0003C311
		private void UnitMoveAnimationStoped(UnitPresenter unit, bool takeOwnerPosition)
		{
			if (this.DoNotShowPanels(unit.UnitLogic))
			{
				return;
			}
			if (this.MoveCondition(unit.UnitLogic))
			{
				this.ShowTokensPresenter(unit.UnitLogic, true);
			}
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x0003E13D File Offset: 0x0003C33D
		private bool MoveCondition(Unit unit)
		{
			return unit != null && unit.UnitType != UnitType.Worker && this.tokenManager.HexCondition(unit);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x0003E159 File Offset: 0x0003C359
		private void UnitDragStart(UnitPresenter unit)
		{
			if (this.DoNotShowPanels(unit.UnitLogic))
			{
				return;
			}
			this.unitBeforeMove = this.tokenManager.GetLastMovedUnit();
			if (this.unitBeforeMove == unit.UnitLogic)
			{
				this.HidePanels();
			}
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000CD404 File Offset: 0x000CB604
		private void UnitDragStop(UnitPresenter unit)
		{
			if (unit.UnitLogic == this.tokenManager.GetLastMovedUnit())
			{
				this.HidePanels();
			}
			if (this.DoNotShowPanels(unit.UnitLogic))
			{
				return;
			}
			if (this.unitBeforeMove == unit.UnitLogic || this.unitBeforeMove == null)
			{
				if (this.MoveCondition(unit.UnitLogic))
				{
					this.ShowTokensPresenter(unit.UnitLogic, true);
					return;
				}
			}
			else
			{
				if (this.unitBeforeMove != null && this.unitBeforeMove != unit.UnitLogic && !this.tokenManager.SameAsLastMovedUnit(unit.UnitLogic) && this.MoveCondition(unit.UnitLogic))
				{
					this.ShowTokensPresenter(this.unitBeforeMove, true);
					return;
				}
				if (this.unitBeforeMove != null && this.unitBeforeMove != unit.UnitLogic && this.tokenManager.SameAsLastMovedUnit(unit.UnitLogic) && this.MoveCondition(unit.UnitLogic))
				{
					this.ShowTokensPresenter(unit.UnitLogic, true);
				}
			}
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x0003E18F File Offset: 0x0003C38F
		public void ShowTokensPresenter(Unit unit, bool show)
		{
			if (this.DoNotShowPanels(unit))
			{
				return;
			}
			if (show)
			{
				this.matFaction = unit.Owner.matFaction;
				this.AssingTokensPanel(unit, this.matFaction.faction);
				return;
			}
			this.HidePanels();
			this.Clear();
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000CD4F8 File Offset: 0x000CB6F8
		private void AssingTokensPanel(Unit unit, Faction faction)
		{
			GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(unit.position);
			if (faction == Faction.Togawa)
			{
				if (this.tokenManager.CanPlaceTrap(gameHexPresenter.GetGameHexLogic(), unit))
				{
					this.trapsPanel.ShowTrapPanelIcon(gameHexPresenter, unit);
					return;
				}
				if (this.tokenManager.CanRearmTrap(gameHexPresenter.GetGameHexLogic(), faction))
				{
					this.armTrapPanel.ShowPanel(gameHexPresenter, unit);
					return;
				}
			}
			else if (this.tokenManager.CanPlaceFlag(gameHexPresenter.GetGameHexLogic(), unit))
			{
				this.flagsPanel.ShowPanel(gameHexPresenter, unit);
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000CD580 File Offset: 0x000CB780
		private void EnemyTokenInteraction(TrapToken trap)
		{
			Unit unit = this.tokenManager.GetLastMovedUnit();
			if (unit == null && trap.Position.GetOwnerUnitCount() > 0)
			{
				unit = trap.Position.GetOwnerUnits()[0];
			}
			if (unit != null)
			{
				this.trapAnimation.ShowTrapPenalty(trap, GameController.GetUnitPresenter(unit).transform);
			}
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000CD5D8 File Offset: 0x000CB7D8
		public void HidePanels()
		{
			if (this.trapsPanel.gameObject.activeInHierarchy)
			{
				this.trapsPanel.Hide();
			}
			if (this.armTrapPanel.gameObject.activeInHierarchy)
			{
				this.armTrapPanel.Hide();
			}
			if (this.flagsPanel.gameObject.activeInHierarchy)
			{
				this.flagsPanel.Hide();
			}
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000CD63C File Offset: 0x000CB83C
		private bool DoNotShowPanels(Unit unit)
		{
			return unit == null || !unit.Owner.IsHuman || GameController.GameManager.SpectatorMode || (GameController.GameManager.IsMultiplayer && unit != null && GameController.GameManager.PlayerOwner != unit.Owner) || (unit != null && unit.position.Owner != unit.Owner);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0003E1CE File Offset: 0x0003C3CE
		public void ArmTrap(Unit unit)
		{
			this.tokenManager.ArmTrap(unit.position);
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000CD6A4 File Offset: 0x000CB8A4
		public void TokenPlaced()
		{
			Unit lastMovedUnit = this.tokenManager.GetLastMovedUnit();
			if (GameController.GameManager.moveManager.GetActualAction() != null)
			{
				(HumanInputHandler.Instance.movePresenter as MovePresenter).OnTokenPlaced(lastMovedUnit);
			}
			if (TokensController.OnTokenPlaced != null)
			{
				TokensController.OnTokenPlaced();
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x0003E1E2 File Offset: 0x0003C3E2
		public void UpdateHexOnTokenPlaced(FactionAbilityToken placedToken)
		{
			GameController.Instance.GetGameHexPresenter(placedToken.Position).token.GetComponent<TokenPresenter>().ShowAndPlaceToken(placedToken, GameController.GetUnitPresenter(placedToken.Owner.character).transform.position);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x0003E21E File Offset: 0x0003C41E
		public void UpdateHexOnTrapArmed(FactionAbilityToken placedToken)
		{
			GameController.Instance.GetGameHexPresenter(placedToken.Position).token.GetComponent<TokenPresenter>().UpdatePresenter(null);
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0003E240 File Offset: 0x0003C440
		public void Clear()
		{
			this.matFaction = null;
			this.unitBeforeMove = null;
		}

		// Token: 0x040017F4 RID: 6132
		[SerializeField]
		private GameObject placeTrapGroup;

		// Token: 0x040017F5 RID: 6133
		[SerializeField]
		private GameObject placeFlagGroup;

		// Token: 0x040017F6 RID: 6134
		[SerializeField]
		private GameObject armTrapGroup;

		// Token: 0x040017F7 RID: 6135
		[SerializeField]
		private PlaceFlagPanel flagsPanel;

		// Token: 0x040017F8 RID: 6136
		[SerializeField]
		private PlaceTrapPanel trapsPanel;

		// Token: 0x040017F9 RID: 6137
		[SerializeField]
		private ArmTrapPanel armTrapPanel;

		// Token: 0x040017FA RID: 6138
		[SerializeField]
		private TrapTriggered trapAnimation;

		// Token: 0x040017FB RID: 6139
		[SerializeField]
		private TokenPresenter tokenPrefab;

		// Token: 0x040017FC RID: 6140
		private Unit unitBeforeMove;

		// Token: 0x040017FD RID: 6141
		private MatFaction matFaction;

		// Token: 0x02000451 RID: 1105
		// (Invoke) Token: 0x06002291 RID: 8849
		public delegate void PlaceToken();
	}
}
