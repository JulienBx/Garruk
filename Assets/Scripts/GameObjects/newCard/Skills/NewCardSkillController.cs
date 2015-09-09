using UnityEngine;
using TMPro;

public class NewCardSkillController : NewFocusedCardSkillController 
{
	
	private NewCardSkillRessources ressources;
	
	public override void initialize()
	{
		this.ressources = gameObject.GetComponent<NewCardSkillRessources> ();
	}
	public override void show()
	{
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.s.Name;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().text = this.s.Power.ToString();
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().color = this.ressources.colors [this.s.Level - 1];
	}
	public override void setSkill(Skill s)
	{
		base.setSkill (s);
	}
	public void changeLayer(int layerVariation, int sortingLayerId)
	{
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingLayerID =sortingLayerId;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().sortingLayerID = sortingLayerId;
	}
	public override void highlightSkill(bool value)
	{
		base.highlightSkill(value);
	}
}

