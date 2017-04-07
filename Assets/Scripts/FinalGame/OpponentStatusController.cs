using System;
using UnityEngine;
using TMPro;

public class OpponentStatusController : MonoBehaviour
{
	void Awake(){

	}

	public void show(bool b){
		gameObject.transform.GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setText(string s, Color c){
		gameObject.transform.GetComponent<TextMeshPro>().text = s;
		gameObject.transform.GetComponent<TextMeshPro>().color = c;
	}
}