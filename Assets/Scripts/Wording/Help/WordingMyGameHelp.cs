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
		tutorialContents.Add(new string[]{"Bienvenue sur l'écran de gestion de vos équipes, commençons par créer une équipe et donnons lui un nom",""}); //0
		tutorialContents.Add(new string[]{"Vous allez déplacer 4 de vos unités dans l'équipe",""}); //1
		tutorialContents.Add(new string[]{"Votre équipe est complète il est bientot temps pour moi de vous quitter",""}); //2
		tutorialContents.Add(new string[]{"Lorsque vous serez prêt, cliquez sur combattre pour affronter de nouveaux adversaire",""}); //3
		tutorialContents.Add(new string[]{"Si vous souhaitez améliorer vos connaissances tactiques allez visiter la cristalopédia",""}); //4
		tutorialContents.Add(new string[]{"Pour faire du commerce avec vos carte allez visiter le marché",""}); //5
		tutorialContents.Add(new string[]{"Enfin, si vous souhaitez me convoquer à nouveau cliquez sur le bouton aide que vous trouverez partout",""}); //6

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Organisez vos unités en équipes de 4. N'oubliez jamais que l'ordre des unités dans l'équipe déterminé l'ordre d'action pendant les batailles",""}); //1
		helpContents.Add(new string[]{"Accédez à l'ensemble de vos unités, et n'hésitez pas à cliquer sur une unité pour consulter le détail de ses compétences",""}); //3
		helpContents.Add(new string[]{"Plus vous posséderez d'unités, plus il sera difficile de parcourir votre armée. Les filtres vous permettront de trouver rapidement des unités répondant à des critères spécifiques",""}); //5
	}
}