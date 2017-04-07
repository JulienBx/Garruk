using System;
using TMPro;
using UnityEngine;

public class TimerFrontController : MonoBehaviour
{
	float time = 0.4f;
	float timer;
	bool counting;
	Color color;

	public void show(bool b){
		gameObject.transform.GetComponent<MeshRenderer>().enabled = b ;
	}

	public void setTimer(int i, Color c){
		gameObject.transform.GetComponent<TextMeshPro>().text = ""+i;
		gameObject.transform.GetComponent<TextMeshPro>().color = c;
		this.color = c;
		this.timer=0f;
		this.counting=true;
		this.show(true);
	}

	public void addTime(float f){
		this.timer+=f;
		if(this.timer<=time){
			gameObject.transform.GetComponent<TextMeshPro>().color = new Color(this.color.r, this.color.g, this.color.b, this.timer/this.time); 
			gameObject.transform.GetComponent<TextMeshPro>().outlineColor = new Color(255f, 255f, 255f, this.timer/this.time); 
		}
		else if(this.timer>=2*time){
			this.counting=false;
			this.show(false);
		}
		else{
			gameObject.transform.GetComponent<TextMeshPro>().color = new Color(this.color.r, this.color.g, this.color.b, 1-(this.timer-this.time)/this.time); 
			gameObject.transform.GetComponent<TextMeshPro>().outlineColor = new Color(255f, 255f, 255f, 1-(this.timer-this.time)/this.time); 
		}
	}

	public bool isCounting(){
		return this.counting;
	}
}


