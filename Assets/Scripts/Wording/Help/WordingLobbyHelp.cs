using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLobbyHelp
{
	public static IList<string[]> helpContents;

	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingLobbyHelp()
	{
		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Les gisements de cristal les plus riches découverts à ce jour se trouvent sur les satellites de Cristalia. Votre périple sur Cristalia vous amènera à découvrir et à coloniser de nombreux satellites. Chaque satellite colonisé vous donnera accès à un astre plus important, mais prenez garde à ne pas vous faire éjecter car vous pourriez revenir au satellite précédent ",""}); //1
		helpContents.Add(new string[]{"Accédez ici à un compte-rendu de vos dernières batailles",""}); //3
		helpContents.Add(new string[]{"Chaque satellite dispose de ces propres richesses. Chaque satellite dispose de ses réserves de cristal, à vous de découvrir les satellites les plus intéressants!",""}); //5
		helpContents.Add(new string[]{"Ces statistiques vous permettront d'évaluer l'état de la colinisation du satellite et votre niveau par rapport aux autres colons",""}); //7
	}
}