using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingGameModes
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingGameModes()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Entraînement",""}); //0
		references.Add(new string[]{"Conquête",""}); //1
		references.Add(new string[]{"Jouer",""}); //2
		references.Add(new string[]{"Continuer",""}); //3
		references.Add(new string[]{"Commencer",""}); //4
		references.Add(new string[]{"Vous ne pouvez lancer de match sans avoir au préalable créé un deck",""}); //5
		references.Add(new string[]{"Votre adversaire n'est plus disponible",""}); //6
		references.Add(new string[]{"En attente de joueurs",""}); //7
		references.Add(new string[]{"Match de division",""}); //8
		references.Add(new string[]{"Match amical",""}); //9

	}
}