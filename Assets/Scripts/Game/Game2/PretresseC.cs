using UnityEngine;
using System.Collections.Generic;

public class PretresseC : SkillC
{
	public PretresseC(){
		base.id = 112 ;
		base.ciblage = 1;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);

		int bonus = Mathf.RoundToInt(1+Mathf.RoundToInt(((1f+level)*z)/100f));

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(13, new List<int>{bonus}),0);

		target.addAttackModifyer(new ModifyerM(bonus, 0, "", "",1));
	}
}
