using UnityEngine;
using System.Collections.Generic;

public class Antibiotique : GameSkill
{
	public Antibiotique()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Antibiotique";
		base.ciblage = 9 ;
		base.auto = false;
		base.id = 7;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		
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
		GameController.instance.playSound(31);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameView.instance.getPlayingCardController(target).emptyModifiers();
		GameView.instance.getPlayingCardController(target).show();
		GameView.instance.displaySkillEffect(target, "Succès\nEffets dissipés", 2);
		GameView.instance.addAnim(1,GameView.instance.getTile(target));
	}	
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);

		string text = "Dissipe les effets";
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		int tempScore ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard2;
		int proba = WordingSkills.getProba(s.Id,s.Power);

		score+=2*(targetCard.Attack-targetCard.getAttack())+1*(targetCard.Life-targetCard.GetTotalLife())+5*(targetCard.Move-targetCard.getMove());
		if(targetCard.isPoisoned()){
			score+=20;
		}
		else if(targetCard.isEffraye()){
			score+=15;
		}
		score = Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*score) ;
		score+=targetCard.getLife()-40;
		if(targetCard.isMine){
			score = -1*score;
		}

		if(currentCard.isVirologue()){
			int levelMin2 = Mathf.RoundToInt(s.Power*(25f+currentCard.Skills[0].Power*5f)/100f);
			int levelMax2 = Mathf.RoundToInt((12+s.Power*3)*(25f+currentCard.Skills[0].Power*5f)/100f);
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					tempScore = 0 ;
					tempScore+=2*(targetCard.Attack-targetCard.getAttack())+1*(targetCard.Life-targetCard.GetTotalLife())+5*(targetCard.Move-targetCard.getMove());
					if(targetCard.isPoisoned()){
						tempScore+=20;
					}
					else if(targetCard.isEffraye()){
						tempScore+=15;
					}
					tempScore = Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*tempScore) ;
					tempScore+=targetCard.getLife()-40;
					if(targetCard.isMine){
						tempScore = -1*tempScore;
					}
					score+=tempScore;
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
