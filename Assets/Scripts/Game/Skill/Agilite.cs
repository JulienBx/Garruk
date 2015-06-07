using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite()
	{
	}
	
	public override void launch()
	{
		Debug.Log ("Je lance Agilité");
		GameController.instance.lookForValidation (true, "Agilité cible le héros actif", "Lancer agilité");
	}

	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power ;
		
		GameController.instance.getCurrentPCC ().changeEsquive ("Esquive", "Le héros possède "+amount+" % de chances d'esquiver les dégats");
		
		GameController.instance.getCurrentCard().modifiers.Add(new StatModifier(amount, ModifierType.Type_EsquivePercentage));
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
		                             " possède désormais " 
		                             + amount 
		                             + " % de chances de d'esquiver les dégats" 
		                             );
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
