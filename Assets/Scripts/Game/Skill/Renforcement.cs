using UnityEngine;
using System.Collections;

public class Renforcement : GameSkill
{
	public Renforcement(string name)
	{
		skill = new Skill(name);
	}
	
	public override void launch(Skill skill)
	{
		Debug.Log("Je lance renforcement");
		this.skill = skill;
		GameController.instance.lookForTarget(this);
	}
	
	public override void setTarget(PlayingCardController pcc)
	{
		pcc.card.modifiers.Add(new StatModifier(-skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack));
	}
}
