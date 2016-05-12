using UnityEngine;
using System.Collections.Generic;

public class Malediction : GameSkill
{
	public Malediction()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Malédiction";
		base.ciblage = 12 ;
		base.auto = true;
		base.id = 106 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts("Malédiction", "Choisis une faction. Les unités de cette faction recevront un malus d'attaque");
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllEnemyTypes();
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameController.instance.play(this.id);
	}

	public override void resolve(List<Tile> targets)
	{	                     
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(false);

		List<int> characters = GameView.instance.getEveryone();
		for(int i = 0 ; i < characters.Count ; i++){
			if(GameView.instance.getCard(characters[i]).CardType.Id == targets[0].x)
			{
				GameController.instance.applyOn(characters[i]);
			}
		}
		GameController.instance.playSound(23);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
			
		int level = GameView.instance.getCurrentSkill().Power*5+10;
		GameCard targetCard = GameView.instance.getCard(target);
		int malus = Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*level/100f));

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(-1*malus, 1, 106, base.name, ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, "-"+malus+"ATK", 0);
		GameView.instance.addAnim(2,GameView.instance.getTile(target));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score ;

		List<int> allys = GameView.instance.getAllysAndMe(false);
		int bestScore = -50 ; 
		GameCard targetCard ;
		int cardType ;

		for(int i = 0 ; i < allys.Count ; i++){
			score = 0 ;
			targetCard = GameView.instance.getCard(allys[i]);
			cardType = targetCard.CardType.Id;

			List<int> everyone = GameView.instance.getEveryone();
			for(int j = 0 ; j < everyone.Count ; j++){
				if(GameView.instance.getCard(everyone[j]).CardType.Id==cardType){
					if(GameView.instance.getCard(everyone[j]).isMine){
						score+=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
					}
					else{
						score-=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
					}
				}
			}
			if(bestScore<score){
				bestScore = score;
			}
		}

		score = bestScore * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}

	public override int getBestChoice(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score ;

		List<int> allys = GameView.instance.getAllysAndMe(false);
		int bestScore = -50 ; 
		GameCard targetCard ;
		int cardType ;
		int bestChoice = -1 ;

		for(int i = 0 ; i < allys.Count ; i++){
			score = 0 ;
			targetCard = GameView.instance.getCard(allys[i]);
			cardType = targetCard.CardType.Id;

			List<int> everyone = GameView.instance.getEveryone();
			for(int j = 0 ; j < everyone.Count ; j++){
				if(GameView.instance.getCard(everyone[j]).CardType.Id==cardType){
					if(GameView.instance.getCard(everyone[j]).isMine){
						score+=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
					}
					else{
						score-=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
					}
				}
			}
			if(bestScore<score){
				bestScore = score;
				bestChoice = cardType;
			}
		}

		return bestChoice;
	}
}
