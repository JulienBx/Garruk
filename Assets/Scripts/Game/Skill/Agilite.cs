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
		int amount = GameController.instance.getCurrentSkill().ManaCost ;
		
		GameController.instance.setEsquive (amount);
		
		
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
		                             " acquiert " 
		                             + amount 
		                             + " % de chances de d'esquiver les dégats" 
		                             );
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
