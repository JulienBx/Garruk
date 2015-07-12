using UnityEngine;
using TMPro;

public class NewCardSkillController : MonoBehaviour 
{
	
	private Skill s;
	public Color[] colors;
	
	private void show()
	{
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.s.Name;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().text = this.s.Power.ToString();
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().color = this.colors [this.s.Level - 1];
	}
	public void setSkill(Skill s)
	{
		this.s = s;
		this.show ();
	}
	public void changeLayer(int layerVariation)
	{
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;
	}
}

