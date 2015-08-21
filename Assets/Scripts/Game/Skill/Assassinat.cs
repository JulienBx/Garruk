using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		int successChances = base.skill.ManaCost;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.failedToCastOnSkill(target, 2);
			}
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		int currentLife = GameView.instance.getCard(target).GetLife();
		GameController.instance.addCardModifier(target, currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		if(indexFailure==1){
			GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
		}
		else{
			GameView.instance.displaySkillEffect(target, "ECHEC ASSASSINAT", 4);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(Card targetCard){
		
		int chances = base.skill.ManaCost;
		
		int probaEsquive = targetCard.GetEsquive();
		int proba = (chances)*(100-probaEsquive)/100;
		string text = "HIT : ";
		if (probaEsquive!=0){
			text+=proba+"% : "+chances+"%(ASS) - "+probaEsquive+"%(ESQ)";
		}
		else{
			text+=proba+"% : ";
		}
		
		return text ;
	}
}
