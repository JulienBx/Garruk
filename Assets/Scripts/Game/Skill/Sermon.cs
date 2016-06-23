using UnityEngine;
using System.Collections.Generic;

public class Sermon : GameSkill
{
	public Sermon()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 0 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Sermon","Lecture"});
		texts.Add(new string[]{".Actif 1 tour",". For 1 turn"});
		texts.Add(new string[]{"+ARG1 ATK","+ARG1 ATK"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 102 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int level = GameView.instance.getCurrentSkill().Power+5;

		List<int> tempTiles = GameView.instance.getEveryone();
		int i = 0 ;
		int tempInt ;

		while (i<tempTiles.Count){ 
			tempInt = tempTiles[i];
			if (tempInt!=-1)
			{
				if (GameView.instance.getPlayingCardController(tempInt).canBeTargeted())	
				{
					if (Random.Range(1,101) <= GameView.instance.getCard(tempInt).getEsquive())
					{                             
						GameController.instance.esquive(tempInt,1);
					}
					else{
						if (Random.Range(1,101) <= proba){
							GameController.instance.applyOn2(tempInt,UnityEngine.Random.Range(1,level+1));
						}
						else{
							GameController.instance.esquive(tempInt,this.getText(0));
						}
					}
				}
			}
			i++;
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(37);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		int level = GameView.instance.getCurrentSkill().Power+5;
		GameCard targetCard = GameView.instance.getCard(target);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, 1, 102, this.getText(0), this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(2, new List<int>{value}), 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard ;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = 0;
		List<int> neighbours = GameView.instance.getEveryone();
		int bonus = Mathf.RoundToInt((6f+s.Power)/2f);

		for(int i = 0 ; i < neighbours.Count ; i++){
			targetCard = GameView.instance.getCard(neighbours[i]);
			if(targetCard.CardType.Id!=5 && targetCard.CardType.Id!=8){
				if(targetCard.isMine){
					score-=Mathf.RoundToInt((100f-targetCard.getMagicalEsquive())*(bonus*0.02f));
				}
				else{
					score+=Mathf.RoundToInt((100f-targetCard.getMagicalEsquive())*(bonus*0.02f));
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
