using UnityEngine;
using System.Collections.Generic;

public class ShurikenC : SkillC
{
	public ShurikenC(){
		base.id = 9 ;
		base.ciblage = 12;
		base.animId = 4;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID ;
		CardC target ;
		int level = skill.Power;
		List<int> neighbours = Game.instance.getBoard().getNearestNeighbours4D(Game.instance.getBoard().getCurrentBoard(),new TileM(x,y));
		bool hasFailed = false ;
		bool hasDodged = false ;

		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
			for(int i = 0 ; i < neighbours.Count ; i++){
				Debug.Log(neighbours[i]);
				targetID = neighbours[i];
				if(targetID!=-1){
					target = Game.instance.getCards().getCardC(targetID);
					if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
						if(UnityEngine.Random.Range(1,101)<=target.getEsquive()){
							hasDodged = true ;
							if(Game.instance.isIA() || Game.instance.isTutorial()){
								Game.instance.getSkills().skills[this.id].dodge(targetID);
							}
							else{
								Game.instance.launchCorou("DodgeSkillRPC", this.id, targetID);
							}
						}
						else{
							if(Game.instance.isIA() || Game.instance.isTutorial()){
								Game.instance.getSkills().skills[this.id].effects(targetID, level);
							}
							else{
								Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, level);
							}
						}
					}
				}
			}
		}
		else{
			hasFailed = true;
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].fail();
			}
			else{
				Game.instance.launchCorou("FailSkillRPC", this.id);
			}
		}

		if(hasFailed){
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].playFailSound();
			}
			else{
				Game.instance.launchCorou("PlayFailSoundRPC", this.id);
			}
		}
		else if (hasDodged){
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].playDodgeSound();
			}
			else{
				Game.instance.launchCorou("PlayDodgeSoundRPC", this.id);
			}
		}
		else{
			Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id), 1);
		
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].playSound();
			}
			else{
				Game.instance.launchCorou("PlaySoundRPC", this.id);
			}
		}

		if(Game.instance.getCurrentCard().isSanguinaire()){
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].sanguinaireEffects(Game.instance.getCurrentCardID(), Game.instance.getCurrentCard().getCardM().getSkill(0).Power);
			}
			else{
				Game.instance.launchCorou("SanguinaireEffectsSkillRPC", this.id, Game.instance.getCurrentCard().getCardM().getSkill(0).Power);
			}
		}
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(5+level));

		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target ;
		CardC caster = Game.instance.getCurrentCard();
		List<int> neighbours = Game.instance.getBoard().getNearestNeighbours4D(board, t);

		int score = 0 ;
		int tempScore ;

		for(int i = 0 ; i < neighbours.Count ;i++){
			if(Game.instance.getCards().getCardC(neighbours[i]).canBeTargeted()){
				if(neighbours[i]!=Game.instance.getCurrentCardID()){
					target = Game.instance.getCards().getCardC(neighbours[i]);
					tempScore = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*(20f+s.Power*8f)/100f));
					tempScore = Mathf.RoundToInt(s.getProba(s.Power)*(tempScore*(100-target.getEsquive())/100f)/100f);
					score+=tempScore;
				}
			}
		}
		return score;
	}
}
