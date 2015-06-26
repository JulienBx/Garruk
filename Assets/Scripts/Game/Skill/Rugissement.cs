using UnityEngine;
using System.Collections.Generic;

public class Rugissement : GameSkill
{
	public Rugissement()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets ;
		GameController.instance.startPlayingSkill();
		int success = 0 ;
		 
		for(int i = 0 ; i < GameController.instance.playingCards.Length;i++){
			if(i!=GameController.instance.currentPlayingCard && !GameController.instance.getPCC(i).isDead && GameController.instance.getPCC(i).isMine){
				if (Random.Range(1,101) > GameController.instance.getCard(i).GetMagicalEsquive())
				{
					GameController.instance.applyOn(i);
					success = 1 ; 
				}
				else{
					GameController.instance.failedToCastOnSkill(i, 1);
				}
			}
		}
		
		GameController.instance.playSkill(success);
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 9, "Renforcement", "Attaque augmentée de "+amount, "Actif 1 tour");
		GameController.instance.displaySkillEffect(target, "+"+amount+" ATK", 3, 0);
		
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return (GameController.instance.nbMyPlayersAlive()>0) ;
	}
	
	public override string getSuccessText(){
		return "Rugissement" ;
	}
}
