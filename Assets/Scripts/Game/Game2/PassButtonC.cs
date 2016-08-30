using System;
using UnityEngine;
using TMPro;

public class PassButtonC : MonoBehaviour
{
	void Awake()
	{
		this.show(false);
		this.showDescription(false);
		this.setText(WordingGame.getText(65));
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
}

