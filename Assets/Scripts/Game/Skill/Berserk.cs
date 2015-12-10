using UnityEngine;
using System.Collections.Generic;

public class Berserk : GameSkill
{
	public Berserk(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Berserk";
		base.ciblage = 1 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		bool isSuccess = false ;
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			GameController.instance.applyOn(target);
			isSuccess = true ;
		}
		GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getDamagesAgainst(targetCard, this.getPercentageAttack(level));
		int lifeDamages = (int)Mathf.Min(currentCard.getLife(),currentCard.getLife()*this.getPercentageLife(level)/100f);
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, "-"+damages+" PV"));
		GameView.instance.getPlayingCardController(target).updateLife();
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(lifeDamages, -1, 0, text, "-"+damages+" PV"));
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).updateLife();
		
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public int getPercentageAttack(int level){
		int percentage = -1;
		if(level<4){
			percentage = 110;
		}
		else if(level<6){
			percentage = 120;
		}
		else if(level<8){
			percentage = 130;
		}
		else if(level<10){
			percentage = 140;
		}
		else {
			percentage = 150;
		}
		return percentage;
	}
	
	public int getPercentageLife(int level){
		int percentage = -1;
		if(level<2){
			percentage = 40;
		}
		else if(level<3){
			percentage = 35;
		}
		else if(level<5){
			percentage = 30;
		}
		else if(level<7){
			percentage = 25;
		}
		else if(level<9){
			percentage = 20;
		}
		else {
			percentage = 15;
		}
		return percentage;
	}
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getDamagesAgainst(targetCard, this.getPercentageAttack(level));
		int lifeDamages = (int)Mathf.Min(currentCard.getLife(),currentCard.getLife()*this.getPercentageLife(level)/100f);
		
		text += "\nPV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
