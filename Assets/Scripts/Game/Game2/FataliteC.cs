using UnityEngine;
using System.Collections.Generic;

public class FataliteC : SkillC
{
	public FataliteC(){
		base.id = 101 ;
		base.ciblage = 1;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 0 ;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(129), 2);
		target.addStateModifyer(new ModifyerM(-1, 15, WordingGame.getText(129), WordingSkills.getName(this.id),-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();

		int score = target.getAttack()+target.getLife();
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
