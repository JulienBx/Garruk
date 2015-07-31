using UnityEngine;
using TMPro;

public class BackButtonController : MonoBehaviour 
{

	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		NewStoreController.instance.backToPacks();
		gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
	}
}

