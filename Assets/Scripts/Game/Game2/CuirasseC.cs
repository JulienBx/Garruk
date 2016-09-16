using UnityEngine;
using System.Collections.Generic;

public class CuirasseC : SkillC
{
	public CuirasseC(){
		this.id = 70 ;
		base.ciblage = 0;
		base.animId = 2;
		base.soundId = 25;
	}

	public override void effects(int targetID){
		print("CUIRASSE");
		CardC caster = Game.instance.getCards().getCardC(targetID);
		int level = caster.getCardM().getSkill(0).Power;

		int bonusBouclier = level*4;

		caster.displayAnim(base.animId);
		caster.displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(87, new List<int>{bonusBouclier}), 0);

		caster.addShieldModifyer(new ModifyerM(bonusBouclier, 1, "", "",-1));
	}
}
