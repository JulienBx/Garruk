using UnityEngine;
using System.Collections.Generic;

public class AntibiotiqueC : SkillC
{
	public AntibiotiqueC(){
		base.id = 7 ;
		base.ciblage = 7;
		base.animId = 3;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingSkills.getName(114), 0);
		target.emptyModifyers();
		Game.instance.loadDestinations();
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText()+"\nHIT% : "+(50+5*level)+"%";
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		return 0;
	}
}
