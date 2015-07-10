using UnityEngine;

public class MyGameSkillListController : MonoBehaviour 
{

	public Sprite[] sprites;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
		gameObject.transform.FindChild("Title").GetComponent<TextMesh>().color=new Color(0f,0f,0f);
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		gameObject.transform.FindChild("Title").GetComponent<TextMesh>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		newMyGameController.instance.filterASkill (gameObject.transform.FindChild("Title").GetComponent<TextMesh>().text);
	}
}

