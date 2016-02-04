using UnityEngine;
using System.Collections.Generic;

public class Lance : GameSkill
{
	public Lance(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Lance";
		base.ciblage = 8 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.display1TileAwayOpponentsTargets();
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
		int damages = currentCard.getDamagesAgainst(targetCard,50+5*level);
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, "-"+damages+" PV"));
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}

	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getDamagesAgainst(targetCard,50+5*level);
		
		text += "\nPV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
