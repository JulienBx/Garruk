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
		helpContents.Add(new string[]{"Les meilleurs joueurs combattent sur les satellites de Cristalia. Progressez de satellite en satellite et gagnez les récompenses promises aux vainqueurs","Best fighters gather on Cristalian satellites. Conquest mode will get you an access to these satellites. Try to make your way to the biggest one, where fight rewards are the highest!"}); //1
		helpContents.Add(new string[]{"Accédez ici à un compte-rendu de vos dernières batailles","Here you will find a report on your last fights"}); //3
		helpContents.Add(new string[]{"Sur satellite vous sera proposée une campagne de 6 à 10 combats. A l'issue de la campagne, les meilleurs combattants accèdent au satellite suivant tandis que les moins bons revienennt au précédent. Des primes sont octroyées aux meilleurs","Each satellite offers a 6 to 10 fights campaign. The objective is to determine if you are strong enough to move onto the next satellite, or too weak to stay on your current one. Rewards are given to participants at each campaign ending"}); //5
		helpContents.Add(new string[]{"Ces statistiques vous permettent de consulter l'avancement de votre campagne","Your campaign progression is displayed here. You will also find the list of rewards promised to the best players"}); //7
	}
}