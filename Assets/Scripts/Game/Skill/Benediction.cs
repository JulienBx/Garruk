using UnityEngine;
using System.Collections.Generic;

public class Benediction : GameSkill
{
	public Benediction()
	{
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Bénédiction","Blessing"});
		texts.Add(new string[]{"Choisis une faction. Les unités de cette faction recevront un bonus d'attaque","Choose a faction. All of its units gets an attack bonus"});
		texts.Add(new string[]{". Pour 1 tour",". For 1 turn"});
		texts.Add(new string[]{"+ARG1 ATK","+ARG1 ATK"});
		base.ciblage = 12 ;
		base.auto = true;
		base.id = 103 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts(this.getText(0), this.getText(1));
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllAllyTypes();
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameController.instance.play(this.id);
	}

	public override void resolve(List<Tile> targets)
	{	                     
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(false);

		List<int> characters = GameView.instance.getEveryone();
		for(int i = 0 ; i < characters.Count ; i++){
			if(GameView.instance.getCard(characters[i]).CardType.Id == targets[0].x){
				GameController.instance.applyOn(characters[i]);
			}
		}
		GameController.instance.playSound(37);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
			
		int level = GameView.instance.getCurrentSkill().Power*2+10;
		GameCard targetCard = GameView.instance.getCard(target);
		int malus = Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*level/100f));

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(malus, 1, 103, this.getText(0), this.getText(2)));
		GameView.instance.displaySkillEffect(target, this.getText(3, new List<int>{malus}), 0);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
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
						score=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
					}
					else{
						score+=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
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
						score-=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
					}
					else{
						score+=Mathf.RoundToInt(GameView.instance.getCard(everyone[j]).getAttack()*(10f+2*s.Power)/100f);
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
