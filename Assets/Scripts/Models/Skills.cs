using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Skills{

    public List<Skill> skills ;

    public Skills()
    {
        this.skills = new List<Skill>();
    }

    public Skill getSkill(int index)
    {
        return this.skills [index];
    }

    public int getCount()
    {
        return this.skills.Count;
    }
    public void add()
    {
        this.skills.Add(new Skill());
    }
    public void parseSkills(string s)
    {
        string[] array = s.Split (new string[]{"#SKILL#"},System.StringSplitOptions.None);
        for(int i=0;i<array.Length-1;i++)
        {
            string[] skillInformation = array[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
            this.skills.Add (new Skill());
            skills[i].Id = System.Convert.ToInt32(skillInformation[0]);
            skills[i].IdCardType = System.Convert.ToInt32(skillInformation[1]);
            skills[i].IdSkillType = System.Convert.ToInt32(skillInformation[2]);
        }
    }
}