using UnityEngine;
using System.Collections.Generic;

public class ViseeC : SkillC
{
	public ViseeC(){
		base.id = 25 ;
		base.ciblage = 14;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		target.addStateModifyer(new ModifyerM(20+10*level, 10, WordingGame.getText(135, new List<int>{20+10*level}), WordingSkills.getName(this.id),2));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		string text = WordingGame.getText(135, new List<int>{20+10*level});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		int score=s.Power;

		return score;
	}
}
