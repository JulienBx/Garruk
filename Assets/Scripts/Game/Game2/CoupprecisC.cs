using UnityEngine;
using System.Collections.Generic;

public class CoupprecisC : SkillC
{
	public CoupprecisC(){
		base.id = 59 ;
		base.ciblage = 1;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID = Game.instance.getBoard().getTileC(x,y).getCharacterID();
		int level = skill.Power;
		CardC target;
				
		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
			if(targetID!=-1){
				target = Game.instance.getCards().getCardC(targetID);
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					if(nbIntsToSend==0){
						Game.instance.getSkills().skills[this.id].effects(targetID, level);
					}
					else if(nbIntsToSend==1){
						Game.instance.getSkills().skills[this.id].effects(targetID, level, UnityEngine.Random.Range(1,101));
					}
					else if(nbIntsToSend==2){
						Game.instance.getSkills().skills[this.id].effects(targetID, level, UnityEngine.Random.Range(1,101), UnityEngine.Random.Range(1,101));
					}
					Game.instance.getSkills().skills[this.id].playSound();
				}
				else{
					if(nbIntsToSend==0){
						Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, level);
					}
					else if(nbIntsToSend==1){
						Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, level, UnityEngine.Random.Range(1,101));
					}
					else if(nbIntsToSend==2){
						Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, level, UnityEngine.Random.Range(1,101), UnityEngine.Random.Range(1,101));
					}
					Game.instance.launchCorou("PlaySoundRPC", this.id);
				}
			}
			else{
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					if(nbIntsToSend==0){
						Game.instance.getSkills().skills[this.id].effects(x, level, y);
					}
					else if(nbIntsToSend==1){
						Game.instance.getSkills().skills[this.id].effects(x, level, y, UnityEngine.Random.Range(1,101));
					}
					Game.instance.getSkills().skills[this.id].playSound();
				}
				else{
					if(nbIntsToSend==0){
						Game.instance.launchCorou("EffectsSkillRPC", this.id, x, level, y);
					}
					else if(nbIntsToSend==1){
						Game.instance.launchCorou("EffectsSkillRPC", this.id, x, level, y, UnityEngine.Random.Range(1,101));
					}
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

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsNoShieldAgainst(target,Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));

		caster.displaySkillEffect(WordingSkills.getName(this.id), 1);

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsNoShieldAgainst(target,Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));

		string text = WordingGame.getText(78, new List<int>{target.getLife(),target.getLife()-degats});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
		CardC caster = Game.instance.getCurrentCard();

		int score = caster.getDamageScoreNoShield(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*s.Power)/100f));

		return score;
	}
}
