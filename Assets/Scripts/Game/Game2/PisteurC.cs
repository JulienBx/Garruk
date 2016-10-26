using UnityEngine;
using System.Collections.Generic;

public class PisteurC : SkillC
{
	public PisteurC(){
		base.id = 14 ;
		base.ciblage = 11;
		base.animId = 5;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC caster = Game.instance.getCurrentCard();
		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);
		Game.instance.getBoard().eraseTraps(Game.instance.getCurrentCard().getCardM().isMine());
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		return (3*Game.instance.getBoard().getNumberOfHiddenEnnemyTraps(Game.instance.getCurrentCard().getCardM().isMine()));
	}
}
