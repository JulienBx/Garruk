using UnityEngine;
using System.Collections.Generic;

public class Adrenaline : GameSkill
{
	public Adrenaline()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Adrénaline";
		base.ciblage = 4 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}
		
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,6);
			}
		}
		
		if(GameView.instance.getCurrentCard().isGenerous()){
			List<int> targets = GameView.instance.getAllys();
			targets.Remove(target);
			target = targets[Random.Range(0,targets.Count)];
			GameController.instance.applyOn(target);	
		}
		
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public int getMoveBonus(int level){
		int move = -1;
		if(level<3){
			move = 1;
		}
		else if(level<5){
			move = 2;
		}
		else if(level<9){
			move = 3;
		}
		else{
			move = 4;
		}
		return move;
	}
	
	public int getDurationBonus(int level){
		int duration = -1;
		if(level<7){
			duration = 1;
		}
		else{
			duration = 2;
		}
		return duration;
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int bonusMove = this.getMoveBonus(level);
		int bonusDuration = this.getDurationBonus(level);
		
		if(bonusDuration==1){
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(bonusMove, bonusDuration, 6, text, "+"+bonusMove+" MOV. Actif 1 tour"));
		}
		else{
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(bonusMove, bonusDuration, 6, text, "+"+bonusMove+" MOV. Actif "+bonusDuration+" tours"));
		}
		
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.recalculateDestinations();
		GameView.instance.displaySkillEffect(target, base.name+"\n+"+bonusMove+" MOV. Actif 1 tour", 1);
	}	
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int bonusMove = this.getMoveBonus(level);
		int bonusDuration = this.getDurationBonus(level);
		
		if(bonusDuration==1){
			text += "\n+"+bonusMove+" MOV pendant "+bonusDuration+" tour";
		}
		else{
			text += "+"+bonusMove+" MOV pendant "+bonusDuration+" tours";
		}
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
