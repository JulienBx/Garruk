using UnityEngine;
using System.Collections.Generic;

public class Senilite : GameSkill
{
	public Senilite()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Sénilité";
		base.ciblage = 3 ; 
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
		int level = GameView.instance.getCurrentSkill().Power;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				int value = -1*Random.Range(1,2+level);
				GameController.instance.applyOn2(target,value);
				isSuccess = true ;
			}
			else{
				GameController.instance.esquive(target,56);
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
	
	public override void applyOn(int target, int value){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(value, -1, 57, text, value+" ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n"+value+" ATK. Permanent", 1);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		
		text += "-[1-"+(level+1)+"] ATK. Permanent";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
