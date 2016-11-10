using UnityEngine;
using System.Collections.Generic;

public class MiracleC : SkillC
{
	public MiracleC(){
		base.id = 107 ;
		base.ciblage = 18;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void effects(int targetID, int level, int z){
		CardC caster = Game.instance.getCurrentCard();

		List<int> opponents = Game.instance.getTargetableAllys(Game.instance.getCurrentCard().getCardM().isMine());
		z = opponents[Mathf.Min(Mathf.FloorToInt(z/(100/opponents.Count)),opponents.Count-1)];
		CardC target = Game.instance.getCards().getCardC(z);

		int bonusPV = Mathf.RoundToInt((target.getTotalLife()*(level*4f))/100f);
		int bonusATK = Mathf.RoundToInt((target.getAttack()*(level*4f))/100f);

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(11, new List<int>{bonusPV})+"\n"+WordingGame.getText(13, new List<int>{bonusATK}), 0);

		target.addAttackModifyer(new ModifyerM(bonusATK, 0, "", "",-1));
		target.addLifeModifyer(new ModifyerM(bonusPV, 14, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC caster = Game.instance.getCurrentCard();
		CardC target ;

		List<int> opponents = Game.instance.getTargetableAllys(Game.instance.getCurrentCard().getCardM().isMine());
		int score = 0 ;
		int tempScore ;
		int bonusPV ;
		int bonusATK ;

		for(int i = 0 ; i < opponents.Count ; i++){
			target = Game.instance.getCards().getCardC(opponents[i]);
			bonusPV = Mathf.RoundToInt((target.getTotalLife()*(s.Power*4f))/100f);
			bonusATK = Mathf.RoundToInt((target.getAttack()*(s.Power*4f))/100f);

			if(target.getCardM().isMine()){
				tempScore = bonusPV+bonusATK;
			}
			else{
				tempScore = -1*(bonusPV+bonusATK);
			}

			score+=tempScore;
		}
		score = score/opponents.Count;

		return score;
	}
}
