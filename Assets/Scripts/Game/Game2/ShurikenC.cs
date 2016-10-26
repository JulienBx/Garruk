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
