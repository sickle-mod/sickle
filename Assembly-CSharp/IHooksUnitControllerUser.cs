using System;
using Scythe.GameLogic;
using Scythe.UI;

// Token: 0x0200007F RID: 127
public interface IHooksUnitControllerUser
{
	// Token: 0x0600046B RID: 1131
	Unit GetSelectedUnit();

	// Token: 0x0600046C RID: 1132
	bool CursorRaycastBlocked();

	// Token: 0x0600046D RID: 1133
	void CursorHitUnit(UnitPresenter unitPresenter);

	// Token: 0x0600046E RID: 1134
	void CursorHitHex();

	// Token: 0x0600046F RID: 1135
	void CursorNoHit();

	// Token: 0x06000470 RID: 1136
	bool UnitUnderTheCursorIsCorrect(UnitPresenter unitUnderTheCursor);

	// Token: 0x06000471 RID: 1137
	ExchangePanelPresenter GetExchangePanel();

	// Token: 0x06000472 RID: 1138
	void OnConnectionBroken();
}
