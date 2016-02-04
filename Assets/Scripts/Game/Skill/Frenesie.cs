using UnityEngine;
using System.Collections.Generic;

public class Frenesie : GameSkill
{
	public Frenesie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Frénésie";
		base.ciblage = 0 ;
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOn(-1);
		GameController.instance.showResult(true);
		GameController.instance.endPlay();
	}
	
	public int getAttackBonus(int level){
		int attackBonus = 0 ;
		if(level<=2){
			attackBonus = 2;
		}
		else if(level<=4){
			attackBonus = 3;
		}
		else if(level<=6){
			attackBonus = 4;
		}
		else if(level<=8){
			attackBonus = 5;
		}
		else if(level<=10){
			attackBonus = 6;
		}
		return attackBonus;
	}
	
	public int getLifeMalus(int level){
		int lifeMalus = 0 ;
		if(level<=1){
			lifeMalus = 10;
		}
		else if(level<=3){
			lifeMalus = 8;
		}
		else if(level<=5){
			lifeMalus = 6;
		}
		else if(level<=7){
			lifeMalus = 4;
		}
		else if(level<=9){
			lifeMalus = 2;
		}
		return lifeMalus;
	}
	
	public override void applyOn(int i){
		string text = base.name;
		int level = GameView.instance.getCurrentSkill().Power;
		
		int bonusAttack = this.getAttackBonus(level);
		GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).attackModifyers.Add(new Modifyer(bonusAttack, 1, 18, text, "+"+level+" ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).updateAttack();
		
		int lifeMalus = this.getLifeMalus(level);
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(lifeMalus, -1, 0, text, "+"+bonusAttack+" ATK\n-"+lifeMalus+" PV"));
	}
}
