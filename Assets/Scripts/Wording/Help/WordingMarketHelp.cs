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
		helpContents.Add(new string[]{"Les joueurs peuvent acheter des unités aux enchères, ou tenter de vendre leur unité au prix souhaité ! Certains joueurs parviennent à s'enrichir grâce au marché","Players can buy units from other players in the market. Here you can also sell your less interesting units!"}); //1
		helpContents.Add(new string[]{"Ces filtres vous permettront de trouver l'unité idéale pour vos équipes!","Find the best fit for your teams using these filters"}); //3
	}
}