using UnityEngine;
using System.Collections.Generic;

public class Renfoderme : GameSkill
{
	public Renfoderme()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Renfoderme";
		base.ciblage = 4 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}

	public override void resolve(List<int> targetsPCC)
	{	
		bool isSuccess = false ;
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				GameController.instance.applyOn(target);
				isSuccess = true ;
			}
			else{
				GameController.instance.esquive(target,39);
			}
		}
		
		if(GameView.instance.getCurrentCard().isGenerous()){
			List<int> targets = GameView.instance.getAllys();
			targets.Remove(target);
			target = targets[Random.Range(0,targets.Count)];
			GameController.instance.applyOn(target);
			isSuccess = true ;	
		}
		GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}
	
	public int getShieldBonus(int level){
		int shield = -1;
		if(level<2){
			shield = 5;
		}
		else if(level<4){
			shield = 10;
		}
		else if(level<5){
			shield = 15;
		}
		else if(level<7){
			shield = 20;
		}
		else if(level<8){
			shield = 25;
		}
		else if(level<10){
			shield = 30;
		}
		else{
			shield = 35;
		}
		return shield;
	}

	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int bonusShield = this.getShieldBonus(level);
		
		GameView.instance.getCard(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, text, "Bouclier : "+bonusShield+"%. Permanent"));
		GameView.instance.displaySkillEffect(target, base.name+"\nBouclier : "+bonusShield+"%. Permanent", 1);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}	

	public override string getTargetText(int target){
		
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = this.getShieldBonus(level);
		
		text += "\nAjoute un bouclier "+bonusShield+"%";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
