using System;
using UnityEngine;

public class ForfeitC : MonoBehaviour
{
	void Awake()
	{
		this.show(false);
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
	}

	public void size(Vector3 position, Vector3 scale){
		gameObject.transform.localPosition = position;
		gameObject.transform.localScale = scale;
	}
}