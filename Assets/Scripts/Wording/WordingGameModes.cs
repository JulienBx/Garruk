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
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getName(int idGameType)
	{
		return names[idGameType-1][ApplicationModel.player.IdLanguage];
	}
	static WordingGameModes()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Arène","Arena"}); //0
		references.Add(new string[]{"Conquête","Conquest"}); //1
		references.Add(new string[]{"Combattre","Fight"}); //2
		references.Add(new string[]{"Continuer","Load"}); //3
		references.Add(new string[]{"Commencer","Start"}); //4
		references.Add(new string[]{"Vous ne pouvez lancer de match sans avoir au préalable créé un deck","You can not start a fight unless you own a full team"}); //5
		references.Add(new string[]{"Votre adversaire n'est plus disponible","Your opponent is not available anymore"}); //6
		references.Add(new string[]{"En attente d'un colon","Waiting for a colonist"}); //7
		references.Add(new string[]{"Bataille classée","Ranked fight"}); //8
		references.Add(new string[]{"Bataille d'entrainement","Practice fight"}); //9
		references.Add(new string[]{"Entrainement","Training"}); //10
		references.Add(new string[]{"J'y vais","Let's go"}); //11
		references.Add(new string[]{"Votre équipe doit comporter uniquement des unités de la faction ",""}); //12

		names=new List<string[]>();
		names.Add(new string[]{"Alderaan #1","Alderaan #1"}); //0
		names.Add(new string[]{"Kamino #2","Kamino #2"}); //1
		names.Add(new string[]{"Dakara #3","Dakara #3"}); //2
		names.Add(new string[]{"Pacem #4","Pacem #4"}); //3
		names.Add(new string[]{"Ixion #5","Ixion #5"}); //4
		names.Add(new string[]{"Daralis #6","Daralis #6"}); //5
		names.Add(new string[]{"Amitsur #7","Amitsur #7"}); //6
		names.Add(new string[]{"Tatouine #8","Tatouine #8"}); //7
		names.Add(new string[]{"Terminus #9","Terminus #9"}); //8
		names.Add(new string[]{"Trantor #10","Trantor #10"}); //9
	}
}