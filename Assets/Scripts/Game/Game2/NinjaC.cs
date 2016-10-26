using UnityEngine;
using System.Collections.Generic;

public class NinjaC : SkillC
{
	public NinjaC(){
		base.id = 67 ;
		base.ciblage = 0;
		base.animId = 4;
		base.soundId = 25;
	}

	public override void effects(int targetID, int level){
		SoundController.instance.playSound(base.soundId);

		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, 2+level);

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}
}
