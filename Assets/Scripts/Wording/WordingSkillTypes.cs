using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSkillTypes
{
	public static IList<string[]> descriptions;
	public static IList<string[]> names;
	public static IList<string[]> letter;
    public static IList<int> idSkillTypes;

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
        idSkillTypes=new List<int>();
        names=new List<string[]>();
        descriptions=new List<string[]>();
        letter=new List<string[]>();

        idSkillTypes.Add(0);
		names.Add(new string[]{"Combat","Close attack"});
        descriptions.Add(new string[]{"Attaque directe sur un personnage adjacent (pas d'attaque en diagonale)","Direct attack on a neighbour (no diagonal attack is allowed)"});
        letter.Add(new string[]{"C","C"});

        idSkillTypes.Add(1);
		names.Add(new string[]{"Tir","Distant attack"});
        descriptions.Add(new string[]{"Attaque à distance","Distant attack"});
        letter.Add(new string[]{"T","D"});

        idSkillTypes.Add(2);
		names.Add(new string[]{"Soutien","Help"});
        descriptions.Add(new string[]{"Donne un bonus à une unité alliée","Gives a bonus to an ally unit"});
        letter.Add(new string[]{"S","H"});

        idSkillTypes.Add(3);
		names.Add(new string[]{"Affaiblissement","Weaken"});
        descriptions.Add(new string[]{"Donne un malus à une unité ennemie","Gives a malus to an ennemy unit"});
        letter.Add(new string[]{"A","W"});

        idSkillTypes.Add(4);
		names.Add(new string[]{"Auto","Auto"});
        descriptions.Add(new string[]{"L'unité se donne un bonus","The unit gives itself a bonus"});
        letter.Add(new string[]{"A","A"});

        idSkillTypes.Add(5);
		names.Add(new string[]{"Piège","Trap"});
        descriptions.Add(new string[]{"Pose un piège sur le champ de bataille","Sets a trap on the battlefield"});
        letter.Add(new string[]{"Pi","T"});

        idSkillTypes.Add(6);
		names.Add(new string[]{"Passive","Passive"});
        descriptions.Add(new string[]{"Les compétences passives se déclenchent automatiquement et ne peuvent êre utilisées par le joueur","Players cannot launch passive skills. They trigger automatically during the fight"});
        letter.Add(new string[]{"Pa","P"});
	}
}