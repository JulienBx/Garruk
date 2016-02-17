﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSkillTypes
{
	public static IList<string[]> descriptions;
	public static IList<string[]> names;

	public static string getName(int idSkillType)
	{
		return names[idSkillType][ApplicationModel.player.IdLanguage];
	}
	public static string getDescription(int idSkillType)
	{
		return descriptions[idSkillType][ApplicationModel.player.IdLanguage];
	}
	static WordingSkillTypes()
	{
		names=new List<string[]>();
		names.Add(new string[]{"Combat","X"});
		names.Add(new string[]{"Tir","X"});
		names.Add(new string[]{"Soutien","X"});
		names.Add(new string[]{"Dégénrescence","X"});
		names.Add(new string[]{"Auto","X"});
		names.Add(new string[]{"Piège","X"});

		descriptions=new List<string[]>();
		descriptions.Add(new string[]{"Compétence d'attaque directe (sur un personnage adjacent)",""});
		descriptions.Add(new string[]{"Compétence d'attaque à distance",""});
		descriptions.Add(new string[]{"Compétence permettant d'aider un allié pendant le combat",""});
		descriptions.Add(new string[]{"Compétence permettant d'affaiblir un ennemi",""});
		descriptions.Add(new string[]{"Compétence permettant à une unité de se renforcer elle-même",""});
		descriptions.Add(new string[]{"Compétence permettant de poser un piège sur le champ de bataille",""});
	}
}