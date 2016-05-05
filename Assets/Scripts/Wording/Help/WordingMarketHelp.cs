using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMarketHelp
{
	public static IList<string[]> helpContents;

	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingMarketHelp()
	{
		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Les colons peuvent acheter des unités aux enchères, moyennant une compensation décidée par le vendeur ! Certains colons s'enrichissent sur Cristalia grâce au commerce d'unités",""}); //1
		helpContents.Add(new string[]{"Ces filtres vous permettront de mettre la main sur l'unité idéale pour vos équipes!",""}); //3
	}
}