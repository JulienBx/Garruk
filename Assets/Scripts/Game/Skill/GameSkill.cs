using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public int numberOfExpectedTargets ; 
	public Card card ;
	public Skill skill ;
	
	public virtual void init(Card c, Skill s){
		this.card = c ;
		this.skill = s ;
	}
	
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
	
	public virtual void applyOn(int target, int arg, int argé)
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

	public virtual string isLaunchable()
	{
		return "";
	}
	
	public virtual HaloTarget getTargetPCCText(Card c)
	{
		return null;
	}
	
	public virtual string getTargetText(Card targetCard)
	{
		return null;
	}
	
	public virtual HaloTarget getTargetTileText()
	{
		return null;
	}

	public virtual string getTimelineText(){
		return "" ;
	}
}
