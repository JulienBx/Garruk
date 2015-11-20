using UnityEngine;
using System.Collections.Generic;

public class TargetPCCHandler
{
	List<int> targetsPCC ;
	int numberOfExpectedTargets ;
	
	public TargetPCCHandler(int n){
		this.numberOfExpectedTargets = n ;
		this.targetsPCC = new List<int>();
	}
	
	public void addTargetPCC(int targetID)
	{
		this.targetsPCC.Add(targetID);
		if (this.targetsPCC.Count==this.numberOfExpectedTargets){
			GameView.instance.hideTargets();
			GameSkills.instance.getCurrentSkill().resolve(this.targetsPCC);
		}
	}	
}




