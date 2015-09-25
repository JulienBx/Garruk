using UnityEngine;
using System.Collections.Generic;

public class GameSkill
{
	public int numberOfExpectedTargets ; 
	public Card card ;
	public Skill skill ;
	public List<int> targets;
	public List<int> results;
	public List<int> values;
	public List<Tile> tileTargets;
	
	public virtual void init(Card c, Skill s){
		this.card = c ;
		this.skill = s ;
		this.targets = new List<int>();
		this.results = new List<int>();
		this.values = new List<int>();
		this.tileTargets = new List<Tile>();
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
	
	public virtual void addTarget(int a, int b)
	{ 
		this.targets.Add(a);
		this.results.Add(b);
	}
	
	public virtual void addTarget(int a, int b, int c)
	{ 
		this.targets.Add(a);
		this.results.Add(b);
		this.values.Add(b);
	}
	
	public virtual void addTarget(Tile t, int b)
	{ 
		this.tileTargets.Add(t);
		this.results.Add(b);
	}
	
	public virtual void applyOn()
	{ 
		Debug.Log("Skill non implémenté");
	}
	
	public virtual void activateTrap(int[] targets, int[] args)
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
	
	public virtual string getTargetText(int id, Card targetCard)
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
