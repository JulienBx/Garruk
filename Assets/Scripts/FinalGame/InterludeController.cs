using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InterludeController : MonoBehaviour
{
	public Sprite[] bandSprites ;

	float timer ;
	float time = 0.50f;
	bool displaying;

	Vector3 startPosition1, startPosition2, startPosition3;
	Vector3 endPosition1, endPosition2, endPosition3;

	void Awake(){
		this.displaying = false;
		this.show(false);
	}

	public bool isDisplaying(){
		return this.displaying;
	}

	public void resize(float realwidth){
		Vector3 position = gameObject.transform.FindChild("Bar1").localPosition ;
		position.x = -realwidth/2f-8f;
		gameObject.transform.FindChild("Bar1").localPosition = position;
		this.startPosition1 = new Vector3(position.x, position.y, position.z);
		if(realwidth<=6){
			this.endPosition1 = new Vector3(-4.8f, position.y, position.z);
		}
		else{
			this.endPosition1 = new Vector3(-(realwidth-15f)/2f, position.y, position.z);
		}

		position = gameObject.transform.FindChild("Bar2").localPosition ;
		position.x = realwidth/2f+8f;
		gameObject.transform.FindChild("Bar2").localPosition = position;
		this.startPosition2 = new Vector3(position.x, position.y, position.z);
		if(realwidth<=6){
			this.endPosition2 = new Vector3(4.8f, position.y, position.z);
		}
		else{
			this.endPosition2 = new Vector3((realwidth-15f)/2f, position.y, position.z);
		}

		position = gameObject.transform.FindChild("Bar3").localPosition ;
		position.x = -realwidth/2f-8f;
		gameObject.transform.FindChild("Bar3").localPosition = position;
		this.startPosition3 = new Vector3(position.x, position.y, position.z);
		if(realwidth<=6){
			this.endPosition3 = new Vector3(-4.8f, position.y, position.z);
		}
		else{
			this.endPosition3 = new Vector3(-(realwidth-15f)/2f, position.y, position.z);
		}
	}

	public void setBlue(string s){
		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.bandSprites[0];
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.bandSprites[1];
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.bandSprites[2];
		this.setText(s);
	}

	public void setRed(string s){
		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().sprite = this.bandSprites[3];
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().sprite = this.bandSprites[4];
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().sprite = this.bandSprites[5];
		this.setText(s);
	}

	public void launch(){
		this.displaying = true ;
		this.timer = 0f;
		NewGameController.instance.getTilesController().showAllColliders(false);
		this.show(true);
	}

	public void addTime(float f){
		this.timer += f ;
		if(this.timer>4*this.time){
			this.displaying=false;
			NewGameController.instance.handleEndInterlude();
		}
		else if(this.timer>3*this.time){
			float rapport1 = Mathf.Min(1,Mathf.Max(0,this.timer-3f*this.time)/(this.time*0.5f));
			float rapport2 = Mathf.Min(1,Mathf.Max(0,this.timer-3.25f*this.time)/(this.time*0.5f));
			float rapport3 = Mathf.Min(1,Mathf.Max(0,this.timer-3.5f*this.time)/(this.time*0.5f));

			gameObject.transform.FindChild("Bar1").localPosition = new Vector3(this.endPosition1.x+rapport1*(this.startPosition1.x-this.endPosition1.x),this.endPosition1.y+rapport1*(this.startPosition1.y-this.endPosition1.y),this.startPosition3.z+rapport1*(this.startPosition1.z-this.endPosition1.z));
			gameObject.transform.FindChild("Bar2").localPosition = new Vector3(this.endPosition2.x+rapport2*(this.startPosition2.x-this.endPosition2.x),this.endPosition2.y+rapport2*(this.startPosition2.y-this.endPosition2.y),this.startPosition3.z+rapport2*(this.startPosition2.z-this.endPosition2.z));
			gameObject.transform.FindChild("Bar3").localPosition = new Vector3(this.endPosition3.x+rapport3*(this.startPosition3.x-this.endPosition3.x),this.endPosition3.y+rapport3*(this.startPosition3.y-this.endPosition3.y),this.startPosition3.z+rapport3*(this.startPosition3.z-this.endPosition3.z));
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color (255f, 255f, 255f, 1-((this.timer-3*this.time)/this.time));
		}
		else if(this.timer<this.time){
			float rapport1 = Mathf.Min(1,Mathf.Max(0,this.timer)/(this.time*0.5f));
			float rapport2 = Mathf.Min(1,Mathf.Max(0,this.timer-0.25f*this.time)/(this.time*0.5f));
			float rapport3 = Mathf.Min(1,Mathf.Max(0,this.timer-0.5f*this.time)/(this.time*0.5f));

			gameObject.transform.FindChild("Bar1").localPosition = new Vector3(this.startPosition1.x+rapport1*(this.endPosition1.x-this.startPosition1.x),this.startPosition1.y+rapport1*(this.endPosition1.y-this.startPosition1.y),this.startPosition3.z+rapport1*(this.endPosition3.z-this.startPosition3.z));
			gameObject.transform.FindChild("Bar2").localPosition = new Vector3(this.startPosition2.x+rapport2*(this.endPosition2.x-this.startPosition2.x),this.startPosition2.y+rapport2*(this.endPosition2.y-this.startPosition2.y),this.startPosition3.z+rapport2*(this.endPosition3.z-this.startPosition3.z));
			gameObject.transform.FindChild("Bar3").localPosition = new Vector3(this.startPosition3.x+rapport3*(this.endPosition3.x-this.startPosition3.x),this.startPosition3.y+rapport3*(this.endPosition3.y-this.startPosition3.y),this.startPosition3.z+rapport3*(this.endPosition3.z-this.startPosition3.z));
			gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().color = new Color (255f, 255f, 255f, (this.timer/this.time));
		}
	}

	public void show(bool b){
		gameObject.transform.FindChild("Bar1").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Bar2").GetComponent<SpriteRenderer>().enabled = b;
		gameObject.transform.FindChild("Bar3").GetComponent<SpriteRenderer>().enabled = b;
		this.showText(b);
		gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().enabled = b;
	}

	public void showText(bool b){
		gameObject.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = b;
	}

	public void setText(string s){
		gameObject.transform.FindChild("Text").GetComponent<TextMeshPro>().text = s ;
	}
}

