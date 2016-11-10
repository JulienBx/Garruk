using UnityEngine;
using System.Collections.Generic;

public class JusticeC : SkillC
{
	public JusticeC(){
		base.id = 8 ;
		base.ciblage = 10;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC caster = Game.instance.getCurrentCard();
		CardC targetMax = Game.instance.getCards().getMaxLifeCard();
		CardC targetMin = Game.instance.getCards().getMinLifeCard();

		int degats = caster.getDegatsAgainst(targetMax, 5+2*level);

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		targetMax.displayAnim(base.animId);
		targetMax.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);
		targetMax.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		targetMin.displayAnim(2);
		targetMin.displaySkillEffect(WordingGame.getText(11, new List<int>{degats}), 2);
		targetMin.addDamageModifyer(new ModifyerM(-1*degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC caster = Game.instance.getCurrentCard();
		CardC targetMax = Game.instance.getCards().getMaxLifeCard();
		CardC targetMin = Game.instance.getCards().getMinLifeCard();

		int score = caster.getDamageScore(targetMax, 5+2*s.Power);
		if(targetMin.getCardM().isMine()){
			score -= Mathf.Min(targetMin.getTotalLife()-targetMin.getLife(), 5+2*s.Power);
		}
		else{
			score += Mathf.Min(targetMin.getTotalLife()-targetMin.getLife(), 5+2*s.Power);
		}

		return score;
	}
}
