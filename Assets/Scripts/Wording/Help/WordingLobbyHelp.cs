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
		helpContents.Add(new string[]{"Les gisements de cristal les plus riches découverts à ce jour se trouvent sur les satellites de Cristalia. Le mode conquête vous permettra d'explorer et de coloniser ces satellites, en récupérant de nombreux cristaux pour améliorer vos unités ou en recruter d'autres",""}); //1
		helpContents.Add(new string[]{"Accédez ici à un compte-rendu de vos dernières batailles",""}); //3
		helpContents.Add(new string[]{"Chaque satellite vous proposera une campagne de 6 à 10 combats. A l'issue de cette campagne, des primes sont octroyées aux meilleurs combattants. Vous pouvez consulter ces primes sur le tableau des primes.",""}); //5
		helpContents.Add(new string[]{"Ces statistiques vous permettent de consulter l'état de la campagne et vos chances de gagner les primes promises aux meilleurs",""}); //7
	}
}