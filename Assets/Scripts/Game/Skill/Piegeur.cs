using UnityEngine;
using System.Collections.Generic;

public class Piegeur : GameSkill
{
	public Trap getTrap(int level, bool isFirstP){	
		Trap t ;
		bool isVisible = (isFirstP==GameView.instance.getIsFirstPlayer());
		int amount = 0 ;
		int type = 1;
		string title = "Electropiège" ;
		
		if(level==1){
			amount = 8;
		}
		else if(level==2){
			amount = 9;
		}
		else if(level==3){
			amount = 10;
		}
		else if(level==4){
			amount = 10;
		}
		else if(level==5){
			amount = 11;
		}
		else if(level==6){
			amount = 12;
		}
		else if(level==7){
			amount = 12;
		}
		else if(level==8){
			amount = 13;
		}
		else if(level==9){
			amount = 14;
		}
		else if(level==10){
			amount = 14;
		}
		
		string description = "<i>Créé par la compétence piégeur</i>\nInflige "+amount+" dégats à l'unité piégée" ;
		t = new Trap(amount, type, isVisible, title, description);
		
		return t;
	}
	
	public List<Tile> getTiles(int level, int w, int h, int nbFreeRows){
		List<Tile> tiles = new List<Tile>();
		
		int nbTraps = 0 ;
		
		if (level<=3){
			nbTraps = 1;
		}
		else if (level<=6){
			nbTraps = 2;
		}
		else if (level<=9){
			nbTraps = 3;
		}
		else if (level==10){
			nbTraps = 4;
		}
		
		bool isNewTrap ;
		int x = 0, y = 0;
		
		for (int i = 0 ; i < nbTraps ; i++){
			isNewTrap = false ;
			
			while(!isNewTrap){
				isNewTrap = true ;
				x = Random.Range(0,w);
				y = Random.Range(nbFreeRows,h-nbFreeRows);
				for(int j = 0 ; j < i ; j++){
					if(x==tiles[j].x && y==tiles[j].y){
						isNewTrap = false;
					}
					else if(GameView.instance.getTileController(x,y).isRock()){
						isNewTrap = false;
					}
				}
			}
			tiles.Add (new Tile(x,y));
		}
		
		return tiles ;
	}
}
