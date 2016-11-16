using UnityEngine;
using System.Collections.Generic;

public class ChasseurC : SkillC
{
	public ChasseurC(){
		base.id = 131 ;
		base.ciblage = 6;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(133, new List<int>{50+10*level}), 2);
		target.addStateModifyer(new ModifyerM(targetID, 17, WordingGame.getText(134)+WordingSkills.getName(target.getCardM().getCharacterType()), WordingSkills.getName(this.id),50+10*level));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = Mathf.RoundToInt(target.getLife()/5f);

		return score;
	}
}
