using System;
using UnityEngine;

public class TileController : MonoBehaviour
{
	public Sprite[] backgroundSprites;
	public Sprite[] foregroundSprites;

	TileModel tile ;

	void Awake ()
	{
		this.show(false);
	}

	public void setTile(TileModel t){
		this.tile = t;
	}

	public void show(bool b){
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Foreground").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b;
	}

	public void setPosition(Vector3 p){
		gameObject.transform.localPosition = p;
	}

	public void setScale(Vector3 s){
		gameObject.transform.localScale = s;
	}

	public void setRock(){
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().sprite = this.backgroundSprites[1];
	}
}