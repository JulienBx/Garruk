using UnityEngine;
using System.Collections.Generic;

public class CuirasseC : SkillC
{
	public CuirasseC(){
		base.id = 70 ;
		base.ciblage = 0;
		base.animId = 2;
		base.soundId = 25;
	}

	public override void effects(int targetID, int level){
		CardC caster = Game.instance.getCards().getCardC(targetID);

		int bonusBouclier = level*4;

		if(caster.getCardM().isMine() || Game.instance.getCurrentCardID()!=-1){
			caster.displayAnim(base.animId);
			caster.displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(87, new List<int>{bonusBouclier}), 0);
		}

		caster.addShieldModifyer(new ModifyerM(bonusBouclier, 1, "", "",-1));
	}
}
