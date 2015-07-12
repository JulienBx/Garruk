using UnityEngine;
using TMPro;

public class NewFocusedCardController : MonoBehaviour 
{
	public NewFocusedCardRessources ressources;
	public Card c;
	public GameObject[] skills;

	void Awake()
	{
		this.skills=new GameObject[0];
		this.ressources = this.gameObject.GetComponent<NewFocusedCardRessources> ();
	}
	public void setCard(Card c)
	{
		this.c = c;
		this.show ();
	}
	void show()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.faces[0];
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.c.TitleClass;
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Power.ToString();
		this.gameObject.transform.FindChild ("Power").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.PowerLevel - 1];
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Life.ToString();
		this.gameObject.transform.FindChild ("Life").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.LifeLevel - 1];
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Move.ToString();
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Attack.ToString();
		this.gameObject.transform.FindChild ("Attack").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.AttackLevel - 1];
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Speed.ToString();
		this.gameObject.transform.FindChild ("Quickness").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.SpeedLevel - 1];
		this.gameObject.transform.FindChild ("ExperienceGauge").localPosition = new Vector3 (this.c.PercentageToNextLevel * 1.14f, 0.855f, 0);
		this.gameObject.transform.FindChild ("ExperienceGauge").localScale = new Vector3 (this.c.PercentageToNextLevel * (-0.5f), 1.541f, 0);
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<TextMeshPro>().text = this.c.ExperienceLevel.ToString();
		for(int i=0;i<this.skills.Length;i++)
		{
			Destroy (this.skills[i]);
		}
		this.skills = new GameObject[this.c.Skills.Count];
		for(int i=0;i<c.Skills.Count;i++)
		{
			this.skills[i]= Instantiate(ressources.skillObject) as GameObject;
			this.skills[i].transform.parent=this.gameObject.transform;
			this.skills[i].transform.localPosition=new Vector3(-0.36f,-1.35f-i*0.75f,0);
			this.skills[i].transform.GetComponent<NewFocusedCardSkillController>().setSkill(c.Skills[i]);
		}
	}
}

