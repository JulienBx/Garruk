using UnityEngine;
using System.Collections.Generic;

public class GameSkill : Photon.MonoBehaviour {

	public Skill Skill;
	public int SkillNumber;
	public List<StatModifier> StatModifiers;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public virtual void launch()
	{
		Debug.Log("Skill non implémenté");
	}

	public virtual void Apply(int target)
	{
		Debug.Log("GameSkill");
	}
}
