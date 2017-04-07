using System;
using UnityEngine;

public class IntelligenceController
{
	bool counting;
	float timer;
	float time;
	int level;

	public IntelligenceController (int i)
	{
		this.level = i;
		this.time = UnityEngine.Random.Range(4-level,10-2*level);
		this.counting = false;
	}

	public void startTimer(){
		this.timer = 0f;
		this.counting = true;
	}

	public void addTime(float f){
		this.timer+=f;
		if(this.timer>=time){
			this.counting=false;
			NewGameController.instance.startGame(false);
		}
	}

	public bool isCounting(){
		return this.counting;
	}
}