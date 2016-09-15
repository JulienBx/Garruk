using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEndGame
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEndGame()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Retour","Retour"}); //0
		references.Add(new string[]{"Récompenses","Rewards"}); //1
		references.Add(new string[]{"Vous avez perdu le combat sans infliger le moindre dégat\nVous ne gagnez donc ni points d'expérience ni cristaux!","As you have not inflicted any damages to your opponent units, Cristalian authorities decided not to give you any rewards"}); //2
		references.Add(new string[]{"Vous avez gagné!\nVous ne gagnez pas de points d'expérience et de cristaux lors de combats avec vos amis.","You are the strongest !\nYou don't get to earn rewards as you have been fighting a friend"}); //3
		references.Add(new string[]{"Vous avez remporté le combat!\n Vous gagnez ","You have won this fight!\n You earn "}); //4
		references.Add(new string[]{" cristaux ("," cristals ("}); //5
		references.Add(new string[]{" cristaux)"," cristals)"}); //6
		references.Add(new string[]{"\n\nVos unités gagnent ","\n\nYour units gain "}); //7
		references.Add(new string[]{" points d'expérience"," experience points"}); //8
		references.Add(new string[]{"Votre ami a été le plus fort!\nVous ne gagnez pas de points d'expérience et de cristaux lors de batailles avec vos amis","You lost !\nYou don't get to earn rewards as you have been fighting a friend"}); //9
		references.Add(new string[]{"Votre adversaire a remporté le combat!\n Vous gagnez ","Your opponent has won !\n You earn "}); //10
		references.Add(new string[]{"Vous avez quitté votre dernier combat\n Vous ne gagnez donc ni points d'expérience ni cristaux!","You quit your last fight, Cristalian authorities decided not to give you any rewards"}); //11

	}
}