using UnityEngine;
using System.Collections.Generic;

public class Reflexe : GameSkill
{
	public new List<StatModifier> StatModifiers = new List<StatModifier>();

	public void Init()
	{
		StatModifiers.Add(new StatModifier(Skill.Power, Skill.XMin, Skill.Ponderation, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}
	public override void launch()
	{
		GameNetworkCard gnc = transform.parent.parent.GetComponent<GameNetworkCard>();
		if (!GameBoard.instance.TimeOfPositionning && GameTimeLine.instance.PlayingCard.Equals(gnc) 
		    && !GamePlayingCard.instance.attemptToAttack && !GamePlayingCard.instance.hasAttacked
		    && gnc.photonView.isMine)
		{
			GameTile.instance.SetCursorToTarget();
			GamePlayingCard.instance.SkillCasted = this.SkillNumber;
			GamePlayingCard.instance.attemptToCast = true;
		}

	}
	public override void Apply(int target)
	{
		GameObject go = PhotonView.Find(target).gameObject;
		GameNetworkCard gnc = go.GetComponent<GameNetworkCard>();
		foreach (StatModifier sm in StatModifiers)
		{
			gnc.Card.modifiers.Add(sm);
		}
		Instantiate(gnc.AttackAnim, go.transform.position + new Vector3(0, 0, -2), Quaternion.identity);
		GameTimeLine.instance.SortCardsBySpeedAfterBuff();
		GameTimeLine.instance.Arrange();
		if (GamePlayingCard.instance.gameCard.Card.Equals(gnc.Card))
		{
			GamePlayingCard.instance.changeStats();
		}
		if (GameHoveredCard.instance.gameCard.Card != null)
		{
			if (GameHoveredCard.instance.gameCard.Card.Equals(gnc.Card))
			{
				GameHoveredCard.instance.changeStats();
			}
		}
		gnc.ShowFace();

	}
}
