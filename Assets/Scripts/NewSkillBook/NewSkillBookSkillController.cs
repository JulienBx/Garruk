using UnityEngine;
using TMPro;

public class NewSkillBookSkillController : MonoBehaviour 
{
	public Skill s;
	private int id;

	public void setId(int id)
	{
		this.id = id;
		this.gameObject.transform.FindChild ("buttonBorder").GetComponent<NewSkillBookDisplaySkillCardsController> ().setId (id);
	}
	public void setNbCards(int nbCards)
	{
		bool display;
		if(nbCards==0)
		{
			display=false;
		}
		else
		{
			display=true;
		}
		this.gameObject.transform.FindChild("NbCards").gameObject.SetActive(display);
		this.gameObject.transform.FindChild ("buttonBorder").gameObject.SetActive (display);
		this.gameObject.transform.FindChild ("Jauge").gameObject.SetActive (display);
		this.gameObject.transform.FindChild ("JaugeBackground").gameObject.SetActive (display);
		this.gameObject.transform.FindChild ("JaugeDescription").gameObject.SetActive (display);
		string text = " carte";
		if(nbCards>1)
		{
			text=text+"s";
		}
		this.gameObject.transform.FindChild ("NbCards").GetComponent<TextMeshPro> ().text = nbCards+text;
	}
	public void setPercentage(int percentage)
	{
		this.gameObject.transform.FindChild ("JaugeDescription").GetComponent<TextMeshPro> ().text = "Niveau Max atteint : "+percentage;
		Vector3 tempPosition = this.gameObject.transform.FindChild ("Jauge").transform.localPosition;
		tempPosition.x = -1.84f + percentage * 0.01f * 1.13f;
		this.gameObject.transform.FindChild ("Jauge").transform.localPosition = tempPosition;
		Vector3 tempScale = this.gameObject.transform.FindChild ("Jauge").transform.localScale;
		tempScale.x = percentage * 0.01f * 2.46f;
		this.gameObject.transform.FindChild ("Jauge").transform.localScale = tempScale;

		Color color = new Color ();
		if(percentage==0)
		{
			color=new Color(0f/255f,0f/255f,0f/255f);
		}
		else if(percentage<56)
		{
			color=new Color(170f/255f,124f/255f,80f/255f);
		}
		else if(percentage<76)
		{
			color=new Color(168f/255f,168f/255f,168f/255f);
		}
		else
		{
			color=new Color(255f/255f,232f/255f,99f/255f);
		}
		//this.gameObject.transform.GetComponent<SpriteRenderer> ().color = color;
		this.gameObject.transform.FindChild ("Jauge").GetComponent<SpriteRenderer> ().color = color;
	}
	public void show()
	{
		this.gameObject.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = this.s.Name;
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.s.Description;
	}

}
