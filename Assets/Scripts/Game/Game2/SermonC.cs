using UnityEngine;
using System.Collections.Generic;

public class SermonC : SkillC
{
	public SermonC(){
		base.id = 102 ;
		base.ciblage = 3;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID ;
		CardC target ;
		int level = skill.Power;
		bool hasFailed = false ;
		bool hasDodged = false ;
		List<int> targets = Game.instance.getBoard().getEveryoneWithSermon(Game.instance.getBoard().getCurrentBoard(), Game.instance.getCurrentCard(), new TileM(x,y));

		if(UnityEngine.Random.Range(0,101)<=WordingSkills.getProba(this.id, level)){
			for(int i = 0 ; i < targets.Count ; i++){
				targetID = i;
				if(!Game.instance.getCards().getCardC(targetID).isDead()){
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
								Game.instance.getSkills().skills[this.id].effects(targetID, level, UnityEngine.Random.Range(1,101));
							}
							else{
								Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, level, UnityEngine.Random.Range(1,101));
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
	}

	public override void effects(int targetID, int level, int z){
		int degats ;
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		degats = Mathf.RoundToInt(1+((1f+level)*z)/100f);
		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(13, new List<int>{degats}), 0);
		target.addAttackModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target ;
		CardC caster = Game.instance.getCurrentCard();
		List<int> targets = Game.instance.getBoard().getEveryoneWithSermon(board, Game.instance.getCurrentCard(), t);

		int score = 0 ;
		int tempScore ;

		for(int i = 0 ; i < targets.Count ;i++){
			target = Game.instance.getCards().getCardC(targets[i]);
			tempScore = 2+s.Power;
			if(Game.instance.getCards().getCardC(targets[i]).getCardM().isMine()){
				tempScore=-1*tempScore;
			}
			score+=tempScore;
		}
		return score;
	}
}


