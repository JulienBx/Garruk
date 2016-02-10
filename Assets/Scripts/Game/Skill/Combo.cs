using UnityEngine;
using System.Collections.Generic;

public class Combo : GameSkill
{
	public Combo(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Combo";
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
		int proba = GameView.instance.getCurrentSkill().proba;
		int max = 5+GameView.instance.getCurrentSkill().Power;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(1,max));
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getDamagesAgainst(targetCard,value*Mathf.RoundToInt(20*currentCard.getAttack()/100f));
		string text = "HIT X"+value+"\n-"+damages+"PV";

		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text = "HIT X"+value+"\n-"+damages+"PV\n(lache)";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text = "HIT X"+value+"\n-"+damages+"PV\n(lache)";
				}
			}
		}
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,12,base.name,damages+" dégats subis"));
		GameView.instance.addAnim(GameView.instance.getTile(target), 12);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damagesMin = currentCard.getDamagesAgainst(targetCard,Mathf.RoundToInt(20*currentCard.getAttack()/100f));
		int damagesMax = currentCard.getDamagesAgainst(targetCard,(5+GameView.instance.getCurrentSkill().Power)*Mathf.RoundToInt(20*currentCard.getAttack()/100f));
		string text = "PV : "+currentCard.getLife()+" -> ["+(currentCard.getLife()-damagesMax)+"-"+(currentCard.getLife()-damagesMin)+"]";

		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damagesMin = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damagesMin);
					damagesMax = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damagesMax);
					text = "PV : "+currentCard.getLife()+" -> ["+(currentCard.getLife()-damagesMax)+"-"+(currentCard.getLife()-damagesMin)+"]\n(lache)";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damagesMin = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damagesMin);
					damagesMax = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damagesMax);
					text = "PV : "+currentCard.getLife()+" -> ["+(currentCard.getLife()-damagesMax)+"-"+(currentCard.getLife()-damagesMin)+"]\n(lache)";
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
