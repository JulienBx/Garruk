using System;
using UnityEngine;

public class GameCardsController
{
	GameObject[] gamecards;

	public GameCardsController ()
	{
		this.gamecards = new GameObject[8];
	}

	public void addCard(int i, GameObject g){
		this.gamecards[i]=g;
	}

	public GameCardController getCardController(int i){
		return this.gamecards[i].GetComponent<GameCardController>();
	}

	public void show(){
		
	}

	public void createMyFakeHand(bool isFirstPlayer, GameObject g1, GameObject g2, GameObject g3, GameObject g4){
		int offset = 0 ;
		if(!isFirstPlayer){
			offset = 4;
		}

		this.addCard(0+offset,g1);
		this.addCard(1+offset,g2);
		this.addCard(2+offset,g3);
		this.addCard(3+offset,g4);

		this.getCardController(0+offset).setGameCard(new GameCardModel(10,15,new SkillModel(68,2), new SkillModel(1,2), new SkillModel(2,4), new SkillModel(3,6)));
		this.getCardController(1+offset).setGameCard(new GameCardModel(11,16,new SkillModel(69,4), new SkillModel(4,3), new SkillModel(5,5), new SkillModel(6,7)));
		this.getCardController(2+offset).setGameCard(new GameCardModel(12,17,new SkillModel(70,6), new SkillModel(7,2), new SkillModel(8,4), new SkillModel(9,6)));
		this.getCardController(3+offset).setGameCard(new GameCardModel(13,18,new SkillModel(71,8), new SkillModel(10,3), new SkillModel(11,5), new SkillModel(12,7)));
		this.getCardController(0+offset).setIsMine(true);
		this.getCardController(1+offset).setIsMine(true);
		this.getCardController(2+offset).setIsMine(true);
		this.getCardController(3+offset).setIsMine(true);
	}

	public void createHisFakeHand(bool isFirstPlayer, GameObject g1, GameObject g2, GameObject g3, GameObject g4){
		int offset = 4 ;
		if(!isFirstPlayer){
			offset = 0;
		}

		this.addCard(0+offset,g1);
		this.addCard(1+offset,g2);
		this.addCard(2+offset,g3);
		this.addCard(3+offset,g4);

		this.getCardController(0+offset).setGameCard(new GameCardModel(14,19,new SkillModel(72,2), new SkillModel(1,2), new SkillModel(2,4), new SkillModel(3,6)));
		this.getCardController(1+offset).setGameCard(new GameCardModel(15,20,new SkillModel(73,4), new SkillModel(4,3), new SkillModel(5,5), new SkillModel(6,7)));
		this.getCardController(2+offset).setGameCard(new GameCardModel(16,21,new SkillModel(75,6), new SkillModel(7,2), new SkillModel(8,4), new SkillModel(9,6)));
		this.getCardController(3+offset).setGameCard(new GameCardModel(17,22,new SkillModel(76,8), new SkillModel(10,3), new SkillModel(11,5), new SkillModel(12,7)));
		this.getCardController(0+offset).setIsMine(false);
		this.getCardController(1+offset).setIsMine(false);
		this.getCardController(2+offset).setIsMine(false);
		this.getCardController(3+offset).setIsMine(false);
	}

	public void initCards(){
		for(int i = 0 ; i < 8 ; i++){
			this.getCardController(i).initCard();
			if(i<4){
				NewGameController.instance.setCharacterToTile(i,new TileModel(1+i%4,0));
			}
			else{
				NewGameController.instance.setCharacterToTile(i,new TileModel(4-i%4,7));
			}
			if(this.getCardController(i).isMine()){
				this.getCardController(i).setMovable(true);
			}		
		}

	}

	public void resize(){
		for(int i = 0 ; i < 8 ; i++){
			this.getCardController(i).setPosition(NewGameController.instance.getTilesController().getTileController(this.getCardController(i).getTileModel()).getLocalPosition());
			this.getCardController(i).setScale(NewGameController.instance.getTilesController().getTileController(this.getCardController(i).getTileModel()).getLocalScale()*0.9f);
			this.getCardController(i).show(true);
		}
	}
}