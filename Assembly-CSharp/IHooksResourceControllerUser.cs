using System;
using Scythe.UI;

// Token: 0x0200007D RID: 125
public interface IHooksResourceControllerUser
{
	// Token: 0x06000462 RID: 1122
	void CursorHitUnit(ResourcePresenter resourcePresenter, UnitPresenter unitPresenter);

	// Token: 0x06000463 RID: 1123
	bool UnitUnderTheCursorIsCorrect(UnitPresenter unitUnderTheCursor);
}
