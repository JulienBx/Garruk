using System;
using UnityEngine;
using TMPro;

public class TutorielController : MonoBehaviour
{
	float animationTimeLimit = 0.5f ;
	float scaleBonus = 0.2f ;

	int id ;
	float animationTimer ;
	bool animationUp ;
	bool animated ;
	Vector3 position;

	void Awake ()
	{
		this.show(false);
	}

	public bool isAnimation(){
		return this.animated;
	}

	public void show(bool b){
		gameObject.transform.GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.GetComponent<BoxCollider2D>().enabled = b ;
		gameObject.transform.FindChild("Cross").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("Cross").FindChild("CrossText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b ;
		gameObject.transform.FindChild("TitleText").GetComponent<MeshRenderer>().enabled = b ;
		gameObject.transform.FindChild("DescriptionText").GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setTexts(string s1, string s2, string s3){
		gameObject.transform.FindChild("TitleText").GetComponent<TextMeshPro>().text = s1 ;
		gameObject.transform.FindChild("DescriptionText").GetComponent<TextMeshPro>().text = s2 ;
		gameObject.transform.FindChild("Cross").FindChild("CrossText").GetComponent<TextMeshPro>().text = s3 ;
	}

	public virtual void setPosition(Vector3 position){
		this.position = position;
		gameObject.transform.localPosition = this.position;
		this.setCrossScale(new Vector3(1f,1f,1f));
	}

	public virtual void setTempCrossScale(float f){
		Vector3 tempScale = new Vector3(1+f, 1+f, 1+f);
		gameObject.transform.FindChild("Cross").localScale = tempScale;
	}

	public virtual void setCrossScale(Vector3 scale){
		gameObject.transform.FindChild("Cross").localScale = scale;
	}

	public void OnMouseEnter()
	{
		gameObject.transform.FindChild("Cross").GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
	}

	public void OnMouseDown()
	{
		NewGameController.instance.hitTutorial();
	}

	public void OnMouseExit()
	{
		gameObject.transform.FindChild("Cross").GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
		gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
	}

	public void addTime(float f){
		this.animationTimer = Mathf.Min(this.animationTimer+f,this.animationTimeLimit);
		if(this.animationUp){
			this.setTempCrossScale((this.animationTimer/this.animationTimeLimit)*this.scaleBonus);
		}
		else{
			this.setTempCrossScale(((this.animationTimeLimit-this.animationTimer)/this.animationTimeLimit)*this.scaleBonus);
		}
		if(this.animationTimer==this.animationTimeLimit){
			this.animationUp = !this.animationUp;
			this.animationTimer=0f;
		}
	}

	public void startAnimation(){
		this.animationTimer = 0f;
		this.animated = true;
	}

	public void stopAnimation(){
		this.animated = false;
	}
}