using UnityEngine;
using System.Collections.Generic;

public class SkillC
{
	public int numberOfExpectedTargets ; 

	public List<int> targets;
	public List<int> results;
	public List<int> values;
	public List<TileM> tileTargets;
	public IList<string[]> texts ;
	public IList<string[]> texts2 ;
	public int ciblage ;
	public bool auto ;
	public int id ;
	public int dodgeSoundId = 25;
	public int failSoundId = 25;
	public int soundId;
	public int animId;
	public int nbIntsToSend;

	public SkillC(){
		this.ciblage=99;
		this.id = 99;
	}

	public virtual void resolve(int x, int y, Skill skill){
		int targetID = Game.instance.getBoard().getTileC(x,y).getCharacterID();
		int level = skill.Power;
		CardC target;
				
		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
			if(targetID!=-1){
				target = Game.instance.getCards().getCardC(targetID);
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

		if(Game.instance.getCurrentCard().isSanguinaire()){
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].sanguinaireEffects(Game.instance.getCurrentCardID(), Game.instance.getCurrentCard().getCardM().getSkill(0).Power);
			}
			else{
				Game.instance.launchCorou("SanguinaireEffectsSkillRPC", this.id, Game.instance.getCurrentCard().getCardM().getSkill(0).Power);
			}
		}

		if(Game.instance.getCurrentCard().getCardM().getCharacterType()==72){
			List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(new TileM(x,y));
			for(int i = 0 ; i < neighbours.Count;i++){
				if(Game.instance.getBoard().getTileC(neighbours[i]).getCharacterID()!=-1 && Game.instance.getBoard().getTileC(neighbours[i]).getCharacterID()!=Game.instance.getCurrentCardID()){
					targetID = Game.instance.getBoard().getTileC(neighbours[i]).getCharacterID();
					target = Game.instance.getCards().getCardC(targetID);
					if(UnityEngine.Random.Range(1,101)<=5*Game.instance.getCurrentCard().getCardM().getSkill(0).Power){
						if(UnityEngine.Random.Range(1,101)<=target.getEsquive()){
							
						}
						else{
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
									Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, UnityEngine.Random.Range(1,101));
								}
								else if(nbIntsToSend==2){
									Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, UnityEngine.Random.Range(1,101), UnityEngine.Random.Range(1,101));
								}
							}
						}
					}
				}
			}
		}
	}

	public virtual void resolve(Skill skill, int targetID){
		CardC target = Game.instance.getCards().getCardC(targetID);
		int level = skill.Power;
		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
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
					Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, UnityEngine.Random.Range(1,101));
				}
				else if(nbIntsToSend==2){
					Game.instance.launchCorou("EffectsSkillRPC", this.id, targetID, UnityEngine.Random.Range(1,101), UnityEngine.Random.Range(1,101));
				}
				Game.instance.launchCorou("PlaySoundRPC", this.id);
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

		if(Game.instance.getCurrentCard().isSanguinaire()){
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].sanguinaireEffects(Game.instance.getCurrentCardID(), Game.instance.getCurrentCard().getCardM().getSkill(0).Power);
			}
			else{
				Game.instance.launchCorou("SanguinaireEffectsSkillRPC", this.id, Game.instance.getCurrentCard().getCardM().getSkill(0).Power);
			}
		}
	}

	public virtual List<TileM> getTargetTiles(int[,] board, CardC card, TileM tile){
		List<TileM> targets = new List<TileM>();
		if(this.ciblage==1){
			targets = Game.instance.getBoard().getAdjacentOpponentsTargets(board, card, tile);
		}
		else if(this.ciblage==2){
			targets = Game.instance.getBoard().getAdjacentAllysTargets(board, card, tile);
		}
		else if(this.ciblage==3){
			targets = Game.instance.getBoard().getMySelfWithNeighbours(board, card, tile);
		}
		else if(this.ciblage==4){
			targets = Game.instance.getBoard().get1TileAwayOpponents(board, card, tile);
		}
		else if(this.ciblage==5){
			targets = Game.instance.getBoard().getAdjacentCristals(board, card, tile);
		}
		else if(this.ciblage==6){
			targets = Game.instance.getBoard().getOpponentsTargets(board, card, tile);
		}
		else if(this.ciblage==7){
			targets = Game.instance.getBoard().getAdjacentTargets(board, card, tile);
		}
		else if(this.ciblage==8){
			targets = Game.instance.getBoard().getAllysTargets(board, card, tile);
		}
		else if(this.ciblage==9){
			targets = Game.instance.getBoard().getAdjacentFreeTilesTargets(board, card, tile);
		}
		else if(this.ciblage==10){
			targets = Game.instance.getBoard().getMyselfWithOpponents(board, card, tile);
		}
		else if(this.ciblage==11){
			targets = Game.instance.getBoard().getMyselfWithOpponentTraps(board, card, tile);
		}
		else if(this.ciblage==12){
			targets = Game.instance.getBoard().getMyselfWithNeighbours4D(board, card, tile);
		}
		else if(this.ciblage==13){
			targets = Game.instance.getBoard().getNearestNeighbours4D(board, card, tile);
		}
		else if(this.ciblage==14){
			targets = Game.instance.getBoard().getMyself(board, card, tile);
		}
		else if(this.ciblage==15){
			targets = Game.instance.getBoard().getNearestNeighboursDiagonal(board, card, tile);
		}
		else if(this.ciblage==16){
			targets = Game.instance.getBoard().getNearestNeighbourTiles4D(board, card, tile);
		}
		else if(this.ciblage==17){
			targets = Game.instance.getBoard().getMyselfWithSomeone(board, card, tile);
		}
		else if(this.ciblage==18){
			targets = Game.instance.getBoard().getMyselfWithSomeone(board, card, tile);
		}
		else if(this.ciblage==19){
			targets = Game.instance.getBoard().getAnyoneTargets(board, card, tile);
		}
		else if(this.ciblage==20){
			targets = Game.instance.getBoard().getAnyoneAdjacentCristal(board, card, tile);
		}
		else if(this.ciblage==21){
			targets = Game.instance.getBoard().getAnyoneAdjacentCristoid(board, card, tile);
		}
		else if(this.ciblage==22){
			targets = Game.instance.getBoard().getMyselfWithSermon(board, card, tile);
		}
		return targets;
	}

	public virtual string getEmptyTargetText(){
		string s = "" ;
		if(this.ciblage==1){
			s = WordingGame.getText(72);
		}
		else if(this.ciblage==2){
			s = WordingGame.getText(85);
		}
		else if(this.ciblage==3){
			s = WordingGame.getText(86);
		}
		else if(this.ciblage==4){
			s = WordingGame.getText(101);
		}
		else if(this.ciblage==5){
			s = WordingGame.getText(102);
		}
		else if(this.ciblage==6){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==7){
			s = WordingGame.getText(86);
		}
		else if(this.ciblage==8){
			s = WordingGame.getText(115);
		}
		else if(this.ciblage==9){
			s = WordingGame.getText(30);
		}
		else if(this.ciblage==10){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==11){
			s = WordingGame.getText(120);
		}
		else if(this.ciblage==12){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==13){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==14){
			s = WordingGame.getText(134);
		}
		else if(this.ciblage==15){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==16){
			s = WordingGame.getText(127);
		}
		else if(this.ciblage==17){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==18){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==19){
			s = WordingGame.getText(104);
		}
		else if(this.ciblage==20){
			s = WordingGame.getText(132);
		}
		else if(this.ciblage==21){
			s = WordingGame.getText(104);
		}
		return s;
	}

	public virtual void sanguinaireEffects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		int degats = caster.getDegatsMaxAgainst(target, 13-level);

		target.displayAnim(1);
		target.displaySkillEffect(WordingGame.getText(77, new List<int>{degats}), 2);

		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));
	}

	public virtual void resolve(Skill s){
		Debug.Log("Skill non implémenté");
	}

	public virtual void effects(int x){
		Debug.Log("Skill non implémenté ef1");
	}

	public virtual void effects(int x, int y){
		Debug.Log("Skill non implémenté ef2");
	}

	public virtual void effects2(int x, int y){
		Debug.Log("Skill non implémenté effects2");
	}

	public virtual void effects(int x, int y, int z){
		Debug.Log("Skill non implémenté ef3");
	}

	public virtual void effects(int x, int y, int z, int z2){
		Debug.Log("Skill non implémenté ef3");
	}

	public virtual void fail(){
		Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(75), 2);
	}

	public virtual void dodge(int x){
		Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id), 1);
		Game.instance.getCards().getCardC(x).displaySkillEffect(WordingGame.getText(76)+"\n"+WordingSkills.getName(this.id),0);
	}

	public virtual string getSkillText(int i, int level){
		Debug.Log("Skill non implémenté gst");
		return "";
	}

	public virtual int getActionScore(TileM t, Skill s, int[,] board){
		Debug.Log("Skill non implémenté gas");
		return 0 ;
	}

	public virtual void playSound(){
		SoundController.instance.playSound(this.soundId);
	}

	public virtual void playFailSound(){
		SoundController.instance.playSound(this.failSoundId);
	}

	public virtual void playDodgeSound(){
		SoundController.instance.playSound(this.dodgeSoundId);
	}

}
