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
		references.Add(new string[]{"\n\nLe département de formation cristalienne vous offre 5 unités pour démarrer votre entrainement!",""}); //11
		references.Add(new string[]{"Votre apprentissage est maintenant terminé, il est temps de découvrir le mode conquête!",""}); //12

		cardTypesDescription=new List<string[]>();
		cardTypesDescription.Add(new string[]{"\n\nLes MEDIC ont développé différentes drogues à base de Cristal. Ces drogues peuvent aider leurs alliés en les soignant, en les rendant plus résistants ou plus forts. L'utilisation fréquente de ces substances a permis au medic de développer leur résistance mais leur faible expérience du combat les rend assez inoffensifs au corps à corps.",""}); //0
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Assassin",""}); //1
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Prédateur",""}); //2
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Trooper",""}); //3
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Mentaliste",""}); //4
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Robot",""}); //5
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Cristoid",""}); //6
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Mystique",""}); //7
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Artisan",""}); //8
		cardTypesDescription.Add(new string[]{"\n\nDescription de la classe Terraformeur",""}); //9


	}

}