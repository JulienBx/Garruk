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
			case 10:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(3);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 11:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(3);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 12:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(3);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 13:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(3);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 20:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(1);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 21:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(1);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 22:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(1);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 23:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(1);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 30:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(0);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 31:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(0);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 32:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(0);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 33:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(0);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 40:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(4);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 41:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(4);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 42:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(4);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 43:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(4);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 50:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(6);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 51:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(6);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 52:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(6);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 53:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(6);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 60:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(5);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 61:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(5);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 62:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(5);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 63:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(5);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 70:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(7);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 71:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(7);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 72:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(7);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 73:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(7);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 80:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(8);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 81:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(8);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 82:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(8);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 83:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(8);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
			case 90:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(9);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 91:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(9);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 92:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(9);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[0];
				break;
			case 93:
				this.cardType.transform.GetComponent<SpriteRenderer>().sprite=BackOfficeController.instance.returnCardTypePicto(9);
				this.stars[0].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[1].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				this.stars[2].transform.GetComponent<SpriteRenderer>().sprite=this.sprites[1];
				break;
		}
	}

}

