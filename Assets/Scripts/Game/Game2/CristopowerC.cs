using UnityEngine;
using System.Collections.Generic;

public class CristopowerC : SkillC
{
	public CristopowerC(){
		base.id = 128 ;
		base.ciblage = 5;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int x, int d, int y, int z){
		CardC caster = Game.instance.getCurrentCard();
		Game.instance.getBoard().getTileC(x,y).setRock(false);

		int bonus = 5+d+Mathf.RoundToInt(z*5f/100f);

		Game.instance.getBoard().getTileC(x,y).displayAnim(base.animId);

		caster.displayAnim(2);
		caster.displaySkillEffect(WordingGame.getText(13, new List<int>{bonus}), 0);
		caster.addAttackModifyer(new ModifyerM(bonus, 0, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC caster = Game.instance.getCurrentCard();

		int score=Mathf.RoundToInt(((3.5f+s.Power)*caster.getLife())/30f);

		return score;
	}
}