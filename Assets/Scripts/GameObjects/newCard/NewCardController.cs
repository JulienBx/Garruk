using UnityEngine;
using TMPro;

public class NewCardController : MonoBehaviour 
{

	public Card c;
	private NewCardRessources ressources;
	private GameObject[] skills;
	private GameObject panelSold;

	void Awake()
	{
		this.skills=new GameObject[0];
		this.ressources = this.gameObject.GetComponent<NewCardRessources> ();
		this.panelSold = this.gameObject.transform.FindChild ("PanelSold").gameObject;
	}
	public void show()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.faces[0];
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.c.Title;
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
			this.skills[i].transform.localPosition=new Vector3(-0.4f,-0.3f-i*0.2f,0);
			this.skills[i].transform.GetComponent<NewCardSkillController>().setSkill(c.Skills[i]);
		}
		if(this.c.IdOWner==-1)
		{
			this.setSoldPanel(true);
		}
		else
		{
			this.setSoldPanel(false);
		}
	}
	public void setSoldPanel(bool value)
	{
		this.panelSold.SetActive (value);
	}
	public void changeLayer(int layerVariation)
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Power").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Move").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Quickness").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i].transform.GetComponent<NewCardSkillController>().changeLayer(layerVariation);
		}
	}
}

