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
		base.id = 107 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,  GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targetsP)
	{	
		List<int> targets = GameView.instance.getAllys(GameView.instance.getCurrentCard().isMine);

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

		int percentage = 5+GameView.instance.getCurrentSkill().Power*2;
		int bonusLife = Mathf.RoundToInt(targetCard.Life*percentage/100f);
		int bonusAttack = Mathf.RoundToInt(targetCard.getAttack()*percentage/100f);

		string text = "";
		text+="+"+bonusLife+"PV\n";
		text+="+"+bonusAttack+"ATK";

		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).updateLife(currentCard.getLife());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(bonusAttack, -1, 107, base.name, ". Permanent"));
		GameView.instance.getPlayingCardController(target).addPVModifyer(new Modifyer(bonusLife, -1, 107, base.name, ". Permanent"));
		GameView.instance.displaySkillEffect(target, text, 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
		SoundController.instance.playSound(28);
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		List<int> ennemis = GameView.instance.getAllys(false);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score = 0 ;

		for(int i = 0 ; i < ennemis.Count ; i++){
			GameCard targetCard = GameView.instance.getCard(ennemis[UnityEngine.Random.Range(0,ennemis.Count)]);
			score+=Mathf.RoundToInt(2*targetCard.getAttack()*(5+2*s.Power)/100f);
			score+=Mathf.RoundToInt(targetCard.getLife()*(5+2*s.Power)/100f);
		}

		score = (score/ennemis.Count) * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
