﻿using UnityEngine;
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
		helpContents.Add(new string[]{"Les colons peuvent acquérir des unités appartenant à leurs confrères, moyennant une compensation décidée par le vendeur! Vendez vos boulets en essayant de trouver un prix suffisamment attractif, et veillez les bonnes affaires!",""}); //1
		helpContents.Add(new string[]{"Le marché est immense, mais ces filtres vous permettront de mettre la main sur l'unité idéale pour vos équipes!",""}); //3
	}
}