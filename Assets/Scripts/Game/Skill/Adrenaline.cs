using UnityEngine;
using System.Collections.Generic;

public class Adrenaline : GameSkill
{
	public Adrenaline()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Adrénaline";
		base.ciblage = 2 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentAllyTargets();
	}
		
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
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
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), GameView.instance.getCurrentCard().Skills[0].Power*5);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		int level = GameView.instance.getCurrentSkill().Power*2+5;
		int soin = Mathf.Min(level,targetCard.GetTotalLife()-targetCard.getLife());

		if(soin==0){
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(2, 1, 6, base.name, "+2MOV. Actif 1 tour"));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, base.name+"\n+2MOV pour un tour", 1);	
			GameView.instance.addAnim(GameView.instance.getTile(target), 6);
		}
		else{
			GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 6, base.name, "+"+soin+" PV"));
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(2, 1, 6, base.name, "+2MOV. Actif 1 tour"));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, base.name+"\n+"+soin+"PV\n+2MOV pour un tour", 1);	
			GameView.instance.addAnim(GameView.instance.getTile(target), 6);
		}
		GameView.instance.recalculateDestinations();
	}

	public override void applyOnViro(int target, int value){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		int level = Mathf.RoundToInt((GameView.instance.getCurrentSkill().Power*2f+5f)*value/100f);
		int soin = Mathf.Min(level,targetCard.GetTotalLife()-targetCard.getLife());
		int move = Mathf.RoundToInt(2f*value/100f);

		if(soin==0){
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(move, 1, 6, base.name, "+"+move+"MOV. Actif 1 tour"));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, base.name+"\n(Virus)\n+"+move+"MOV pour un tour", 1);	
			GameView.instance.addAnim(GameView.instance.getTile(target), 6);
		}
		else{
			GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 6, base.name, "+"+soin+" PV"));
			GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(move, 1, 6, base.name, "+"+move+"MOV. Actif 1 tour"));
			GameView.instance.getPlayingCardController(target).showIcons();
			GameView.instance.displaySkillEffect(target, base.name+"\n(Virus)\n+"+soin+"PV\n+"+move+"MOV pour un tour", 1);	
			GameView.instance.addAnim(GameView.instance.getTile(target), 6);
		}
		GameView.instance.recalculateDestinations();
	}	
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power*2+5;
		int soin = Mathf.Min(level,targetCard.GetTotalLife()-targetCard.getLife());

		string text = "PV : "+targetCard.getLife()+" -> "+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soin)+"\n+2MOV. Actif 1 tour";

		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
