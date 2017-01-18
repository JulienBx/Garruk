using UnityEngine;
using System.Collections.Generic;

public class CopieurC : SkillC
{
	public CopieurC(){
		base.id = 105 ;
		base.ciblage = 7;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		Skill s = target.getCardM().getSkill(1);

		caster.displaySkillEffect(WordingGame.getText(137)+WordingSkills.getName(s.Id)+WordingGame.getText(138), 0);
		caster.setSkill(1,s);
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);

		Skill s = target.getCardM().getSkill(1);
		string text = WordingGame.getText(137)+WordingSkills.getName(s.Id)+WordingGame.getText(138);

		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();

		Skill s2 = target.getCardM().getSkill(1);
		int score = s2.Power;

		return score;
	}
}
