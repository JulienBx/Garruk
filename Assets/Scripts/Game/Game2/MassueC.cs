using UnityEngine;
using System.Collections.Generic;

public class MassueC : SkillC
{
	public MassueC(){
		base.id = 63 ;
		base.ciblage = 1;
		base.animId = 0;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(0.5f*caster.getAttack()+(0.7f+0.1f*level)*caster.getAttack()*z/100f));

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degatsMin = caster.getDegatsAgainst(target, Mathf.RoundToInt(0.5f*caster.getAttack()));
		int degatsMax = caster.getDegatsAgainst(target, Mathf.RoundToInt((1.2f+0.1f*level)*caster.getAttack()));

		string text = WordingGame.getText(96, new List<int>{target.getLife(),target.getLife()-degatsMin,target.getLife()-degatsMax});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*0.5f), Mathf.RoundToInt(caster.getAttack()*(1.2f+0.1f*s.Power)));
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
