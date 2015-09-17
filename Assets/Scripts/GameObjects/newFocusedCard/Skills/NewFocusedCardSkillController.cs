using UnityEngine;
using TMPro;

public class NewFocusedCardSkillController : MonoBehaviour 
{

	public Skill s;
	
	public virtual void show()
	{
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = gameObject.transform.parent.GetComponent<NewFocusedCardController>().getSkillSprite(this.s.Level - 1);
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.s.Name;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().text = this.s.Power.ToString();
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.s.Description;
	}
	public virtual void setSkill(Skill s)
	{
		this.s = s;
		this.show ();
	}
	public virtual void highlightSkill(bool value)
	{
		if(value)
		{
			this.gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(1f, 0f, 0f);
			this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().color=new Color(1f, 0f, 0f);
			this.gameObject.transform.FindChild("Description").GetComponent<TextMeshPro>().color=new Color(1f, 0f, 0f);
		}
		else
		{
			this.gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f);
			this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().color=new Color(1f, 1f, 1f);
			this.gameObject.transform.FindChild("Description").GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f);
		}
	}
}

