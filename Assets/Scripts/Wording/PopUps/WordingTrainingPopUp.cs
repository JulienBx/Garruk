using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingTrainingPopUp
{
	public static IList<string[]> references;
	public static IList<string[]> cardTypesDescription;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	public static string getCardTypeDescription(int idCardType)
	{
		return cardTypesDescription[idCardType][ApplicationModel.player.IdLanguage];
	}
	static WordingTrainingPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"BRAVO !\n\nUne victoire de plus avec la faction ","Congratulations! One more victory with faction "}); //0
		references.Add(new string[]{"\n\nIl vous faut encore ","You need "}); //1
		references.Add(new string[]{" victoire "," more victory"}); //2
		references.Add(new string[]{" victoires "," more victories"}); //3
		references.Add(new string[]{"pour débloquer une nouvelle faction","to unlock a new faction"}); //4
		references.Add(new string[]{"pour achever votre formation et accéder au mode conquête","to achieve training and access to Conquest Mode"}); //5
		references.Add(new string[]{"DOMMAGE !\n\nContinuez à faire progresser vos unités","You lost! Keep on improving your team"}); //6
		references.Add(new string[]{"OK!","OK!"}); //7
		references.Add(new string[]{"BRAVO !\n\nL'entrainement de la faction ","Congratulations. Faction "}); //8
		references.Add(new string[]{" est désormais terminé  "," training has been completed"}); //9
		references.Add(new string[]{"Le département de la formation cristalienne vous autorise dorénavant jouer la faction ","You have unlocked faction"}); //10
		references.Add(new string[]{"\n\nLe département de formation cristalienne vous offre 4 unités pour démarrer votre entrainement!","Cristalian training department gives you 4 units to move on in your training program"}); //11
		references.Add(new string[]{"Votre apprentissage est maintenant terminé, il est temps de découvrir le mode conquête!","Your have achieved your training. Feel free to test the Conquest Mode!"}); //12

		cardTypesDescription=new List<string[]>();
		cardTypesDescription.Add(new string[]{"\n\nLes MEDIC ont développé différentes drogues à base de Cristal. Ces drogues peuvent aider leurs alliés en les soignant, en les rendant plus résistants ou plus forts. L'utilisation fréquente de ces substances a permis aux MEDICS de développer leur résistance mais leur faible expérience du combat les rend assez inoffensifs au corps à corps.","MEDIC have been creating different drugs based on cristal powder. They can heal their allies, or make them stronger by ingecting these drugs with their dart pistols. Frequent use of cristal drugs also strenghtened their bodies and made them strong, but they still do not like close combat. "}); //0
		cardTypesDescription.Add(new string[]{"\n\nLes ASSASSINS sont les unités les plus mobiles sur Cristalia. Utilisés comme messagers pendant la première ère cristalienne, les ASSASSINS ont été les premiers à utiliser le Cristal pour créer des pièges, initialement conçus pour se protéger des bêtes sauvages. Les assassins font de gros dégats mais sont très peu résistants. ","ASSASSINS were employed as messengers at first on Cristalia. Thus they have a great movement range during fights, however they can not wear heavy armors and does not have much strength. ASSASSINS nimbleness make them very dangerous from close range."}); //1
		cardTypesDescription.Add(new string[]{"\n\nLes PREDATEURS sont les premiers colons à avoir été envoyés sur Cristalia. A une époque ou seuls les plus forts pouvaient survivre sur la planète, les PREDATEURS ont développé une force surhumaine et utilisé le Cristal pour protéger leur corps des attaques","PREDATORS have been the first colonizators to ben sent on Cristalia. Long cristal exposure made their bodies thick and freed their aggressivity. They are now considered as the best close range fighters on Cristalia, but only rely on brute force."}); //2
		cardTypesDescription.Add(new string[]{"\n\nL'arsenal d’armes des TROOPERS est sans équivalent. Le Cristal récupéré sur la planète leur a également permis de créer diverses armes et d’en faire la faction Cristalienne la plus puissante à distance. Les TROOPERS sont souvent planqués sur le champ de bataille, protégés par des unités plus courageuses (ou plus stupides).","TROOPERS's set of weapons is their strongest asset during fights. They are now the deadliest long-range fighters on Cristalia. As they used the Cristal on their weapons, TROOPERS kept a weak body comparing to other factions."}); //3
		cardTypesDescription.Add(new string[]{"\n\nLes MENTALISTES sont d'anciens ouvriers travaillant aux mines de cristal. Au bout d'années de travail au contact du minerai, les MENTALISTES ont découvert qu'ils pouvaient lire dans les pensées d'autrui, et communiquer par télépathie. Les mentalistes sont plutôt faibles et peu résistants, mais peuvent détraquer les meilleures stratèges par le pouvoir de leur esprit. ",""}); //4
		cardTypesDescription.Add(new string[]{"\n\nLes ROBOTS ont de tout temps assistés les colons. Employés initialement pour aider à la récolte du Cristal grâce à leur résistance supérieure à celle des humains, les robots ont peu à peu été détournés de leur utilisation initiale pour participer aux combats, et sont depuis la dernière réforme du code de guerre cristalien considérés comme des unités à part entière.",""}); //5
		cardTypesDescription.Add(new string[]{"\n\nLes CRISTOIDES sont des humains ayant muté au contact du Cristal, vivant en symbiose avec celui-ci. De nombreuses compétences spéciales leurs permettent de s'entraider, ou d'utiliser le cristal présent sur le champ de bataille pour se renforcer.","CRISTOIDS has been mutating over the past years and produce Cristal with their bodies. They live in symbiosis with it and developped odd skills. They can use battlefield cristals to strenghten their body or help their fellow Cristoid allies."}); //6
		cardTypesDescription.Add(new string[]{"\n\nLes MYSTIQUES sont arrivés récemment sur Cristalia et considèrent que le Cristal est une manifestation divine. Les étranges pouvoirs qu'il possèdent peuvent affecter les combats de différentes façons, mais Dieu ne répond pas toujours à leurs prières","MYSTICS came recently on Cristalia and believe that Cristal is a manifestation of divinity. Their strange set of skills can affect the battlefield in many ways, but God does not always hear their prayers."}); //7
		cardTypesDescription.Add(new string[]{"\n\nLes ARTISANS savent travailler le Cristal et créer des armures ou des batiments utiles au combat. Habitués à travailler le cristal sans relache, les ARTISANS ont développé une force conséquente et peuvent également participer au combat.",""}); //8
		cardTypesDescription.Add(new string[]{"\n\nLes TERRAFORMEURS sont des humains ayant évolué au contact des Cristoides pendant de nombreuses années. Leur connaissance du terrain cristalien et des propriétés du Cristal leur permet d'exploiter les propriétés du champ de bataille pour piéger leurs ennemis ou les ralentir. Un bon terraformeur peut transformer une équipe moyenne en équipe redoutable.",""}); //9
	}

}