using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class CardTypes{

    public List<CardType> cardTypes ;

    public CardTypes()
    {
        this.cardTypes = new List<CardType>();
    }

    public CardType getCardType(int index)
    {
        return this.cardTypes [index];
    }

    public int getCount()
    {
        return this.cardTypes.Count;
    }
    public void add()
    {
        this.cardTypes.Add(new CardType());
    }
    public void parseCardTypes(string s)
    {
        string[] array = s.Split (new string[]{"#CARDTYPE#"},System.StringSplitOptions.None);
        for(int i=0;i<array.Length-1;i++)
        {
            string[] cardTypeInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
            this.cardTypes.Add (new CardType());
            cardTypes[i].Id = System.Convert.ToInt32(cardTypeInformation[0]);
			cardTypes[i].MinLife = System.Convert.ToInt32(cardTypeInformation[1]);
			cardTypes[i].MaxLife = System.Convert.ToInt32(cardTypeInformation[2]);
			cardTypes[i].MinAttack = System.Convert.ToInt32(cardTypeInformation[3]);
			cardTypes[i].MaxAttack = System.Convert.ToInt32(cardTypeInformation[4]);
			cardTypes[i].Rank = System.Convert.ToInt32(cardTypeInformation[5]);
        }
    }
}