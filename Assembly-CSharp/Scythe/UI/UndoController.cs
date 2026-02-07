using System;
using System.Collections.Generic;
using Scythe.Analytics;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004C5 RID: 1221
	public class UndoController : MonoBehaviour
	{
		// Token: 0x060026E4 RID: 9956 RVA: 0x00040CE4 File Offset: 0x0003EEE4
		private void Awake()
		{
			this.UpdateUndoType();
			this.SetUndoInteractivity(true);
			this.AttachUndoHoover();
			GameController.GameManager.actionManager.SectionActionFinished += this.PushToStack;
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x00040D14 File Offset: 0x0003EF14
		private void OnDisable()
		{
			if (GameController.GameManager != null)
			{
				GameController.GameManager.actionManager.SectionActionFinished -= this.PushToStack;
			}
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x00040D38 File Offset: 0x0003EF38
		public void OnAutosave()
		{
			this.PushToStack();
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000E6910 File Offset: 0x000E4B10
		public void PrepareAfterGameLoad(bool undo)
		{
			this.UpdateUndoType();
			GameController.GameManager.actionManager.SectionActionFinished -= this.PushToStack;
			GameController.GameManager.actionManager.SectionActionFinished += this.PushToStack;
			if (!undo)
			{
				this.ClearStack();
				this.startingStackCount = this.undoStack.Count;
			}
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000E6974 File Offset: 0x000E4B74
		public void UpdateUndoType()
		{
			this.undoType = (ChallengesLogicStarter.UndoTypes)GameController.GameManager.UndoType;
			if (this.undoType == ChallengesLogicStarter.UndoTypes.Unlimited && this.undoStack.Count > 1)
			{
				this.oldestStateAchieved = false;
				this.SetUndoInteractivity(true);
				return;
			}
			if (this.undoType == ChallengesLogicStarter.UndoTypes.SingleTurn)
			{
				this.oldestStateAchieved = true;
				this.SetUndoInteractivity(false);
			}
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000E69D0 File Offset: 0x000E4BD0
		public void PushToStack()
		{
			if (this.CanPushStateToStack())
			{
				if (ActionLogPresenter.Instance == null)
				{
					this.undoStack.Push(new KeyValuePair<int, string>(0, GameController.Game.SaveToString()));
				}
				else
				{
					this.undoStack.Push(new KeyValuePair<int, string>(ActionLogPresenter.Instance.LogCount(), GameController.Game.SaveToString()));
				}
				if (!this.startStatePushed)
				{
					this.startStatePushed = true;
				}
				if (this.undoStack.Count > 1)
				{
					this.oldestStateAchieved = false;
				}
			}
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x00040D40 File Offset: 0x0003EF40
		public void PopFromStack()
		{
			if (this.CanPushStateToStack())
			{
				this.undoStack.Pop();
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x00040D56 File Offset: 0x0003EF56
		private void AttachUndoHoover()
		{
			this.undoButton.gameObject.GetComponent<PointerEventsController>().buttonHoover += this.UndoToggleSFX;
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x00040D79 File Offset: 0x0003EF79
		private void UndoToggleSFX()
		{
			if (this.undoButton.IsInteractable())
			{
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardShowEnemysboardRelease, AudioSourceType.Buttons);
			}
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000E6A58 File Offset: 0x000E4C58
		private bool CanPushStateToStack()
		{
			return !GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsCampaign && !GameController.Instance.GameIsLoaded && GameController.GameManager.PlayerCurrent.IsHuman && !GameController.GameManager.GameFinished;
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x00040D90 File Offset: 0x0003EF90
		public void OnEndTurn()
		{
			this.TriggerUndoInteractivityChange(false);
			this.startStatePushed = false;
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x00040DA0 File Offset: 0x0003EFA0
		public void ClearStack()
		{
			this.undoStack.Clear();
			this.SetUndoInteractivity(false);
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000E6AAC File Offset: 0x000E4CAC
		public void Undo()
		{
			if (this.CanUndo())
			{
				this.SetUndoInteractivity(false);
				WorldSFXManager.PlaySound(SoundEnum.CommonCheckBoxV1, AudioSourceType.Buttons);
				int count = this.undoStack.Count;
				bool flag = GameController.GameManager.actionManager.SectionActionSelected();
				GameController.Instance.PrepareBeforeUndo();
				this.LoadPreviousState(flag);
				GameController.Instance.AfterUndo();
				if ((this.undoType == ChallengesLogicStarter.UndoTypes.SingleTurn && count == this.undoStack.Count) || this.oldestStateAchieved)
				{
					this.SetUndoInteractivity(false);
					return;
				}
				this.SetUndoInteractivity(true);
			}
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x00040DB4 File Offset: 0x0003EFB4
		private bool CanUndo()
		{
			return this.undoStack.Count >= 1;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000E6B38 File Offset: 0x000E4D38
		private KeyValuePair<int, string> LoadPreviousState(bool sectionActionSelected)
		{
			KeyValuePair<int, string> keyValuePair = default(KeyValuePair<int, string>);
			if (this.undoStack.Count > 1)
			{
				if (sectionActionSelected || GameController.GameManager.GameFinished)
				{
					keyValuePair = this.undoStack.Peek();
					this.LoadFrame(keyValuePair);
					if (this.undoType == ChallengesLogicStarter.UndoTypes.SingleTurn)
					{
						this.oldestStateAchieved = true;
					}
				}
				else if (!sectionActionSelected)
				{
					this.undoStack.Pop();
					keyValuePair = this.undoStack.Peek();
					this.LoadFrame(keyValuePair);
					if (this.undoStack.Count == 1 || this.undoType == ChallengesLogicStarter.UndoTypes.SingleTurn)
					{
						this.oldestStateAchieved = true;
					}
				}
			}
			else
			{
				keyValuePair = this.undoStack.Peek();
				this.LoadFrame(keyValuePair);
				this.oldestStateAchieved = true;
			}
			AnalyticsEventData.IncreaseUndoCounter();
			return keyValuePair;
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x00040DC7 File Offset: 0x0003EFC7
		private void LoadFrame(KeyValuePair<int, string> frame)
		{
			GameController.Game.LoadFromString(frame.Value);
			GameController.GameManager.UndoType = (int)this.undoType;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x00040DEA File Offset: 0x0003EFEA
		public void TriggerUndoInteractivityChange(bool interactable)
		{
			if (this.undoType == ChallengesLogicStarter.UndoTypes.Off)
			{
				return;
			}
			this.SetUndoInteractivity(interactable);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x00040DFD File Offset: 0x0003EFFD
		public void TriggerUnlimitedUndoInteractivityChange(bool interactable)
		{
			if (this.undoType == ChallengesLogicStarter.UndoTypes.Unlimited)
			{
				this.SetUndoInteractivity(interactable);
			}
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x00040E0F File Offset: 0x0003F00F
		public void SetUndoInteractivity(bool interactable)
		{
			if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.IsCampaign)
			{
				this.undoButton.interactable = interactable;
				return;
			}
			this.undoButton.interactable = false;
		}

		// Token: 0x04001BD3 RID: 7123
		[SerializeField]
		private Button undoButton;

		// Token: 0x04001BD4 RID: 7124
		private Stack<KeyValuePair<int, string>> undoStack = new Stack<KeyValuePair<int, string>>();

		// Token: 0x04001BD5 RID: 7125
		private ChallengesLogicStarter.UndoTypes undoType;

		// Token: 0x04001BD6 RID: 7126
		private bool startStatePushed;

		// Token: 0x04001BD7 RID: 7127
		private int startingStackCount;

		// Token: 0x04001BD8 RID: 7128
		private bool oldestStateAchieved = true;
	}
}
