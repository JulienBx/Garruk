using UnityEngine;
using System.Collections.Generic;

public class Faveur : GameSkill
{
	public Faveur()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "PistoBoost";
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
		GameView.instance.advanceTurn(target);
		GameView.instance.displaySkillEffect(target, "Prochaine unité à jouer", 2);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 104);
	}	

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);

		string text = "Sera la prochaine unité à jouer";
		
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
