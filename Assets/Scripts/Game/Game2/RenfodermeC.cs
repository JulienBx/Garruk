using UnityEngine;
using System.Collections.Generic;

public class RenfodermeC : SkillC
{
	public RenfodermeC(){
		base.id = 39 ;
		base.ciblage = 2;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int bouclier = 4*level;

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(87, new List<int>{bouclier}), 2);

		target.addShieldModifyer(new ModifyerM(bouclier, 1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();

		int bouclier = 4*s.Power;
		int score = Mathf.RoundToInt(bouclier * (target.getLife()/30f));

		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);
		return score;
	}
}
