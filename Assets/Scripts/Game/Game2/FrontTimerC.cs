using System;
using UnityEngine;

public class FrontTimerC : MonoBehaviour
{
	void Awake()
	{
		
	}

	public virtual void show(bool b){
		gameObject.transform.GetComponent<MeshRenderer>().enabled = b;
	}
}


