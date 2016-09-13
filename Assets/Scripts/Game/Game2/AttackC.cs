using UnityEngine;
using System.Collections.Generic;

public class AttackC : SkillC
{
	public AttackC(){
		this.id = 0 ;
		base.ciblage = 1;
		base.animId = 0;
		base.soundId = 25;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID = Game.instance.getBoard().getTileC(x,y).getCharacterID();
		CardC target = Game.instance.getCards().getCardC(targetID);
		//int level = skill.Power;
		int level = 1;
		if(UnityEngine.Random.Range(0,101)<=WordingSkills.getProba(this.id, level)){
			if(UnityEngine.Random.Range(0,101)<=target.getEsquive()){
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].dodge(targetID);
				}
				else{
					GameRPC.instance.launchRPC("DodgeSkillRPC", this.id, targetID);
				}
			}
			else{
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].effects(targetID);
				}
				else{
					GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID);
				}
			}
		}
		else{
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].fail();
			}
			else{
				GameRPC.instance.launchRPC("FailSkillRPC", this.id);
			}
		}
	}

	public override void effects(int targetID){
		SoundController.instance.playSound(base.soundId);

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
		return score;
	}
}
