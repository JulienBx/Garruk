using System;
using UnityEngine;

public class CancelButtonC : MonoBehaviour
{
	void Awake ()
	{
		this.show(false);
	}

	public void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void show(bool b){
		gameObject.transform.GetComponent<BoxCollider>().enabled = b;
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b;
	}
}


