using System;
using UnityEngine;
using TMPro;

public class InterludeC : MonoBehaviour
{
	public Sprite[] bandSprites ;

	float timer ;
	float time = 1f;
	bool displaying;

	Vector3 startPosition1, startPosition2, startPosition3;
	Vector3 endPosition1, endPosition2, endPosition3;

	void Awake(){
		this.displaying = false;
	}

	public bool isDisplaying(){
		return this.displaying;
	}

	public void size(float realwidth){
		Vector3 position = gameObject.transform.FindChild("Bar1").localPosition ;
		position.x = -realwidth/2f-3f;
		gameObject.transform.FindChild("Bar1").localPosition = position;
		this.startPosition1 = new Vector3(position.x, position.y, position.z);
		this.endPosition1 = new Vector3(0f, position.y, position.z);

		position = gameObject.transform.FindChild("Bar2").localPosition ;
		position.x = realwidth/2f+3f;
		gameObject.transform.FindChild("Bar2").localPosition = position;
		this.startPosition2 = new Vector3(position.x, position.y, position.z);
		this.endPosition2 = new Vector3(0f, position.y, position.z);

		position = gameObject.transform.FindChild("Bar3").localPosition ;
		position.x = -realwidth/2f-3f;
		gameObject.transform.FindChild("Bar3").localPosition = position;
		this.startPosition3 = new Vector3(position.x, position.y, position.z);
		this.endPosition3 = new Vector3(0f, position.y, position.z);

		gameObject.transform.FindChild("Text").GetComponent<TextContainer>().width = realwidth ;
		gameObject.transform.FindChild("Text").GetComponent<TextContainer>().height = 2*(realwidth/20f) ;
	}

	public void launchType(int i){
		if(i==0){
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.bandSprites[0];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.bandSprites[1];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.bandSprites[2];
		}
		else if(i==1){
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.bandSprites[0];
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.bandSprites[1];
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.bandSprites[2];
		}
		this.displaying = true ;
	}

	public void addTime(float f){
		this.timer += f ;
		
		if(this.timer>4*this.time){
			gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = false ;
			gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false ;
			this.displaying = false ;
		}
		/*
		else if(this.time>3*this.time){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time-3f*this.animationTime)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = (realwidth)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = -(realwidth)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = (realwidth)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(1-rapport, 1-rapport, 1-rapport);
		}
		else if(this.time<this.time){
			Vector3 position ;
			float rapport = Mathf.Min(1,(this.time)/this.animationTime);
			position = gameObject.transform.FindChild("Bar1").localPosition ;
			position.x = realwidth-(realwidth)*rapport;
			gameObject.transform.FindChild("Bar1").localPosition = position;
			position = gameObject.transform.FindChild("Bar2").localPosition ;
			position.x = -realwidth+(realwidth)*rapport;
			gameObject.transform.FindChild("Bar2").localPosition = position;
			position = gameObject.transform.FindChild("Bar3").localPosition ;
			position.x = realwidth-(realwidth)*rapport;
			gameObject.transform.FindChild("Bar3").localPosition = position;
			gameObject.transform.FindChild("Text").localScale=new Vector3(rapport, rapport, rapport);
		}
		*/
	}
}

