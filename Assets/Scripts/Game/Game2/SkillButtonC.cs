using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SkillButtonC : MonoBehaviour
{
	int id ;
	bool launchable ;
	CardC card ;
	List<TileM> targets;

	void Awake()
	{
		this.id=-2;
		this.show(false);
		this.showDescription(false);
		this.launchable = false ;
	}

	public void setId(int i){
		this.id = i ;
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider>().enabled = b ;
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void showDescription(bool b){
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void setCard(CardC c){
		this.card = c ;
		if(this.id==0){
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = WordingSkills.getName(0) ;
		}
		else{
			print(this.id);
			print(this.card.getCardM().getSkill(this.id).Id);
			gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().text = WordingSkills.getName(this.card.getCardM().getSkill(this.id).Id) ;
		}
		gameObject.GetComponent<SpriteRenderer>().sprite = Game.instance.getSkillSprite(c.getCardM().getCharacterType());
	}

	public void forbid(){
		this.grey();
		gameObject.transform.FindChild("DescriptionZone").FindChild("DescriptionText").GetComponent<TextMeshPro>().text = WordingGame.getText(71) ;
	}

	public void OnMouseEnter()
	{
		this.showDescription(true);
	}

	public void OnMouseExit()
	{
		this.showDescription(false);
	}

	public void grey(){
		gameObject.GetComponent<SpriteRenderer>().color = new Color(80f/255f, 80f/255f, 80f/255f, 255f/255f) ;
		gameObject.transform.FindChild("DescriptionZone").GetComponent<SpriteRenderer>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
		gameObject.transform.FindChild("DescriptionZone").FindChild("TitleText").GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f) ;
	}

	public void update(){
		int tempInt;
		this.targets = GameSkills.instance.getSkill(this.card.getCardM().getSkill(this.id).Id).getTargetTiles(this.card);
		if(this.targets.Count==0){
			//GameSkills.instance.getSkill(this.card.getCardM().getSkill(this.id)).getCiblageText();
		}
	}
}

