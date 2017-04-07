using System;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
	int realTimer;
	float timer;
	bool counting;

	void Awake ()
	{
		this.realTimer=0;
		this.timer=0f;
	}

	public bool isCounting(){
		return this.counting;
	}

	public void setTimer(int i){
		this.realTimer=i;
		this.timer=i;
	}

	public void startTimer(int i){
		this.counting = true;
		this.setTimer(i);
		this.updateTimerText();
		this.show(true);
	}

	public void updateTimerText(){
		gameObject.transform.GetComponent<TextMeshPro>().text = ""+this.realTimer;
	}

	public void addTime(float f){
		this.timer-=f;
		if(this.timer<=0){
			this.counting=false;
			NewGameController.instance.endTimer();
		}
		else if(Mathf.Floor(this.timer)+1<=this.realTimer){
			this.realTimer=(int)(Mathf.Floor(this.timer));
			this.updateTimerText();
			if(this.realTimer==10){
				NewGameController.instance.displayTimerFront(realTimer, new Color(71f/255f,150f/255f,189f/255f, 0f));
			}
			else if(this.realTimer==5){
				NewGameController.instance.displayTimerFront(realTimer, new Color(71f/255f,150f/255f,189f/255f, 0f));
			}
			else if(this.realTimer==4){
				NewGameController.instance.displayTimerFront(realTimer, new Color(71f/255f,150f/255f,189f/255f, 0f));
			}
			else if(this.realTimer==3){
				NewGameController.instance.displayTimerFront(realTimer, new Color(231f/255f, 0f, 66f/255f, 0f));
			}
			else if(this.realTimer==2){
				NewGameController.instance.displayTimerFront(realTimer, new Color(231f/255f, 0f, 66f/255f, 0f));
			}
			else if(this.realTimer==1){
				NewGameController.instance.displayTimerFront(realTimer, new Color(231f/255f, 0f, 66f/255f, 0f));
			}
		}
	}

	public void stopTimer(){
		this.counting=false;
		this.show(false);
	}

	public void show(bool b){
		gameObject.transform.GetComponent<MeshRenderer>().enabled = b ;
	}

	public void resize(float realWidth){
		float scale = Mathf.Min(1f,realWidth/6.05f);
		gameObject.transform.localPosition = new Vector3(0f, 4f*scale+0.15f, 0f);
	}
}