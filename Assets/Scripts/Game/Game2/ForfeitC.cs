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

	public void OnMouseEnter()
	{
		this.blue();
	}

	public void OnMouseExit()
	{
		this.white();
	}

	public void blue(){
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f) ;
	}

	public void white(){
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(255f/255f,255f/255f,255f/255f, 1f) ;
	}

	public void OnMouseDown()
	{
		Game.instance.hitForfeit()	;
	}
}