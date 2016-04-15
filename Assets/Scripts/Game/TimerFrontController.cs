using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TimerFrontController : MonoBehaviour
{
	float timerTurn = 0f ; 
	float turnTime = 0.3f;
	public bool isShowing = false ;

	void Awake(){
		this.show(false);
	}

	public void setTime(int i){
		this.timerTurn=0f;
		gameObject.GetComponent<TextMeshPro>().text = ""+i;
		gameObject.GetComponent<TextMeshPro>().outlineColor = new Color(1f,1f,1f, 0f);
		if(i>15){
			gameObject.GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 0f);
		}
		else if(i<=5){
			gameObject.GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 0f);
		}
		else if(i<=10){
			gameObject.GetComponent<TextMeshPro>().color = new Color(243f/255f, 110f/255f, 42f/255f, 0f);
		}
		else if(i<=15){
			gameObject.GetComponent<TextMeshPro>().color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
		}
	}

	public void addTime(float f){
		this.timerTurn+=f ;

		if(this.timerTurn<=this.turnTime){
			Color c = gameObject.GetComponent<TextMeshPro>().color;
			Color o = gameObject.GetComponent<TextMeshPro>().outlineColor;

			c = new Color (c.r, c.g, c.b, (this.timerTurn/this.turnTime));
			o = new Color (o.r, o.g, o.b, (this.timerTurn/this.turnTime));

			gameObject.GetComponent<TextMeshPro>().color = c ; 
			gameObject.GetComponent<TextMeshPro>().outlineColor = o ;
		}
		else if(this.timerTurn>3*this.turnTime){
			this.show(false);
		}
		else if(this.timerTurn>2*this.turnTime){
			Color c = gameObject.GetComponent<TextMeshPro>().color;
			Color o = gameObject.GetComponent<TextMeshPro>().outlineColor;

			c = new Color (c.r, c.g, c.b, 1f-((this.timerTurn-2*this.turnTime)/this.turnTime));
			o = new Color (o.r, o.g, o.b, 1f-((this.timerTurn-2*this.turnTime)/this.turnTime));

			gameObject.GetComponent<TextMeshPro>().color = c ; 
			gameObject.GetComponent<TextMeshPro>().outlineColor = o ;
		}
	}

	public void show(bool b){
		gameObject.GetComponent<MeshRenderer>().enabled = b ;
		this.isShowing = b ;
	}
}

