using UnityEngine;
using System.Collections.Generic;

public class LanceC : SkillC
{
	public LanceC(){
		base.id = 91 ;
		base.ciblage = 4;
		base.animId = 0;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();
		string text ;

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));

		target.displayAnim(base.animId);
		text = WordingGame.getText(77, new List<int>{degats});
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		target.displaySkillEffect(text, 2);
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();
		string text ;

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));
		text = WordingGame.getText(78, new List<int>{target.getLife(), target.getLife()-degats});

		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*s.Power)/100f));
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
