using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SkillTypes{

    public List<SkillType> skillTypes ;

    public SkillTypes()
    {
        this.skillTypes = new List<SkillType>();
    }

    public SkillType getSkillType(int index)
    {
        return this.skillTypes [index];
    }

    public int getCount()
    {
        return this.skillTypes.Count;
    }
    public void add()
    {
        this.skillTypes.Add(new SkillType());
    }
    public void parseSkillTypes(string s)
    {
        string[] array = s.Split (new string[]{"#SKILLTYPE#"},System.StringSplitOptions.None);
        for(int i=0;i<array.Length-1;i++)
        {
            string[] skillTypeInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
            this.skillTypes.Add (new SkillType());
            skillTypes[i].Id = System.Convert.ToInt32(skillTypeInformation[0]);
        }
    }
}