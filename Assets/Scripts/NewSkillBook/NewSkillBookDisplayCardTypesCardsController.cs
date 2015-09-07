using UnityEngine;
using TMPro;

public class NewSkillBookDisplayCardTypesCardsController : MonoBehaviour 
{

	void OnMouseOver()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		NewSkillBookController.instance.displayCardTypesCardsHandler ();
	}

}

