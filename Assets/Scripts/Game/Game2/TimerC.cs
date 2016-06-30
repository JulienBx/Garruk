using System;
using UnityEngine;

public class TimerC : MonoBehaviour
{
	void Awake()
	{
		this.show(false);
	}

	public virtual void show(bool b){
		gameObject.transform.GetComponent<MeshRenderer>().enabled = b;
	}

	public virtual void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}
}


