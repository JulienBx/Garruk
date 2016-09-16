using UnityEngine;
using System.Collections.Generic;

public class SkillC : MonoBehaviour
{
	public int numberOfExpectedTargets ; 

	public List<int> targets;
	public List<int> results;
	public List<int> values;
	public List<Tile> tileTargets;
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
		CardC target = Game.instance.getCards().getCardC(targetID);
		int level = skill.Power;
		if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
			if(UnityEngine.Random.Range(1,101)<=target.getEsquive()){
				if(Game.instance.isIA() || Game.instance.isTutorial()){
					Game.instance.getSkills().skills[this.id].dodge(targetID);
					Game.instance.getSkills().skills[this.id].playDodgeSound();
				}
				else{
					StartCoroutine(GameRPC.instance.launchRPC("DodgeSkillRPC", this.id, targetID));
					StartCoroutine(GameRPC.instance.launchRPC("PlayDodgeSoundRPC", this.id));
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
					Game.instance.getSkills().skills[this.id].playSound();
				}
				else{
					if(nbIntsToSend==0){
						StartCoroutine(GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID, level));
					}
					else if(nbIntsToSend==1){
						StartCoroutine(GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID, UnityEngine.Random.Range(1,101)));
					}

					StartCoroutine(GameRPC.instance.launchRPC("PlaySoundRPC", this.id));
				}
			}
		}
		else{
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].fail();
				Game.instance.getSkills().skills[this.id].playFailSound();
			}
			else{
				StartCoroutine(GameRPC.instance.launchRPC("FailSkillRPC", this.id));
				StartCoroutine(GameRPC.instance.launchRPC("PlayFailSoundRPC", this.id));
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
				Game.instance.getSkills().skills[this.id].playSound();
			}
			else{
				if(nbIntsToSend==0){
					StartCoroutine(GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID, level));
				}
				else if(nbIntsToSend==1){
					StartCoroutine(GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID, UnityEngine.Random.Range(1,101)));
				}

				StartCoroutine(GameRPC.instance.launchRPC("PlaySoundRPC", this.id));
			}	
		}
		else{
			if(Game.instance.isIA() || Game.instance.isTutorial()){
				Game.instance.getSkills().skills[this.id].fail();
				Game.instance.getSkills().skills[this.id].playFailSound();
			}
			else{
				StartCoroutine(GameRPC.instance.launchRPC("FailSkillRPC", this.id));
				StartCoroutine(GameRPC.instance.launchRPC("PlayFailSoundRPC", this.id));
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
		return s;
	}

	public virtual void resolve(Skill s){
		Debug.Log("Skill non implémenté");
	}

	public virtual void effects(int x){
		Debug.Log("Skill non implémenté");
	}

	public virtual void effects(int x, int y){
		Debug.Log("Skill non implémenté");
	}

	public virtual void effects(int x, int y, int z){
		Debug.Log("Skill non implémenté");
	}

	public virtual void fail(){
		Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id)+"\n"+WordingGame.getText(75), 2);
		Game.instance.getCurrentCard().launchSkillEffect();
	}

	public virtual void dodge(int x){
		Game.instance.getCurrentCard().displaySkillEffect(WordingSkills.getName(this.id), 1);
		Game.instance.getCards().getCardC(x).displaySkillEffect(WordingGame.getText(76)+"\n"+WordingSkills.getName(this.id),0);
	}

	public virtual string getSkillText(int i, int level){
		Debug.Log("Skill non implémenté");
		return "";
	}

	public virtual int getActionScore(TileM t, Skill s, int[,] board){
		Debug.Log("Skill non implémenté");
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
