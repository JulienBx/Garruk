using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCardTypes
{
	public static IList<string[]> descriptions;
	public static IList<string[]> names;

	public static string getName(int idCardType)
	{
		return names[idCardType][ApplicationModel.idLanguage];
	}
	public static string getDescription(int idCardType)
	{
		return descriptions[idCardType][ApplicationModel.idLanguage];
	}
	static WordingCardTypes()
	{
		names=new List<string[]>();
		names.Add(new string[]{"Medic",""});
		names.Add(new string[]{"Assassin",""});
		names.Add(new string[]{"Prédateur",""});
		names.Add(new string[]{"Soldat",""});
		names.Add(new string[]{"Mentaliste",""});
		names.Add(new string[]{"Robot",""});
		names.Add(new string[]{"Cristoide",""});
		names.Add(new string[]{"Mystique",""});
		names.Add(new string[]{"Artisan",""});
		names.Add(new string[]{"Terraformeur",""});

		descriptions=new List<string[]>();
		descriptions.Add(new string[]{"Le medic est une unité de soutient dont le rôle est d'aider ses coéquipiers ou de les rendre meilleures",""});
		descriptions.Add(new string[]{"Description de Assassin",""});
		descriptions.Add(new string[]{"Description de Prédateur",""});
		descriptions.Add(new string[]{"Description de Soldat",""});
		descriptions.Add(new string[]{"Description de Mentaliste",""});
		descriptions.Add(new string[]{"Description de Robot",""});
		descriptions.Add(new string[]{"Description de Cristoide",""});
		descriptions.Add(new string[]{"Description de Mystique",""});
		descriptions.Add(new string[]{"Description de Artisan",""});
		descriptions.Add(new string[]{"Description de Terraformeur",""});
	}
}