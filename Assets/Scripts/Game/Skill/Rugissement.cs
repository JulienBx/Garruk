using UnityEngine;
using System.Collections.Generic;

public class Rugissement : GameSkill
{
	public Rugissement()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Cri de rage";
		base.ciblage = 4 ;
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		List<int> targets = GameView.instance.getAllys();
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;
		int numberOfTargets = this.getNumberOfTargets(level);
		int target;
		
		for(int i = 0 ; i < numberOfTargets; i++){
			target = targets[Random.Range(0,targets.Count)];
			if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
				GameController.instance.esquive(target,1);
			}
			else{
				if (Random.Range(1,101) < proba){
					GameController.instance.applyOn(target);
				}
				else{
					GameController.instance.esquive(target,19);
				}
			}
			targets.Remove(target);
		}
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public int getNumberOfTargets(int level){
		int numberOfTargets = -1;
		if(level<4){
			numberOfTargets = 1;
		}
		else if(level<7){
			numberOfTargets = 2;
		}
		else{
			numberOfTargets = 3;
		}
		return numberOfTargets;
	}
	
	public int getAttackBonus(int level){
		int attackBonus = -1;
		if(level<2){
			attackBonus = 2;
		}
		else if(level<5){
			attackBonus = 3;
		}
		else if(level<8){
			attackBonus = 4;
		}
		else if(level<10){
			attackBonus = 5;
		}
		else{
			attackBonus = 6;
		}
		return attackBonus;
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		int bonusAttack = this.getAttackBonus(level);
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(bonusAttack, 1, 19, text, "+"+bonusAttack+" ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n"+"+"+bonusAttack+" ATK. Actif 1 tour", 1);
	}
}
