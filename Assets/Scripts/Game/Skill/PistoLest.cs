using UnityEngine;
using System.Collections.Generic;

public class PistoLest : GameSkill
{
	public PistoLest()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "PistoLest";
		base.ciblage = 3 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				int amount = Random.Range(1,2*level+1);
				GameController.instance.applyOn2(target, amount);
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1){
							GameController.instance.applyOnViro2(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), amount, GameView.instance.getCurrentCard().Skills[0].Power*5);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,5);
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getDamagesAgainst(targetCard, amount);
		int move = -1*Mathf.Min(targetCard.getMove()-1,2);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 5, base.name, damages+" dégats subis"));
		GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(move, 1, 5, base.name, move+"MOV. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.recalculateDestinations();

		GameView.instance.displaySkillEffect(target, "-"+damages+"PV\n"+move+"MOV pour un tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 5);
	}	

	public override void applyOnViro2(int target, int amount, int amount2){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = Mathf.RoundToInt(currentCard.getDamagesAgainst(targetCard, amount)*amount2/100f);
		int move = -1*Mathf.Min(targetCard.getMove(), Mathf.RoundToInt(2*amount2/100f));

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 5, base.name, damages+" dégats subis"));
		GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(move, 1, 5, base.name, move+"MOV. Actif 1 tour"));
		GameView.instance.recalculateDestinations();
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, "Virus\n-"+damages+"PV\n-"+move+"MOV pour un tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 5);
	}	

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getDamagesAgainst(targetCard, 2*level);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-1)+"-"+(targetCard.getLife()-damages)+"]\n-2MOV. Actif 1 tour";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
