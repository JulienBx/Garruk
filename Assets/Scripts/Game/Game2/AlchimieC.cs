using UnityEngine;
using System.Collections.Generic;

public class AlchimieC : SkillC
{
	public AlchimieC(){
		base.id = 42 ;
		base.ciblage = 14;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void resolve(int x, int y, Skill skill){
		TileM targetTile ;
		int level = skill.Power;
		List<TileM> neighbours = Game.instance.getBoard().getTileNeighbours(new TileM(x,y));

		for(int i = 0 ; i < neighbours.Count ; i++){
			targetTile = neighbours[i];
			if(Game.instance.getBoard().getTileC(targetTile).isEmpty()){
				if(UnityEngine.Random.Range(1,101)<=WordingSkills.getProba(this.id, level)){
					Debug.Log(WordingSkills.getProba(this.id, level));
					if(Game.instance.isIA() || Game.instance.isTutorial()){
						this.effects(targetTile.x, targetTile.y);
					}
					else{
						Game.instance.launchCorou("EffectsSkillRPC", this.id, targetTile.x, targetTile.y);
					}
				}
				else{
					if(Game.instance.isIA() || Game.instance.isTutorial()){
						this.effects2(targetTile.x, targetTile.y);
					}
					else{
						Game.instance.launchCorou("EffectsSkill2RPC", this.id, targetTile.x, targetTile.y);
					}
				}
			}
		}
	}

	public override void effects(int x, int y){
		Game.instance.getBoard().createRock(x, y);
		Game.instance.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(136),0);
	}

	public override void effects2(int x, int y){
		Game.instance.getBoard().getTileC(x,y).displaySkillEffect(WordingGame.getText(135),1);
	}

	public override string getSkillText(int targetID, int level){
		string s = Game.instance.getCurrentSkillButtonC().getSkillText();
		return s ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		int score = -1;

		return score;
	}
}
