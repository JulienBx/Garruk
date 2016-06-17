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
		tutorialContents.Add(new string[]{"Bienvenue au centre de recrutement! Commençons par engager 5 unités cristaliennes","Welcome to the store! Let's start by hiring 5 Cristalian units"}); //0
		tutorialContents.Add(new string[]{"Voici vos premières unités. Offertes par le comité d'accueil Cristalien, ces unités sont plus faibles que la moyenne mais devraient vous permetttre de remporter quelques combats","Here are your first units! As they have been offered to you, these units are weaker than the average ones. Use your cristals to buy better units!"}); //1
		tutorialContents.Add(new string[]{"Examinons de plus près un de vos nouveaux champions","Let's examine one of your new champions"}); //2
		tutorialContents.Add(new string[]{"Le blason en haut à gauche correspond à la faction de la recrue. La faction détermine le type de l'unité, et les compétences qu'elle peut avoir. Vos unités sont toutes de type PREDATEUR","The top-left image is the faction of your unit. Faction determines unit skills and movement range. You have been given PREDATORS only to start with"}); //3
		tutorialContents.Add(new string[]{"Les PREDATEURS sont les premiers colons à avoir été envoyés sur Cristalia. Leur longue exposition au Cristal a renforcé leur corps et a libéré leur agressivité naturelle, les rendant très efficace au corps à corps","PREDATORS have been the first colonizators to be sent on Cristalia. Long exposure to Cristal have thickened their skin and freed their agressivity. They are the best close range fighters on Cristalia"}); //4
		tutorialContents.Add(new string[]{"L'attaque (ATK) et les points de vie (PV) sont les deux stats de chaque unité. Si votre unité attaque, elle inflige un nombre de dégats égal à son ATK sur les PV de l'unité ciblée","Attack (ATK) and Health Points (HP) are the two most important stats of your unit. When your unit attacks, it inflicts basic damages equal to its ATK to the ennemy HP"}); //5
		tutorialContents.Add(new string[]{"Une unité possède de 2 à 4 compétences, La compétence passive (plus sombre) donne des bonus permanents pendant le combat. Vous pourrez déclencher les compétences actives pendant le tour de l'unité","Depending on their experience level, units can use up to 4 skills. Every unit has a unique passive skill (permanent bonus during fights), and 1 to 3 active skills that you can use during fights"}); //6
		tutorialContents.Add(new string[]{"Accédons au détail des compétences en cliquant sur la carte","Let's click on the unit to see its details"}); //7
		tutorialContents.Add(new string[]{"Des informations plus détaillées s'affichent comme le nom de l'unité et son niveau","You can now see your unit name and experience level"}); //8
		tutorialContents.Add(new string[]{"Vous pouvez aussi voir le niveau de puissance de la compétence, et les effets associés","You can also see eack skill power level and effects"}); //9
		tutorialContents.Add(new string[]{"Il est maintenant de former une équipe avec vos unités pour disputer votre premier combat","Let's move on to the last step before fighting : organizing your units into fight teams"}); //10

		helpContents=new List<string[]>();
		helpContents.Add(new string[]{"Le centre de recrutement propose plusieurs offres chaque semaine. Plus vous recruterez d'unités simultanément, plus le prix par unité sera bas","Store displays different offers each week. The more units you buy, the less each unit will cost you!"}); //1
		helpContents.Add(new string[]{"L'argent terrien peut vous permettre d'acheter des cristaux à la boutique","Terran money can be converted to cristals in the shop"}); //3
		helpContents.Add(new string[]{"Les unités acquises au centre de recrutement peuvent ensuite être retrouvées dans 'Mes unités'","You can access to the units you just bought in 'My Units' menu"}); //5
	}
}