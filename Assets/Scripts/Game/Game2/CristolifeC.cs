using UnityEngine;
using System.Collections.Generic;

public class CristolifeC : SkillC
{
	public CristolifeC(){
		base.id = 129 ;
		base.ciblage = 5;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int x, int d, int y, int z){
		CardC caster = Game.instance.getCurrentCard();
		Game.instance.getBoard().getTileC(x,y).setRock(false);

		int bonus = d+Mathf.RoundToInt(z*5f/100f);

		Game.instance.getBoard().getTileC(x,y).displayAnim(base.animId);

		caster.displayAnim(2);
		caster.displaySkillEffect(WordingGame.getText(11, new List<int>{bonus}), 0);
		caster.addLifeModifyer(new ModifyerM(bonus, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC caster = Game.instance.getCurrentCard();

		int score=Mathf.RoundToInt(((8.5f+s.Power)*caster.getAttack())/20f);

		return score;
	}
}