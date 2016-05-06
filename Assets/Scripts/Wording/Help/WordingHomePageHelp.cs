using UnityEngine;
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
		tutorialContents.Add(new string[]{"Vous avez gagné votre première bataille sur Cristalia! D'autres colons plus forts encore vous attendent... Améliorez votre équipe et disputez d'autres combats!",""}); //0
		tutorialContents.Add(new string[]{"Vous n'avez pas démérité mais ce premier combat est perdu. Recrutez d'autres unités ou entrainez votre équipe pour retenter votre chance !",""}); //1
		tutorialContents.Add(new string[]{"Vous êtes ici sur votre tableau de bord depuis lequel vous pouvez accéder aux combats, aux nouveautés, ou aux dernières offres du centre de recrutement",""}); //2
		tutorialContents.Add(new string[]{"Commencez par engager des unités au centre de recrutement",""}); //3
		tutorialContents.Add(new string[]{"Découvrez le mode conquête pour gagner plus de cristaux",""}); //4
		tutorialContents.Add(new string[]{"Le mode conquête est maintenant disponible à la place des matchs d'entrainement !",""}); //5


		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"La meilleure manière de s'enrichir reste de combattre d'autres colons! Le mode conquête vous permettra de gagner de plus en plus de cristaux au fil de vos victoires",""}); //1
		helpContents.Add(new string[]{"Les combats de Cristalia opposent des équipes de 4 unités. Vous pouvez préparer plusieurs équipes et choisir celle que vous souhaitez utiliser pour combattre. L'ordre des unités dans l'équipe détermine l'ordre dans lequel elles agiront pendant la bataille, ne l'oubliez jamais!",""}); //3
		helpContents.Add(new string[]{"Le centre de recrutement vous proposera ici ses meilleures offres. Chaque semaine de nouvelles offres sont proposées!",""}); //5
		helpContents.Add(new string[]{"L'alliance des colons de Cristalia (la fameuse ACC) propose à tous les nouveaux arrivants un réseau social pour rester informés des nouveautés et communiquer avec les quelques colons amicaux de la planète...",""}); //7
	}
}