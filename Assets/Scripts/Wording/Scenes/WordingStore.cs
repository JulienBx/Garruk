using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingStore
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingStore()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Acheter",""}); //0
		references.Add(new string[]{"Retour",""}); //1
		references.Add(new string[]{"La boutique",""}); //2
		references.Add(new string[]{"Faites l'acquisition de nouvelles recrues en sélectionnant le pack qui vous intéresse.",""}); //3
		references.Add(new string[]{"Le meilleur moyen d'accéder aux cartes rares",""}); //4
		references.Add(new string[]{"Gagner des crédits",""}); //5
		references.Add(new string[]{"J'y vais",""}); //6
		references.Add(new string[]{"Merci de bien vouloir saisir une valeur",""}); //7
		references.Add(new string[]{"Vendre",""}); //8
	}
}