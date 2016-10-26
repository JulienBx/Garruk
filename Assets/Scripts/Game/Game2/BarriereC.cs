using UnityEngine;
using System.Collections.Generic;

public class BarriereC : SkillC
{
	public BarriereC(){
		base.id = 29 ;
		base.ciblage = 14;
		base.animId = 2;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level, int z){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		target.addShieldModifyer(new ModifyerM(10+4*level, 13, "", "",-1));
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();

		string text = WordingGame.getText(87, new List<int>{10+4*level});
		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
//		CardC target = Game.instance.getCards().getCardC(Game.instance.getBoard().getTileC(t).getCharacterID());
//		CardC caster = Game.instance.getCurrentCard();
//		int score = 0;
//
//		if(Game.instance.getCurrentCard().getBouclier()>0){
//			score = 0;
//		}
//		else{
//			score = Mathf.RoundToInt(((10f+4f*s.Power)/2f)*(Game.instance.getCurrentCard().getLife()/30f));
//		}
//
//		return score;
		return 0 ;
	}
}
