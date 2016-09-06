using UnityEngine;
using System.Collections.Generic;

public class FrenetiqueC : SkillC
{
	public FrenetiqueC(){
		this.id = 69 ;
		base.ciblage = 0;
		base.animId = 1;
		base.soundId = 25;
	}

	public override void resolve(Skill skill){
		CardC target = Game.instance.getCurrentCard();
		int level = skill.Power;
		if(UnityEngine.Random.Range(0,101)<=WordingSkills.getProba(this.id, level)){
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].effects(Game.instance.getCurrentCardID());
			}
			else{
				GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, Game.instance.getCurrentCardID());
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

		CardC caster = Game.instance.getCurrentCard();
		int level = Game.instance.getCurrentCard().getCardM().getSkill(0).Power;

		int degats = caster.getDegatsAgainst(caster, this.getDegats(level));
		int bonusAttack = this.getAttackBonus(level);

		caster.displayAnim(base.animId);
		caster.displaySkillEffect(WordingGame.getText(13, new List<int>{bonusAttack})+"\n"+WordingGame.getText(8, new List<int>{degats}), 1);

		caster.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		caster.addAttackModifyer(new ModifyerM(bonusAttack, 0, "", "",-1));
	}

	public int getDegats(int level){
		if(level==0){
			return 10;
		}
		else if(level<=2){
			return 9;
		}
		else if(level<=4){
			return 8;
		}
		else if(level<=6){
			return 7;
		}
		else if(level<=8){
			return 6;
		}
		else{
			return 5;
		}
	}

	public int getAttackBonus(int level){
		if(level<=1){
			return 3;
		}
		else if(level<=3){
			return 4;
		}
		else if(level<=5){
			return 5;
		}
		else if(level<=7){
			return 6;
		}
		else{
			return 7;
		}
	}
}
