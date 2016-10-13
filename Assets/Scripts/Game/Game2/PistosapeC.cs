using UnityEngine;
using System.Collections.Generic;

public class PistosapeC : SkillC
{
	public PistosapeC(){
		base.id = 4 ;
		base.ciblage = 6;
		base.animId = 4;
		base.soundId = 25;
		base.nbIntsToSend = 2;
	}

	public override void effects(int targetID, int level, int z, int z2){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = (1+Mathf.RoundToInt((1+level)*z/100f));
		int malus = -1*(1+Mathf.RoundToInt((1+level)*z2/100f));

		degats = caster.getDegatsAgainst(target, degats);

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(8, new List<int>{degats})+"\n"+WordingGame.getText(83, new List<int>{malus}), 2);

		target.addAttackModifyer(new ModifyerM(malus, 0, "", "",1));
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int maxMalus = (2+level);
		int maxDegats = caster.getDegatsAgainst(target, 2+level);

		string text = WordingGame.getText(96, new List<int>{target.getLife(),Mathf.Max(0,target.getLife()-1), Mathf.Max(0,target.getLife()-maxDegats)})+"\n"+WordingGame.getText(103, new List<int>{target.getAttack(),Mathf.Max(1,target.getAttack()-1), Mathf.Max(1,target.getAttack()-maxMalus)});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, 1, 2+s.Power);
		score+=Mathf.RoundToInt(2f*(1.5f+0.5f*s.Power));

		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);
		return score;
	}
}
