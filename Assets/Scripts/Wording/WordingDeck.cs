using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingDeck
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingDeck()
	{
		references=new List<string[]>();
		references.Add(new string[]{"1er à jouer","1st to play"}); //0
		references.Add(new string[]{"2ème à jouer","2nd to play"}); //1
		references.Add(new string[]{"3ème à jouer","3rd to play"}); //2
		references.Add(new string[]{"4ème à jouer","4th to play"}); //3
		references.Add(new string[]{"Créez une équipe!","Create a new team"}); //4
		references.Add(new string[]{"Aucune équipe complète","No full team to play"}); //5
		references.Add(new string[]{"Votre équipe active","Your active team"}); //6
		references.Add(new string[]{"Les noms d'équipe ne doit pas dépasser 12 caractères","Your team name must not exceed 12 characters"}); //7
		references.Add(new string[]{"Les noms d'équipes ne peuvent contenir de caractères spéciaux","You can not use special characters to name your teams"}); //8
		references.Add(new string[]{"Vous ne pouvez pas posséder deux équipes avec le même nom","You can not own two teams with the same name"}); //9
		references.Add(new string[]{"Veuillez choisir un nom pour votre équipe","Please choose a team name"}); //10
		references.Add(new string[]{"Veuillez créer une équipe pour pouvoir y ajouter votre unité","Please create a team first"}); //11
		references.Add(new string[]{"Chaque équipe doit comporter 4 types d'unités différentes (code de guerre Cristalien, article 3)","Teams must be made of 4 different unit types (Cristalian War Code, Article 3)"}); //12
		references.Add(new string[]{"Il vous faut une équipe complète pour pouvoir entrer dans l'arène","You need a full team to enter the arena "}); //13
	}
}