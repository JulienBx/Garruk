using UnityEngine;
using System.Collections.Generic;

public class Laser : GameSkill
{
	public Laser()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Laser";
		base.ciblage = 3 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;
	
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(10+level, 20+2*level+1));
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
		if(currentCard.isFou()){
			GameController.instance.launchFou(22,GameView.instance.getCurrentPlayingCard());
		}
	}

	public override void launchFou(int c){
		int myLevel = GameView.instance.getCard(c).Skills[0].Power;
		GameView.instance.getPlayingCardController(c).addDamagesModifyer(new Modifyer((10-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"), false);
		GameView.instance.displaySkillEffect(c, base.name+"\n-"+(10-myLevel)+"PV", 0);
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		if(currentCard.isFou()){
			value = Mathf.RoundToInt(1.25f*value);
		}
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, value);
		string text = base.name+"\n-"+damages+"PV";
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 22, base.name, damages+" dégats subis"), false);
		GameView.instance.displaySkillEffect(target, text, 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 22);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages = 10 + level;
		int maxDamages = 20+2*level;
		if(currentCard.isFou()){
			minDamages = Mathf.RoundToInt(1.25f*minDamages);
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		minDamages = currentCard.getMagicalDamagesAgainst(targetCard,minDamages);
		maxDamages = currentCard.getMagicalDamagesAgainst(targetCard,maxDamages);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
