using UnityEngine;
using System.Collections.Generic;

public class Miracle : GameSkill
{
	public Miracle()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Miracle";
		base.ciblage = 0 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,  WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<Tile> targetsP)
	{	
		List<int> targets = GameView.instance.getOpponents(GameView.instance.getCurrentCard().isMine);

		int target = targets[Random.Range(0,targets.Count)];
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		
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
		GameCard currentCard = GameView.instance.getCurrentCard();

		int percentage = 10+GameView.instance.getCurrentSkill().Power*3;
		int bonusLife = Mathf.RoundToInt(targetCard.Life*percentage/100f);
		int bonusAttack = Mathf.RoundToInt(targetCard.getAttack()*percentage/100f);

		string text = "";
		text+="+"+bonusLife+"PV\n";
		text+="+"+bonusAttack+"ATK";

		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(bonusAttack, -1, 107, base.name, ". Permanent"));
		GameView.instance.getPlayingCardController(target).addPVModifyer(new Modifyer(bonusLife, -1, 107, base.name, ". Permanent"));
		GameView.instance.displaySkillEffect(target, text, 2);
		GameView.instance.addAnim(GameView.instance.getTile(target), 107);
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
