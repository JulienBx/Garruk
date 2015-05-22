using UnityEngine;
using System.Collections.Generic;

public class Reflexe : GameSkill
{
	public Reflexe(string name)
	{
		skill = new Skill(name);
	}

	public override void launch(Skill skill)
	{
		Debug.Log("Je lance réflexe");
		this.skill = skill;
		GameController.instance.lookForTarget(this);
	}

	public override void setTarget(PlayingCardController pcc)
	{
		//pcc.card.modifiers.Add(new StatModifier(Mathf.CeilToInt(skill.XMin + skill.Power * skill.Ponderation), ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
		pcc.card.modifiers.Add(new StatModifier(skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}
}
