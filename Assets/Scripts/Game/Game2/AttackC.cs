using UnityEngine;
using System.Collections.Generic;

public class AttackC : SkillC
{
	public AttackC(){
		base.id = 0 ;
		base.ciblage = 1;
		base.animId = 0;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID = Game.instance.getBoard().getTileC(x,y).getCharacterID();
		CardC target = Game.instance.getCards().getCardC(targetID);
		//int level = skill.Power;
		int level = 1;
		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
			if(UnityEngine.Random.Range(1,101)<=target.getEsquive()){
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].dodge(targetID);
					Game.instance.getSkills().skills[this.id].playDodgeSound();
				}
				else{
					Game.instance.launchCorou("DodgeSkillRPC", this.id, targetID);
					Game.instance.launchCorou("PlayDodgeSoundRPC", this.id);
				}
			}
			else{
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].effects(targetID);
					Game.instance.getSkills().skills[this.id].playSound();
				}
				else{
					Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID);
					Game.instance.launchCorou("PlaySoundRPC", this.id);
				}
			}
		}
		else{
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].fail();
				Game.instance.getSkills().skills[this.id].playFailSound();
			}
			else{
				Game.instance.launchCorou("FailSkillRPC", this.id);
				Game.instance.launchCorou("PlayFailSoundRPC", this.id);
			}
		}
	}

	public override void effects(int targetID){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, caster.getAttack());

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);
		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();
		int degats = caster.getDegatsAgainst(target, caster.getAttack());
		string text = WordingGame.getText(78, new List<int>{target.getLife(),target.getLife()-degats});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScore(target, caster.getAttack());
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		return score;
	}
}
