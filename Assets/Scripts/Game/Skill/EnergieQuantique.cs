using UnityEngine;
using System.Collections.Generic;

public class EnergieQuantique : GameSkill
{
	public EnergieQuantique()
	{
	
	}
	
	public override void launch()
	{
		GameController.instance.lookForValidation(true, "Choisir une cible à attaquer", "Lancer NRJ Quantique");
	}
	
	public override void resolve(int[] args)
	{
		int targetID ;
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		int myPlayerID = GameController.instance.currentPlayingCard;
		int currentPlayerID = GameController.instance.currentPlayingCard;
		List<int> potentialTargets = new List<int>();
		
		int debut ;
		if (currentPlayerID<5){
			debut = 5 ;
		}
		else{
			debut = 0 ;
		}
		PlayingCardController pcc ;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Energie quantique", 3, 2);
		
		for (int i = debut ; i < debut + 5 ; i++){
			pcc = GameController.instance.getPCC(i) ;
			if (!pcc.isDead && !(pcc.cannotBeTargeted==-1)){
				potentialTargets.Add(i);
			}
		}
		
		targetID = potentialTargets[Random.Range(0, potentialTargets.Count-1)];
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
		{                             
			GameController.instance.addModifier(targetID, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			GameController.instance.displaySkillEffect(targetID, "prend "+amount+" dégats", 3, 1);
		}
		else{
			GameController.instance.displaySkillEffect(targetID, "Esquive", 3, 0);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
