using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Assassinat";
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
				GameController.instance.esquive(target,10);
			}
		}
		GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		string text = base.name;
		
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = targetCard.getLife();
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, damages+" dégats subis"));
		GameView.instance.getPlayingCardController(target).updateLife();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public override string getTargetText(int id){	
		GameCard targetCard = GameView.instance.getCard(id);
		int chances = GameView.instance.getCurrentSkill().proba;
		string text = base.name+"\nAnéantit l'unité";
		int probaEsquive = targetCard.getEsquive();
		int proba = (chances)*(100-probaEsquive)/100;
		text += "\nHIT : ";
		if (probaEsquive!=0){
			text+=proba+"% : "+chances+"%(ASS) - "+probaEsquive+"%(ESQ)";
		}
		else{
			text+=proba+"%";
		}
		return text ;
	}
}
