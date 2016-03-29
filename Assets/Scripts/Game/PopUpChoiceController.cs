using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class PopUpChoiceController : MonoBehaviour
{
	public Sprite[] factions ;

	void Awake(){
		this.show(false);
	}

	public void setTexts(string t, string d){
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text = t;
		gameObject.transform.FindChild("Description").GetComponent<TextMeshPro>().text = d;
	}
	
	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled=b;
		gameObject.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled=b;
		gameObject.transform.FindChild("Description").GetComponent<MeshRenderer>().enabled=b;
		for (int i = 1 ; i < 5 ; i++){
			gameObject.transform.FindChild("Choice"+i).GetComponent<PictoChoiceController>().show(false);
		}
	}

	public void displayAllEnemyTypes(){
		List<int> enemies = GameView.instance.getOpponents();
		List<int> cardtypes = new List<int>();
		int compteur = 1 ;
		int c ;
		for(int i = 0 ; i < enemies.Count ; i++){
			c = GameView.instance.getCard(enemies[i]).CardType.Id; 
			if(!cardtypes.Contains(c)){
				gameObject.transform.FindChild("Choice"+compteur).GetComponent<PictoChoiceController>().setFace(GameView.instance.getFactionSprite(c), c);
				gameObject.transform.FindChild("Choice"+i).GetComponent<PictoChoiceController>().show(true);
				cardtypes.Add(c);
				compteur++;
			}
		}
	}
}


