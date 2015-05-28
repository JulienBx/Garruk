using UnityEngine;

public class Furtivite : GameSkill
{
	public override void launch()
	{
		Debug.Log("Je lance Furtivité");
		GameController.instance.lookForValidation(true, "Furtivité cible le héros actif", "Lancer furtivité");
	}
	
	public override void resolve(int[] args)
	{
		Debug.Log("Je résous Furtivité");
		int targetID = args [0];
		GameController.instance.getCurrentPCC().setCannotBeTargeted(true);
		GameController.instance.play(GameController.instance.getCurrentCard().Title + " a lancé furtivité");
		GameController.instance.getCurrentCard().modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
		GameController.instance.reloadTimeline();
	}
}
