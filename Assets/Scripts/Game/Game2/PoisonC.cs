using UnityEngine;
using System.Collections.Generic;

public class PoisonC : SkillC
{
	public PoisonC(){
		base.id = 94 ;
		base.ciblage = 1;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = 5+level;

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(113, new List<int>{degats}), 2);
		target.addStateModifyer(new ModifyerM(degats, 6, WordingGame.getText(113, new List<int>{degats}), WordingSkills.getName(this.id),-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = Mathf.RoundToInt(2*(5+s.Power)*(target.getLife()/30f));

		return score;
	}
}
