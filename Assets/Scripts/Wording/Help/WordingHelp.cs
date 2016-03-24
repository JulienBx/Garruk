using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHelp
{
	public static IList<string[]> references;
	public static IList<string[]> helpContents;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingHelp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Quitter le tutoriel",""}); //0
		references.Add(new string[]{"Quitter l'aide",""}); //1
		references.Add(new string[]{"Vous êtes encore en apprentissage !\n Attendez d'avoir disputé votre première bataille \npour pouvoir ensuite réaliser cette action",""}); //2
		references.Add(new string[]{"Continuer",""}); //3

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Description de la faction ...",""}); //0
		helpContents.Add(new string[]{"Description de l'expérience ...",""}); //1
		helpContents.Add(new string[]{"Description des points d'attaque / vie",""}); //2
		helpContents.Add(new string[]{"Description des compétences",""}); //3
		helpContents.Add(new string[]{"Description du skill focused",""}); //4
		helpContents.Add(new string[]{"Description de l'augmentation ...",""}); //5
	}
}