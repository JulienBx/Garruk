using UnityEngine;
using System.Collections.Generic;

public class Reflexe : GameSkill
{
	public new List<StatModifier> StatModifiers = new List<StatModifier>();

	public void Init()
	{
		StatModifiers.Add(new StatModifier(Skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}
	void OnMouseDown()
	{
		GamePlayingCard.instance.SkillCasted = this;
		GamePlayingCard.instance.attemptToCast = true;
	}
	public override void Apply(int target)
	{
		photonView.RPC("SendChanges", PhotonTargets.AllBuffered, target);
	}

	// RPC
	public void SendChanges(int target)
	{
		GameObject go = PhotonView.Find(target).gameObject;
		GameNetworkCard gnc = go.GetComponent<GameNetworkCard>();
		foreach (StatModifier sm in StatModifiers)
		{
			gnc.Card.modifiers.Add(sm);
		}

	}
}
