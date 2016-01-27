using UnityEngine;
using TMPro;

public class NewCardSkillController : NewFocusedCardSkillController 
{

	public override void show()
	{
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = WordingSkills.getName(this.s.Id);
		//this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().text = this.s.Power.ToString();
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = gameObject.transform.parent.GetComponent<NewCardController>().getSkillSprite(this.s.Level - 1);
	}
	public override void setSkill(Skill s)
	{
		base.setSkill (s);
	}
	public override void setDescription(string d)
	{
		base.setDescription (d);
	}
	public override void showDescription()
	{
	}
	public void changeLayer(int layerVariation, int sortingLayerId)
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().sortingOrder +=layerVariation;
		this.gameObject.transform.GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerId;
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingLayerID =sortingLayerId;
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerId;
	}
	public override void highlightSkill(bool value)
	{
		if(value)
		{
			this.gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(1f, 0f, 0f);
		}
		else
		{
			this.gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f);
		}
	}
}

