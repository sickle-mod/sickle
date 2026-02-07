using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000242 RID: 578
	public class GameRoom : MonoBehaviour
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x00033476 File Offset: 0x00031676
		// (set) Token: 0x06001163 RID: 4451 RVA: 0x0003347E File Offset: 0x0003167E
		public int NumberOfPlayersChoosing { get; private set; }

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06001164 RID: 4452 RVA: 0x0009319C File Offset: 0x0009139C
		// (remove) Token: 0x06001165 RID: 4453 RVA: 0x000931D4 File Offset: 0x000913D4
		public event global::System.Action PlayerKicked;

		// Token: 0x06001166 RID: 4454 RVA: 0x0009320C File Offset: 0x0009140C
		public void Init(LobbyGame roomData)
		{
			this.roomData = roomData;
			this.choosingMats = false;
			this.RemoveAllSlots();
			if (PlatformManager.IsMobile)
			{
				this._mobileChatLocal.ClearChat();
				this._mobileChatGlobal.ClearChat();
			}
			this.closeLobbyButton.SetActive(false);
			this.minimizeRoomButton.SetActive(true);
			this.lobbyTitle.text = this.roomData.Name;
			this.readyImage.gameObject.SetActive(false);
			base.gameObject.SetActive(true);
			this.gamePropertiesPanel.Init(roomData);
			this.FillSlots(roomData);
			int num = roomData.PlayersList.FindIndex((PlayerInfo player) => player.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id);
			PlayerInfo.me.Slot = roomData.PlayersList[num].Slot;
			roomData.PlayersList[num] = PlayerInfo.me;
			foreach (PlayerInfo playerInfo in roomData.PlayersList)
			{
				this.AddPlayer(playerInfo, true);
			}
			foreach (Bot bot in roomData.BotsList)
			{
				this.AddBot(bot, true);
			}
			this.PromoteNewAdmin(roomData.AdminId);
			this.UpdateHint();
			this.AdjustStartButton();
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000933A4 File Offset: 0x000915A4
		private void RemoveAllSlots()
		{
			foreach (PlayerSlot playerSlot in this.slots)
			{
				playerSlot.DestroySlot();
			}
			this.slots.Clear();
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00093400 File Offset: 0x00091600
		private void FillSlots(LobbyGame roomData)
		{
			for (int i = 0; i < roomData.MaxPlayers; i++)
			{
				PlayerSlot playerSlot = global::UnityEngine.Object.Instantiate<PlayerSlot>(this.playerSlotPrefab, this.gridLayout);
				playerSlot.ActivateAsEmpty(i, roomData.InvadersFromAfar, this);
				playerSlot.RemoveBotSuccess += this.OnRemoveBotSuccess;
				playerSlot.AddBotSuccess += this.OnAddBotSuccess;
				playerSlot.AddBotError += this.OnAddBotError;
				playerSlot.ChoosingMatsSuccess += this.OnChoosingMatsSuccess;
				this.slots.Add(playerSlot);
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00093498 File Offset: 0x00091698
		private void AdjustStartButton()
		{
			if (!PlayerInfo.me.IsAdmin)
			{
				this.start.interactable = true;
				this.start.onClick.RemoveAllListeners();
				this.start.onClick.AddListener(new UnityAction(this.ChangeReadyState));
				this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Ready");
				return;
			}
			if (this.AllPlayersReady() && this.HumanPlayersCount() > 1)
			{
				this.start.interactable = true;
			}
			else
			{
				this.start.interactable = false;
			}
			this.start.onClick.RemoveAllListeners();
			if (this.roomData.AllRandom)
			{
				this.start.onClick.AddListener(new UnityAction(this.StartGame));
				this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Start");
				return;
			}
			this.start.onClick.AddListener(new UnityAction(this.StartSelectingMats));
			this.textOnStartButton.text = ScriptLocalization.Get("Lobby/StartSelectingMats");
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000935B0 File Offset: 0x000917B0
		public void AddPlayer(PlayerInfo player, bool onInit = false)
		{
			this.slots[player.Slot].ChangeToPlayer(player);
			if (player.IsReady && !player.IsAdmin)
			{
				this.slots[player.Slot].ChangeReadyState(true);
			}
			if (!onInit)
			{
				this.roomData.PlayersList.Add(player);
				LobbyGame lobbyGame = this.roomData;
				int players = lobbyGame.Players;
				lobbyGame.Players = players + 1;
				this.gamePropertiesPanel.Init(this.roomData);
				if (PlayerInfo.me.IsAdmin)
				{
					this.EnableAddBotPanel();
					if (this.AllPlayersReady() && this.HumanPlayersCount() > 1)
					{
						this.start.interactable = true;
					}
					else
					{
						this.start.interactable = false;
					}
				}
				this.UpdateHint();
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00093678 File Offset: 0x00091878
		public void RemovePlayer(Guid id)
		{
			if (PlayerInfo.me.PlayerStats.Id == id)
			{
				base.gameObject.SetActive(false);
				PlayerInfo.me.CurrentLobbyRoom = null;
				PlayerInfo.me.RoomId = string.Empty;
				this.minimizeRoomButton.SetActive(false);
				this.PlayerKicked();
				return;
			}
			PlayerInfo playerInfo2 = this.roomData.PlayersList.Find((PlayerInfo playerInfo) => playerInfo.PlayerStats.Id == id);
			if (playerInfo2 == null)
			{
				return;
			}
			this.roomData.PlayersList.Remove(playerInfo2);
			LobbyGame lobbyGame = this.roomData;
			int num = lobbyGame.Players;
			lobbyGame.Players = num - 1;
			this.gamePropertiesPanel.Init(this.roomData);
			int i = 0;
			while (i < this.slots.Count)
			{
				if (this.slots[i].GetPlayerId() == id)
				{
					if (!this.choosingMats)
					{
						this.slots[i].ChangeToEmpty();
						break;
					}
					num = this.NumberOfPlayersChoosing;
					this.NumberOfPlayersChoosing = num - 1;
					if (this.matAndFactionChoose.GetCurrentSlot() != i)
					{
						if (this.matAndFactionChoose.GetCurrentSlot() > i)
						{
							int faction = this.slots[i].GetFaction();
							int playerMat = this.slots[i].GetPlayerMat();
							this.matAndFactionChoose.AddLeaversMats(faction, playerMat);
						}
						this.slots[i].ChangeToEmpty();
						break;
					}
					this.slots[i].ChangeToEmpty();
					if (this.HumanPlayersCount() > 1)
					{
						this.MoveToNextPlayer();
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (this.HumanPlayersCount() == 1)
			{
				this.start.interactable = false;
				this.start.onClick.RemoveAllListeners();
				if (this.choosingMats)
				{
					this.choosingMats = false;
					for (int j = 0; j < this.slots.Count; j++)
					{
						this.slots[j].RevertMatsChoices();
						this.slots[j].DisableReadyPanel();
						this.slots[j].DeactivateTimer();
						if (!this.slots[j].IsEmpty() && j != PlayerInfo.me.Slot)
						{
							this.slots[j].SetRemovePlayerButtonState(true);
						}
					}
				}
				if (!this.roomData.AllRandom)
				{
					this.textOnStartButton.text = ScriptLocalization.Get("Lobby/StartSelectingMats");
					this.start.onClick.AddListener(new UnityAction(this.StartSelectingMats));
				}
				else
				{
					this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Start");
					this.start.onClick.AddListener(new UnityAction(this.StartGame));
				}
			}
			else if (this.NumberOfPlayersChoosing <= 5)
			{
				for (int k = 0; k < this.slots.Count; k++)
				{
					this.slots[k].UpdateSpecialAbilityAvailability(this.slots[k].GetFaction());
				}
			}
			if (PlayerInfo.me.IsAdmin && !this.choosingMats)
			{
				PlayerInfo.me.IsReady = true;
				this.EnableAddBotPanel();
				if (this.AllPlayersReady() && this.HumanPlayersCount() > 1)
				{
					this.start.interactable = true;
				}
				else
				{
					this.start.interactable = false;
				}
			}
			this.UpdateHint();
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00093A08 File Offset: 0x00091C08
		public void AddBot(Bot botData, bool init = false)
		{
			if (!init && !this.slots[botData.Slot].TakenByBot())
			{
				this.roomData.BotsList.Add(botData);
				LobbyGame lobbyGame = this.roomData;
				int players = lobbyGame.Players;
				lobbyGame.Players = players + 1;
				this.gamePropertiesPanel.Init(this.roomData);
			}
			this.slots[botData.Slot].ChangeToBot(botData);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00093A80 File Offset: 0x00091C80
		public void RemoveBot(int slot)
		{
			this.slots[slot].ChangeToEmpty();
			this.roomData.BotsList.RemoveAt(this.roomData.BotsList.FindIndex((Bot bot) => bot.Slot == slot));
			LobbyGame lobbyGame = this.roomData;
			int players = lobbyGame.Players;
			lobbyGame.Players = players - 1;
			this.gamePropertiesPanel.Init(this.roomData);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00093B04 File Offset: 0x00091D04
		private void OnAddBotSuccess(Bot botData)
		{
			this.roomData.BotsList.Add(botData);
			LobbyGame lobbyGame = this.roomData;
			int players = lobbyGame.Players;
			lobbyGame.Players = players + 1;
			this.gamePropertiesPanel.Init(this.roomData);
			this.EnableAddBotPanel();
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00033487 File Offset: 0x00031687
		private void OnAddBotError(Exception e)
		{
			this.EnableAddBotPanel();
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00093B50 File Offset: 0x00091D50
		private void OnRemoveBotSuccess(Bot bot)
		{
			this.roomData.BotsList.Remove(bot);
			LobbyGame lobbyGame = this.roomData;
			int players = lobbyGame.Players;
			lobbyGame.Players = players - 1;
			this.gamePropertiesPanel.Init(this.roomData);
			this.EnableAddBotPanel();
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00093B9C File Offset: 0x00091D9C
		public void PromoteNewAdmin(Guid adminId)
		{
			if (adminId == PlayerInfo.me.PlayerStats.Id)
			{
				PlayerInfo.me.IsAdmin = true;
				PlayerInfo.me.IsReady = true;
				if (!this.choosingMats)
				{
					this.readyImage.gameObject.SetActive(false);
					this.slots[PlayerInfo.me.Slot].DisableReadyPanel();
					this.EnableAddBotPanel();
					this.editGameOptions.gameObject.SetActive(false);
					if (this.AllPlayersReady() && this.HumanPlayersCount() > 1)
					{
						this.start.interactable = true;
					}
					else
					{
						this.start.interactable = false;
					}
					if (this.roomData.AllRandom)
					{
						this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Start");
						this.start.onClick.RemoveAllListeners();
						this.start.onClick.AddListener(new UnityAction(this.StartGame));
					}
					else
					{
						this.textOnStartButton.text = ScriptLocalization.Get("Lobby/StartSelectingMats");
						this.start.onClick.RemoveAllListeners();
						this.start.onClick.AddListener(new UnityAction(this.StartSelectingMats));
					}
					for (int i = 0; i < this.slots.Count; i++)
					{
						if (!this.slots[i].IsEmpty() && i != PlayerInfo.me.Slot)
						{
							this.slots[i].SetRemovePlayerButtonState(true);
						}
					}
				}
				else
				{
					int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
					if (PlayerInfo.me.Slot < currentSlot)
					{
						this.start.onClick.RemoveAllListeners();
						this.start.onClick.AddListener(new UnityAction(this.StartGame));
						this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Start");
						this.start.interactable = currentSlot == this.slots.Count;
					}
					else if (this.slots[currentSlot].TakenByBot())
					{
						this.matAndFactionChoose.SetRandomFaction();
						this.matAndFactionChoose.SetRandomMat();
						this.OnChoosingMatsSuccess();
					}
				}
			}
			else
			{
				if (!PlayerInfo.me.IsReady && !this.choosingMats)
				{
					this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Ready");
					this.editGameOptions.gameObject.SetActive(false);
					this.start.interactable = true;
					this.start.onClick.RemoveAllListeners();
					this.start.onClick.AddListener(new UnityAction(this.ChangeReadyState));
				}
				for (int j = 0; j < this.slots.Count; j++)
				{
					if (this.slots[j].GetPlayerId() == adminId)
					{
						this.slots[j].DisableReadyPanel();
						break;
					}
				}
				this.roomData.PlayersList.Find((PlayerInfo player) => player.PlayerStats.Id == adminId).IsAdmin = true;
			}
			this.UpdateHint();
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0003348F File Offset: 0x0003168F
		private void UpdateHint()
		{
			if (this.choosingMats)
			{
				this.UpdateChoosingMatsHint();
				return;
			}
			if (PlayerInfo.me.IsAdmin)
			{
				this.UpdateAdminHint();
				return;
			}
			this.UpdateClientHint();
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00093ED4 File Offset: 0x000920D4
		private void UpdateAdminHint()
		{
			if (this.AllPlayersReady() && this.HumanPlayersCount() > 1)
			{
				this.SetHintText(string.Empty);
				return;
			}
			if (this.HumanPlayersCount() == 1)
			{
				this.SetHintText(ScriptLocalization.Get("Lobby/SLSWaitForAtLeastTwoPlayers"));
				return;
			}
			if (this.roomData.AllRandom)
			{
				this.SetHintText(ScriptLocalization.Get("Lobby/SLSWaitForOtherPlayers"));
				return;
			}
			this.SetHintText(ScriptLocalization.Get("Lobby/AdminInfo"));
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00093F48 File Offset: 0x00092148
		private void UpdateClientHint()
		{
			if (!PlayerInfo.me.IsReady)
			{
				this.SetHintText(ScriptLocalization.Get("Lobby/SLSTapReady"));
				return;
			}
			if (this.AllPlayersReady())
			{
				this.SetHintText(ScriptLocalization.Get("Lobby/WaitingForGameMaster"));
				return;
			}
			this.SetHintText(ScriptLocalization.Get("Lobby/SLSWaitForOtherPlayers"));
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00093F9C File Offset: 0x0009219C
		private void UpdateChoosingMatsHint()
		{
			if (this.matAndFactionChoose.GetCurrentSlot() == this.slots.Count)
			{
				if (!PlayerInfo.me.IsAdmin)
				{
					this.SetHintText(ScriptLocalization.Get("Lobby/WaitingForGameMaster"));
					return;
				}
				this.SetHintText(string.Empty);
				return;
			}
			else
			{
				if (this.matAndFactionChoose.GetCurrentSlot() == PlayerInfo.me.Slot)
				{
					this.SetHintText(ScriptLocalization.Get("Lobby/SelectMats"));
					return;
				}
				this.SetHintText(ScriptLocalization.Get("Lobby/OpponentChoosing"));
				return;
			}
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x000334B9 File Offset: 0x000316B9
		private void SetHintText(string text)
		{
			this.hint.text = text;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00094024 File Offset: 0x00092224
		private void EnableAddBotPanel()
		{
			if (this.OnlyReservedSlotLeft())
			{
				return;
			}
			int num = this.slots.Count;
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (this.slots[i].IsAvailable())
				{
					this.slots[i].ActivateAddBotPanel();
					num = i;
					break;
				}
				this.slots[i].DisableAddBotPanel();
			}
			for (int j = num + 1; j < this.slots.Count; j++)
			{
				if (this.slots[j].IsEmpty())
				{
					this.slots[j].DisableAddBotPanel();
				}
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x000940D4 File Offset: 0x000922D4
		private bool OnlyReservedSlotLeft()
		{
			int num = 0;
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (this.slots[i].IsAvailable())
				{
					num++;
				}
			}
			return num == 1 && this.HumanPlayersCount() == 1;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x000334C7 File Offset: 0x000316C7
		public void EditGameOptions()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000334D5 File Offset: 0x000316D5
		public void LeaveRoom()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.minimizeRoomButton.SetActive(false);
			base.gameObject.SetActive(false);
			this.lobby.LeaveRoom();
			this.RemoveAllSlots();
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00094120 File Offset: 0x00092320
		public void ChangeReadyState()
		{
			bool newState = !PlayerInfo.me.IsReady;
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.readyImage.gameObject.SetActive(newState);
			this.start.interactable = false;
			this.slots[PlayerInfo.me.Slot].ChangeReadyState(newState);
			LobbyRestAPI.UpdateReadyState(delegate(string response)
			{
				this.start.interactable = true;
			}, delegate(Exception error)
			{
				this.readyImage.gameObject.SetActive(!newState);
				this.start.interactable = true;
				this.slots[PlayerInfo.me.Slot].ChangeReadyState(!newState);
			});
			this.UpdateHint();
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000941C0 File Offset: 0x000923C0
		public void ChangeReadyState(Guid id, bool newState)
		{
			if (this.choosingMats)
			{
				return;
			}
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (this.slots[i].GetPlayerId() == id)
				{
					this.slots[i].ChangeReadyState(newState);
					break;
				}
			}
			if (PlayerInfo.me.IsAdmin)
			{
				if (this.AllPlayersReady() && this.HumanPlayersCount() > 1)
				{
					this.start.interactable = true;
				}
				else
				{
					this.start.interactable = false;
				}
			}
			this.UpdateHint();
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00094258 File Offset: 0x00092458
		private bool AllPlayersReady()
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (!this.slots[i].IsReady())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0003350C File Offset: 0x0003170C
		private int HumanPlayersCount()
		{
			return this.roomData.PlayersList.Count;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00094294 File Offset: 0x00092494
		public void StartSelectingMats()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.start.interactable = false;
			global::System.Random random = new global::System.Random();
			List<int> list = new List<int>();
			for (int i = 0; i < this.slots.Count; i++)
			{
				this.slots[i].DisableAddBotPanel();
				if (i < this.roomData.Players)
				{
					list.Add(i);
				}
			}
			List<int> oldSlots = new List<int>(this.slots.Count);
			List<int> newSlots = new List<int>(this.slots.Count);
			List<Guid> newIds = new List<Guid>(this.slots.Count);
			for (int j = 0; j < this.slots.Count; j++)
			{
				if (!this.slots[j].IsEmpty() && !this.slots[j].TakenByDLCPlayer())
				{
					int num = ((list.Count < 5) ? list.Count : 5);
					int num2 = list[random.Next(0, num)];
					list.Remove(num2);
					newSlots.Add(num2);
					newIds.Add(this.slots[j].GetPlayerId());
					oldSlots.Add(j);
				}
			}
			for (int k = 0; k < this.slots.Count; k++)
			{
				if (!this.slots[k].IsEmpty() && this.slots[k].TakenByDLCPlayer())
				{
					int num3 = list[random.Next(0, list.Count)];
					list.Remove(num3);
					newSlots.Add(num3);
					newIds.Add(this.slots[k].GetPlayerId());
					oldSlots.Add(k);
				}
			}
			LobbyRestAPI.StartSelectingMats(new StartingOrder(oldSlots, newSlots, newIds), delegate(string response)
			{
				this.SetSlotsOrder(oldSlots, newSlots, newIds);
			}, delegate(Exception error)
			{
				Debug.LogError(error);
			});
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000944E4 File Offset: 0x000926E4
		public void SetSlotsOrder(List<int> oldSlots, List<int> newSlots, List<Guid> newIds)
		{
			GameRoom.<>c__DisplayClass67_0 CS$<>8__locals1 = new GameRoom.<>c__DisplayClass67_0();
			CS$<>8__locals1.oldSlots = oldSlots;
			this.NumberOfPlayersChoosing = this.slots.Count((PlayerSlot slot) => !slot.IsEmpty());
			CS$<>8__locals1.i = 0;
			while (CS$<>8__locals1.i < 7)
			{
				GameRoom.<>c__DisplayClass67_1 CS$<>8__locals2 = new GameRoom.<>c__DisplayClass67_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				GameRoom.<>c__DisplayClass67_1 CS$<>8__locals3 = CS$<>8__locals2;
				Predicate<int> predicate;
				if ((predicate = CS$<>8__locals2.CS$<>8__locals1.<>9__1) == null)
				{
					predicate = (CS$<>8__locals2.CS$<>8__locals1.<>9__1 = (int slotValue) => slotValue == CS$<>8__locals2.CS$<>8__locals1.i);
				}
				CS$<>8__locals3.index = newSlots.FindIndex(predicate);
				if (CS$<>8__locals2.index != -1)
				{
					int num = this.slots.FindIndex((PlayerSlot slot) => slot.GetSlotIndex() == CS$<>8__locals2.CS$<>8__locals1.oldSlots[CS$<>8__locals2.index]);
					PlayerSlot playerSlot = this.slots[num];
					this.slots[num] = this.slots[CS$<>8__locals2.CS$<>8__locals1.i];
					this.slots[CS$<>8__locals2.CS$<>8__locals1.i] = playerSlot;
					playerSlot.ChangeReadyState(false);
				}
				int num2 = CS$<>8__locals1.i + 1;
				CS$<>8__locals1.i = num2;
			}
			for (int i = 0; i < this.slots.Count; i++)
			{
				this.slots[i].transform.SetSiblingIndex(i);
				this.slots[i].UpdateSlot(i);
				this.slots[i].SetRemovePlayerButtonState(false);
			}
			this.start.interactable = false;
			this.readyImage.gameObject.SetActive(false);
			this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Ready");
			this.start.onClick.RemoveAllListeners();
			this.StartChoosing();
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0003351E File Offset: 0x0003171E
		private void StartChoosing()
		{
			this.choosingMats = true;
			this.matAndFactionChoose = new MatAndFactionChoose(this.roomData.InvadersFromAfar);
			this.MoveToNextPlayer();
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000946B4 File Offset: 0x000928B4
		public void MoveToNextPlayer()
		{
			this.matAndFactionChoose.MoveToNextPlayer();
			int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
			if (currentSlot == PlayerInfo.me.Slot)
			{
				this.slots[currentSlot].EnableChoosingPanel(this.matAndFactionChoose);
				this.start.interactable = true;
				this.start.onClick.AddListener(new UnityAction(this.slots[currentSlot].EndSetting));
			}
			else if (currentSlot == this.slots.Count)
			{
				if (PlayerInfo.me.IsAdmin)
				{
					this.start.onClick.RemoveAllListeners();
					this.start.onClick.AddListener(new UnityAction(this.StartGame));
					this.textOnStartButton.text = ScriptLocalization.Get("Lobby/Start");
					this.start.interactable = true;
				}
			}
			else
			{
				if (this.slots[currentSlot].IsEmpty())
				{
					this.MoveToNextPlayer();
					return;
				}
				if (this.slots[currentSlot].TakenByBot() && PlayerInfo.me.IsAdmin)
				{
					this.matAndFactionChoose.SetRandomFaction();
					this.matAndFactionChoose.SetRandomMat();
					this.OnChoosingMatsSuccess();
				}
				else
				{
					this.slots[currentSlot].EnableTimer();
				}
			}
			this.UpdateHint();
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00094810 File Offset: 0x00092A10
		public void OnChoosingMatsSuccess()
		{
			this.start.interactable = false;
			int currentFaction = this.matAndFactionChoose.GetCurrentFaction();
			int currentPlayerMat = this.matAndFactionChoose.GetCurrentPlayerMat();
			int currentSlot = this.matAndFactionChoose.GetCurrentSlot();
			LobbyRestAPI.MatsSelected(new MatAndFactionChoosen(currentFaction, currentPlayerMat, currentSlot), delegate(string response)
			{
			}, delegate(Exception error)
			{
				Debug.LogError(error);
			});
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00094898 File Offset: 0x00092A98
		public void FactionAndMatChoosen(int faction, int mat, int slot)
		{
			this.slots[slot].SetMatAndFaction(faction, mat);
			this.slots[slot].ChangeReadyState(true);
			this.slots[slot].DeactivateTimer();
			this.matAndFactionChoose.RemoveData(faction, mat);
			this.MoveToNextPlayer();
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x000948F0 File Offset: 0x00092AF0
		public void StartGame()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			LobbyRestAPI.StartGame(delegate(string response)
			{
			}, delegate(Exception error)
			{
				Debug.LogError(error);
			});
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00033543 File Offset: 0x00031743
		public void Hide()
		{
			this.minimizeRoomButton.SetActive(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0003355D File Offset: 0x0003175D
		public void Restore()
		{
			this.minimizeRoomButton.SetActive(true);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0009494C File Offset: 0x00092B4C
		public void ChangeChat(bool local)
		{
			if (local)
			{
				if (PlatformManager.IsMobile)
				{
					this.ChangeChildsActivity(this._mobileChatGlobal.transform, false);
					this.ChangeChildsActivity(this._mobileChatLocal.transform, true);
				}
				else
				{
					this.ChangeChildsActivity(this.chatGlobal.transform, false);
					this.ChangeChildsActivity(this.chatLocal.transform, true);
				}
				this.ChangeOpacity(this.chatGlobalText, 0.3f);
				this.ChangeOpacity(this.chatLocalText, 1f);
				this.ChangeOpacity(this.chatLocalInactiveImage, 0f);
				this.ChangeOpacity(this.chatGlobalInactiveImage, 1f);
				return;
			}
			if (PlatformManager.IsMobile)
			{
				this.ChangeChildsActivity(this._mobileChatLocal.transform, false);
				this.ChangeChildsActivity(this._mobileChatGlobal.transform, true);
			}
			else
			{
				this.ChangeChildsActivity(this.chatLocal.transform, false);
				this.ChangeChildsActivity(this.chatGlobal.transform, true);
			}
			this.ChangeOpacity(this.chatGlobalText, 1f);
			this.ChangeOpacity(this.chatLocalText, 0.3f);
			this.ChangeOpacity(this.chatLocalInactiveImage, 1f);
			this.ChangeOpacity(this.chatGlobalInactiveImage, 0f);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00094A8C File Offset: 0x00092C8C
		private void ChangeChildsActivity(Transform chat, bool activity)
		{
			for (int i = 0; i < chat.transform.childCount; i++)
			{
				chat.transform.GetChild(i).gameObject.SetActive(activity);
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0008F818 File Offset: 0x0008DA18
		private void ChangeOpacity(TextMeshProUGUI text, float opacity)
		{
			Color color = text.color;
			color.a = opacity;
			text.color = color;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0008F818 File Offset: 0x0008DA18
		private void ChangeOpacity(Image image, float opacity)
		{
			Color color = image.color;
			color.a = opacity;
			image.color = color;
		}

		// Token: 0x04000D71 RID: 3441
		[SerializeField]
		private Lobby lobby;

		// Token: 0x04000D72 RID: 3442
		[SerializeField]
		private PlayerSlot playerSlotPrefab;

		// Token: 0x04000D73 RID: 3443
		[SerializeField]
		private TextMeshProUGUI lobbyTitle;

		// Token: 0x04000D74 RID: 3444
		[SerializeField]
		private GameObject closeLobbyButton;

		// Token: 0x04000D75 RID: 3445
		[SerializeField]
		private GameObject minimizeRoomButton;

		// Token: 0x04000D76 RID: 3446
		[SerializeField]
		private Transform gridLayout;

		// Token: 0x04000D77 RID: 3447
		[SerializeField]
		private GamePropertiesPanel gamePropertiesPanel;

		// Token: 0x04000D78 RID: 3448
		[SerializeField]
		private TextMeshProUGUI chatLocalText;

		// Token: 0x04000D79 RID: 3449
		[SerializeField]
		private TextMeshProUGUI chatGlobalText;

		// Token: 0x04000D7A RID: 3450
		[SerializeField]
		private Image chatLocalInactiveImage;

		// Token: 0x04000D7B RID: 3451
		[SerializeField]
		private Image chatGlobalInactiveImage;

		// Token: 0x04000D7C RID: 3452
		[SerializeField]
		private MobileChat _mobileChatLocal;

		// Token: 0x04000D7D RID: 3453
		[SerializeField]
		private MobileChat _mobileChatGlobal;

		// Token: 0x04000D7E RID: 3454
		[SerializeField]
		private Chat chatLocal;

		// Token: 0x04000D7F RID: 3455
		[SerializeField]
		private Chat chatGlobal;

		// Token: 0x04000D80 RID: 3456
		[SerializeField]
		private TextMeshProUGUI hint;

		// Token: 0x04000D81 RID: 3457
		[SerializeField]
		private Button editGameOptions;

		// Token: 0x04000D82 RID: 3458
		[SerializeField]
		private Button start;

		// Token: 0x04000D83 RID: 3459
		[SerializeField]
		private TextMeshProUGUI textOnStartButton;

		// Token: 0x04000D84 RID: 3460
		[SerializeField]
		private Image readyImage;

		// Token: 0x04000D85 RID: 3461
		private const string startSelectingMatsTerm = "Lobby/StartSelectingMats";

		// Token: 0x04000D86 RID: 3462
		private const string startTerm = "Lobby/Start";

		// Token: 0x04000D87 RID: 3463
		private const string readyTerm = "Lobby/Ready";

		// Token: 0x04000D88 RID: 3464
		private const string waitingForMorePlayersTerm = "Lobby/SLSWaitForAtLeastTwoPlayers";

		// Token: 0x04000D89 RID: 3465
		private const string waitingForOtherPlayersTerm = "Lobby/SLSWaitForOtherPlayers";

		// Token: 0x04000D8A RID: 3466
		private const string waitingForGameMasterTerm = "Lobby/WaitingForGameMaster";

		// Token: 0x04000D8B RID: 3467
		private const string adminInfoTerm = "Lobby/AdminInfo";

		// Token: 0x04000D8C RID: 3468
		private const string opponentChoosingTerm = "Lobby/OpponentChoosing";

		// Token: 0x04000D8D RID: 3469
		private const string selectMatsTerm = "Lobby/SelectMats";

		// Token: 0x04000D8E RID: 3470
		private const string clickReadyTerm = "Lobby/SLSTapReady";

		// Token: 0x04000D8F RID: 3471
		private LobbyGame roomData;

		// Token: 0x04000D90 RID: 3472
		private List<PlayerSlot> slots = new List<PlayerSlot>();

		// Token: 0x04000D91 RID: 3473
		private MatAndFactionChoose matAndFactionChoose;

		// Token: 0x04000D92 RID: 3474
		private bool choosingMats;
	}
}
