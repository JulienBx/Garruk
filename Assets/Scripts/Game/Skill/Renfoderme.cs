using UnityEngine;
using System.Collections.Generic;

public class Renfoderme : GameSkill
{
	public Renfoderme()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Renfoderme","Shield"});
		texts.Add(new string[]{". Permanent",". Permanent"});
		texts.Add(new string[]{"Bouclier ARG1%","ARG1% shield"});
		texts.Add(new string[]{"Bouclier ARG1%\nVirus","ARG1% shield\nVirus"});
		base.ciblage = 2 ;
		base.auto = false;
		base.id = 39 ;
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
		Debug.Log(target);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), 10+GameView.instance.getCurrentCard().Skills[0].Power*5);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(28);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 10+level*2;
		
		GameView.instance.getPlayingCardController(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, this.getText(0), this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(2, new List<int>{bonusShield}), 2);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(1,GameView.instance.getTile(target));
	}	

	public override void applyOnViro(int target, int value){
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = Mathf.RoundToInt((10+level*4)*value/100f);
		
		GameView.instance.getPlayingCardController(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, this.getText(0), ". Permanent."));
		GameView.instance.displaySkillEffect(target, this.getText(3, new List<int>{bonusShield}), 2);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(1,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 10+2*level;
		
		string text = this.getText(2, new List<int>{bonusShield});
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard2;
		int proba = WordingSkills.getProba(s.Id,s.Power);

		score+=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(10+2*s.Power-targetCard.getBouclier()));

		if(currentCard.isVirologue()){
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					if(targetCard2.isMine){
						score-=Mathf.RoundToInt(((proba-targetCard2.getEsquive())/100f)*(10+2*s.Power-targetCard2.getBouclier()));
					}
					else{
						score+=Mathf.RoundToInt(((proba-targetCard2.getEsquive())/100f)*(10+2*s.Power-targetCard2.getBouclier()));
					}
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
