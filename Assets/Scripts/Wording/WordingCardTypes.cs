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
		return names[idCardType][ApplicationModel.player.IdLanguage];
	}
	public static string getDescription(int idCardType)
	{
		return descriptions[idCardType][ApplicationModel.player.IdLanguage];
	}
	static WordingCardTypes()
	{
		names=new List<string[]>();
		names.Add(new string[]{"Medic","Medic"});
		names.Add(new string[]{"Assassin","Assassin"});
		names.Add(new string[]{"Prédateur","Predator"});
		names.Add(new string[]{"Trooper","Trooper"});
		names.Add(new string[]{"Mentaliste","Mentalist"});
		names.Add(new string[]{"Robot","Robot"});
		names.Add(new string[]{"Cristoide","Cristoid"});
		names.Add(new string[]{"Mystique","Zealot"});
		names.Add(new string[]{"Scientifique","Scientist"});
		names.Add(new string[]{"Terraformeur","Terraformer"});

		descriptions=new List<string[]>();
		descriptions.Add(new string[]{"Le medic est une unité de soutien. Ses capacités lui permettent d'aider ou de soigner ses alliés","Medic units can heal or boost their allys with their unique set of skills"});
		descriptions.Add(new string[]{"L'assassin est une unité rapide mais peu résistante, capable d'utiliser diverses attaques","Assassins are fast units but do not have much HP. They can use a wide set of skills to inflict average damages"});
		descriptions.Add(new string[]{"Le prédateur est l'unité la plus offensive de Cristalia, mais ne peut attaquer que de près","Predators are the strongest units, but they are slow and need to come closer to their ennemies to attack"});
		descriptions.Add(new string[]{"Le trooper dispose d'armes pour attaquer à distance. Ses faibles protections le rendent inapte au combat rappproché","The trooper can use his long range weapons to attack ennemies. Troopers are difficult to protect as they do not have much HP"});
		descriptions.Add(new string[]{"Le mentaliste est le maitre de l'illusion et peut dérouter le colon ennemi par ses surprenantes compétences","Mentalist master illusions and can get to their ennemies mind to confuse or use them"});
		descriptions.Add(new string[]{"Les robots sont des unités solides mais très lentes, capables de se spécialiser selon le type de  ","Robots a"});
		descriptions.Add(new string[]{"Description de Cristoide",""});
		descriptions.Add(new string[]{"Description de Mystique",""});
		descriptions.Add(new string[]{"Description de Artisan",""});
		descriptions.Add(new string[]{"Description de Terraformeur",""});
	}
}