using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingStoreHelp
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
	static WordingStoreHelp()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Bienvenue au centre de recrutement. Plusieurs offres sont disponibles, commençons par recrutrer 5 cristaliens",""}); //0
		tutorialContents.Add(new string[]{"Le comité d'accueil de Cristalia est fier de vous présenter vos premières unités!",""}); //1
		tutorialContents.Add(new string[]{"Examinons de plus près un de vos nouveaux champions",""}); //2
		tutorialContents.Add(new string[]{"Le blason en haut à gauche correspond à la faction de la recrue. La faction détermine le type de l'unité, et les compétences auxquelles elle aura accès",""}); //3
		tutorialContents.Add(new string[]{"L'attaque et les points de vie de l'unité déterminent sa force et sa résistance",""}); //4
		tutorialContents.Add(new string[]{"Chaque unité dispose de compétences, utiles pour le combat. La compétence passive, propre au nom de la carte, est affichée en bas",""}); //5
		tutorialContents.Add(new string[]{"Accédons au détail des compétences en cliquant sur la carte",""}); //6
		tutorialContents.Add(new string[]{"Des informations plus détaillées s'affichent comme le nom de l'unité et son niveau",""}); //7
		tutorialContents.Add(new string[]{"Les effets des compétences sont également affichés. Les compétences passives se déclenchent automatiquement au fil du combat, les compétences actives peuvent être déclenchées pendant le tour de l'unité",""}); //8
		tutorialContents.Add(new string[]{"Maintenant que vous avez vos premières unités, il est temps de construire une équipe pour entrer dans l'arène et disputer vos premiers combats",""}); //9

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Le centre de recrutement propose plusieurs offres. Plus vous recruterez d'unités simultanément, plus le prix par unité sera bas",""}); //1
		helpContents.Add(new string[]{"L'argent terrien peut vous permettre d'acheter des cristaux à la boutique",""}); //3
		helpContents.Add(new string[]{"Les unités acquises au centre de recrutement peuvent ensuite être retrouvées dans 'Mes unités'",""}); //5
	}
}