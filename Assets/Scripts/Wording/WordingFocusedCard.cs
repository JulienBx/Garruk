using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingFocusedCard
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingFocusedCard()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Merci de bien vouloir saisir un nom",""}); //0
		references.Add(new string[]{"Le nom saisi est identique à l'ancien",""}); //1
		references.Add(new string[]{"Le nom doit au moins comporter 4 caractères",""}); //2
		references.Add(new string[]{"Le nom doit faire moins de 12 caractères",""}); //3
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux",""}); //4
		references.Add(new string[]{"Merci de bien vouloir saisir un prix",""}); //5
		references.Add(new string[]{"Probabilité de succès",""}); //6
		references.Add(new string[]{"Cette compétence a un taux de réussite de : ",""}); //7
		references.Add(new string[]{" %.",""}); //8
		references.Add(new string[]{" Victoires \n",""}); //9
		references.Add(new string[]{" Défaites",""}); //10
		references.Add(new string[]{"Niv ",""}); //11
	}
}