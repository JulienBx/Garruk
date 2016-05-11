using UnityEngine;
using System.Collections.Generic;

public class Sacrifice : GameSkill
{
	public Sacrifice()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "";
		base.ciblage = 4 ;
		base.auto = false;
		base.id = 108 ;
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
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
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
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = targetCard.getLife();
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 108, base.name, damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		List<Tile> voisins = GameView.instance.getPlayingCardController(target).getTile().getImmediateNeighbourTiles();

		int amount = Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*(50f+level*10f)/100f));
		for (int i = 0 ; i < voisins.Count ; i++){
			if(GameView.instance.getTileController(voisins[i]).getCharacterID()!=-1){
				if(!GameView.instance.getCard(GameView.instance.getTileController(voisins[i]).getCharacterID()).isDead){
					GameView.instance.getPlayingCardController(GameView.instance.getTileController(voisins[i]).getCharacterID()).addDamagesModifyer(new Modifyer(amount, -1, 108, base.name, damages+" dégats subis"), (GameView.instance.getTileController(voisins[i]).getCharacterID()==GameView.instance.getCurrentPlayingCard()),-1);
					GameView.instance.displaySkillEffect(GameView.instance.getTileController(voisins[i]).getCharacterID(), "-"+amount+"PV", 0);
					GameView.instance.addAnim(5,GameView.instance.getTile(GameView.instance.getTileController(voisins[i]).getCharacterID()));
				}
			}
		}

		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);
		GameView.instance.addAnim(4,GameView.instance.getTile(target));
		SoundController.instance.playSound(34);
	}	
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int value = currentCard.getNormalDamagesAgainst(targetCard,Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*(50f+level*10f)/100f)));

		string text = "Tue l'unité et inflige "+value+" dégats aux unités adjacentes!";

		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
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
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int level = GameView.instance.getCurrentSkill().Power;

		int target ; 
		int levelMin;

		score -=100 + 30 - targetCard.getLife();

		List<Tile> tempTiles = t.getImmediateNeighbourTiles();
		for(int i = 0 ; i < tempTiles.Count ; i++){
			target = GameView.instance.getTileController(tempTiles[i]).getCharacterID();
			if(target!=-1){
				targetCard = GameView.instance.getCard(target);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*(50f+level*10f)/100f)));

				if(levelMin>=targetCard.getLife()){
					if(targetCard.isMine){
						score+=100;
					}
					else{
						score-=100;
					}
				}
				else{
					if(targetCard.isMine){
						score+=levelMin+Mathf.RoundToInt(5-targetCard.getLife()/10f);
					}
					else{
						score-=levelMin+Mathf.RoundToInt(5-targetCard.getLife()/10f);
					}
				}
			}
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
