using UnityEngine;
using System.Collections.Generic;

public class Soin : GameSkill
{
	public Soin()
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
		bool isSuccess = false ;
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				GameController.instance.applyOn(target);
				isSuccess = true;
			}
			else{
				GameController.instance.esquive(target,6);
			}
		}
		
		if(GameView.instance.getCurrentCard().isGenerous()){
			List<int> targets = GameView.instance.getAllys();
			targets.Remove(target);
			target = targets[Random.Range(0,targets.Count)];
			GameController.instance.applyOn(target);
			isSuccess = true ;	
		}
		GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int soin = Mathf.Min(2*level,targetCard.GetTotalLife()-targetCard.getLife());
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*soin, -1, 0, text, "+"+soin+" PV"));
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);	
	}	
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int soin = 2*level;
			
		text += "\nSoigne "+soin+" PV";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
