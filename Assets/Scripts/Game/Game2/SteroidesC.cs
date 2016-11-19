using UnityEngine;
using System.Collections.Generic;

public class SteroidesC : SkillC
{
	public SteroidesC(){
		base.id = 56 ;
		base.ciblage = 2;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int bonus = (1+Mathf.RoundToInt((4+level)*z/100f));

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(13, new List<int>{bonus}), 0);

		target.addAttackModifyer(new ModifyerM(bonus, 0, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int maxMalus = (5+level);

		string text = WordingGame.getText(103, new List<int>{target.getAttack(), target.getAttack()+1, target.getAttack()+maxMalus});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();

		int score = Mathf.RoundToInt(2f*(3.5f+0.5f*s.Power)*(target.getLife()/30f));
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
