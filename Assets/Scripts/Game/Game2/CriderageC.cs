using UnityEngine;
using System.Collections.Generic;

public class CriderageC : SkillC
{
	public CriderageC(){
		base.id = 19 ;
		base.ciblage = 3;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID ;
		CardC target ;
		int level = skill.Power;
		List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(new TileM(x,y));
		bool hasFailed = false ;
		bool hasDodged = false ;

		if(UnityEngine.Random.Range(0,101)<=WordingSkills.getProba(this.id, level)){
			for(int i = 0 ; i < neighbours.Count ; i++){
				targetID = Game.instance.getBoard().getTileC(neighbours[i].x, neighbours[i].y).getCharacterID();
				if(targetID!=-1){
					target = Game.instance.getCards().getCardC(targetID);
					if(UnityEngine.Random.Range(0,101)<=WordingSkills.getProba(this.id, level)){
						if(UnityEngine.Random.Range(0,101)<=target.getEsquive()){
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
								Game.instance.getSkills().skills[this.id].effects(targetID, level, Random.Range(1,101));
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
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int attackBonus = 1+Mathf.RoundToInt((1+level)*z/100f);
		target.displayAnim(base.animId);
		target.displaySkillEffect(WordingGame.getText(13, new List<int>{attackBonus}), 2);

		target.addAttackModifyer(new ModifyerM(attackBonus, 0, "", "",1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int attackBonus = 2+level;

		string text = WordingGame.getText(99, new List<int>{attackBonus});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target ;
		CardC caster = Game.instance.getCurrentCard();
		List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(t);

		int attackBonus = Mathf.RoundToInt(3+s.Power/2f);
		int score = 0 ;
		int tempScore ;

		for(int i = 0 ; i < neighbours.Count ;i++){
			if(Game.instance.getBoard().getTileC(neighbours[i]).getCharacterID()!=-1){
				if(Game.instance.getBoard().getTileC(neighbours[i]).getCharacterID()!=Game.instance.getCurrentCardID()){
					target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(neighbours[i]).getCharacterID());
					tempScore = attackBonus;
					tempScore = Mathf.RoundToInt(s.getProba(s.Power)*(tempScore*(100-target.getEsquive())/100f)/100f);
					if(!target.getCardM().isMine()){
						tempScore=-1*tempScore;
					}
					score+=tempScore;
				}
			}
		}

		return score;
	}
}
