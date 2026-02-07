using System;
using System.Collections.Generic;
using AsmodeeNet.Utils.Extensions;
using DG.Tweening;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class MusicManager : MonoBehaviour
{
	// Token: 0x060008AD RID: 2221 RVA: 0x0002DD62 File Offset: 0x0002BF62
	private void Awake()
	{
		MusicManager.Instance = this;
		this.musicAudioSource = base.GetComponent<AudioSource>();
		if (GameController.GameManager != null)
		{
			GameController.GameManager.combatManager.OnCombatStageChanged += this.AdjustCombatMusic;
		}
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x0002DD98 File Offset: 0x0002BF98
	private void OnDestroy()
	{
		if (GameController.GameManager != null)
		{
			GameController.GameManager.combatManager.OnCombatStageChanged -= this.AdjustCombatMusic;
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x0002DDBC File Offset: 0x0002BFBC
	private void Update()
	{
		if (!this.musicAudioSource.isPlaying && !this.paused)
		{
			this.MusicFactionUpdate();
		}
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0002DDD9 File Offset: 0x0002BFD9
	public void StopMusic()
	{
		this.musicAudioSource.Stop();
		this.paused = true;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0002DDED File Offset: 0x0002BFED
	public void OnUndo()
	{
		GameController.GameManager.combatManager.OnCombatStageChanged += this.AdjustCombatMusic;
		this.ResetCombatMusicAfterUndo();
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0002DE10 File Offset: 0x0002C010
	public void InitGameMusic()
	{
		this.StopMusic();
		this.CreatePlayList();
		this.SetMusicFromPlaylist(this.playlistNormal);
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0002DE2A File Offset: 0x0002C02A
	private void CreatePlayList()
	{
		if (GameController.GameManager.IsMultiplayer)
		{
			this.CreatePlaylistMultiplayer();
			return;
		}
		if (GameController.GameManager.IsCampaign)
		{
			this.CreatePlaylistCampaign();
			return;
		}
		this.CreatePlaylistHotseat();
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00079E8C File Offset: 0x0007808C
	private void CreatePlaylistHotseat()
	{
		IEnumerable<Player> playersWithoutAI = GameController.GameManager.GetPlayersWithoutAI();
		this.playlistNormal.Clear();
		foreach (Player player in playersWithoutAI)
		{
			this.playlistNormal.Add(this.GetFactionStartingMusic(player.matFaction.faction));
		}
		this.Shuffle<AudioClip>(this.playlistNormal);
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00079F0C File Offset: 0x0007810C
	private void Shuffle<AudioClip>(IList<AudioClip> list)
	{
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int num = GameController.GameManager.random.Next(i + 1);
			AudioClip audioClip = list[num];
			list[num] = list[i];
			list[i] = audioClip;
		}
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0002DE58 File Offset: 0x0002C058
	private void CreatePlaylistMultiplayer()
	{
		this.playlistNormal.Add(this.GetFactionStartingMusic(GameController.GameManager.PlayerOwner.matFaction.faction));
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x0002DE7F File Offset: 0x0002C07F
	private void CreatePlaylistCampaign()
	{
		this.playlistNormal.Add(this.GetFactionStartingMusic(GameController.GameManager.GetPlayersWithoutAI().First<Player>().matFaction.faction));
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x0002DEAB File Offset: 0x0002C0AB
	private AudioClip GetFactionStartingMusic(Faction faction)
	{
		return this.factionStartingMusic[(int)faction];
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0002DEB5 File Offset: 0x0002C0B5
	private void ResetCombatMusicAfterUndo()
	{
		this.battleMusicActive = false;
		this.FactionFadeIn();
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0002DEC4 File Offset: 0x0002C0C4
	public void FactionFadeOut()
	{
		this.musicAudioSource.DOFade(0f, this.fadeOutTime).OnComplete(new TweenCallback(this.FactionFadeIn));
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0002DEEE File Offset: 0x0002C0EE
	private void FactionFadeIn()
	{
		this.StopMusic();
		this.MusicFactionUpdate();
		this.musicAudioSource.Play();
		this.musicAudioSource.DOFade(100f, this.fadeInTime);
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0002DF1E File Offset: 0x0002C11E
	private void MusicFactionUpdate()
	{
		if (this.battleMusicActive)
		{
			this.SetMusicFromPlaylist(this.playlistBattle);
			return;
		}
		this.SetMusicFromPlaylist(this.playlistNormal);
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00079F5C File Offset: 0x0007815C
	private void SetMusicFromPlaylist(List<AudioClip> playlist)
	{
		if (!this.musicAudioSource.isPlaying && playlist.Count > 0)
		{
			this.currentClip++;
			if (this.currentClip >= playlist.Count)
			{
				this.currentClip = 0;
			}
			this.musicAudioSource.clip = playlist[this.currentClip];
		}
		this.musicAudioSource.Play();
		this.paused = false;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00079FCC File Offset: 0x000781CC
	public void AdjustCombatMusic(CombatStage combatStage)
	{
		if (!this.CanPlayCombatMusic(combatStage))
		{
			return;
		}
		if (!this.battleMusicActive)
		{
			this.PrepareCombatMusic();
			this.battleMusicActive = true;
			return;
		}
		switch (combatStage)
		{
		case CombatStage.SelectingBattlefield:
			this.SetPlaylistBattle();
			this.AdjustBattleMusicToActivePlayers();
			return;
		case CombatStage.Diversion:
			this.AddDefenderMusicIfAble();
			return;
		case CombatStage.Preparation:
			this.AddDefenderMusicIfAble();
			return;
		case CombatStage.DeterminatingTheWinner:
		case CombatStage.EndingTheBattle:
			break;
		case CombatStage.CombatResovled:
			this.battleMusicActive = false;
			this.FactionFadeOut();
			break;
		default:
			return;
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0002DF41 File Offset: 0x0002C141
	private bool CanPlayCombatMusic(CombatStage combatStage)
	{
		if (this.CombatEnded(combatStage))
		{
			return this.PlayerWasInEndedCombat(combatStage);
		}
		return this.PlayerOwnerInCombat(combatStage);
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x0002873A File Offset: 0x0002693A
	private bool CombatEnded(CombatStage combatStage)
	{
		return combatStage == CombatStage.CombatResovled;
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0002DF65 File Offset: 0x0002C165
	private bool PlayerWasInEndedCombat(CombatStage combatStage)
	{
		return combatStage == CombatStage.CombatResovled && this.battleMusicActive;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0002DF76 File Offset: 0x0002C176
	private bool PlayerOwnerInCombat(CombatStage combatStage)
	{
		return !GameController.GameManager.IsMultiplayer || GameController.GameManager.combatManager.IsPlayerInCombat(GameController.GameManager.PlayerOwner);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0002DFA4 File Offset: 0x0002C1A4
	private void PrepareCombatMusic()
	{
		this.SetPlaylistBattle();
		this.musicAudioSource.DOFade(0f, this.fadeOutTime).OnComplete(new TweenCallback(this.PlayCombatMusic));
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0007A040 File Offset: 0x00078240
	private void SetPlaylistBattle()
	{
		this.playlistBattle.Clear();
		if (GameController.GameManager.IsMultiplayer)
		{
			this.playlistBattle.Add(this.GetFactionBattleMusic(GameController.GameManager.PlayerOwner.matFaction.faction));
			return;
		}
		this.playlistBattle.Add(this.GetFactionBattleMusic(GameController.GameManager.combatManager.GetAttacker().matFaction.faction));
		this.AddDefenderMusicIfAble();
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0007A0BC File Offset: 0x000782BC
	private void PlayCombatMusic()
	{
		this.musicAudioSource.Stop();
		this.musicAudioSource.clip = this.playlistBattle[0];
		this.musicAudioSource.Play();
		this.paused = false;
		this.musicAudioSource.DOFade(100f, this.fadeInTime);
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0007A114 File Offset: 0x00078314
	private void AddDefenderMusicIfAble()
	{
		if (GameController.GameManager.IsMultiplayer)
		{
			return;
		}
		Player defender = GameController.GameManager.combatManager.GetDefender();
		if (defender != null && !this.playlistBattle.Contains(this.GetFactionBattleMusic(defender.matFaction.faction)))
		{
			this.playlistBattle.Add(this.GetFactionBattleMusic(defender.matFaction.faction));
		}
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0002DFD4 File Offset: 0x0002C1D4
	private AudioClip GetFactionBattleMusic(Faction faction)
	{
		return this.factionBattleMusic[(int)faction];
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0002DFDE File Offset: 0x0002C1DE
	private void AdjustBattleMusicToActivePlayers()
	{
		if (!this.playlistBattle.Contains(this.musicAudioSource.clip))
		{
			this.ResetMusic();
		}
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0002DFFE File Offset: 0x0002C1FE
	private void ResetMusic()
	{
		this.musicAudioSource.Stop();
		this.musicAudioSource.DOFade(0f, this.fadeOutTime).OnComplete(new TweenCallback(this.MusicFactionUpdate));
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0002E033 File Offset: 0x0002C233
	public void PrepareFactoryMusic()
	{
		this.musicAudioSource.DOFade(0f, this.fadeOutTime).OnComplete(new TweenCallback(this.PlayFactoryMusic));
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0007A17C File Offset: 0x0007837C
	private void PlayFactoryMusic()
	{
		this.musicAudioSource.Stop();
		this.musicAudioSource.clip = this.factoryMusic;
		this.musicAudioSource.Play();
		this.paused = false;
		this.musicAudioSource.DOFade(100f, this.fadeInTime);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0002E05D File Offset: 0x0002C25D
	public void EndFactoryMusic()
	{
		this.FactionFadeOut();
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0002E065 File Offset: 0x0002C265
	public void PrepareWinnerMusic()
	{
		this.musicAudioSource.DOFade(0f, this.fadeOutTime).OnComplete(new TweenCallback(this.PlayWinnerMusic));
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0002E08F File Offset: 0x0002C28F
	private void PlayWinnerMusic()
	{
		this.musicAudioSource.clip = this.winTheme;
		this.musicAudioSource.Play();
		this.paused = false;
		this.musicAudioSource.DOFade(100f, this.fadeInTime);
	}

	// Token: 0x04000770 RID: 1904
	public static MusicManager Instance;

	// Token: 0x04000771 RID: 1905
	[SerializeField]
	private AudioClip[] factionStartingMusic;

	// Token: 0x04000772 RID: 1906
	[SerializeField]
	private AudioClip[] factionBattleMusic;

	// Token: 0x04000773 RID: 1907
	[SerializeField]
	private AudioClip factoryMusic;

	// Token: 0x04000774 RID: 1908
	[SerializeField]
	private AudioClip winTheme;

	// Token: 0x04000775 RID: 1909
	[SerializeField]
	private float fadeOutTime = 2f;

	// Token: 0x04000776 RID: 1910
	[SerializeField]
	private float fadeInTime = 2f;

	// Token: 0x04000777 RID: 1911
	private bool battleMusicActive;

	// Token: 0x04000778 RID: 1912
	private List<AudioClip> playlistNormal = new List<AudioClip>();

	// Token: 0x04000779 RID: 1913
	private List<AudioClip> playlistBattle = new List<AudioClip>();

	// Token: 0x0400077A RID: 1914
	private int currentClip = -1;

	// Token: 0x0400077B RID: 1915
	private AudioSource musicAudioSource;

	// Token: 0x0400077C RID: 1916
	private bool paused;
}
