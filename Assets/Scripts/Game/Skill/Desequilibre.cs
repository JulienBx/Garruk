using UnityEngine;
using System.Collections.Generic;

public class Desequilibre : GameSkill
{
	public Desequilibre()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Déséquilibre";
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
			Debug.Log("Esquive "+GameView.instance.getCard(target).getEsquive());
			GameController.instance.esquive(target,1);
		}
		else{
			int proba = GameView.instance.getCurrentSkill().proba;
			if(Random.Range(1,101)<=proba){
				GameController.instance.applyOn(target);
				isSuccess = true ;
			}
			else{
				GameController.instance.esquive(target,92);
			}
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
		GameView.instance.getPlayingCardController(target).updateLife();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
		
		Tile targetTile;
		targetTile = GameView.instance.getPlayingCardController(target).getTile();
		Tile currentTile;
		currentTile = GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile();
		Tile nextTile;
		
		if(!targetCard.isMine){
			nextTile = new Tile(targetTile.x+(targetTile.x-currentTile.x),targetTile.y+(targetTile.y-currentTile.y));
		}
		else{
			nextTile = new Tile(targetTile.x+(targetTile.x-currentTile.x),targetTile.y+(targetTile.y-currentTile.y));
		}
		GameView.instance.clickDestination(nextTile,target,false);
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
