using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingSkillBook
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingSkillBook()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Compétences",""}); //0
		references.Add(new string[]{"Cristalopedia",""}); //1
		references.Add(new string[]{"Progression",""}); //2
		references.Add(new string[]{"Disciplines",""}); //3
		references.Add(new string[]{"Factions",""}); //4
		references.Add(new string[]{"Discipline associé à la compétence.",""}); //5
		references.Add(new string[]{"Faction associé à la compétence.",""}); //6
		references.Add(new string[]{"Pourcentage de réussite lorsqu'une compétence est lancée.",""}); //7
		references.Add(new string[]{"Compétences acquises",""}); //8
		references.Add(new string[]{"Niveau de la collection",""}); //9
		references.Add(new string[]{"Points de collection",""}); //10
		references.Add(new string[]{"Classement de la collection",""}); //11
		references.Add(new string[]{"Cristalopédia présente l'ensemble des compétences du jeu. Chaque compétence est caractérisée par un certain nombre d'informations",""}); //12
		references.Add(new string[]{"Ces indicateurs mesurent votre progression dans l'acquisition des compétences. Essayez de toute les collectionner !",""}); //13
		references.Add(new string[]{"Non acquise",""}); //14
		references.Add(new string[]{"Acquise (niveau ",""}); //15
		references.Add(new string[]{")",""}); //16
	}
}