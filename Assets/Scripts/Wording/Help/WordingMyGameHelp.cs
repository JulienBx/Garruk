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
		tutorialContents.Add(new string[]{"Accédez à vos unités et préparez les combats. Formez votre première équipe de 4 Cristaliens!","Access to your units and prepare for the fights. Create your first 4-Cristalian team!"}); //0
		tutorialContents.Add(new string[]{"Déplacer maintenant vos meilleures unités vers votre équipe","Now move your best units on to the team slots"}); //1
		tutorialContents.Add(new string[]{"Bravo, votre équipe est maintenant complète!","Congratulations, your team is now ready to fight"}); //2
		tutorialContents.Add(new string[]{"L'arène vous permet de disputer des combats et de gagner des cristaux","Arena is the place where you will fight and collect cristal rewards"}); //3
		tutorialContents.Add(new string[]{"Vos connaissances sur Cristalia sont rassemblées dans la Cristalopedia","Cristalopedia gathers all the knowledge you own on Cristalia"}); //4
		tutorialContents.Add(new string[]{"Au marché vous pourrez vendre vos unités à d'autres joueurs ou leur en acheter","Market is the best place to sell your units or to acquire new ones"}); //5
		tutorialContents.Add(new string[]{"Il est temps pour moi de vous quitter, mais je ne serai pas loin. Utilisez le bouton d'aide pour me convoquer!","It is time for me to go welcoming other players. To ask me for help, please click on the help button whenever you need!"}); //6

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Organisez vos unités en équipes de 4 pour entrer dans l'arène. L'ordre des unités dans les équipes détermine leur ordre d'action en combat","You will need 4-units team to enter the arena. Units order in your team determines units the play order during fights"}); //1
		helpContents.Add(new string[]{"Toutes vos unités disponibles (sauf celles en vente sur le marché) sont accessibles ici. Touchez une unité pour voir le détail de ses compétences","Every unit you own is displayed here (except the one you are selling in the market). Touch a unit to access to its details"}); //3
		helpContents.Add(new string[]{"Les filtres vous permettent de trouver facilement les unités adaptées à vos besoins","Filters will help you finding the unit fitting your needs the most"}); //5
	}
}