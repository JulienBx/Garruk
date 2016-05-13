using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingMyGameHelp
{
	public static IList<string[]> tutorialContents;
	public static IList<string[]> helpContents;

	public static string getTutorialContent(int idReference)
	{
		return tutorialContents[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getHelpContent(int idReference)
	{
		return helpContents[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingMyGameHelp()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Vous pouvez consulter ici vos unités et les organiser en équipes (seule une équipe complète peut combattre sur Cristalia). Commencez par créer une équipe!",""}); //0
		tutorialContents.Add(new string[]{"Maintenant choisissez les membres de votre équipe et déplacez-les vers celle-ci.",""}); //1
		tutorialContents.Add(new string[]{"Bravo, votre équipe est complète. Vous êtes maintenant prêt à affronter les dangers de Cristalia.",""}); //2
		tutorialContents.Add(new string[]{"Pour combattre et gagner de nouveaux cristaux, il vous suffira d'accéder à l'arène",""}); //3
		tutorialContents.Add(new string[]{"Vos connaissances sur Cristalia seront rassemblées dans la Cristalopedia",""}); //4
		tutorialContents.Add(new string[]{"Enfin pour tenter de vendre vos unités ou d'en acquérir de nouvelles, faites un tour aux enchères",""}); //5
		tutorialContents.Add(new string[]{"Il est temps pour moi de vous quitter, mais je ne serai pas loin. Utilisez le bouton d'aide pour me convoquer!",""}); //6

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Organisez vos unités en équipes de 4 pour entrer dans l'arène. L'ordre des unités dans les équipes détermine leur ordre d'action en combat",""}); //1
		helpContents.Add(new string[]{"Toutes vos unités disponibles (sauf celles mises aux enchères) sont accessibles ici. Touchez une unité pour accéder à sa carte détaillée",""}); //3
		helpContents.Add(new string[]{"Les filtres vous permettront de trouver rapidement des unités répondant à vos besoins. Torak, colon de la première vague, disposant d'une armée de plus de 1200 unités, ne peut plus s'en passer!",""}); //5
	}
}