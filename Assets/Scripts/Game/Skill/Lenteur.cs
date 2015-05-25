using UnityEngine;
using System.Collections.Generic;

public class Lenteur : GameSkill
{
	public Lenteur(string name)
	{
		skill = new Skill(name);
	}
	
	public override void launch(Skill skill)
	{
		Debug.Log("Je lance Lenteur");
		this.skill = skill;
		GameController.instance.lookForTarget(this);
	}
	
	public override void setTarget(PlayingCardController pcc)
	{
		pcc.card.modifiers.Add(new StatModifier(-skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move));
	}
}
