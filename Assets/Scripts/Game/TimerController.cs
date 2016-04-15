using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TimerController : MonoBehaviour
{
	float timerTurn = 0f ; 
	int realTime = 0;
	public float turnTime = 30f;
	public bool isShowing = false ;

	void Awake(){
		this.show(false);
	}

	public void setTime(){
		this.timerTurn = 30f;
		this.realTime = 30 ;
	}

	public void setTime(float f){
		this.timerTurn = f;
		this.realTime = Mathf.CeilToInt(f) ;
	}

	public void addTime(float f){
		this.timerTurn-=f ;

		if(this.timerTurn<=0){
			if(GameView.instance.getCurrentCard().isMine){
				GameView.instance.hideAllTargets();
				GameController.instance.findNextPlayer();
			}
		}
		else{
			int i = Mathf.Max(0,Mathf.CeilToInt(this.timerTurn));
			if(i!=realTime){
				if(i==20 ||i==15 || i==10 || i==5 || i==4 || i==3 || i==2 || i==1){
					GameView.instance.timerFront.GetComponent<TimerFrontController>().setTime(i);
					GameView.instance.timerFront.GetComponent<TimerFrontController>().show(true);
				}

				realTime = i ;
				gameObject.GetComponent<TextMeshPro>().text = ""+realTime ;
				if(realTime <= 5f){
					gameObject.GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
				}
				else if(realTime <= 10f){
					gameObject.GetComponent<TextMeshPro>().color = new Color(243f/255f, 110f/255f, 42f/255f, 1f);
				}
				else if(realTime <= 15f){
					gameObject.GetComponent<TextMeshPro>().color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
				}
				else{
					gameObject.GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
				}
			}
		}
	}

	public void show(bool b){
		gameObject.GetComponent<MeshRenderer>().enabled = b ;
		this.isShowing = b ;
		if(b){
			gameObject.GetComponent<TextMeshPro>().text = ""+realTime ;
			if(realTime <= 5f){
				gameObject.GetComponent<TextMeshPro>().color = new Color(231f/255f, 0f, 66f/255f, 1f);
			}
			else if(realTime <= 10f){
				gameObject.GetComponent<TextMeshPro>().color = new Color(243f/255f, 110f/255f, 42f/255f, 1f);
			}
			else if(realTime <= 15f){
				gameObject.GetComponent<TextMeshPro>().color = new Color(255f/255f, 220f/255f, 20f/255f, 1f);
			}
			else{
				gameObject.GetComponent<TextMeshPro>().color = new Color(71f/255f,150f/255f,189f/255f, 1f);
			}
		}
	}
}

