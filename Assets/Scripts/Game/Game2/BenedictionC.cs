using UnityEngine;
using System.Collections.Generic;

public class BenedictionC : SkillC
{
	public BenedictionC(){
		base.id = 103 ;
		base.ciblage = 7;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target ;
		CardC caster = Game.instance.getCurrentCard();

		List<int> targets = Game.instance.getCards().getAnyoneSharingFactionWith(Game.instance.getCards().getCardC(targetID));
		int atkMalus ;

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		for(int i = 0 ; i < targets.Count ;i++){
			target = Game.instance.getCards().getCardC(targets[i]);
			atkMalus = Mathf.RoundToInt((target.getAttack()*(3f*level))/100f);
			target.displayAnim(base.animId);
			target.displaySkillEffect(WordingGame.getText(13, new List<int>{atkMalus}), 0);
			target.addAttackModifyer(new ModifyerM(atkMalus, 0, "", "", 1));
		}
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target ;
		CardC caster = Game.instance.getCurrentCard();
		int score = 0;

		List<int> targets = Game.instance.getCards().getAnyoneSharingFactionWith(Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID()));
		int atkMalus ;

		for(int i = 0 ; i < targets.Count ;i++){
			target = Game.instance.getCards().getCardC(targets[i]);
			atkMalus = Mathf.RoundToInt((target.getAttack()*(3f*s.Power))/100f);
			if(target.getCardM().isMine()){
				score -= atkMalus;
			}
			else{
				score += atkMalus;
			}
		}

		return score ;
	}
}
