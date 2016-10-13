using UnityEngine;
using System.Collections.Generic;

public class CoriaceC : SkillC
{
	public CoriaceC(){
		base.id = 68;
		base.ciblage = 0;
		base.animId = 2;
		base.soundId = 25;
	}

	public override void effects(int targetID, int z){
		CardC caster = Game.instance.getCards().getCardC(targetID);
		int level = caster.getCardM().getSkill(0).Power;

		int life = -1*(Mathf.Min(caster.getTotalLife()-caster.getLife(), level+Mathf.Max(1,Mathf.RoundToInt(level*z/100f))));

		caster.displayAnim(base.animId);
		caster.displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(11, new List<int>{-1*life}), 0);

		caster.addDamageModifyer(new ModifyerM(life, -1, "", "",-1));
	}
}
