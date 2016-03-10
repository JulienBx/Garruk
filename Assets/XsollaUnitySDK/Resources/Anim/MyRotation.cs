using UnityEngine;
using System.Collections;

public class MyRotation : MonoBehaviour {

	bool isRotating = true;
	private Animator anmator;

	void Awake()
	{
		anmator = GetComponent<Animator> ();
		if(anmator != null)
			SetLoading (isRotating);
	}

	public void SetLoading(bool isLoading){
		gameObject.SetActive(isLoading);
		anmator.SetBool ("IsRotating", isLoading);

	}

	public void StartRotation(){
		gameObject.SetActive(true);
		anmator.SetBool ("IsRotating", true);
	}

	public void StopRotation(){
		gameObject.SetActive(false);
		anmator.SetBool ("IsRotating", false);
	}

}
 