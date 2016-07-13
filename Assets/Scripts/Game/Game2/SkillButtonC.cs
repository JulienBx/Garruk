using System;
using UnityEngine;
using TMPro;

public class SkillButtonC : MonoBehaviour
{
	bool launchable ;

	void Awake()
	{
		this.show(false);
		this.launchable = false ;
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
		this.showDescription(b);
	}

	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void init(CardC c, int i){
		if(i==0){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = WordingSkills.getName(0) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = c.getAttackText() ;
		}
		else{
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = WordingSkills.getName(c.getCardM().getSkill(i).Id) ;
			gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = c.getSkillText(i) ;
		}
	}

	public void OnMouseEnter()
	{
		this.showDescription(true);
	}

	public void OnMouseExit()
	{
		this.showDescription(false);
	}

}

