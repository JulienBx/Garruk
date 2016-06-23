using UnityEngine;
using System.Collections.Generic;
using System;

public class Desequilibre : GameSkill
{
	public Desequilibre()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Déséquilibre","Unbalance"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		texts.Add(new string[]{"Repoussé","Pushed back"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2\nrepousse l'unité","HP : ARG1 -> ARG2\nPushes the enemy back"});
		base.ciblage = 1 ;
		base.auto = false;
		base.id = 92 ;
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

		if (UnityEngine.Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (UnityEngine.Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(25);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f)));
		string text = this.getText(0)+"\n"+this.getText(1,new List<int>{damages});

		Tile targetTile = GameView.instance.getPlayingCardController(target).getTile();
		Tile currentTile = GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile();
		Tile nextTile;
		
		if(!targetCard.isMine){
			nextTile = new Tile(targetTile.x+(targetTile.x-currentTile.x),targetTile.y+(targetTile.y-currentTile.y));
		}
		else{
			nextTile = new Tile(targetTile.x+(targetTile.x-currentTile.x),targetTile.y+(targetTile.y-currentTile.y));
		}
		if(nextTile.x>=0 && nextTile.x<GameView.instance.getBoardWidth() && nextTile.y>=0 && nextTile.y<GameView.instance.getBoardHeight()){
			if(!GameView.instance.getTileController(nextTile).isRock() && GameView.instance.getTileController(nextTile).getCharacterID()==-1){
				GameView.instance.dropCharacter(target, nextTile, true, true);
				GameView.instance.recalculateDestinations();
				text+="\n"+this.getText(2);
			}
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 92, "", ""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard,Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f)));

		string text = this.getText(3, new List<int>{targetCard.getLife(),(targetCard.getLife()-damages)});
		
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f));
		int score ;
		if(damages>=targetCard.getLife()){
			score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(200)+targetCard.getLife()/10f);
		}
		else{
			score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(damages)+5-targetCard.getLife()/10f);
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
