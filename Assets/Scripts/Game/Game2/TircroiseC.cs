using UnityEngine;
using System.Collections.Generic;

public class TircroiseC : SkillC
{
	public TircroiseC(){
		base.id = 31 ;
		base.ciblage = 15;
		base.animId = 4;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(5+level+(10f*z)/100f));

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degatsMin = caster.getDegatsAgainst(target, 5+level);
		int degatsMax = caster.getDegatsAgainst(target, 15+level);

		string text = WordingGame.getText(96, new List<int>{target.getLife(),target.getLife()-degatsMin,target.getLife()-degatsMax});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target ;
		CardC caster = Game.instance.getCurrentCard();

		int score = 0 ;
		int tempScore ;

		target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		tempScore = caster.getDamageScore(target, 5+s.Power, 15+s.Power);
		tempScore = Mathf.RoundToInt(s.getProba(s.Power)*(tempScore*(100-target.getEsquive())/100f)/100f);
		return score;
	}
}
