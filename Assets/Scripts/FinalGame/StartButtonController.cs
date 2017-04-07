using System;
using UnityEngine;
using TMPro;

public class StartButtonController : MonoBehaviour
{
	public StartButtonController ()
	{
		
	}

	public void show(bool b){
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setTexts(string s1, string s2){
		gameObject.transform.FindChild("TitleText").GetComponent<TextMeshPro>().text = s1 ;
		gameObject.transform.FindChild("DescriptionText").GetComponent<TextMeshPro>().text = s2 ;
	}

	public virtual void setPosition(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void OnMouseEnter()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseDown()
	{
		NewGameController.instance.hitStartButton();
	}

	public void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
	}
}