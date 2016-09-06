using System;
using UnityEngine;
using TMPro;

public class PassButtonC : MonoBehaviour
{
	bool launchable;

	void Awake()
	{
		this.show(false);
		this.showDescription(false);
		this.setText(WordingGame.getText(65));
		this.launchable = false;
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setText(string s){
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = s ;
	}

	public void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void init(CardC c){
		if(Game.instance.getDraggingCardID()==-1){
			
		}
	}

	public void OnMouseEnter()
	{
		if(this.launchable){
			this.blue();
		}
		else{
			this.red();
		}
		this.showDescription(true);
	}

	public void OnMouseExit()
	{
		this.showDescription(false);
		this.reinit();
	}

	public void OnMouseDown()
	{
		if(this.launchable && Game.instance.getDraggingSBID()==-1){
			Game.instance.hitPassButton();
		}
	}

	public void grey(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
	}

	public void red(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
	}

	public void blue(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void reinit(){
		if(this.launchable){
			this.white();
		}
		else{
			this.grey();
		}
	}

	public void white(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(255f/255f, 255f/255f, 255f/255f, 255f/255f) ;
	}

	public void setLaunchable(bool b){
		this.launchable = b ;
		if(b){
			this.white();
		}
		else{
			this.grey();
		}
	}
}

