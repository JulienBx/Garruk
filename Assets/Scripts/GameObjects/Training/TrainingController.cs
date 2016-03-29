using UnityEngine;
using TMPro;

public class TrainingController : MonoBehaviour
{	

	public Sprite[] sprites;
	private GameObject[] stars;
	private GameObject cardType;

	void Awake()
	{
		this.cardType=this.gameObject.transform.FindChild("CardType").gameObject;
		this.stars=new GameObject[3];
		for(int i=0;i<3;i++)
		{
			this.stars[i]=this.gameObject.transform.FindChild("Star"+i).gameObject;
		}
	}

	public void draw(int trainingStatus)
	{
		switch(trainingStatus)
		{
			case 0:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(2);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 1:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(2);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 2:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(2);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 3:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(2);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
		}
	}

}

