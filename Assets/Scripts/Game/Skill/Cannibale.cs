using UnityEngine;
using System.Collections.Generic;

public class Cannibale : GameSkill
{
	public Cannibale(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Cannibale" ;
		base.ciblage = 2 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentAllyTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			GameController.instance.applyOn(target);
		}
	}
	
	public int getBonusPercentage(int level){
		int percentage = -1;
		if(level<10){
			percentage=5+5*level;
		}
		else{
			percentage = 60 ;
		}
		return percentage ;
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = targetCard.getLife();
		int percentage = this.getBonusPercentage(GameView.instance.getCurrentSkill().Power);
		int bonusLife = Mathf.FloorToInt(targetCard.getLife()*percentage/100f);
		int bonusAttack = Mathf.FloorToInt(targetCard.getAttack()*percentage/100f);
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, damages+" dégats subis"));
		GameView.instance.getPlayingCardController(target).updateLife();
		
		GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).attackModifyers.Add(new Modifyer(bonusAttack, -1, 21, text, "+"+bonusAttack+" ATK. Permanent"));
		GameView.instance.getCard(GameView.instance.getCurrentPlayingCard()).pvModifyers.Add(new Modifyer(bonusLife, -1, 21, text, "+"+bonusLife+" PV. Permanent"));
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\n+"+bonusAttack+" ATK\n+"+bonusLife+" PV", 0);
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).updateAttack();
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).updateLife();
	}

	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = targetCard.getLife();
		int percentage = this.getBonusPercentage(GameView.instance.getCurrentSkill().Power);
		int bonusLife = Mathf.FloorToInt(targetCard.getLife()*percentage/100f);
		int bonusAttack = Mathf.FloorToInt(targetCard.getAttack()*percentage/100f);
		
		text += "\nAbsorbe "+bonusAttack+" ATK et "+bonusLife+" PV";
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
