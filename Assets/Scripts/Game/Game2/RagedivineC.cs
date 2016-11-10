using UnityEngine;
using System.Collections.Generic;

public class RagedivineC : SkillC
{
	public RagedivineC(){
		base.id = 109 ;
		base.ciblage = 10;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC caster = Game.instance.getCurrentCard();

		List<int> opponents = Game.instance.getTargetableAnyone();
		z = opponents[Mathf.Min(Mathf.FloorToInt(z/(100/opponents.Count)),opponents.Count-1)];
		CardC target = Game.instance.getCards().getCardC(z);

		int degats = target.getLife();

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC caster = Game.instance.getCurrentCard();
		CardC target ;

		List<int> opponents = Game.instance.getTargetableAnyone();
		int score = 0 ;
		int tempScore ;

		for(int i = 0 ; i < opponents.Count ; i++){
			target = Game.instance.getCards().getCardC(opponents[i]);
		
			tempScore = caster.getDegatsNoShieldAgainst(target, target.getLife());
			score+=tempScore;
		}
		score = score/opponents.Count;

		return score;
	}
}
