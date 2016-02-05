using UnityEngine;
using System.Collections.Generic;

public class PistoSoin : GameSkill
{
	public PistoSoin()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Soin";
		base.ciblage = 4 ;
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
				int amount = Random.Range(1*level,3*level+1);
				GameController.instance.applyOn2(target, amount);
			}
			else{
				GameController.instance.esquive(target,2);
			}
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int soin = Mathf.Min(amount,targetCard.GetTotalLife()-targetCard.getLife());
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 2, "PistoSoin", "Gagne "+soin+"PV"));
		GameView.instance.displaySkillEffect(target, "+"+soin+"PV", 1);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 2);
	}	
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int soinMin = 1*level;
		int soinMax = 3*level;
			
		string text = "PV : "+targetCard.getLife()+" -> ["+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soinMin)+"-"+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+soinMax)+"]";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
