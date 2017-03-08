using System;
using UnityEngine;
using TMPro;

public class TutorielController : MonoBehaviour
{
	int id ;

	void Awake ()
	{
		this.show(false);
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("Cross").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setText(string s1, string s2){
		gameObject.transform.FindChild("TitleText").GetComponent<TextMeshPro>().text = s1 ;
		gameObject.transform.FindChild("DescriptionText").GetComponent<TextMeshPro>().text = s2 ;
	}

	public virtual void setPosition(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void OnMouseEnter()
	{
		gameObject.transform.FindChild("Cross").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseExit()
	{
		gameObject.transform.FindChild("Cross").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseDown()
	{
		Game.instance.pushStartButton();
	}
}