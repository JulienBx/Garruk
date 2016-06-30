using System;
using UnityEngine;

public class Slider : MonoBehaviour
{
	public Slider ()
	{

	}

	public virtual void activateCollider(bool b){
		gameObject.transform.GetComponent<BoxCollider>().enabled = b;
	}
}


