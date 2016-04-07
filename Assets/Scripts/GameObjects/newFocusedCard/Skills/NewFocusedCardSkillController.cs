using UnityEngine;
using TMPro;

public class NewFocusedCardSkillController : SpriteButtonController 
{

	public Skill s;
	public string d;

	public virtual void show()
	{
		this.setInitialState();
		this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(this.s.getPictureId());
		this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(this.s.Level);
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = WordingSkills.getName(this.s.Id);
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().text = WordingFocusedCard.getReference(11)+this.s.Power.ToString();
	}
	public virtual void setSkill(Skill s)
	{
		this.s = s;
		this.show ();
	}
	public virtual void setDescription(string d)
	{
		this.d = d;
		this.showDescription ();
	}
	public virtual void showDescription()
	{
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.d;
	}
	public virtual void highlightSkill(bool value)
	{
		if(value)
		{
			this.gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(218f/255f, 70f/255f, 70f/255f);
			this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().color=new Color(218f/255f, 70f/255f, 70f/255f);
			this.gameObject.transform.FindChild("Description").GetComponent<TextMeshPro>().color=new Color(218f/255f, 70f/255f, 70f/255f);
		}
		else
		{
			this.gameObject.transform.FindChild("Name").GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f);
			this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().color=new Color(1f, 1f, 1f);
			this.gameObject.transform.FindChild("Description").GetComponent<TextMeshPro>().color=new Color(1f, 1f, 1f);
		}
	}
	public override void mainInstruction()
	{
		gameObject.transform.parent.parent.GetComponent<NewFocusedCardController>().displaySkillFocused(base.getId());
	}
	public override void setHoveredState()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		if(base.getId()!=0)
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
		else
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(0f,0f,0f);
		}
	}
	public override void showToolTip ()
	{
		if(base.getId()!=0)
		{
			BackOfficeController.instance.displayToolTip(WordingFocusedCard.getReference(15),WordingFocusedCard.getReference(16));
		}
		else
		{
			BackOfficeController.instance.displayToolTip(WordingFocusedCard.getReference(17),WordingFocusedCard.getReference(18));
		}
	}
}

