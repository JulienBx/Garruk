using UnityEngine;
using TMPro;

public class NewFocusedCardSkillController : MonoBehaviour 
{
	public Color[] colors;
	public Sprite[] pictos;
	private Skill s;
	
	private void show()
	{
		this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = this.pictos [0];
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.s.Name;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().text = this.s.Power.ToString();
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().color = this.colors [this.s.Level - 1];
		this.gameObject.transform.FindChild ("Description").GetComponent<TextMeshPro> ().text = this.s.Description;
	}
	public void setSkill(Skill s)
	{
		this.s = s;
		this.show ();
	}
}

