using System;
using UnityEngine;

public class PopUpValidationC : MonoBehaviour
{
	void Awake()
	{
		this.show(false);
	}

	public void show(bool b){
		gameObject.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("Title").GetComponent<MeshRenderer>().enabled = b;
		gameObject.transform.FindChild("Description").GetComponent<MeshRenderer>().enabled = b;
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Button").FindChild("Text").GetComponent<MeshRenderer>().enabled = b;
	}
}