using UnityEngine;
using System.Collections.Generic;

public class SkillC
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
	public int soundId;
	public int animId;

	public SkillC(){
		this.ciblage=99;
		this.id = 99;
	}

	public virtual void resolve(int x, int y, Skill skill){
		int targetID = Game.instance.getBoard().getTileC(x,y).getCharacterID();
		CardC target = Game.instance.getCards().getCardC(targetID);
		int level = skill.Power;
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
					Game.instance.getSkills().skills[this.id].effects(targetID, level);
				}
				else{
					GameRPC.instance.launchRPC("EffectsSkillRPC", this.id, targetID, level);
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

	public virtual List<TileM> getTargetTiles(int[,] board, CardC card){
		List<TileM> targets = new List<TileM>();
		if(this.ciblage==1){
			targets = Game.instance.getBoard().getAdjacentOpponentsTargets(board, card);
		}
		if(this.ciblage==2){
			targets = Game.instance.getBoard().getAdjacentAllysTargets(board, card);
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
}
