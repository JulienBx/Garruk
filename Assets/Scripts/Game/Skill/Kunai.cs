using UnityEngine;
using System.Collections.Generic;

public class Kunai : GameSkill
{
	public Kunai()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Kunaï";
		base.ciblage = 0 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,  WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power));
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		List<int> targets = GameView.instance.getOpponents();

		int target = targets[Random.Range(0,targets.Count)];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				int value = this.getValue(GameView.instance.getCurrentSkill().Power);
				GameController.instance.applyOn2(target, value);
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}
	
	public int getValue(int level){
		int value = -1;
		if(level==1){
			value=Random.Range(1,8);
		}
		else if(level==2){
			value=Random.Range(2,10);
		}
		else if(level==3){
			value=Random.Range(3,12);
		}
		else if(level==4){
			value=Random.Range(4,14);
		}
		else if(level==5){
			value=Random.Range(5,16);
		}
		else if(level==6){
			value=Random.Range(6,18);
		}
		else if(level==7){
			value=Random.Range(7,20);
		}
		else if(level==8){
			value=Random.Range(8,22);
		}
		else if(level==9){
			value=Random.Range(9,24);
		}
		else if(level==10){
			value=Random.Range(10,26);
		}
		return value;
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getMagicalDamagesAgainst(targetCard, value);

		string text = "-"+damages+"PV";
						
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y>GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text="-"+damages+"PV"+"\n(lache)";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y<GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), 5+currentCard.getSkills()[0].Power+damages);
					text="-"+damages+"PV"+"\n(lache)";
				}
			}
		}
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,9,"Pistolero",damages+" dégats subis"), false);
		GameView.instance.addAnim(GameView.instance.getTile(target), 8);
	}
}
