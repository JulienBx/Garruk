using UnityEngine;

public class Furtivite : GameSkill
{
	public override void launch ()
	{
		Debug.Log ("Je lance Furtivité");
		GameController.instance.lookForValidation (true, "Furtivité cible le héros actif", "Lancer furtivité");
	}

	public override void resolve (int[] args)
	{
		Debug.Log ("Je résous Furtivité");
		int targetID = args [0];
		GameController.instance.setCannotBeTargeted ();
		GameController.instance.play (GameController.instance.getCurrentCard ().Title + " a lancé furtivité");
		GameController.instance.addModifier (GameController.instance.currentPlayingCard, GameController.instance.getCurrentSkill ().ManaCost, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Attack, 1);
	}

	public override bool isLaunchable (Skill s)
	{
		return true;
	}
}
