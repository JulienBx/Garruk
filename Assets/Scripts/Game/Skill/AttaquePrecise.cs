using UnityEngine;

public class AttaquePrecise : GameSkill
{
	public override void launch()
	{
		Debug.Log ("Je lance attaque precise");
		//StatModifiers.Add(new StatModifier(Skill.Power, Skill.XMin, Skill.Ponderation, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
	}
}
