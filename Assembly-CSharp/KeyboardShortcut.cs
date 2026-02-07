using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
[Serializable]
public struct KeyboardShortcut
{
	// Token: 0x04000292 RID: 658
	public KeyCode keyCode;

	// Token: 0x04000293 RID: 659
	public KeyboardShortcut.ActionsEnum action;

	// Token: 0x02000059 RID: 89
	[SerializeField]
	public enum ActionsEnum
	{
		// Token: 0x04000295 RID: 661
		CameraUp,
		// Token: 0x04000296 RID: 662
		CameraDown,
		// Token: 0x04000297 RID: 663
		CameraLeft,
		// Token: 0x04000298 RID: 664
		CameraRight,
		// Token: 0x04000299 RID: 665
		CameraZoomIn,
		// Token: 0x0400029A RID: 666
		CameraZoomOut,
		// Token: 0x0400029B RID: 667
		CameraRorateLeft,
		// Token: 0x0400029C RID: 668
		CameraRotateRight,
		// Token: 0x0400029D RID: 669
		ActionAccept,
		// Token: 0x0400029E RID: 670
		EndTurn,
		// Token: 0x0400029F RID: 671
		SelectAnother,
		// Token: 0x040002A0 RID: 672
		ShowiHideOptions,
		// Token: 0x040002A1 RID: 673
		Undo,
		// Token: 0x040002A2 RID: 674
		ShowHideHUD,
		// Token: 0x040002A3 RID: 675
		ShowHideStarTab,
		// Token: 0x040002A4 RID: 676
		ShowHideScoreTab,
		// Token: 0x040002A5 RID: 677
		ShowHideStructureTab,
		// Token: 0x040002A6 RID: 678
		ShowHideObjectiveTab,
		// Token: 0x040002A7 RID: 679
		ShowHideRecruitmentTab,
		// Token: 0x040002A8 RID: 680
		ShowHideMechTab,
		// Token: 0x040002A9 RID: 681
		ShowHideFactionTab,
		// Token: 0x040002AA RID: 682
		ExpandTopPanel,
		// Token: 0x040002AB RID: 683
		ExpandDownTopPanel,
		// Token: 0x040002AC RID: 684
		FactoryTop1,
		// Token: 0x040002AD RID: 685
		FactoryTop2,
		// Token: 0x040002AE RID: 686
		FactoryBottom,
		// Token: 0x040002AF RID: 687
		Section1Top1,
		// Token: 0x040002B0 RID: 688
		Section1Top2,
		// Token: 0x040002B1 RID: 689
		Section1Bottom,
		// Token: 0x040002B2 RID: 690
		Section2Top1,
		// Token: 0x040002B3 RID: 691
		Section2Top2,
		// Token: 0x040002B4 RID: 692
		Section2Bottom,
		// Token: 0x040002B5 RID: 693
		Section3Top1,
		// Token: 0x040002B6 RID: 694
		Section3Top2,
		// Token: 0x040002B7 RID: 695
		Section3Bottom,
		// Token: 0x040002B8 RID: 696
		Section4Top1,
		// Token: 0x040002B9 RID: 697
		Section4Top2,
		// Token: 0x040002BA RID: 698
		Section4Bottom,
		// Token: 0x040002BB RID: 699
		LoadUnloadEverything,
		// Token: 0x040002BC RID: 700
		LoadAllUnloadOneWorker,
		// Token: 0x040002BD RID: 701
		LoadUnloadWood,
		// Token: 0x040002BE RID: 702
		LoadUnloadFood,
		// Token: 0x040002BF RID: 703
		LoadUnloadOil,
		// Token: 0x040002C0 RID: 704
		LoadUnloadMetal,
		// Token: 0x040002C1 RID: 705
		LoadUnloadAllResources,
		// Token: 0x040002C2 RID: 706
		SendChatMsg,
		// Token: 0x040002C3 RID: 707
		OpenCloseChat,
		// Token: 0x040002C4 RID: 708
		SlowCameraMovementDown,
		// Token: 0x040002C5 RID: 709
		ExpandShrinkBottomPanel,
		// Token: 0x040002C6 RID: 710
		FastForward,
		// Token: 0x040002C7 RID: 711
		Choice1,
		// Token: 0x040002C8 RID: 712
		Choice2,
		// Token: 0x040002C9 RID: 713
		Choice3,
		// Token: 0x040002CA RID: 714
		Choice4,
		// Token: 0x040002CB RID: 715
		Choice5,
		// Token: 0x040002CC RID: 716
		Choice6,
		// Token: 0x040002CD RID: 717
		Choice7,
		// Token: 0x040002CE RID: 718
		Choice8,
		// Token: 0x040002CF RID: 719
		MinMaxOdrerPanel,
		// Token: 0x040002D0 RID: 720
		ChatFocus,
		// Token: 0x040002D1 RID: 721
		RevealCombatCards
	}
}
