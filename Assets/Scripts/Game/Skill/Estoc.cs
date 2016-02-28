using UnityEngine;
using System.Collections.Generic;

public class Estoc : GameSkill
{
	public Estoc(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Estoc";
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
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
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
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f)));
		string text = base.name+"\n-"+damages+"PV\n-10ATK";				
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text=base.name+"\n-"+damages+"PV"+"\n(lache)\n-10ATK";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text=base.name+"\n-"+damages+"PV"+"\n(lache)\n-10ATK";
				}
			}
		}
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(-10, 1, 11, base.name, "-10ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,base.name,damages+" dégats subis"));
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 11);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f)));
		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\nATK : "+targetCard.getAttack()+" -> "+Mathf.Max(1, targetCard.getAttack()-10)+" pour 1 tour";				
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\n(lache)\nATK : "+targetCard.getAttack()+" -> "+Mathf.Max(1, targetCard.getAttack()-10)+" pour 1 tour";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\n(lache)\nATK : "+targetCard.getAttack()+" -> "+Mathf.Max(1, targetCard.getAttack()-10)+" pour 1 tour";
				}
			}
		}
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
