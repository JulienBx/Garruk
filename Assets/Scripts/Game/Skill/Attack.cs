using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Attaque";
		base.ciblage = 1 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
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
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = "";
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getDamagesAgainst(targetCard);

		text += "-"+damages+"PV";
						
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text+="\n(Bonus lache)";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text+="\n(Bonus lache)";
				}
			}
		}
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,1,"Attaque","Dommages subis par attaque"));
		GameView.instance.addAnim(GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getDamagesAgainst(targetCard);

		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);

		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text+=" (Bonus lache)";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Power+damages);
					text+=" (Bonus lache)";
				}
			}
		}	

		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
