using UnityEngine;
using System.Collections.Generic;

public class Combo : GameSkill
{
	public Combo()
	{
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
		bool isSuccess = false ;
		GameController.instance.play(GameView.instance.runningSkill);
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;
		int target = targetsPCC[0];
		int nbAttacks = this.getNbAttacks(level);
		int nbSuccessfulAttacks = 0 ;
		for(int i = 0; i < nbAttacks ; i++){
			if (Random.Range(1,101) < GameView.instance.getCard(target).getEsquive()){
				
			}
			else{
				nbSuccessfulAttacks++;
			}
		}
		
		if (nbSuccessfulAttacks==0){
			GameController.instance.esquive(target,1);
		}
		else{
			GameController.instance.applyOn2(target,nbSuccessfulAttacks);
			isSuccess = true ;
		}
		GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}
	
	public int getNbAttacks(int level){
		int nbAttacks = 0 ;
		if(level==1){
			nbAttacks = Random.Range(1,6);
		}
		else if(level==2){
			nbAttacks = Random.Range(1,7);
		}
		else if(level==3){
			nbAttacks = Random.Range(1,6);
		}
		else if(level==4){
			nbAttacks = Random.Range(1,8);
		}
		else if(level==5){
			nbAttacks = Random.Range(1,7);
		}
		else if(level==6){
			nbAttacks = Random.Range(1,9);
		}
		else if(level==7){
			nbAttacks = Random.Range(1,8);
		}
		else if(level==8){
			nbAttacks = Random.Range(1,7);
		}
		else if(level==9){
			nbAttacks = Random.Range(1,9);
		}
		else if(level==10){
			nbAttacks = Random.Range(1,8);
		}
		return nbAttacks;
	}
	
	public int getPercentage(int level){
		int percentage = 0 ;
		if(level<=2){
			percentage = 20;
		}
		else if(level==3){
			percentage = 25;
		}
		else if(level==4){
			percentage = 20;
		}
		else if(level==5){
			percentage = 25;
		}
		else if(level==6){
			percentage = 20;
		}
		else if(level==7){
			percentage = 25;
		}
		else if(level==8){
			percentage = 30;
		}
		else if(level==9){
			percentage = 25;
		}
		else if(level==10){
			percentage = 30;
		}
		return percentage ;
	}

	public override void applyOn(int target, int value){
		string text = base.name;
		
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int percentage = this.getPercentage(level);
		int damages = currentCard.getDamagesAgainst(targetCard,percentage)*value;
		
		if (currentCard.isLache()){
			if(GameView.instance.getIsFirstPlayer() == currentCard.isMine){
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y==GameView.instance.getPlayingCardController(target).getTile().y-1){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text+="\nBonus lache";
				}
			}
			else{
				if (GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().y-1==GameView.instance.getPlayingCardController(target).getTile().y){
					damages = Mathf.Min(targetCard.getLife(), currentCard.getSkills()[0].Level+damages);
					text+="\nBonus lache";
				}
			}
		}
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 0, text, "HIT X"+value+"\n"+damages+" dégats subis"));
		GameView.instance.getPlayingCardController(target).updateLife();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
//	
//	public override string getTargetText(int id, Card targetCard){
//		
//		int currentLife = targetCard.GetLife();
//		int bouclier = targetCard.GetBouclier();
//		
//		int damageBonusPercentage = GameView.instance.getCard(GameController.instance.getCurrentPlayingCard()).GetDamagesPercentageBonus(targetCard);
//		int amount = base.card.GetAttack()*this.skill.ManaCost*(100+damageBonusPercentage)/10000;
//		string text = "";
//		
//		if (base.card.isLache()){
//			if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
//				if(GameView.instance.getPlayingCardTile(id).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
//					amount = (100+base.card.getPassiveManacost())*amount/100;
//					text="LACHE\n";
//				}
//			}
//			else{
//				if(GameView.instance.getPlayingCardTile(id).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
//					amount = (100+base.card.getPassiveManacost())*amount/100;
//					text="LACHE\n";
//				}
//			}
//		}
//		
//		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
//		
//		if(currentLife-Mathf.Min(currentLife,amount)==0){
//			text += "PV : "+currentLife+"->0";
//		}
//		else{
//			text += "PV : "+currentLife+"->"+(currentLife-Mathf.Min(currentLife,amount))+"-"+(currentLife-Mathf.Min(currentLife,(4*amount)));
//		}
//		
//		int probaEsquive = targetCard.GetEsquive();
//		int probaHit = Mathf.Max(0,100-probaEsquive) ;
//		
//		text += "HIT% : "+probaHit;
//		
//		return text ;
//	}
}
