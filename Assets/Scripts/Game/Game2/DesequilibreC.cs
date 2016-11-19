using UnityEngine;
using System.Collections.Generic;

public class DesequilibreC : SkillC
{
	public DesequilibreC(){
		base.id = 92 ;
		base.ciblage = 1;
		base.animId = 0;
		base.soundId = 25;
		base.nbIntsToSend = 0;
	}

	public override void effects(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();
		TileM targetTile = target.getTileM();
		TileM casterTile = caster.getTileM();
		TileM moveTile = new TileM(targetTile.x+targetTile.x-casterTile.x, targetTile.y+targetTile.y-casterTile.y);
		string text ;

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));

		target.displayAnim(base.animId);
		text = WordingGame.getText(77, new List<int>{degats});
		target.addDamageModifyer(new ModifyerM(degats, -1, "", "",-1));

		if(moveTile.x>=0 && moveTile.y>=0 && moveTile.x<Game.instance.getBoard().getBoardWidth() && moveTile.y<Game.instance.getBoard().getBoardHeight()){
			if(Game.instance.getBoard().getTileC(moveTile).isEmpty()){
				text+="\n"+WordingGame.getText(95);
				Game.instance.moveOn(moveTile.x,moveTile.y,targetID);
			}
		}
		target.displaySkillEffect(text, 2);
	}

	public override string getSkillText(int targetID, int level){
		CardC target = Game.instance.getCards().getCardC(targetID);
		CardC caster = Game.instance.getCurrentCard();
		TileM targetTile = target.getTileM();
		TileM casterTile = caster.getTileM();
		TileM moveTile = new TileM(targetTile.x+targetTile.x-casterTile.x, targetTile.y+targetTile.y-casterTile.y);
		string text ;

		int degats = caster.getDegatsAgainst(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*level)/100f));
		text = WordingGame.getText(78, new List<int>{target.getLife(), target.getLife()-degats});

		if(moveTile.x>=0 && moveTile.y>=0 && moveTile.x<Game.instance.getBoard().getBoardWidth() && moveTile.y<Game.instance.getBoard().getBoardHeight()){
			if(Game.instance.getBoard().getTileC(moveTile).isEmpty()){
				text+="\n"+WordingGame.getText(95);
			}
		}

		return text ;
	}

	public override int getActionScore(TileM t, Skill s, int[,] board){
		CardC target = Game.instance.getCards().getCardC(board[t.x,t.y]);
		CardC caster = Game.instance.getCurrentCard();
		TileM targetTile = target.getTileM();
		TileM casterTile = caster.getTileM();
		TileM moveTile = new TileM(targetTile.x+targetTile.x-casterTile.x, targetTile.y+targetTile.y-casterTile.y);

		int score = caster.getDamageScore(target, Mathf.RoundToInt(caster.getAttack()*(20f+8f*s.Power)/100f));
		score = Mathf.RoundToInt(s.getProba(s.Power)*(score*(100-target.getEsquive())/100f)/100f);

		if(moveTile.x>=0 && moveTile.y>=0 && moveTile.x<Game.instance.getBoard().getBoardWidth() && moveTile.y<Game.instance.getBoard().getBoardHeight()){
			if(Game.instance.getBoard().getTileC(moveTile).isTarget()){
				score+=20;
			}
			if(Game.instance.getBoard().getTileC(moveTile).isEmpty()){
				if(moveTile.y==0 || moveTile.y==7){
					score+=5*Game.instance.getIndexMeteores();
				}
				else if(moveTile.y==1 || moveTile.y==6){
					score+=Mathf.Max(0,5*(Game.instance.getIndexMeteores()-1));
				}
				else if(moveTile.y==2 || moveTile.y==5){
					score+=Mathf.Max(0,5*(Game.instance.getIndexMeteores()-2));
				}
				else if(moveTile.y==3 || moveTile.y==4){
					score+=Mathf.Max(0,5*(Game.instance.getIndexMeteores()-3));
				}
			}
		}

		return score;
	}
}
