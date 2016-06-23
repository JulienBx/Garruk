using UnityEngine;
using System.Collections.Generic;

public class Vitamines : GameSkill
{
	public Vitamines()
	{
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Vitamines","Vitamins"});
		texts.Add(new string[]{"+1MOV. Actif 1 tour","+1MOV. For 1 turn"});
		texts.Add(new string[]{"+ARG1 PV","+ARG1 HP"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2\nlâche","HP : ARG1 -> ARG2\ncoward"});
		base.ciblage = 2 ;
		base.auto = false;
		base.id = 6 ;
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
		GameController.instance.playSound(37);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);

		int level = GameView.instance.getCurrentSkill().Power*2+2;
		int soin = Mathf.Min(level,targetCard.GetTotalLife()-targetCard.getLife());

		if(soin==0){
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(1, 1, 6, this.getText(0), this.getText(1)));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, this.getText(1), 2);	
			GameView.instance.addAnim(7,GameView.instance.getTile(target));
		}
		else{
			GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 6, this.getText(0), ""), false,-1);
			GameView.instance.getPlayingCardController(target).addMoveModifyer(new Modifyer(1, 1, 6, this.getText(0), this.getText(1)));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, this.getText(2, new List<int>{soin})+"\n"+this.getText(1), 2);	
			GameView.instance.addAnim(7,GameView.instance.getTile(target));
		}
		GameView.instance.recalculateDestinations();
	}

	public override void applyOnViro(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);

		int level = Mathf.RoundToInt((GameView.instance.getCurrentSkill().Power*2f+2f)*value/100f);
		int soin = Mathf.Min(level,targetCard.GetTotalLife()-targetCard.getLife());
		int move = Mathf.RoundToInt(1f*value/100f);

		if(soin==0){
			GameView.instance.getPlayingCardController(target).addMoveModifyer(new Modifyer(move, 1, 6, this.getText(0), "+"+move+"MOV. Actif 1 tour"));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, "(Virus)\n+"+move+"MOV\n1 tour", 2);	
			GameView.instance.addAnim(7,GameView.instance.getTile(target));
		}
		else{
			GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 6, this.getText(0), "+"+soin+" PV"), false,-1);
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(move, 1, 6, this.getText(0), "+"+move+"MOV. Actif 1 tour"));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, "(Virus)\n+"+soin+"PV\n+"+move+"MOV\n1 tour", 2);	
			GameView.instance.addAnim(7,GameView.instance.getTile(target));
		}
		GameView.instance.recalculateDestinations();
	}	
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power*2+2;
		int soin = Mathf.Min(level,targetCard.GetTotalLife()-targetCard.getLife());

		string text = "PV : "+targetCard.getLife()+" -> "+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soin)+"\n+1MOV, Actif 1 tour";

		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
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

		int missingLife = targetCard.GetTotalLife()-targetCard.getLife();

		score+=Mathf.RoundToInt((proba-targetCard.getEsquive()/100f)*Mathf.Min(missingLife,2+2*s.Power));
		score+=10;

		if(currentCard.isVirologue()){
			int levelMin2 = Mathf.RoundToInt(s.Power*(10f+currentCard.Skills[0].Power*5f)/100f);
			int levelMax2 = Mathf.RoundToInt((2+s.Power*2)*(10f+currentCard.Skills[0].Power*5f)/100f);
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					missingLife = targetCard2.GetTotalLife()-targetCard2.getLife();
					if(targetCard2.isMine){
						score-=Mathf.RoundToInt((proba-targetCard2.getEsquive()/100f)*Mathf.Min(missingLife,5+2*s.Power));
						score-=10;
					}
					else{
						score+=Mathf.RoundToInt((proba-targetCard2.getEsquive()/100f)*Mathf.Min(missingLife,5+2*s.Power));
						score+=10;
					}
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
