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
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
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
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 108, base.name, damages+" dégats subis"), false);
		List<Tile> voisins = GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).getTile().getImmediateNeighbourTiles();

		int amount = Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*(50f+level*10f)/100f));
		for (int i = 0 ; i < voisins.Count ; i++){
			if(GameView.instance.getTileController(voisins[i]).getCharacterID()!=-1){
				if(!GameView.instance.getCard(GameView.instance.getTileController(voisins[i]).getCharacterID()).isDead){
					GameView.instance.getPlayingCardController(GameView.instance.getTileController(voisins[i]).getCharacterID()).addDamagesModifyer(new Modifyer(amount, -1, 108, base.name, damages+" dégats subis"), (GameView.instance.getTileController(voisins[i]).getCharacterID()==GameView.instance.getCurrentPlayingCard()));
					GameView.instance.displaySkillEffect(GameView.instance.getTileController(voisins[i]).getCharacterID(), "-"+amount+"PV", 108);
					GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getTileController(voisins[i]).getCharacterID()), 108);
				}
			}
		}

		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 108);
	}	
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power;
		int value = Mathf.Max(1,Mathf.RoundToInt(targetCard.getAttack()*(50f+level*10f)/100f));

		string text = "Tue l'unité et inflige "+value+" dégats aux unités adjacentes!";

		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
