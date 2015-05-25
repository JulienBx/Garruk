﻿using UnityEngine;
using System.Collections;

public class Sape : GameSkill
{
	public Sape(string name)
	{
		skill = new Skill(name);
	}
	
	public override void launch(Skill skill)
	{
		Debug.Log("Je lance sape");
		this.skill = skill;
		GameController.instance.lookForTarget(this);
	}
	
	public override void setTarget(PlayingCardController pcc)
	{
		pcc.card.modifiers.Add(new StatModifier(-skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack));
	}
}
