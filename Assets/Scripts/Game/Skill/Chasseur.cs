using UnityEngine;
using System.Collections.Generic;

public class Chasseur : GameSkill
{
	public Chasseur()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Chasseur";
		base.ciblage = 12 ;
		base.auto = true;
		base.id = 131 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts("Chasseur", "Choisis une faction. L'unité active recevra un bonus contre les unités de cette faction");
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllEnemyTypes();
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> target)
	{	                     
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(false);
		GameController.instance.applyOnMe(target[0].x);
		GameController.instance.endPlay();
	}

	public override void applyOnMe(int i){
		int level = GameView.instance.getCurrentSkill().Power;
		int bonus = 20+4*level;
		int target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addBonusModifyer(new Modifyer(bonus,-1, 131, base.name," VS "+WordingCardTypes.getName(i),i));
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.displaySkillEffect(target, base.name+"Dégats +"+bonus+"% VS "+WordingCardTypes.getName(i), 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 131);
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score = -50 ;
		if(currentCard.hasBonusChasseur()){
			
		}
		else{
			List<int> opponents = GameView.instance.getOpponents(false);
			int bestScore = 0 ; 
			int[] cardTypes = new int[10];
			for(int i = 0 ; i < cardTypes.Length ; i++){
				cardTypes[i]=0;
			}
			GameCard targetCard ;

			for(int i = 0 ; i < opponents.Count ; i++){
				targetCard = GameView.instance.getCard(opponents[i]);
				cardTypes[targetCard.CardType.Id]++;
				if(cardTypes[targetCard.CardType.Id]>bestScore){
					bestScore = cardTypes[targetCard.CardType.Id];
				}
			}
			score = Mathf.RoundToInt(bestScore*40f*(20f+4f*s.Power)/100f) ;
			score = score * GameView.instance.IA.getSoutienFactor() ;
		}
		return score ;
	}

	public override int getBestChoice(Tile t, Skill s){
		List<int> opponents = GameView.instance.getOpponents(false);
		int bestScore = 0 ; 
		int bestChoice = -1;
		int[] cardTypes = new int[10];
		for(int i = 0 ; i < cardTypes.Length ; i++){
			cardTypes[i]=0;
		}
		GameCard targetCard ;

		for(int i = 0 ; i < opponents.Count ; i++){
			targetCard = GameView.instance.getCard(opponents[i]);
			cardTypes[targetCard.CardType.Id]++;
			if(cardTypes[targetCard.CardType.Id]>bestScore){
				bestScore = cardTypes[targetCard.CardType.Id];
				bestChoice = targetCard.CardType.Id;
			}
		}

		return bestChoice ;
	}
}
