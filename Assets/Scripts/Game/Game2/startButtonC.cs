using System;
using UnityEngine;

public class startButtonC : MonoBehaviour
{
	void Awake ()
	{
		this.show(false);
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("StartButton").GetComponent<MeshRenderer>().enabled = b ;
	}
}


