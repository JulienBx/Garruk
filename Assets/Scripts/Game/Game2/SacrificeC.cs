using UnityEngine;
using System.Collections.Generic;

public class SacrificeC : SkillC
{
	public SacrificeC(){
		base.id = 108 ;
		base.ciblage = 2;
		base.animId = 1;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void resolve(int x, int y, Skill skill){
		int targetID ;
		CardC target ;
		int level = skill.Power;
		TileM targetTile = Game.instance.getBoard().getRandomEnnemyTile();
		target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(new TileM(x,y)).getCharacterID());
				
		bool hasFailed = false ;
		bool hasDodged = false ;

		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
			targetID = Game.instance.getBoard().getTileC(targetTile.x, targetTile.y).getCharacterID();
			if(targetID!=-1){
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
							Game.instance.getSkills().skills[this.id].effects(targetID, level, Mathf.RoundToInt((target.getAttack()*(50f+10f*level))/100f));
						}
						else{
							Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, level, Mathf.RoundToInt((target.getAttack()*(50f+10f*level))/100f));
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

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = target.getLife();
		target.displayAnim(0);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));

		List<TileM> targets = Game.instance.getBoard().getOpponentTargets(Game.instance.getBoard().getCurrentBoard(), caster, new TileM());
		int cible = Game.instance.getBoard().getTileC(targets[Mathf.RoundToInt(((targets.Count-1)*z)/100f)]).getCharacterID();
		degats = target.getDegatsAgainst(Game.instance.getCards().getCardC(cible),Mathf.RoundToInt((target.getAttack()*(50f+10f*level))/100f));
		Game.instance.getCards().getCardC(cible).displayAnim(base.animId);
		Game.instance.getCards().getCardC(cible).displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);
		Game.instance.getCards().getCardC(cible).addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();
		List<TileM> neighbours = Game.instance.getBoard().getOpponentTargets(board, caster, t);

		int score = 0 ;
		int tempScore ;

		CardC initialT = Game.instance.getCards().getCardC(board[t.x,t.y]);
		int degats = Mathf.RoundToInt((initialT.getAttack()*(50f+10f*s.Power))/100f);

		for(int i = 0 ; i < neighbours.Count ;i++){
			if(board[neighbours[i].x, neighbours[i].y]>=0){
				if(board[neighbours[i].x, neighbours[i].y]!=Game.instance.getCurrentCardID()){
					target = Game.instance.getCards().getCardC(board[neighbours[i].x, neighbours[i].y]);
					tempScore = caster.getDamageScore(target, degats);
					score+=tempScore;
				}
			}
		}
		score-= Mathf.RoundToInt((100f*(target.getLife()))/20f);
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-initialT.getEsquive())/100f)/100f);
		score = Mathf.RoundToInt(score/neighbours.Count);


		return score;
	}
}