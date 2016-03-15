using UnityEngine;
using System.Collections.Generic;
using TMPro ;

public class TimerController : MonoBehaviour
{
	public float globalTime ;
	
	float timer ;
	int timerMinuts ;
	int timerSeconds ;
	
	float timerDisplay;
	float displayTiming = 0.5f;
	
	bool isMyTurn ;
	bool isDisplayed ;
	bool isMine ;
	
	GameObject timerGO;
	
	void Awake(){
		this.timerGO = gameObject;
		this.timerMinuts = Mathf.FloorToInt(globalTime/60);
		this.timerSeconds = Mathf.FloorToInt(globalTime-timerMinuts*60);
		this.displayTime();
		
		this.isMyTurn = false ;
		this.isDisplayed = true ;
		
		this.globalTime = GameView.instance.turnTime;
		this.timer = globalTime;
	}
	
	public void setIsMyTurn(bool b){
		this.isMyTurn = b ;
		this.display(true);
	}
	
	public void setIsMine(bool b){
		this.isMine = b;
	}
	
	public void display(bool b){
		gameObject.GetComponent<MeshRenderer>().enabled = true ;
		this.timerDisplay = 0f;
		this.isDisplayed = !this.isDisplayed;
	}
	
	public void displayTime(){
		this.timerGO.GetComponent<TextMeshPro>().text = "O²:"+Mathf.FloorToInt(100.0f*this.timer/this.globalTime)+"%";
	}
	
	public string getSeconds(){
		string s = "";
		if(this.timerSeconds<10){
			s = "0"+this.timerSeconds;		
		}
		else{
			s = ""+this.timerSeconds;	
		}
		return s;
	}
}

