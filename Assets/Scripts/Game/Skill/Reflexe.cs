using UnityEngine;
using System.Collections.Generic;

public class Reflexe : GameSkill
{
	public override void launch()
	{
		Debug.Log("Je lance réflexe");
		GameController.instance.lookForTarget(this);
		//StatModifiers.Add(new StatModifier(Skill.Power, Skill.XMin, Skill.Ponderation, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}

	public override void setTarget(PlayingCardController pcc)
	{
		pcc.card.modifiers.Add(new StatModifier(Mathf.CeilToInt(skill.XMin + skill.Power * skill.Ponderation), ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}
}
