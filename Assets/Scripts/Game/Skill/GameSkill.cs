using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public int numberOfExpectedTargets ; 
	
	public virtual void launch()
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void resolve(List<int> targetsPCC)
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void resolve(List<Tile> targetsTile)
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void applyOn()
	{ 
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void applyOn(int target)
	{ 
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void applyOn(int[] targets)
	{ 
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void applyOn(int[] targets, int[] args)
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void applyOn(int target, int arg)
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void activateTrap(int[] targets, int[] args)
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void failedToCastOn(int[] targets, int[] args)
	{
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void failedToCastOn(int targets, int args)
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual bool isLaunchable(Skill s)
	{
		return true;
	}
	
	public virtual HaloTarget getTargetPCCText(Card c)
	{
		return null;
	}
	
	public virtual HaloTarget getTargetTileText()
	{
		return null;
	}
	
	public virtual string getSuccessText(){
		return "" ;
	}
	
	public virtual string getFailureText(){
		return "" ;
	}
	
	public virtual string getTimelineText(){
		return "" ;
	}
}
