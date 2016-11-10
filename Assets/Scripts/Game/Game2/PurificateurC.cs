using UnityEngine;
using System.Collections.Generic;

public class PurificateurC : SkillC
{
	public PurificateurC(){
		base.id = 113;
		base.ciblage = 0;
		base.animId = 3;
		base.soundId = 25;
	}

	public override void effects(int targetID, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(114), 0);
		target.emptyModifyers();

		Game.instance.loadDestinations();
	}
}
