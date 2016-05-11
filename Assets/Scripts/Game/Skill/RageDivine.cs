using UnityEngine;
using System.Collections.Generic;

public class RageDivine : GameSkill
{
	public RageDivine()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Rage Divine";
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 109 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,  GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power)));
		GameController.instance.play(this.id);
	}

	public override void resolve(List<Tile> targetsP)
	{	
		List<int> targets = GameView.instance.getEveryone();

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
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		int damages = targetCard.getLife();
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 109, base.name, damages+" dégats subis"), (target==GameView.instance.getCurrentPlayingCard()),-1);
		GameView.instance.displaySkillEffect(target, "Fureur divine", 0);
		GameView.instance.addAnim(4,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
}
