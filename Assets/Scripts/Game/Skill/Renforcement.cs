﻿using UnityEngine;
using System.Collections;

public class Renforcement : Buff, ISkill
{
	public void Init()
	{
		StatModifiers.Add(new StatModifier(Skill.Power, Skill.XMin, Skill.Ponderation, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack));
	}
}
