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
		references.Add(new string[]{"BRAVO !\n\nUne victoire de plus avec la faction ",""}); //0
		references.Add(new string[]{"\n\nIl vous faut encore ",""}); //1
		references.Add(new string[]{" victoire ",""}); //2
		references.Add(new string[]{" victoires ",""}); //3
		references.Add(new string[]{"pour débloquer une nouvelle faction",""}); //4
		references.Add(new string[]{"pour achever votre formation et accéder au mode conquête",""}); //5
		references.Add(new string[]{"DOMMAGE !\n\nContinuez à faire progresser vos unités",""}); //6
		references.Add(new string[]{"OK!","OK!"}); //7
		references.Add(new string[]{"BRAVO !\n\nL'entrainement de la faction ",""}); //8
		references.Add(new string[]{" est désormais terminé  ",""}); //9
		references.Add(new string[]{"Le département de la formation cristalienne vous autorise dorénavant jouer la faction ",""}); //10
		references.Add(new string[]{"\n\nLe département de formation cristalienne vous offre 4 unités pour démarrer votre entrainement!",""}); //11
		references.Add(new string[]{"Votre apprentissage est maintenant terminé, il est temps de découvrir le mode conquête!",""}); //12

		cardTypesDescription=new List<string[]>();
		cardTypesDescription.Add(new string[]{"\n\nLes MEDIC ont développé différentes drogues à base de Cristal. Ces drogues peuvent aider leurs alliés en les soignant, en les rendant plus résistants ou plus forts. L'utilisation fréquente de ces substances a permis aux MEDICS de développer leur résistance mais leur faible expérience du combat les rend assez inoffensifs au corps à corps.",""}); //0
		cardTypesDescription.Add(new string[]{"\n\nLes ASSASSINS sont les unités les plus mobiles sur Cristalia. Utilisés comme messagers pendant la première ère cristalienne, les ASSASSINS ont été les premiers à utiliser le Cristal pour créer des pièges, initialement conçus pour se protéger des Cristoides sauvages. Les assassins font de gros dégats mais sont très peu résistants. ",""}); //1
		cardTypesDescription.Add(new string[]{"\n\nLes PREDATEURS sont les premiers colons à avoir été envoyés sur Cristalia. A une époque ou seuls les plus forts pouvaient survivre sur la planète, les PREDATEURS ont développé une force surhumaine et ont utilisé le Cristal pour protéger leur corps des attaques",""}); //2
		cardTypesDescription.Add(new string[]{"\n\nUne fois le Cristal découvert sur la planète, les puissances galactiques environnantes ont commencé à dépêcher des soldats sur la planète pour s'en emparer. Ces soldats, les TROOPERS ont développé un arsenal d'armes variées pour pouvoir attaquer les PREDATEURS à distance. Attention, les TROOPERS sont très faibles au corps à corps!",""}); //3
		cardTypesDescription.Add(new string[]{"\n\nLes MENTALISTES sont d'anciens ouvriers travaillant aux mines de cristal. Au bout d'années de travail au contact du minerai, les MENTALISTES ont découvert qu'ils pouvaient lire dans les pensées d'autrui, et communiquer par télépathie. Les mentalistes sont plutôt faibles et peu résistants, mais peuvent détraquer les meilleures stratèges par le pouvoir de leur esprit. ",""}); //4
		cardTypesDescription.Add(new string[]{"\n\nLes ROBOTS ont de tout temps assistés les colons. Employés initialement pour aider à la récolte du Cristal grâce à leur résistance supérieure à celle des humains, les robots ont peu à peu été détournés de leur utilisation initiale pour participer aux combats, et sont depuis la dernière réforme du code de guerre cristalien considérés comme des unités à part entière.",""}); //5
		cardTypesDescription.Add(new string[]{"\n\nLes CRISTOIDES sont les habitants originels de la planète Cristalia. Défaits lors de la grande guerre des Colonies en l'an 147 après l'arrivée des premiers colons, les CRISTOIDES sont dorénavant intégrés aux colons. Les nombreuses mutations génétiques dues à des millénaires passés sur la planète leur confère des pouvoirs spéciaux, leur permettant de s'aider entre eux.",""}); //6
		cardTypesDescription.Add(new string[]{"\n\nLes MYSTIQUES sont arrivés récemment sur Cristalia. Adorateurs du Cristal qu'ils considèrent d'origine divine, les MYSTIQUES passent leur journée à étudier le cristal dans des temples gardés secrets. Les étranges pouvoirs qu'il possèdent peuvent affecter les combats de bien différentes façons, mais les conséquences de leur prières peuvent parfois être facheuses.",""}); //7
		cardTypesDescription.Add(new string[]{"\n\nLes ARTISANS savent travailler le Cristal et créer des armures ou des batiments utiles au combat. Habitués à travailler le cristal sans relache, les ARTISANS ont développé une force conséquente et peuvent également participer au combat.",""}); //8
		cardTypesDescription.Add(new string[]{"\n\nLes TERRAFORMEURS sont des humains ayant évolué au contact des Cristoides pendant de nombreuses années. Leur connaissance du terrain cristalien et des propriétés du Cristal leur permet d'exploiter les propriétés du champ de bataille pour piéger leurs ennemis ou les ralentir. Un bon terraformeur peut transformer une équipe moyenne en équipe redoutable.",""}); //9
	}

}