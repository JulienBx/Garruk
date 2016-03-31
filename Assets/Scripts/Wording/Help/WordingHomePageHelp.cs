﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHomePageHelp
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
	static WordingHomePageHelp()
	{
		tutorialContents=new List<string[]>();
		tutorialContents.Add(new string[]{"Vous avez gagné votre première bataille sur Cristalia! Mais d'autres colons plus forts encore vous attendent... Essayez le mode conquête!",""}); //0
		tutorialContents.Add(new string[]{"Vos unités n'ont pas démérité mais ont perdu ce premier combat. Peut-être faudra-t-il entrainer vos unités ou en recruter d'autres !",""}); //1
		tutorialContents.Add(new string[]{"Vous êtes ici sur votre tableau de bord où vous trouverez toutes les infos nécessaires pour préparer votre prochain combat",""}); //2
		tutorialContents.Add(new string[]{"Nous reviendrons plus tard, allons acheter de nouvelles cartes",""}); //3
		tutorialContents.Add(new string[]{"Il est temps de découvrir le mode conquête",""}); //4
		tutorialContents.Add(new string[]{"Vous pouvez y accéder à partir de la page d'accueil en lieu et place des matchs d'entrainement",""}); //5


		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"La meilleure manière de s'enrichir reste de combattre d'autres colons! Le mode conquête vous permettra de gagner de plus en plus de cistal au fil de vos victoires",""}); //1
		helpContents.Add(new string[]{"Les combats de Cristalia opposent des équipes de 4 unités. Vous pouvez préparer plusieurs équipes et choisir celle que vous souhaitez pour combattre. L'ordre des unités dans l'équipe détermine l'ordre dans lequel elles agiront pendant la bataille, ne l'oubliez jamais!",""}); //3
		helpContents.Add(new string[]{"Le centre de recrutement vous proposera ici ses meilleures offres. Chaque semaine de nouvelles offres sont proposées!",""}); //5
		helpContents.Add(new string[]{"L'alliance des colons de Cristalia (la fameuse ACC) propose à tous les nouveaux arrivants un terminal social pour rester informés des nouveautés et communiquer avec les quelques colons amicaux de la planète...",""}); //7
	}
}