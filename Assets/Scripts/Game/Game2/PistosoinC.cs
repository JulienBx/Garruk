using UnityEngine;
using System.Collections.Generic;

public class PistosoinC : SkillC
{
	public PistosoinC(){
		base.id = 3 ;
		base.ciblage = 8;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int bonus = Mathf.Min(target.getTotalLife()-target.getLife(),(5+level+Mathf.RoundToInt(10*z/100f)));

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(11, new List<int>{bonus}),0);
		target.addDamageModifyer(new ModifyerM(-1*bonus, 0, "", "",1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int maxBonus = 15+level;

		string text = WordingGame.getText(96, new List<int>{target.getLife(), Mathf.Min(target.getTotalLife(),target.getLife()+5+level), Mathf.Min(target.getTotalLife(),target.getLife()+maxBonus)});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = Mathf.Min(10+s.Power,target.getTotalLife()-target.getLife());
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}