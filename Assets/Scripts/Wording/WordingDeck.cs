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
		references.Add(new string[]{"1er à jouer",""}); //0
		references.Add(new string[]{"2ème à jouer",""}); //1
		references.Add(new string[]{"3ème à jouer",""}); //2
		references.Add(new string[]{"4ème à jouer",""}); //3
		references.Add(new string[]{"Aucune armée formée",""}); //4
		references.Add(new string[]{"Aucune armée",""});//5 <-- libellé nécessairement court
		references.Add(new string[]{"Armée sélectionnée",""}); //6
		references.Add(new string[]{"Le nom ne doit pas dépasser 12 caractères",""}); //7
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux",""}); //8
		references.Add(new string[]{"Nom déjà utilisé",""}); //9
		references.Add(new string[]{"Veuillez saisir un nom",""}); //10
		references.Add(new string[]{"Vous devez créer un deck avant de sélectionner une carte",""}); //11
		references.Add(new string[]{"Vous ne pouvez pas posséder dans votre équipe 2 cartes ayant la même compétence passive",""}); //12
	}
}