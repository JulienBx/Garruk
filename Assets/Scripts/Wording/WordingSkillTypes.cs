using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSkillTypes
{
	public static IList<string[]> descriptions;
	public static IList<string[]> names;
	public static IList<string[]> letter;

	public static string getName(int idSkillType)
	{
		return names[idSkillType][ApplicationModel.player.IdLanguage];
	}
	public static string getDescription(int idSkillType)
	{
		return descriptions[idSkillType][ApplicationModel.player.IdLanguage];
	}
	public static string getLetter(int idSkillType)
	{
		return letter[idSkillType][ApplicationModel.player.IdLanguage];
	}
	static WordingSkillTypes()
	{
		names=new List<string[]>();
		names.Add(new string[]{"Combat","Close attack"});
		names.Add(new string[]{"Tir","Distant attack"});
		names.Add(new string[]{"Soutien","Help"});
		names.Add(new string[]{"Affaiblissement","Weaken"});
		names.Add(new string[]{"Auto","Auto"});
		names.Add(new string[]{"Piège","Trap"});
		names.Add(new string[]{"Passive","Passive"});

		descriptions=new List<string[]>();
		descriptions.Add(new string[]{"Attaque directe sur un personnage adjacent (pas d'attaque en diagonale)","Direct attack on a neighbour (no diagonal attack is allowed)"});
		descriptions.Add(new string[]{"Attaque à distance","Distant attack"});
		descriptions.Add(new string[]{"Donne un bonus à une unité alliée","Gives a bonus to an ally unit"});
		descriptions.Add(new string[]{"Donne un malus à une unité ennemie","Gives a malus to an ennemy unit"});
		descriptions.Add(new string[]{"L'unité se donne un bonus","The unit gives itself a bonus"});
		descriptions.Add(new string[]{"Pose un piège sur le champ de bataille","Sets a trap on the battlefield"});
		descriptions.Add(new string[]{"Les compétences passives se déclenchent automatiquement et ne peuvent êre utilisées par le joueur","Players cannot launch passive skills. They trigger automatically during the fight"});

		letter=new List<string[]>();
		letter.Add(new string[]{"C","C"});
		letter.Add(new string[]{"T","D"});
		letter.Add(new string[]{"S","H"});
		letter.Add(new string[]{"A","W"});
		letter.Add(new string[]{"A","A"});
		letter.Add(new string[]{"Pi","T"});
		letter.Add(new string[]{"Pa","P"});
	}
}