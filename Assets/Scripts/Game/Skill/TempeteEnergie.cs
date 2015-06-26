using UnityEngine;
using System.Collections.Generic;

public class TempeteEnergie : GameSkill
{
	public TempeteEnergie()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int arg ;
		int manacost = GameController.instance.getCurrentSkill().ManaCost;
		GameController.instance.startPlayingSkill();
		int success = 0 ;
		
		for(int i = 0 ; i < GameController.instance.playingCards.Length;i++){
			if(!GameController.instance.getPCC(i).isDead){
				if (Random.Range(1,101) > GameController.instance.getCard(i).GetMagicalEsquive())
				{ 
					arg = (Random.Range(5,manacost+1));
					GameController.instance.applyOn(i, arg);
					success = 1 ;
				}
			}
		}
		
		GameController.instance.playSkill(success);
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		GameController.instance.addCardModifier(target, arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.displaySkillEffect(target, "-"+arg+" PV", 3, 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override string getSuccessText(){
		return "A lancé tempete d'énergie" ;
	}
}
