using System;
using UnityEngine;

public class FrontTimerC : MonoBehaviour
{
	void Awake()
	{
		this.show(false);
	}

	public virtual void show(bool b){
		gameObject.transform.GetComponent<MeshRenderer>().enabled = b;
	}
}


