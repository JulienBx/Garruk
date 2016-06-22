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
		references.Add(new string[]{"Pas d'équipe complète","No team ready to play"}); //5
		references.Add(new string[]{"Votre équipe active","Your active team"}); //6
		references.Add(new string[]{"Les noms d'équipe ne doivent pas dépasser 12 caractères","Your team name must not exceed 12 characters"}); //7
		references.Add(new string[]{"Les noms d'équipes ne peuvent contenir de caractères spéciaux","You can not use special characters to name your teams"}); //8
		references.Add(new string[]{"Vous ne pouvez pas posséder deux équipes avec le même nom","You can not own two teams with the same name"}); //9
		references.Add(new string[]{"Veuillez choisir un nom pour votre équipe","Please choose a team name"}); //10
		references.Add(new string[]{"Veuillez créer une équipe pour pouvoir y ajouter votre unité","Please create a team first"}); //11
		references.Add(new string[]{"Chaque équipe doit comporter 4 unités différentes (code de guerre Cristalien, article 3)","Teams must be made of 4 different unit types (Cristalian War Code, Article 3)"}); //12
		references.Add(new string[]{"Il vous faut une équipe complète pour pouvoir entrer dans l'arène","You need a full team to enter the arena "}); //13
		references.Add(new string[]{"Renommer","Rename"}); //14
		references.Add(new string[]{"Vous pouvez renommer votre équipe à tout moment, utile quand vous possédez plusieurs équipes.","You can rename your team anytime!"}); //15
		references.Add(new string[]{"Supprimer","Delete"}); //16
		references.Add(new string[]{"Supprimer votre équipe ne supprimera pas les unités appartenant à celle-ci","Deleting your team will not delete the attached units"}); //17
		references.Add(new string[]{"Sélectionner","Select"}); //18
		references.Add(new string[]{"L'équipe sélectionnée sera l'équipe utilisée dans l'arène","The selected team will be the one fighting in the arena"}); //19
		references.Add(new string[]{"Créer","Create"}); //20
		references.Add(new string[]{"Vous pouvez créer autant d'équipes que vous le souhaitez","You can create as many teams as you need"}); //21
		references.Add(new string[]{"Votre équipe","Your team"}); //22
		references.Add(new string[]{"Il s'agit du nom de votre équipe. Ce nom ne sera visible que de vous-même.","This is the name of your team. Your opponent does not see the name of your teams."}); //23
	}
}