using UnityEngine;
using System.Collections.Generic;

public class PistoLest : GameSkill
{
	public PistoLest()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"PistoLest","PistoLest"});
		texts.Add(new string[]{". Actif 1 tour",". For 1 turn"});
		texts.Add(new string[]{"-ARG1 PV\n+ARG2 MOV\npour 1 tour","-ARG1 HP\n+ARG2 MOV\nfor 1 turn"});
		texts.Add(new string[]{"-ARG1 PV\n+ARG2 MOV\npour 1 tour\nVirus","-ARG1 HP\n+ARG2 MOV\nfor 1 turn\nVirus"});
		texts.Add(new string[]{"PV : ARG1 -> [ARG2-ARG3]\n+ARG4 MOV\nActif 1 tour"});
		base.ciblage = 3 ;
		base.auto = false;
		base.id = 5 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				int amount = Random.Range(1,2*level+2+1);
				GameController.instance.applyOn2(target, amount);
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro2(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), amount, GameView.instance.getCurrentCard().Skills[0].Power*5+10);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(34);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int amount){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, amount);
		int move = -1*Mathf.Min(targetCard.getMove()-1,1);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 5, this.getText(0), ""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.getPlayingCardController(target).addMoveModifyer(new Modifyer(move, 1, 5, this.getText(0), this.getText(1)));
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.recalculateDestinations();

		GameView.instance.displaySkillEffect(target,this.getText(2, new List<int>{damages,move}), 0);	
		GameView.instance.addAnim(2,GameView.instance.getTile(target));
	}	

	public override void applyOnViro2(int target, int amount, int amount2){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = Mathf.RoundToInt(currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(amount*amount2/100f)));
		int move = -1*Mathf.Min(targetCard.getMove()-1, Mathf.RoundToInt(1*amount2/100f));

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 5, this.getText(0), ""), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.getPlayingCardController(target).addMoveModifyer(new Modifyer(move, 1, 5, this.getText(0), this.getText(1)));
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.recalculateDestinations();

		GameView.instance.displaySkillEffect(target, this.getText(3, new List<int>{damages,move}), 0);	
		GameView.instance.addAnim(2,GameView.instance.getTile(target));
	}	

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, 2*level+2);
		int move = -1*Mathf.Min(targetCard.getMove()-1,1);

		string text = this.getText(4, new List<int>{targetCard.getLife(),(targetCard.getLife()-1),(targetCard.getLife()-damages),move});
		
		int amount =WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard2;
		int proba = WordingSkills.getProba(s.Id,s.Power);

		int levelMin = Mathf.FloorToInt((1)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));
		int levelMax = Mathf.FloorToInt((2+s.Power*2)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));

		score+=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*((200*(Mathf.Max(0f,levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f));
		score+=Mathf.RoundToInt(5-targetCard.getLife()/10f);
		score+= Mathf.Min(targetCard.getMove(),1)*10;

		if(currentCard.isVirologue()){
			int levelMin2 = Mathf.RoundToInt(Mathf.FloorToInt(1*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)))*s.Power*(10f+currentCard.Skills[0].Power*5f)/100f);
			int levelMax2 = Mathf.RoundToInt(Mathf.FloorToInt((2+s.Power*2)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)))*(10f+currentCard.Skills[0].Power*5f)/100f);
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					if(targetCard2.isMine){
						score+=Mathf.RoundToInt(((proba-targetCard2.getMagicalEsquive())/100f)*((200*(Mathf.Max(0f,levelMax2-targetCard2.getLife())))+(((levelMin2+Mathf.Min(levelMax2,targetCard2.getLife()))/2f)*Mathf.Min(levelMax2,targetCard2.getLife())))/(levelMax2-levelMin2+1f));
					}
					else{
						score-=Mathf.RoundToInt(((proba-targetCard2.getMagicalEsquive())/100f)*((200*(Mathf.Max(0f,levelMax2-targetCard2.getLife())))+(((levelMin2+Mathf.Min(levelMax2,targetCard2.getLife()))/2f)*Mathf.Min(levelMax2,targetCard2.getLife())))/(levelMax2-levelMin2+1f));
					}
				}
			}
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
