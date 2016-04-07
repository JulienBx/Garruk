using UnityEngine;
using System.Collections.Generic;

public class PerfoTir : GameSkill
{
	public PerfoTir()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "PerfoTir";
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

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		if(currentCard.isFou()){
			GameController.instance.applyOnMe(1);
		}
		else{
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = 5+GameView.instance.getCurrentSkill().Power;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		int level = Mathf.Min(currentCard.getLife(),damages);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(level, -1, 31, base.name, level+" dégats subis"), false);
		GameView.instance.getCard(target).emptyShieldModifyers();
		GameView.instance.getPlayingCardController(target).showIcons();
		string text = "-"+level+"PV";
		if(targetCard.getBouclier()>0){
			text+="\nBouclier détruit";
		}
		GameView.instance.displaySkillEffect(target, text, 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 31);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = 5+GameView.instance.getCurrentSkill().Power;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		int level = currentCard.getNormalDamagesAgainst(targetCard,damages);

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-level);
		if(targetCard.getBouclier()>0){
			text+="\nBouclier détruit";
		}
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"), false);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\nFou\n-"+(11-myLevel)+"PV", 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		}
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
