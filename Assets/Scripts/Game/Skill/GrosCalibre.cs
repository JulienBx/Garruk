using UnityEngine;
using System.Collections.Generic;

public class GrosCalibre : GameSkill
{
	public GrosCalibre()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Gros Calibre";
		base.ciblage = 1 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		int myLevel = currentCard.Skills[0].Power;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
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
		GameController.instance.endPlay();
		if(currentCard.isFou()){
			GameController.instance.launchFou(26,GameView.instance.getCurrentPlayingCard());
		}
	}

	public override void launchFou(int c){
		int myLevel = GameView.instance.getCard(c).Skills[0].Power;
		GameView.instance.getPlayingCardController(c).addDamagesModifyer(new Modifyer((10-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"), false);
		GameView.instance.displaySkillEffect(c, base.name+"\n-"+(10-myLevel)+"PV", 0);
	}

	public override void applyOn(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int maxDamages = Mathf.RoundToInt(currentCard.getAttack()*(1.2f+level/10f));
		if(currentCard.isFou()){
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, Mathf.RoundToInt(maxDamages));

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 26, base.name, damages+" dégats subis"), false);
		GameView.instance.displaySkillEffect(target, base.name+"\n-"+damages+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 26);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int maxDamages = Mathf.RoundToInt(currentCard.getAttack()*(1.2f+level/10f));
		if(currentCard.isFou()){
			maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
		}
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, Mathf.RoundToInt(maxDamages));
		string text = "-"+damages+"PV";		
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
