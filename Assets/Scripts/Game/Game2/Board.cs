using System;
using UnityEngine;
using System.Collections.Generic;

public class Board
{
	int boardWidth = 6 ; 
	int boardHeight = 8;
	int nbTiles = 0 ;
	GameObject[,] tiles ;
	GameObject[] verticalBorders ;
	GameObject[] horizontalBorders ;

	public Board ()
	{
		this.tiles = new GameObject[this.boardWidth,this.boardHeight];
		this.verticalBorders = new GameObject[this.boardWidth+1];
		this.horizontalBorders = new GameObject[this.boardHeight+1];
	}

	public void addHorizontalBorder(int i, GameObject horizontalBorder){
		this.horizontalBorders [i] = horizontalBorder;
	}

	public void sizeHorizontalBorder(int i, Vector3 position, Vector3 scale){
		this.horizontalBorders [i].transform.localPosition = position;
		this.horizontalBorders [i].transform.localScale = scale;
	}

	public void sizeVerticalBorder(int i, Vector3 position, Vector3 scale){
		this.verticalBorders [i].transform.localPosition = position;
		this.verticalBorders [i].transform.localScale = scale;
	}

	public void addVerticalBorder(int i, GameObject verticalBorder){
		this.verticalBorders [i] = verticalBorder;
	}

	public int getBoardWidth(){
		return this.boardWidth;
	}

	public int getBoardHeight(){
		return this.boardHeight;
	}

	public TileC getTileC(int x, int y){
		return this.tiles[x,y].GetComponent<TileC>();
	}

	public TileC getTileC(TileM t){
		return this.tiles[t.x,t.y].GetComponent<TileC>();
	}

	public void addTile(int x, int y, bool isRock, GameObject g){
		this.nbTiles++;
		this.tiles[x,y] = g ;
		this.getTileC(x,y).setTile(x,y);
		this.getTileC(x,y).setRock(isRock);
	}

	public bool isLoaded(){
		return (this.nbTiles==48);
	}

	public int[,] getOpponentStartingTilesOccupancy(){
		int[, ]tilesOccupancy = new int[6,2];
		for(int i = 0 ; i < this.boardWidth ; i++){
			for(int j = 6 ; j < this.boardHeight ; j++){
				if(this.getTileC(i,j).isRock()){
					tilesOccupancy[i,j-6]=2;
				}
				else if(this.getTileC(i,j).hasCharacter()){
					tilesOccupancy[i,j-6]=1;
				}
				else{
					tilesOccupancy[i,j-6]=0;
				}
			}
		}
		return tilesOccupancy;
	}

	public List<TileM> getTileNeighbours(TileM t){
		List<TileM> tiles = new List<TileM>();
		if(t.x>0){
			tiles.Add(new TileM(t.x-1,t.y));
		}
		if(t.y>0){
			tiles.Add(new TileM(t.x,t.y-1));
		}
		if(t.x<this.getBoardWidth()-1){
			tiles.Add(new TileM(t.x+1,t.y));
		}
		if(t.y<this.getBoardHeight()-1){
			tiles.Add(new TileM(t.x,t.y+1));
		}
		return tiles;
	} 

	public List<int> getNearestNeighbours4D(int[,] board, TileM t){
		List<int> targets = new List<int>();
		int compteur = t.x-1 ; 

		while(compteur>=0){
			if(board[compteur, t.y]>=0){
				targets.Add(board[compteur, t.y]);
				compteur=-1;
			}
			else if(board[compteur, t.y]==-2){
				compteur=-1;
			}
			compteur--;
		}

		compteur = t.x+1 ; 

		while(compteur<this.boardWidth){
			if(board[compteur, t.y]>=0){
				targets.Add(board[compteur, t.y]);
				compteur=this.boardWidth;
			}
			else if(board[compteur, t.y]==-2){
				compteur=this.boardWidth;
			}
			compteur++;
		}

		compteur = t.y+1 ; 

		while(compteur<this.boardHeight){
			if(board[t.x, compteur]>=0){
				targets.Add(board[t.x, compteur]);
				compteur=this.boardHeight;
			}
			else if(board[t.x, compteur]==-2){
				compteur=this.boardHeight;
			}
			compteur++;
		}

		compteur = t.y-1 ; 

		while(compteur>=0){
			if(board[t.x, compteur]>=0){
				targets.Add(board[t.x, compteur]);
				compteur=-1;
			}
			else if(board[t.x, compteur]==-2){
				compteur=-1;
			}
			compteur--;
		}

		return targets;
	} 

	public List<TileM> getNearestNeighbours4D(int[,] board, CardC c, TileM t){
		List<TileM> targets = new List<TileM>();
		int compteur = t.x-1 ; 

		while(compteur>=0){
			if(board[compteur, t.y]>=0){
				targets.Add(new TileM(compteur, t.y));
				compteur=-1;
			}
			else if(!c.isSniper()){
				if(board[compteur, t.y]==-2){
					compteur=-1;
				}
			}
			compteur--;
		}

		compteur = t.x+1 ; 

		while(compteur<this.boardWidth){
			if(board[compteur, t.y]>=0){
				targets.Add(new TileM(compteur, t.y));
				compteur=this.boardWidth;
			}
			else if(!c.isSniper()){
				if(board[compteur, t.y]==-2){
					compteur=-1;
				}
			}
			compteur++;
		}

		compteur = t.y+1 ; 

		while(compteur<this.boardHeight){
			if(board[t.x, compteur]>=0){
				targets.Add(new TileM(t.x, compteur));
				compteur=this.boardHeight;
			}
			else if(!c.isSniper()){
				if(board[t.x, compteur]==-2){
					compteur=-1;
				}
			}
			compteur++;
		}

		compteur = t.y-1 ; 

		while(compteur>=0){
			if(board[t.x, compteur]>=0){
				targets.Add(new TileM(t.x, compteur));
				compteur=-1;
			}
			else if(!c.isSniper()){
				if(board[t.x, compteur]==-2){
					compteur=-1;
				}
			}
			compteur--;
		}

		return targets;
	}

	public List<TileM> getNearestNeighbourTiles4D(int[,] board, CardC c, TileM t){
		List<TileM> targets = new List<TileM>();
		int compteur = t.x-1 ; 

		while(compteur>=0){
			if(board[compteur, t.y]==-2){
				targets.Add(new TileM(compteur, t.y));
				compteur=-1;
			}
			else if(!c.isSniper()){
				if(board[compteur, t.y]>=0){
					compteur=-1;
				}
			}
			compteur--;
		}

		compteur = t.x+1 ; 

		while(compteur<this.boardWidth){
			if(board[compteur, t.y]==-2){
				targets.Add(new TileM(compteur, t.y));
				compteur=this.boardWidth;
			}
			else if(!c.isSniper()){
				if(board[compteur, t.y]>=0){
					compteur=this.boardWidth;
				}
			}
			compteur++;
		}

		compteur = t.y+1 ; 

		while(compteur<this.boardHeight){
			if(board[t.x, compteur]==-2){
				targets.Add(new TileM(t.x, compteur));
				compteur=this.boardHeight;
			}
			else if(!c.isSniper()){
				if(board[t.x, compteur]>=0){
					compteur=this.boardHeight;
				}
			}
			compteur++;
		}

		compteur = t.y-1 ; 

		while(compteur>=0){
			if(board[t.x, compteur]==-2){
				targets.Add(new TileM(t.x, compteur));
				compteur=-1;
			}
			else if(!c.isSniper()){
				if(board[t.x, compteur]>=0){
					compteur=-1;
				}
			}
			compteur--;
		}

		return targets;
	}

	public List<TileM> getNearestNeighboursDiagonal(int[,] board, CardC c, TileM t){
		List<TileM> targets = new List<TileM>();

		int compteurX = t.x-1 ; 
		int compteurY = t.y-1 ; 

		while(compteurX>=0 && compteurY>=0){
			if(board[compteurX, compteurY]>=0){
				targets.Add(new TileM(compteurX, compteurY));
				compteurX=-1;
			}
			else if(!c.isSniper()){
				if(board[compteurX, compteurY]==-2){
					compteurX=-1;
				}
			}
			compteurX--;
			compteurY--;
		}

		compteurX = t.x-1 ; 
		compteurY = t.y+1 ; 

		while(compteurX>=0 && compteurY<this.boardHeight){
			if(board[compteurX, compteurY]>=0){
				targets.Add(new TileM(compteurX, compteurY));
				compteurX=-1;
			}
			else if(!c.isSniper()){
				if(board[compteurX, compteurY]==-2){
					compteurX=-1;
				}
			}
			compteurX--;
			compteurY++;
		}

		compteurX = t.x+1 ; 
		compteurY = t.y-1 ; 

		while(compteurY>=0 && compteurX<this.boardWidth){
			if(board[compteurX, compteurY]>=0){
				targets.Add(new TileM(compteurX, compteurY));
				compteurY=-1;
			}
			else if(!c.isSniper()){
				if(board[compteurX, compteurY]==-2){
					compteurY=-1;
				}
			}
			compteurX++;
			compteurY--;
		}

		compteurX = t.x+1 ; 
		compteurY = t.y+1 ; 

		while(compteurY<this.boardHeight && compteurX<this.boardWidth){
			if(board[compteurX, compteurY]>=0){
				targets.Add(new TileM(compteurX, compteurY));
				compteurY=99;
			}
			else if(!c.isSniper()){
				if(board[compteurX, compteurY]==-2){
					compteurY=99;
				}
			}
			compteurX++;
			compteurY++;
		}

		return targets;
	}

	public List<TileM> getMyselfWithNeighbours4D(int[,] board, CardC c, TileM t){
		List<TileM> target = new List<TileM>();
		List<int> targets = this.getNearestNeighbours4D(board, t);
		if(targets.Count>=1){
			target.Add(t);
		}
		return target;
	}

	public TileM getMouseTile(){
		Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		TileM tile = new TileM();

		if(Game.instance.isFirstPlayer()){
			tile.x = Mathf.FloorToInt(vec.x/Game.instance.getTileScale())+3;
			tile.y = Mathf.FloorToInt(vec.y/Game.instance.getTileScale())+4;
		}
		else{
			tile.x = (this.boardWidth-1)-(Mathf.FloorToInt(vec.x/Game.instance.getTileScale())+3);
			tile.y = (this.boardHeight-1)-(Mathf.FloorToInt(vec.y/Game.instance.getTileScale())+4);
		}

		return tile;
	}

	public List<TileM> getAdjacentOpponentsTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(t);
					}
				}
			}
		}
		return cibles;
	}

	public List<TileM> getAdjacentFreeTilesTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();

		foreach (TileM t in neighbourTiles)
		{
			if(this.getTileC(t).isEmpty()){
				cibles.Add(t);
			}
		}
		return cibles;
	}

	public List<TileM> getAdjacentTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					cibles.Add(t);
				}
			}
		}
		return cibles;
	}

	public List<TileM> getOpponentsTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();

		for(int i = 0 ; i < Game.instance.getCards().getNumberOfCards(); i++)
		{
			if(Game.instance.getCards().getCardC(i).canBeTargeted()){
				if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(i).getCardM().isMine()){
					cibles.Add(Game.instance.getCards().getCardC(i).getTileM());
				}
			}
		}
		return cibles;
	}

	public List<TileM> getAllysTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();

		for(int i = 0 ; i < Game.instance.getCards().getNumberOfCards(); i++)
		{
			if(Game.instance.getCards().getCardC(i).canBeTargeted()){
				if(i!=Game.instance.getCurrentCardID()){
					if(card.getCardM().isMine()==Game.instance.getCards().getCardC(i).getCardM().isMine()){
						cibles.Add(Game.instance.getCards().getCardC(i).getTileM());
					}
				}
			}
		}
		return cibles;
	}

	public List<TileM> getAdjacentCristals(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();

		foreach (TileM t in neighbourTiles)
		{
			if (board[t.x,t.y] == -2){
				cibles.Add(t);
			}
		}
		return cibles;
	}

	public List<TileM> get1TileAwayOpponents(int[,] board, CardC card, TileM tile){
		List<TileM> cibles = new List<TileM>();
		int playerID;

		if(tile.x>1){
			playerID = board[tile.x-2,tile.y];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x-2, tile.y));
					}
				}
			}
		}
		if(tile.y>1){
			playerID = board[tile.x,tile.y-2];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x, tile.y-2));
					}
				}
			}
		}
		if(tile.x<this.getBoardWidth()-2){
			playerID = board[tile.x+2,tile.y];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x+2, tile.y));
					}
				}
			}
		}
		if(tile.y<this.getBoardHeight()-2){
			playerID = board[tile.x,tile.y+2];
			if (playerID >= 0){
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()!=Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						cibles.Add(new TileM(tile.x, tile.y+2));
					}
				}
			}
		}
			
		return cibles;
	}

	public List<TileM> getAdjacentAllysTargets(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					if(card.getCardM().isMine()==Game.instance.getCards().getCardC(playerID).getCardM().isMine()){
						if(card.getCardM().getCharacterType()!=Game.instance.getCards().getCardC(playerID).getCardM().getCharacterType()){
							cibles.Add(t);
						}
					}
				}
			}
		}
		return cibles;
	}

	public List<TileM> getMySelfWithNeighbours(int[,] board, CardC card, TileM tile){
		List<TileM> neighbourTiles = this.getTileNeighbours(tile);
		List<TileM> cibles = new List<TileM>();
		bool hasSomeone = false;
		int playerID;

		foreach (TileM t in neighbourTiles)
		{
			playerID = board[t.x,t.y];
			if (playerID >= 0)
			{
				if(Game.instance.getCards().getCardC(playerID).canBeTargeted()){
					hasSomeone = true;
				}
			}
		}

		if(hasSomeone){
			cibles.Add(tile);
		}
		return cibles;
	}

	public List<TileM> getMyself(int[,] board, CardC card, TileM tile){
		List<TileM> cibles = new List<TileM>();

		if(Game.instance.getCurrentCard().canBeTargeted()){
			cibles.Add(tile);
		}
		return cibles;
	}

	public List<TileM> getMyselfWithOpponents(int[,] board, CardC card, TileM tile){
		List<TileM> cibles = new List<TileM>();
		bool hasSomeone = false;

		for (int i = 0 ; i < Game.instance.getCards().getNumberOfCards() ; i++){
			if (Game.instance.getCards().getCardC(i).getCardM().isMine()!=card.getCardM().isMine())
			{
				if(Game.instance.getCards().getCardC(i).canBeTargeted()){
					hasSomeone = true;
				}
			}
		}

		if(hasSomeone){
			cibles.Add(tile);
		}
		return cibles;
	}

	public List<TileM> getMyselfWithOpponentTraps(int[,] board, CardC card, TileM tile){
		List<TileM> cibles = new List<TileM>();
		if(this.getNumberOfHiddenEnnemyTraps(card.getCardM().isMine())>0){
			cibles.Add(tile);
		}
		return cibles;
	}

	public int[,] getCurrentBoard(){
		int[,] board = new int[this.boardWidth,this.boardHeight];
		for(int i = 0 ; i < this.boardWidth ; i++){
			for(int j = 0 ; j < this.boardHeight ; j++){
				board[i,j] = this.getTileC(i,j).getBoardValue();
			}
		}
		return board;
	}

	public void startTargets(List<TileM> targets){
		for (int i = 0 ; i < targets.Count ; i++){
			this.getTileC(targets[i]).setTarget(true);
		}
	}

	public void stopTargets(List<TileM> targets){
		for (int i = 0 ; i < targets.Count ; i++){
			this.getTileC(targets[i]).setTarget(false);
		}
	}

	public TileM getRandomEmptyTile(){
		bool found = false ;
		int x = -1;
		int y = -1;
		while(!found){
			x = UnityEngine.Random.Range(0,this.boardWidth);
			y = UnityEngine.Random.Range(0,this.boardHeight);

			if(this.getTileC(x,y).isEmpty()){
				found = true ;
			}
		}
		return new TileM(x,y);
	}

	public List<TileM> get4RandomEmptyCenterTile(){
		bool found = false ;
		int x = -1;
		int y = -1;
		List<TileM> tiles = new List<TileM>();

		while(tiles.Count<4){
			found = false ;
			while(!found){
				x = UnityEngine.Random.Range(0,this.boardWidth);
				y = UnityEngine.Random.Range(2,6);

				if(this.getTileC(x,y).isEmpty() && this.getTileC(x,y).getTrapId()==-1){
					found = true ;
				}
				for(int i = 0 ; i < tiles.Count ;i++){
					if(tiles[i].x==x && tiles[i].y==y){
						found = false ;
					}
				}

				if(found){
					tiles.Add(new TileM(x,y));
				}
			}
		}
		return tiles;
	}

	public int getNumberOfHiddenEnnemyTraps(bool isMe){
		int compteur = 0 ;
		for(int x = 0 ; x < this.boardWidth ; x++){
			for(int y = 0 ; y < this.boardHeight ; y++){
				if(this.getTileC(x,y).getTrapId()!=-1){
					if(this.getTileC(x,y).getTrapIsMine()!=isMe){
						compteur++;
					}
				}
			}
		}
		return compteur;
	}

	public void eraseTraps(bool isMe){
		for(int x = 0 ; x < this.boardWidth ; x++){
			for(int y = 0 ; y < this.boardHeight ; y++){
				if(this.getTileC(x,y).getTrapId()!=-1){
					if(this.getTileC(x,y).getTrapIsMine()!=isMe){
						this.getTileC(x,y).displayAnim(5);
						this.getTileC(x,y).displaySkillEffect(WordingGame.getText(119),0);
						this.getTileC(x,y).removeTrap();
					}
				}
			}
		}
	}

	public List<TileM> getUnitsStraightLine(TileM casterTile, TileM targetTile, int[,] board){
		bool end = false;
		List<TileM> targets = new List<TileM>();
		targets.Add(targetTile);
		if(casterTile.x==targetTile.x){
			if(targetTile.y>casterTile.y){
				for(int i = targetTile.y+1; i<8 && !end; i++){
					if(board[casterTile.x, i]==-2){
						if(!Game.instance.getCurrentCard().isSniper()){
							end = true;
						}
					}
					else if(board[casterTile.x, i]>=0){
						targets.Add(new TileM(casterTile.x,i));
					}
				}
			}
			else{
				for(int i = targetTile.y-1; i>=0 && !end; i--){
					if(board[casterTile.x, i]==-2){
						if(!Game.instance.getCurrentCard().isSniper()){
							end = true;
						}
					}
					else if(board[casterTile.x, i]>=0){
						targets.Add(new TileM(casterTile.x,i));
					}
				}
			}
		}
		else{
			if(targetTile.x>casterTile.x){
				for(int i = targetTile.x+1; i<6 && !end; i++){
					if(board[i,casterTile.y]==-2){
						if(!Game.instance.getCurrentCard().isSniper()){
							end = true;
						}
					}
					else if(board[i,casterTile.y]>=0){
						targets.Add(new TileM(i,casterTile.y));
					}
				}
			}
			else{
				for(int i = targetTile.x-1; i>=0 && !end; i--){
					if(board[i,casterTile.y]==-2){
						if(!Game.instance.getCurrentCard().isSniper()){
							end = true;
						}
					}
					else if(board[i,casterTile.y]>=0){
						targets.Add(new TileM(i,casterTile.y));
					}
				}
			}
		}

		return targets;
	}
}

