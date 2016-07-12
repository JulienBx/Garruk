﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class Intelligence
{
	public void placeCards(){
		int strategy = UnityEngine.Random.Range(1,11);
		Tile startingTile = new Tile(-1,-1) ;
		int tempInt ;

		int[,] tilesOccupancy = Game.instance.getBoard().getOpponentStartingTilesOccupancy();

		for(int i = 4 ; i < 8 ; i++){
			tempInt = UnityEngine.Random.Range(1,101);
			if(Game.instance.getCards().getCardC(i).getFaction()==0){
				if(tempInt<=25){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(3,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(2,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==1){
				if(tempInt<=25){
					startingTile = new Tile(0,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(5,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==2){
				if(tempInt<=25){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==3){
				if(tempInt<=25){
					startingTile = new Tile(0,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(4,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(5,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==4){
				if(tempInt<=25){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(3,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(5,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==5){
				if(tempInt<=25){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==6){
				if(tempInt<=25){
					startingTile = new Tile(2,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(3,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(3,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(2,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==7){
				if(tempInt<=25){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(0,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(5,1);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,1);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==8){
				if(tempInt<=25){
					startingTile = new Tile(0,0);
				}
				else if(tempInt<=50){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=75){
					startingTile = new Tile(4,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(5,0);
				}
			}
			else if(Game.instance.getCards().getCardC(i).getFaction()==9){
				if(tempInt<=25){
					startingTile = new Tile(1,1);
				}
				else if(tempInt<=50){
					startingTile = new Tile(4,1);
				}
				else if(tempInt<=75){
					startingTile = new Tile(1,0);
				}
				else if(tempInt<=100){
					startingTile = new Tile(4,0);
				}
			}

			while(tilesOccupancy[startingTile.x, startingTile.y]!=0){
				startingTile = new Tile(UnityEngine.Random.Range(0,6), UnityEngine.Random.Range(0,2));
			}

			tilesOccupancy[startingTile.x, startingTile.y]=1;
			Game.instance.moveOn(startingTile.x, startingTile.y+6, i);
		}
	}
}
