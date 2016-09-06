using UnityEngine;
using System.Collections.Generic;
using TMPro ;

public class TimelineC : MonoBehaviour
{	
	public Sprite[] background;

	void Awake(){
		this.show(false);
	}

	public void show(bool b){
		for(int i = 0 ; i < 8 ; i++){
			gameObject.transform.Find("Unit"+i).GetComponent<SpriteRenderer>().enabled = b;
			gameObject.transform.Find("Unit"+i).FindChild("Character").GetComponent<SpriteRenderer>().enabled = b;
		}
		gameObject.transform.Find("Unit0").FindChild("Background").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.Find("Arrow").GetComponent<SpriteRenderer>().enabled = b;
	}

	public void changeFaces(List<int> idCards){
		if(idCards[0]==-10){
			gameObject.transform.Find("Unit0").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Unit0").FindChild("Character").GetComponent<SpriteRenderer>().enabled = false;
		}
		else{
			gameObject.transform.Find("Unit0").GetComponent<SpriteRenderer>().enabled = true;
			gameObject.transform.Find("Unit0").FindChild("Character").GetComponent<SpriteRenderer>().enabled = false;
		}
		for(int i = 0 ; i < 8 ; i++){
			if(idCards[i]!=-10){
				print(i+" - "+idCards[i]);
				if(idCards[i]>=0){
					if(Game.instance.getCards().getCardC(idCards[i]).getCardM().isMine()){
						gameObject.transform.Find("Unit"+i).GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
						gameObject.transform.Find("Unit"+i).FindChild("Character").transform.localScale = new Vector3(-0.45f, 0.45f,0.45f);
					}
					else{
						gameObject.transform.Find("Unit"+i).GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
						gameObject.transform.Find("Unit"+i).FindChild("Character").transform.localScale = new Vector3(0.45f, 0.45f,0.45f);
					}
					gameObject.transform.Find("Unit"+i).FindChild("Character").GetComponent<SpriteRenderer>().sprite = Game.instance.getCards().getCardC(idCards[i]).getCharacterSprite();
					gameObject.transform.Find("Unit"+i).GetComponent<SpriteRenderer>().enabled = true ;
				}
				else{
					gameObject.transform.Find("Unit"+i).GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
					gameObject.transform.Find("Unit"+i).FindChild("Character").GetComponent<SpriteRenderer>().sprite = this.background[-1*idCards[i]];
					gameObject.transform.Find("Unit"+i).FindChild("Character").transform.localScale = new Vector3(0.45f, 0.45f,0.45f);
					gameObject.transform.Find("Unit"+i).GetComponent<SpriteRenderer>().enabled = false ;

				}
			}
		}
	}
}

