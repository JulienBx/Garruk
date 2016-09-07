using System;
using UnityEngine;
using TMPro;

public class TimerC : MonoBehaviour
{
	float timer;
	int displayedTimer;
	bool visible ;

	bool visibleFront;
	float timerFront ;
	float frontTime = 0.25f;

	GameObject frontTimer;

	void Awake()
	{
		this.show(false);
		this.visible = false ;
		this.visibleFront = false ;
		this.frontTimer = GameObject.Find("FrontTimer");
	}

	public bool isVisible(){
		return this.visible;
	}

	public bool isVisibleFront(){
		return this.visibleFront;
	}

	public void setTimer(int f){
		this.timer = f ;
		if(this.displayedTimer!=f){
			gameObject.GetComponent<TextMeshPro>().color = new Color(255f/255f, 255f/255f, 255f/255f, 1f);
			this.displayTimer(f);
		}
		this.visible = true ;
		this.show(true);
	}

	public void displayTimer(int i){
		this.displayedTimer = i ;
		gameObject.GetComponent<TextMeshPro>().text = ""+i;
		if(i==10){
			gameObject.GetComponent<TextMeshPro>().color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
		}
		else if(i==3){
			gameObject.GetComponent<TextMeshPro>().color = new Color(243f/255f, 110f/255f, 42f/255f, 1f);
		}
	}

	public void addTime(float f){
		this.timer -= f;
		if(this.displayedTimer!=Mathf.CeilToInt(this.timer)){
			this.displayTimer(Mathf.CeilToInt(this.timer));
			if(this.displayedTimer==10 || this.displayedTimer==3 || this.displayedTimer==2 || this.displayedTimer==1){
				this.displayFrontTimer(this.displayedTimer);
			}
		}
		if(timer<=0){
			this.visible = false;
			Game.instance.timeOver();
			this.show(false);
		}
	}

	public void addFrontTime(float f){
		this.timerFront += f;
		if(this.timerFront<this.frontTime){
			Color c = this.frontTimer.GetComponent<TextMeshPro>().color;

			c = new Color (c.r, c.g, c.b, Mathf.Min(1,(this.timerFront/this.frontTime)));
			Color o = new Color (1, 1, 1, Mathf.Min(1,(this.timerFront/this.frontTime)));

			this.frontTimer.GetComponent<TextMeshPro>().color = c ; 
			this.frontTimer.GetComponent<TextMeshPro>().outlineColor = o ;
		}
		else if(this.timerFront>2*this.frontTime){
			if(this.timerFront<3*this.frontTime){
				Color c = this.frontTimer.GetComponent<TextMeshPro>().color;

				c = new Color (c.r, c.g, c.b, Mathf.Max(0,(1-(this.timerFront-2*this.frontTime)/this.frontTime)));
				Color o = new Color (1f, 1f, 1f, Mathf.Max(0,(1-(this.timerFront-2*this.frontTime)/this.frontTime)));

				this.frontTimer.GetComponent<TextMeshPro>().color = c ; 
				this.frontTimer.GetComponent<TextMeshPro>().outlineColor = o ;
			}
			else{
				this.visibleFront = false;
				this.showFront(false);
			}
		}
	}

	public virtual void show(bool b){
		gameObject.transform.GetComponent<MeshRenderer>().enabled = b;
	}

	public virtual void showFront(bool b){
		this.frontTimer.transform.GetComponent<MeshRenderer>().enabled = b;
	}

	public virtual void size(Vector3 position){
		gameObject.transform.localPosition = position;
	}

	public void displayFrontTimer(int i){
		this.frontTimer.GetComponent<TextMeshPro>().text = ""+i;
		this.timerFront = 0f;
		this.showFront(true);
		this.visibleFront = true ;
	}

	public void stop(){
		this.visible = false;
	}
}


