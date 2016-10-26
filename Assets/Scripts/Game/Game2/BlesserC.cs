using UnityEngine;
using System.Collections.Generic;

public class BlesserC : SkillC
{
	public BlesserC(){
		base.id = 11 ;
		base.ciblage = 1;
		base.animId = 0;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*0.5f));
		int malus = -1*Mathf.RoundToInt(1f+((1f+level)*z)/100f);
		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats})+"\n"+WordingGame.getText(83, new List<int>{malus}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		target.addAttackModifyer(new ModifyerM(malus, 0, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*0.5f));
		int maxMalus = 2+level;

		string text = WordingGame.getText(78, new List<int>{target.getLife(),target.getLife()-degats})+"\n"+WordingGame.getText(103, new List<int>{target.getAttack(), Mathf.Max(1,target.getAttack()-1), Mathf.Max(1,target.getAttack()-maxMalus)});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*0.5f));
		score+=Mathf.RoundToInt(1.5f+0.5f*s.Power);
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
