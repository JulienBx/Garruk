using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite()
	{
	}
	
	public override void launch()
	{
		GameController.instance.lookForValidation (true, "Agilité cible le héros actif", "Lancer agilité");
	}

	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost ;
		int myPlayerID = GameController.instance.currentPlayingCard;
		string myPlayerName = GameController.instance.getCurrentCard().Title;
		GameController.instance.setEsquive (amount);
		GameController.instance.displaySkillEffect(myPlayerID, myPlayerName+" gagne "+amount+"% d'esquive", 3, 0);
		GameController.instance.play();
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
