using UnityEngine;
using System.Collections.Generic;

public class Steroide : GameSkill
{
	public Steroide()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Stéroides","Steroids"});
		texts.Add(new string[]{". Permanent",". Permanent"});
		texts.Add(new string[]{"+ARG1 ATK","+ARG1 ATK"});
		texts.Add(new string[]{"+ARG1 ATK\nVirus","+ARG1 ATK\nVirus"});
		texts.Add(new string[]{"ATK : ARG1 -> [ARG2-ARG3]\nPermanent","ATK : ARG1 -> [ARG2-ARG3]\nPermanent"});
		base.ciblage = 2 ;
		base.auto = false;
		base.id = 56 ;
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
		int max = GameView.instance.getCurrentSkill().Power+5;

		Debug.Log(targets[0].x+","+targets[0].y);
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				int amount = Random.Range(1,max);
				GameController.instance.applyOn2(target, amount);
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), Mathf.Max(1,Mathf.RoundToInt(amount*(0.10f+GameView.instance.getCurrentCard().Skills[0].Power*0.05f))));
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(37);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, -1, 56,this.getText(0), this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(2, new List<int>{value}), 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override void applyOnViro(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, -1, 56, this.getText(0), this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(3, new List<int>{value}), 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power+5;

		string text = this.getText(4, new List<int>{targetCard.getAttack(),(targetCard.getAttack()+1),(targetCard.getAttack()+level)});
		
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

		int levelMin = 1;
		int levelMax = 5+s.Power;

		score+=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(targetCard.getLife()/50f)*((levelMin+levelMax)/2)*2);

		if(currentCard.isVirologue()){
			levelMax = Mathf.RoundToInt((5+s.Power*2)*(10f+currentCard.Skills[0].Power*5f)/100f);
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					if(targetCard2.isMine){
						score-=Mathf.RoundToInt(((proba-targetCard2.getEsquive())/100f)*(targetCard2.getLife()/50f)*((levelMin+levelMax)/2)*2);
					}
					else{
						score+=Mathf.RoundToInt(((proba-targetCard2.getEsquive())/100f)*(targetCard2.getLife()/50f)*((levelMin+levelMax)/2)*2);
					}
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
