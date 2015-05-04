using UnityEngine;
using System.Collections.Generic;

public class Reflexe : GameSkill
{
	public override void launch()
	{
		Debug.Log ("Je lance réflexe");
		//StatModifiers.Add(new StatModifier(Skill.Power, Skill.XMin, Skill.Ponderation, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}
}
