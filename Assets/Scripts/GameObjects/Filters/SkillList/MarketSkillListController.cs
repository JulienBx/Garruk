using UnityEngine;
using TMPro;

public class MarketSkillListController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	
	public void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(0f,0f,0f);
	}
	public void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		NewMarketController.instance.filterASkill (gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().text);
	}
}

