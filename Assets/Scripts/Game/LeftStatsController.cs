using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LeftStatsController : MonoBehaviour
{
	public GameObject stat;
	public GameObject skill;
	public List<GameObject> buttons;
	public List<Sprite> sprites;

	public LeftStatsController()
	{

	}

	public void setCharacterStat(string title, int move, int life, int attack)
	{
		setCharacterName(title);
		setMove(move.ToString());
		setLife(life.ToString());
		setAttack(attack.ToString());
	}

	public void setSkill(string name, string description, int nbSprite)
	{
		this.skill.transform.Find("skillTitre").GetComponent<Text>().text = name;
		this.skill.transform.Find("skillDescription").GetComponent<Text>().text = description;
		this.skill.transform.Find("skill").GetComponent<Image>().sprite = sprites [nbSprite];
	}

	public void setCharacterName(string name)
	{
		this.stat.transform.Find("characterName").GetComponent<Text>().text = name;
	}

	public void setMove(string name)
	{
		this.stat.transform.Find("move").GetComponent<Text>().text = name;
	}

	public void setLife(string name)
	{
		this.stat.transform.Find("life").GetComponent<Text>().text = name;
	}
	
	public void setAttack(string name)
	{
		this.stat.transform.Find("atk").GetComponent<Text>().text = name;
	}
}

