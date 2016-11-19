using UnityEngine;
using System.Collections.Generic;

public class SeniliteC : SkillC
{
	public SeniliteC(){
		base.id = 57 ;
		base.ciblage = 1;
		base.animId = 3;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int malus = -1*(1+Mathf.RoundToInt((4+level)*z/100f));

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(83, new List<int>{malus}), 2);

		target.addAttackModifyer(new ModifyerM(malus, 0, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int maxMalus = (5+level);

		string text = WordingGame.getText(103, new List<int>{target.getAttack(),Mathf.Max(1,target.getAttack()-1), Mathf.Max(1,target.getAttack()-maxMalus)});
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
