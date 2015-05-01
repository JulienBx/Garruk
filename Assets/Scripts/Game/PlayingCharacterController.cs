using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCharacterController : MonoBehaviour
{
	private PlayingCharacterView playingCharacterView;
	public GUIStyle[] guiStylesMyCharacter ;
	public GUIStyle[] guiStylesHisCharacter ;

	private float scale ;
	public GameObject tile ;

	public int ID = -1 ;

	public bool isMovable ;

	void Awake()
	{
		this.playingCharacterView = gameObject.transform.parent.gameObject.AddComponent <PlayingCharacterView>();
		this.isMovable = false ;
	}

	public void setTile(GameObject t, bool toRotate, bool isPlayer1)
	{
		this.tile = t ;
		this.playingCharacterView.playingCharacterVM.position = this.tile.GetComponent<TileController>().tileView.tileVM.position;
		if (!toRotate)
		{
			this.playingCharacterView.playingCharacterVM.rotation =  Quaternion.Euler(-90,0,0);
		}
		else
		{
			this.playingCharacterView.playingCharacterVM.rotation =  Quaternion.Euler(90,180,0);

			//this.playingCardView.playingCardVM.ScreenPosition.y = Screen.height-this.playingCardView.playingCardVM.ScreenPosition.y;
		}
		this.playingCharacterView.playingCharacterVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCharacterView.playingCharacterVM.position);
		this.playingCharacterView.playingCharacterVM.scale = new Vector3(this.scale, this.scale, this.scale);
		this.playingCharacterView.replace();
	}

	public void changeTile(GameObject t, bool isEmpty)
	{
		if (isEmpty){
			this.tile.GetComponent<TileController>().characterID = -1 ; 
		}
		this.tile = t ;
		this.tile.GetComponent<TileController>().characterID = this.ID ; 

		this.playingCharacterView.playingCharacterVM.position = this.tile.GetComponent<TileController>().tileView.tileVM.position;
		this.playingCharacterView.playingCharacterVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCharacterView.playingCharacterVM.position);

		this.playingCharacterView.playingCharacterVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCharacterView.playingCharacterVM.position);
		this.playingCharacterView.playingCharacterVM.ScreenPosition.y = Screen.height-this.playingCharacterView.playingCharacterVM.ScreenPosition.y;
		this.playingCharacterView.playingCharacterVM.infoRect = new Rect(this.playingCharacterView.playingCharacterVM.ScreenPosition.x-scale*55f, this.playingCharacterView.playingCharacterVM.ScreenPosition.y+scale*20, scale*110, scale*20);
		this.playingCharacterView.replace();
	}

	public void setStyles(bool isMyCharacter){
		if (isMyCharacter){
			this.playingCharacterView.playingCharacterVM.backgroundStyle = guiStylesMyCharacter[0];
			this.playingCharacterView.playingCharacterVM.nameTextStyle = guiStylesMyCharacter[2];
			this.isMovable = true;
		}
		else{
			this.playingCharacterView.playingCharacterVM.backgroundStyle = guiStylesHisCharacter[0];
			this.playingCharacterView.playingCharacterVM.nameTextStyle = guiStylesHisCharacter[2];
		}
	}

	public void resize(int h){
		this.scale = h*1.1f/1000f ;
		this.playingCharacterView.playingCharacterVM.scale = new Vector3(this.scale, this.scale, this.scale);

		this.playingCharacterView.playingCharacterVM.ScreenPosition = Camera.main.WorldToScreenPoint(this.playingCharacterView.playingCharacterVM.position);
		this.playingCharacterView.playingCharacterVM.ScreenPosition.y = Screen.height-this.playingCharacterView.playingCharacterVM.ScreenPosition.y;
		this.playingCharacterView.playingCharacterVM.infoRect = new Rect(this.playingCharacterView.playingCharacterVM.ScreenPosition.x-scale*55f, this.playingCharacterView.playingCharacterVM.ScreenPosition.y+scale*20, scale*110, scale*20);
		this.playingCharacterView.playingCharacterVM.nameTextStyle.fontSize = h * 15 / 1000 ;
		this.playingCharacterView.replace();
	}

	public void setName(string s){
		this.playingCharacterView.playingCharacterVM.name = s ;
	}

	public void setID(int i){
		this.ID = i ;
	}

	public void getDamage()
	{
		GameController.instance.inflictDamage(ID);
	}

	public void hoverCharacter(){
		GameController.instance.hoverTile(this.tile.GetComponent<TileController>().x, this.tile.GetComponent<TileController>().y, this.ID, this.tile.GetComponent<TileController>().isDestination);
	}

	public void clickCharacter(){
		GameController.instance.clickTile(this.tile.GetComponent<TileController>().x, this.tile.GetComponent<TileController>().y, this.ID);
	}

	public void releaseClickCharacter(){
		GameController.instance.releaseClick();
	}
}


