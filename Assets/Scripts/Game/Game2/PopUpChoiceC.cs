using System;
using UnityEngine;

public class PopUpChoiceC : MonoBehaviour
{
	void Awake()
	{
		this.show(false);
	}

	public void show(bool b){
		this.showChoice(0,b);
		this.showChoice(1,b);
		this.showChoice(2,b);
		this.showChoice(3,b);
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("Description").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void showChoice(int i, bool b){
		gameObject.transform.FindChild("Choice"+(i+1)).FindChild("Character").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Choice"+(i+1)).GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("Choice"+(i+1)).GetComponent<SpriteRenderer>().enabled = b ;
	}
}


