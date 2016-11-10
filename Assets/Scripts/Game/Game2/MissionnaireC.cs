using UnityEngine;
using System.Collections.Generic;

public class MissionnaireC : SkillC
{
	public MissionnaireC(){
		base.id = 110;
		base.ciblage = 0;
		base.animId = 5;
		base.soundId = 25;
	}

	public override void effects(int targetID, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(130), 1);
		target.getCardM().setMine(Game.instance.getCurrentCard().getCardM().isMine());
		Game.instance.loadDestinations();
	}
}
