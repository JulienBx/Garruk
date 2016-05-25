using UnityEngine;
using System.Collections.Generic;

public class PistoSoin : GameSkill
{
	public PistoSoin()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Soin";
		base.ciblage = 14 ;
		base.auto = false;
		base.id = 2;
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
				int amount = Random.Range(level+4, 15+level);
				GameController.instance.applyOn2(target, amount);
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro2(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), amount, GameView.instance.getCurrentCard().Skills[0].Power*5+25);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.playSound(37);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		int soin = Mathf.Min(amount,targetCard.GetTotalLife()-targetCard.getLife());
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 2, "PistoSoin", "Gagne "+soin+"PV"), false,-1);
		GameView.instance.displaySkillEffect(target, "+"+soin+"PV", 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
		if(ApplicationModel.player.ToLaunchGameTutorial){
			GameView.instance.hitNextTutorial();
		}
	}	

	public override void applyOnViro2(int target, int amount, int amount2){
		GameCard targetCard = GameView.instance.getCard(target);
		int soin = Mathf.Min(Mathf.RoundToInt(amount*amount2/100f),targetCard.GetTotalLife()-targetCard.getLife());

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 2, "PistoSoin", "Gagne "+soin+"PV"),false,-1);
		GameView.instance.displaySkillEffect(target, "Virus\n+"+soin+"PV", 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}	
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power;
		int soinMin = 4+level;
		int soinMax = 14+level;
		string text = "";

		if(Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soinMin)==Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soinMax)){
			text = "PV : "+targetCard.getLife()+" -> "+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soinMin);
		}
		else{
			text = "PV : "+targetCard.getLife()+" -> ["+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soinMin)+"-"+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soinMax)+"]";
		}

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
		GameCard targetCard2;
		int proba = WordingSkills.getProba(s.Id,s.Power);

		int levelMin = 4+s.Power;
		int levelMax = 14+s.Power;
		int missingLife = targetCard.GetTotalLife()-targetCard.getLife();

		score+=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f));

		if(currentCard.isVirologue()){
			int levelMin2 = Mathf.RoundToInt(s.Power*(25f+currentCard.Skills[0].Power*5f)/100f);
			int levelMax2 = Mathf.RoundToInt((12+s.Power*3)*(25f+currentCard.Skills[0].Power*5f)/100f);
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					missingLife = targetCard2.GetTotalLife()-targetCard2.getLife();
					if(targetCard2.isMine){
						score-=Mathf.RoundToInt(((proba-targetCard2.getMagicalEsquive())/100f)*((missingLife*(Mathf.Max(0f,levelMax2-missingLife)))+(((levelMin2+Mathf.Min(levelMax2,missingLife))/2f)*Mathf.Min(levelMax2,missingLife)))/(levelMax2-levelMin2+1f));
					}
					else{
						score+=Mathf.RoundToInt(((proba-targetCard2.getMagicalEsquive())/100f)*((missingLife*(Mathf.Max(0f,levelMax2-missingLife)))+(((levelMin2+Mathf.Min(levelMax2,missingLife))/2f)*Mathf.Min(levelMax2,missingLife)))/(levelMax2-levelMin2+1f));
					}
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
