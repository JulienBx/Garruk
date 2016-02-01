using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingGameModes
{
	public static IList<string[]> references;
	public static IList<string[]> names;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	public static string getName(int idGameType, int id)
	{
		return names[10*idGameType+id][ApplicationModel.idLanguage];
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

		names=new List<string[]>();
		names.Add(new string[]{"Alderaan #1",""}); //0
		names.Add(new string[]{"Kamino #2",""}); //1
		names.Add(new string[]{"Dakara #3",""}); //2
		names.Add(new string[]{"Pacem #4",""}); //3
		names.Add(new string[]{"Ixion #5",""}); //4
		names.Add(new string[]{"Daralis #6",""}); //5
		names.Add(new string[]{"Amitsur #7",""}); //6
		names.Add(new string[]{"Tatouine #8",""}); //7
		names.Add(new string[]{"Terminus #9",""}); //8
		names.Add(new string[]{"Trantor #10",""}); //9
		names.Add(new string[]{"Coupe Bronze",""}); //10
		names.Add(new string[]{"Coupe Argent",""}); //11
	}
}