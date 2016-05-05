using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingHasLeftRooomPopUp
{
    public static IList<string[]> references;

    public static string getReference(int idReference)
    {
        return references[idReference][ApplicationModel.player.IdLanguage];
    }
    static WordingHasLeftRooomPopUp()
    {
        references=new List<string[]>();
        references.Add(new string[]{"Vous avez quitté votre dernier match avant son terme, le combat est donc perdu!",""}); //0
        references.Add(new string[]{"OK!","OK!"}); //2
    }
}