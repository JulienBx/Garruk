using UnityEngine;
using System.Collections.Generic;

public class Apathie : Buff, ISkill
{
	public void Init()
	{
		StatModifiers.Add(new StatModifier(Skill.Power, -Skill.XMin, -Skill.Ponderation, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}
}
