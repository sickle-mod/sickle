using System;
using DG.Tweening;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.UI;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001EF RID: 495
	public class TokenPresenter : MonoBehaviour, ITooltipInfo, ISeismograph
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x00031512 File Offset: 0x0002F712
		public GameHexPresenter Hex
		{
			get
			{
				return GameController.Instance.GetGameHexPresenter(this.LogicToken.Position);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x00031529 File Offset: 0x0002F729
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x00031531 File Offset: 0x0002F731
		public FactionAbilityToken LogicToken { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0003153A File Offset: 0x0002F73A
		public Player Owner
		{
			get
			{
				if (this.LogicToken != null)
				{
					return this.LogicToken.Owner;
				}
				return null;
			}
		}

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06000E50 RID: 3664 RVA: 0x000899D0 File Offset: 0x00087BD0
		// (remove) Token: 0x06000E51 RID: 3665 RVA: 0x00089A04 File Offset: 0x00087C04
		public static event TokenPresenter.TokenInteraction UnitTokenInteraction;

		// Token: 0x06000E52 RID: 3666 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00031551 File Offset: 0x0002F751
		public void SetTokenLogic(FactionAbilityToken logicToken)
		{
			this.LogicToken = logicToken;
			this.UpdateMeshAndMaterial();
			this.UpdateRotation();
			this.UpdateActiveState();
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0003156C File Offset: 0x0002F76C
		public void UpdatePresenter(Unit unit)
		{
			if (this.LogicToken is TrapToken)
			{
				this.UpdateTrapToken(unit);
			}
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00089A38 File Offset: 0x00087C38
		private void UpdateMeshAndMaterial()
		{
			if (this.LogicToken is FlagToken)
			{
				this.meshFilter.mesh = this.meshes[0];
				this.meshRenderer.material = this.materials[0];
				return;
			}
			if (this.LogicToken is TrapToken)
			{
				this.meshFilter.mesh = this.meshes[1];
				switch (((TrapToken)this.LogicToken).Penalty)
				{
				case PayType.Coin:
					this.meshRenderer.material = this.materials[4];
					return;
				case PayType.Popularity:
					this.meshRenderer.material = this.materials[1];
					return;
				case PayType.Power:
					this.meshRenderer.material = this.materials[3];
					return;
				case PayType.CombatCard:
					this.meshRenderer.material = this.materials[2];
					return;
				case PayType.Resource:
					this.meshRenderer.material = null;
					return;
				default:
					Debug.LogError("Unexpected trap token penalty");
					break;
				}
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00031582 File Offset: 0x0002F782
		public void UpdateActiveState()
		{
			if (this.LogicToken == null)
			{
				this.SetActive(false);
				return;
			}
			if (!base.gameObject.activeSelf)
			{
				this.SetActive(true);
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00089B30 File Offset: 0x00087D30
		private void UpdateRotation()
		{
			if (this.LogicToken is FlagToken)
			{
				base.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				return;
			}
			if (this.LogicToken is TrapToken)
			{
				TrapToken trapToken = this.LogicToken as TrapToken;
				this.SetRotation((!trapToken.Armed) ? new Vector3(0f, 180f, 0f) : new Vector3(0f, 0f, 0f));
				this.frontVisible = !trapToken.Armed;
			}
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00089BCC File Offset: 0x00087DCC
		private void UpdateTrapToken(Unit unit)
		{
			TrapToken trapToken = this.LogicToken as TrapToken;
			if (!trapToken.Armed && !this.frontVisible && (GameController.GameManager.PlayerCurrent.matFaction.faction != Faction.Togawa || (unit != null && unit.Owner.matFaction.faction != Faction.Togawa)) && this.currentAnimation == null)
			{
				this.TrapTriggeredAnimation();
				if (TokenPresenter.UnitTokenInteraction != null)
				{
					TokenPresenter.UnitTokenInteraction(trapToken);
					return;
				}
			}
			else if (trapToken.Armed && this.frontVisible && GameController.GameManager.PlayerCurrent.matFaction.faction == Faction.Togawa)
			{
				this.TrapArmingAnimation();
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00089C70 File Offset: 0x00087E70
		public void ShowAndPlaceToken(FactionAbilityToken logicToken, Vector3 from)
		{
			this.SetTokenLogic(logicToken);
			if (((GameController.GameManager.IsMultiplayer && logicToken.Owner == GameController.GameManager.PlayerOwner) || (!GameController.GameManager.IsMultiplayer && logicToken.Owner == GameController.GameManager.PlayerCurrent && GameController.GameManager.PlayerCurrent.IsHuman)) && !GameController.GameManager.SpectatorMode)
			{
				this.ShowAndPlaceAnimationOwner(from, this.Hex.GetTokenPosition());
				return;
			}
			this.ShowAndPlaceAnimationSpectator(from, this.Hex.GetTokenPosition());
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x000315A8 File Offset: 0x0002F7A8
		private void SetPosition(Vector3 position)
		{
			base.transform.position = position;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x000315B6 File Offset: 0x0002F7B6
		private void SetRotation(Vector3 rotation)
		{
			base.transform.localEulerAngles = rotation;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x000315C4 File Offset: 0x0002F7C4
		private void OnMouseUpAsButton()
		{
			this.OnClickTrap();
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00089D04 File Offset: 0x00087F04
		private void ShowAndPlaceAnimationOwner(Vector3 from, Vector3 to)
		{
			float num = 0.5f;
			this.SetPosition(from);
			this.UpdateRotation();
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			localEulerAngles.y = (float)((this.LogicToken is FlagToken) ? 180 : 0);
			base.transform.LookAt(GameController.Instance.cameraControler.transform);
			this.currentAnimation = DOTween.Sequence();
			this.currentAnimation.Append(base.transform.DOMove(base.transform.position + base.transform.forward * 4f, 0.2f, false));
			this.currentAnimation.Append(base.transform.DOMove(base.transform.position + base.transform.forward * 4f, 0.5f, false));
			this.currentAnimation.Append(base.transform.DOMoveX(to.x, num, false));
			this.currentAnimation.Join(base.transform.DOMoveZ(to.z, num, false));
			this.currentAnimation.Join(base.transform.DOMoveY(0f, num, false));
			this.currentAnimation.Join(base.transform.DOLocalRotate(localEulerAngles, num, RotateMode.Fast));
			this.currentAnimation.Play<Sequence>().OnComplete(delegate
			{
				this.OnPlacingComplete();
			});
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00089E8C File Offset: 0x0008808C
		private void ShowAndPlaceAnimationSpectator(Vector3 from, Vector3 to)
		{
			float num = 0.5f;
			this.SetPosition(from);
			this.currentAnimation = DOTween.Sequence();
			this.currentAnimation.Append(base.transform.DOMoveX(to.x, num, false));
			this.currentAnimation.Join(base.transform.DOMoveZ(to.z, num, false));
			this.currentAnimation.Join(base.transform.DOMoveY(to.y, num, false));
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			localEulerAngles.z += 360f;
			this.currentAnimation.Join(base.transform.DOLocalRotate(localEulerAngles, num, RotateMode.FastBeyond360));
			this.currentAnimation.Play<Sequence>().OnComplete(delegate
			{
				this.OnPlacingComplete();
			});
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000315CC File Offset: 0x0002F7CC
		private void OnPlacingComplete()
		{
			this.currentAnimation = null;
			this.frontVisible = this.LogicToken is FlagToken;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00089F64 File Offset: 0x00088164
		private void TrapTriggeredAnimation()
		{
			float num = 1f;
			float num2 = 2f;
			this.SetPosition(base.transform.position);
			this.SetRotation(Vector3.zero);
			this.currentAnimation = DOTween.Sequence();
			this.currentAnimation.Append(base.transform.DOLocalRotate(new Vector3(0f, 180f, 0f), num, RotateMode.Fast));
			this.currentAnimation.Join(base.transform.DOMoveY(base.transform.position.y + num2, num, false).SetEase(GameController.Instance.unitsVerticalEase));
			this.currentAnimation.Play<Sequence>().OnComplete(delegate
			{
				this.OnTrapTriggeredComplete();
			});
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x000315E9 File Offset: 0x0002F7E9
		private void OnTrapTriggeredComplete()
		{
			this.currentAnimation = null;
			this.frontVisible = true;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0008A028 File Offset: 0x00088228
		private void TrapArmingAnimation()
		{
			float num = 1f;
			float num2 = 2f;
			this.SetPosition(base.transform.position);
			this.SetRotation(Vector3.right * 180f);
			this.frontVisible = false;
			this.currentAnimation = DOTween.Sequence();
			this.currentAnimation.Append(base.transform.DOLocalRotate(new Vector3(0f, 360f, 0f), num, RotateMode.Fast));
			this.currentAnimation.Join(base.transform.DOMoveY(base.transform.position.y + num2, num, false).SetEase(GameController.Instance.unitsVerticalEase));
			this.currentAnimation.Play<Sequence>().OnComplete(delegate
			{
				this.OnTrapArmingComplete();
			});
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x000315F9 File Offset: 0x0002F7F9
		private void OnTrapArmingComplete()
		{
			this.currentAnimation = null;
			this.frontVisible = false;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0008A100 File Offset: 0x00088300
		private void TrapRotateAnimation()
		{
			float num = 1f;
			float num2 = 2f;
			float num3 = 3f;
			this.SetPosition(base.transform.position);
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			localEulerAngles.y = 180f;
			this.currentAnimation = DOTween.Sequence();
			this.currentAnimation.Append(base.transform.DOLocalRotate(localEulerAngles, num, RotateMode.Fast));
			this.currentAnimation.Join(base.transform.DOMoveY(base.transform.position.y + num2, num, false).SetEase(GameController.Instance.unitsVerticalEase));
			this.currentAnimation.Append(base.transform.DOLocalRotate(localEulerAngles, num3, RotateMode.Fast));
			localEulerAngles.y = 360f;
			this.currentAnimation.Append(base.transform.DOLocalRotate(localEulerAngles, num, RotateMode.Fast));
			this.currentAnimation.Join(base.transform.DOMoveY(base.transform.position.y + num2, num, false).SetEase(GameController.Instance.unitsVerticalEase));
			this.currentAnimation.Play<Sequence>().OnComplete(delegate
			{
				this.OnTrapRotateComplete();
			});
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x000315F9 File Offset: 0x0002F7F9
		private void OnTrapRotateComplete()
		{
			this.currentAnimation = null;
			this.frontVisible = false;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00031609 File Offset: 0x0002F809
		public void ForceFinishAnimation()
		{
			if (this.currentAnimation.Equals(null))
			{
				return;
			}
			this.currentAnimation.Complete(true);
			this.currentAnimation = null;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0008A240 File Offset: 0x00088440
		public void OnClickTrap()
		{
			if (this.LogicToken is TrapToken && this.GetPlayer().matFaction.faction == Faction.Togawa && (this.LogicToken as TrapToken).Armed && this.currentAnimation == null && !GameController.GameManager.SpectatorMode)
			{
				Debug.Log("spectator mode " + GameController.GameManager.SpectatorMode.ToString());
				this.TrapRotateAnimation();
			}
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0008A2BC File Offset: 0x000884BC
		public void OnQuakeDetected(Vector3 epicenter, float force, float radius)
		{
			if (this.jump)
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

		// Token: 0x06000E69 RID: 3689 RVA: 0x0003162D File Offset: 0x0002F82D
		private void OnJumpComplete()
		{
			this.jump = false;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0008A35C File Offset: 0x0008855C
		private Player GetPlayer()
		{
			Player player;
			if (GameController.GameManager.IsMultiplayer)
			{
				player = GameController.GameManager.PlayerOwner;
			}
			else if (!GameController.GameManager.PlayerCurrent.IsHuman)
			{
				player = GameController.GameManager.GetPreviousHumanPlayer();
			}
			else
			{
				player = GameController.GameManager.PlayerCurrent;
			}
			return player;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00031636 File Offset: 0x0002F836
		private void PlayPlaceSound()
		{
			WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildArmory, AudioSourceType.WorldSfx);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00031636 File Offset: 0x0002F836
		private void PlayTrapTriggeredSound()
		{
			WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildArmory, AudioSourceType.WorldSfx);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00031636 File Offset: 0x0002F836
		private void PlayTrapArmedSound()
		{
			WorldSFXManager.PlaySound(SoundEnum.PlayersBoardBuildArmory, AudioSourceType.WorldSfx);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0008A3B0 File Offset: 0x000885B0
		public string InfoBasic()
		{
			string text = ((this.LogicToken is FlagToken) ? "FlagToken" : "TrapToken");
			return ScriptLocalization.Get("Tooltips/" + text + "Basic");
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0008A3EC File Offset: 0x000885EC
		public string InfoAdv()
		{
			string text = ((this.LogicToken is FlagToken) ? "FlagToken" : "TrapToken");
			if (this.LogicToken is TrapToken && GameController.GameManager.PlayerMaster == this.LogicToken.Owner)
			{
				text += (this.LogicToken as TrapToken).Penalty.ToString();
			}
			return ScriptLocalization.Get("Tooltips/" + text + "Adv");
		}

		// Token: 0x04000B4E RID: 2894
		private bool frontVisible;

		// Token: 0x04000B4F RID: 2895
		[SerializeField]
		private MeshFilter meshFilter;

		// Token: 0x04000B50 RID: 2896
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x04000B51 RID: 2897
		[SerializeField]
		private Collider tokenCollider;

		// Token: 0x04000B52 RID: 2898
		[SerializeField]
		private ParticleSystem effects;

		// Token: 0x04000B53 RID: 2899
		private Sequence currentAnimation;

		// Token: 0x04000B54 RID: 2900
		[SerializeField]
		private Mesh[] meshes;

		// Token: 0x04000B55 RID: 2901
		[SerializeField]
		private Material[] materials;

		// Token: 0x04000B57 RID: 2903
		[SerializeField]
		private float mass = 0.2f;

		// Token: 0x04000B58 RID: 2904
		private float forceAmortization = 50f;

		// Token: 0x04000B59 RID: 2905
		private bool jump;

		// Token: 0x020001F0 RID: 496
		// (Invoke) Token: 0x06000E78 RID: 3704
		public delegate void TokenInteraction(TrapToken trap);
	}
}
