using UnityEngine;

public class TirALarc : GameSkill
{
	public override void launch(Skill skill)
	{
		Debug.Log("Je lance tir à l'arc");
		this.skill = skill;
		GameController.instance.lookForTarget(this);
	}
	
	public override void setTarget(PlayingCardController pcc)
	{
		pcc.card.modifiers.Add(new StatModifier(-skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life));
	}
}
