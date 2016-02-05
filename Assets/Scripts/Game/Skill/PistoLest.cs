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
			for(int i = 0 ; i < 100 ; i++){
				Debug.Log(Random.Range(1,101));
			}
			if (Random.Range(1,101) <= proba){
				int amount = Random.Range(1,2*level+1);
				GameController.instance.applyOn2(target, amount);
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

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 5, base.name, damages+" dégats subis"));
		GameView.instance.getCard(target).moveModifyers.Add(new Modifyer(-2, 1, 5, base.name, "-2MOV. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, "-"+damages+"PV\n-2MOV pour un tour", 0);	
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
