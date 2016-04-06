using UnityEngine;
using TMPro;

public class NewFocusedCardSkillController : MonoBehaviour 
{

	public Skill s;
	public string d;
	public int attributeIndex;
	private bool isHovered;
	
	public virtual void show()
	{
		this.isHovered=false;
		this.setStandardState();
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
	public virtual void OnMouseDown()
	{
		gameObject.transform.parent.parent.GetComponent<NewFocusedCardController>().displaySkillFocused(this.attributeIndex-3);
	}
	public virtual void OnMouseOver()
	{
		if(!this.isHovered && !ApplicationDesignRules.isMobileScreen)
		{
			this.setHoveredState();
			this.isHovered=true;
		}
	}
	public virtual void OnMouseExit()
	{
		if(this.isHovered && !ApplicationDesignRules.isMobileScreen)
		{
			this.setStandardState();
			this.isHovered=false;
		}
	}
	public bool getIsHovered()
	{
		return isHovered;
	}
	public void setIsHovered(bool value)
	{
		this.isHovered=value;
	}
	private void setHoveredState()
	{
		gameObject.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
	}
	private void setStandardState()
	{
		if(this.attributeIndex!=3)
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
		else
		{
			gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(0f,0f,0f);
		}
	}
}

