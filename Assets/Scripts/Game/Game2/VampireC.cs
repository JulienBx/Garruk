using UnityEngine;
using System.Collections.Generic;

public class VampireC : SkillC
{
	public VampireC(){
		base.id = 40 ;
		base.ciblage = 20;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC caster = Game.instance.getCurrentCard();
		CardC target = Game.instance.getCards().getCardC(z);

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(5+level+Mathf.RoundToInt((7f*z)/100f)));
		int bonus = Mathf.Min(caster.getTotalLife()-caster.getLife(), degats);

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);
		caster.displayAnim(2);
		caster.displaySkillEffect(WordingGame.getText(11, new List<int>{bonus}), 0);
		caster.addDamageModifyer(new ModifyerM(-1*degats, -1, "", "",-1));

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC caster = Game.instance.getCurrentCard();
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);

		int score = caster.getDamageScore(target, 5+s.Power, 12+s.Power)+Mathf.Min(caster.getTotalLife()-caster.getLife(),9+s.Power);

		return score;
	}
}
