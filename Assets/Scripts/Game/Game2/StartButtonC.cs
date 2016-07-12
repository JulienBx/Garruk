using System;
using UnityEngine;
using TMPro;

public class StartButtonC : MonoBehaviour
{
	void Awake ()
	{
		this.show(false);
		gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().text = WordingGame.getText(67);
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("StartButton").GetComponent<MeshRenderer>().enabled = b ;
	}

	public virtual void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void OnMouseEnter()
	{
		gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseExit()
	{
		gameObject.transform.FindChild("StartButton").GetComponent<TextMeshPro>().color = new Color(1f,1f,1f,1f);
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
	}

	public void OnMouseDown()
	{
		Game.instance.pushStartButton();
	}
}


