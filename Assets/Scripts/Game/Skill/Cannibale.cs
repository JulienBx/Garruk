using UnityEngine;
using System.Collections.Generic;

public class Cannibale : GameSkill
{
	public Cannibale(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Cannibale" ;
		base.ciblage = 2 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentAllyTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
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
		int damages = targetCard.getLife();
		int percentage = 10+GameView.instance.getCurrentSkill().Power*5;

		int bonusLife = Mathf.Min(Mathf.RoundToInt(damages*percentage/100f),currentCard.GetTotalLife()-currentCard.getLife());
		int bonusAttack = Mathf.RoundToInt(targetCard.getAttack()*percentage/100f);
		string text = "";
		if(bonusLife>0){
			text+="+"+bonusLife+"PV\n";
		}
		text+="+"+bonusAttack+"ATK";


		int targetMe = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 21, base.name, damages+" dégats subis"));
		GameView.instance.getCard(targetMe).attackModifyers.Add(new Modifyer(bonusAttack, -1, 21, base.name, "+"+bonusAttack+" ATK. Permanent"));
		GameView.instance.getCard(targetMe).damagesModifyers.Add(new Modifyer(-1*bonusLife, -1, 21, base.name, "+"+bonusLife+" PV. Permanent"));
		GameView.instance.getPlayingCardController(targetMe).updateAttack();
		GameView.instance.displaySkillEffect(targetMe, text, 1);
		GameView.instance.addAnim(GameView.instance.getTile(targetMe), 21);

		GameView.instance.displaySkillEffect(target, "Dévoré", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 21);
	}

	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = targetCard.getLife();
		int percentage = 10+GameView.instance.getCurrentSkill().Power*5;

		int bonusLife = -1*Mathf.RoundToInt(damages*percentage/100f);
		int bonusAttack = Mathf.RoundToInt(targetCard.getAttack()*percentage/100f);
		int targetMe = GameView.instance.getCurrentPlayingCard();
		
		text += "\nAbsorbe "+bonusAttack+" ATK et "+bonusLife+" PV";
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
