using UnityEngine;
using System.Collections.Generic;

public class PluiebleueC : SkillC
{
	public PluiebleueC(){
		base.id = 130 ;
		base.ciblage = 13;
		base.animId = 4;
		base.soundId = 25;
		base.nbIntsToSend = 1;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID ;
		CardC target ;
		int level = skill.Power;
		List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(new TileM(x,y));
		neighbours.Add(new TileM(x,y));
		bool hasFailed = false ;
		bool hasDodged = false ;

		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
			for(int i = 0 ; i < neighbours.Count ; i++){
				targetID = Game.instance.getBoard().getTileC(neighbours[i].x, neighbours[i].y).getCharacterID();
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
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		if(target.getCardM().getFaction()==6){
			int soins = Mathf.Min(Mathf.RoundToInt(1+(((2*level+2)*z)/100f)), target.getTotalLife()-target.getLife());
			target.displayAnim(2);
			target.displaySkillEffect(WordingGame.getText(11, new List<int>{soins}), 0);
			target.addDamageModifyer(new ModifyerM(-1*soins, -1, "", "",-1));
		}
		else{
			int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(1+(((2*level+2)*z)/100f)));
			target.displayAnim(5);
			target.displaySkillEffect(WordingGame.getText(13, new List<int>{degats}), 2);
			target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
		}
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target ;
		CardC caster = Game.instance.getCurrentCard();
		List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(t);
		neighbours.Add(t);
		int targetID ;

		int score = 0 ;
		int tempScore = 0 ;

		for(int i = 0 ; i < neighbours.Count ;i++){
			targetID = board[neighbours[i].x, neighbours[i].y];
			if(targetID>=0){
				target = Game.instance.getCards().getCardC(targetID);
				if(target.getCardM().getFaction()==6){
					tempScore = Mathf.Min(4+s.Power, target.getTotalLife()-target.getLife());
					if(target.getCardM().isMine()){
						tempScore = -1*tempScore;
					}
				}
				else{
					tempScore = caster.getDamageScore(target, 1, 2*s.Power+5);
				}
			}

			score+=tempScore;
		}	
		return score;
	}
}