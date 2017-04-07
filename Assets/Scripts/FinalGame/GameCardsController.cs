using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCardsController
{
	GameObject[] gamecards;
	int[] playOrder;
	int index;

	public GameCardsController ()
	{
		this.gamecards = new GameObject[8];
		this.playOrder = new int[8];
		this.index = -1;
	}

	public void addCard(int i, GameObject g){
		this.gamecards[i]=g;
	}

	public GameCardController getCardController(int i){
		return this.gamecards[i].GetComponent<GameCardController>();
	}

	public int getCurrentCardID(){
		return this.playOrder[this.index];
	}

	public void incrementIndex(){
		this.index++;
		if(this.index>7){
			this.index=0;
		}
		if(this.getCurrentCard().isMine()){
			NewGameController.instance.getInterludeController().setBlue(WordingGame.getText(18));
		}
		else{
			NewGameController.instance.getInterludeController().setRed(WordingGame.getText(19));
		}
		NewGameController.instance.getInterludeController().launch();
	}

	public void startBlinking(){
		this.getCurrentCard().startBlinking();
	}

	public void setOrder(int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8){
		this.playOrder[0]=i1;
		this.playOrder[1]=i2;
		this.playOrder[2]=i3;
		this.playOrder[3]=i4;
		this.playOrder[4]=i5;
		this.playOrder[5]=i6;
		this.playOrder[6]=i7;
		this.playOrder[7]=i8;
	}

	public GameCardController getCurrentCard(){
		return this.getCardController(this.playOrder[this.index]);
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

		this.getCardController(0+offset).setGameCard(new GameCardModel(10,15,2,new SkillModel(68,2), new SkillModel(1,2), new SkillModel(2,4), new SkillModel(3,6)));
		this.getCardController(1+offset).setGameCard(new GameCardModel(11,16,3,new SkillModel(69,4), new SkillModel(4,3), new SkillModel(5,5), new SkillModel(6,7)));
		this.getCardController(2+offset).setGameCard(new GameCardModel(12,17,4,new SkillModel(70,6), new SkillModel(7,2), new SkillModel(8,4), new SkillModel(9,6)));
		this.getCardController(3+offset).setGameCard(new GameCardModel(13,18,5,new SkillModel(71,8), new SkillModel(10,3), new SkillModel(11,5), new SkillModel(12,7)));
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

		this.getCardController(0+offset).setGameCard(new GameCardModel(14,19,2,new SkillModel(72,2), new SkillModel(1,2), new SkillModel(2,4), new SkillModel(3,6)));
		this.getCardController(1+offset).setGameCard(new GameCardModel(15,20,3,new SkillModel(73,4), new SkillModel(4,3), new SkillModel(5,5), new SkillModel(6,7)));
		this.getCardController(2+offset).setGameCard(new GameCardModel(16,21,4,new SkillModel(75,6), new SkillModel(7,2), new SkillModel(8,4), new SkillModel(9,6)));
		this.getCardController(3+offset).setGameCard(new GameCardModel(17,22,5,new SkillModel(76,8), new SkillModel(10,3), new SkillModel(11,5), new SkillModel(12,7)));
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

	public void initOrder(bool b){
		if(b){
			this.playOrder[0]=0;
			this.playOrder[1]=4;
			this.playOrder[2]=1;
			this.playOrder[3]=5;
			this.playOrder[4]=2;
			this.playOrder[5]=6;
			this.playOrder[6]=3;
			this.playOrder[7]=7;
		}
		else{
			this.playOrder[1]=0;
			this.playOrder[0]=4;
			this.playOrder[3]=1;
			this.playOrder[2]=5;
			this.playOrder[5]=2;
			this.playOrder[4]=6;
			this.playOrder[7]=3;
			this.playOrder[6]=7;
		}
	}

	public void loadDestinations(){
		for (int i = 0 ; i < 8 ; i++){
			if(!this.getCardController(i).isDead()){
				this.getCardController(i).setDestinations(this.getDestinations(i));
			}
		}
	}

	public bool[,] getDestinations(int i){
		bool[,] hasBeenPassages = new bool[6, 8];
		bool[,] isDestination = new bool[6, 8];
		for(int l = 0 ; l < 6 ; l++){
			for(int k = 0 ; k < 8 ; k++){
				hasBeenPassages[l,k]=false;
				isDestination[l,k]=false;
			}
		}
		List<TileModel> baseTiles = new List<TileModel>();
		List<TileModel> tempTiles = new List<TileModel>();
		List<TileModel> tempNeighbours ;
		baseTiles.Add(this.getCardController(i).getTileModel());
		int move = this.getCardController(i).getGameCardModel().getMove();
		
		int j = 0 ;
		bool mine = this.getCardController(i).isMine()	;
		while (j < move){
			tempTiles = new List<TileModel>();
			for(int k = 0 ; k < baseTiles.Count ; k++){
				tempNeighbours = NewGameController.instance.getTilesController().getTileNeighbours(baseTiles[k]);
				for(int l = 0 ; l < tempNeighbours.Count ; l++){
					if(!hasBeenPassages[tempNeighbours[l].getX(), tempNeighbours[l].getY()]){
						if(NewGameController.instance.getTilesController().getTileController(tempNeighbours[l].getX(), tempNeighbours[l].getY()).canPassOver(mine)){
							tempTiles.Add(tempNeighbours[l]);
							hasBeenPassages[tempNeighbours[l].getX(), tempNeighbours[l].getY()]=true;
						}
					}
					if(NewGameController.instance.getTilesController().getTileController(tempNeighbours[l].getX(), tempNeighbours[l].getY()).isEmpty()){
						if(!isDestination[tempNeighbours[l].getX(), tempNeighbours[l].getY()]){
							isDestination[tempNeighbours[l].getX(), tempNeighbours[l].getY()]=true;
						}
					}
				}	
			}
			baseTiles = new List<TileModel>();
			for(int l = 0 ; l < tempTiles.Count ; l++){
				baseTiles.Add(tempTiles[l]);
			}
			j++;
		}
		isDestination[this.getCardController(i).getTileModel().getX(),this.getCardController(i).getTileModel().getY()]=true;
		return isDestination;
	}
}