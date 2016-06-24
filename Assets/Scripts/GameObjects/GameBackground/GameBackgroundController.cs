using UnityEngine;
using System.Collections;

public class GameBackgroundController : MonoBehaviour {

    private GameObject mountains;
    private GameObject clouds;
    private GameObject light;
    private GameObject plane;
    private GameObject[] meteors;

    private float mountainsPosition;
    private float mountainsStartPosition;
    private float mountainsEndPosition;
    private float mountainsSpeed;

    private float cloudsPosition;
    private float cloudsStartPosition;
    private float cloudsEndPosition;
    private float cloudsSpeed;

    private bool meteorAnimation;
    private int colorationNumber;
    private float meteorSpeed;
    private Color meteorStartColor;
    private Color meteorEndColor;
    private float meteorColorRatio;


    private Vector3 planeStartPosition;
    private Vector3 planeEndPosition;
    private float planeRatio;
    private float planeSpeed;
    private bool toMovePlane;

    private int backgroundColorStep;
    private int[] backgroundColorOrder;
    private Color[] backgroundColors;
    private float backgroundColorRatio;
    private float backgroundColorSpeed;


    private bool toMoveMeteors;
    private Vector3[] meteorsStartPosition;
    private Vector3[] meteorsEndPosition;
    private float[] meteorsSpeed;
    private float[] meteorsRatio;





    // Use this for initialization
    void Awake () {

	    this.mountains=gameObject.transform.FindChild("mountains").gameObject;
	    this.clouds=gameObject.transform.FindChild("clouds").gameObject;
	    this.light=gameObject.transform.FindChild("light").gameObject;
	    this.plane=gameObject.transform.FindChild("plane").gameObject;
	    this.meteors=new GameObject[5];
	    for(int i=0;i<this.meteors.Length;i++)
	    {
	    	this.meteors[i]=gameObject.transform.FindChild("meteor"+i).gameObject;
	    	this.meteors[i].SetActive(false);
	    }

	    this.cloudsSpeed=0.12f;
	    this.cloudsStartPosition=-9.64f;
	    this.cloudsEndPosition=9.64f;
	    this.cloudsPosition=this.cloudsStartPosition;
	    this.clouds.transform.position=new Vector3(this.cloudsPosition,0f,0f);

	//    this.mountainsSpeed=0f;
	//    this.mountainsStartPosition=-19.15f;
	//    this.mountainsEndPosition=19.15f;
	//    this.mountainsPosition=this.mountainsStartPosition;
	//    this.mountains.transform.position=new Vector3(this.mountainsPosition,0f,0f);

	    this.meteorStartColor=new Color(255f/255f,184f/255f,184f/255f);
	    this.meteorEndColor=new Color(255f/255f,81f/255f,81f/255f);

	    this.planeSpeed=0.1f;
	    this.planeStartPosition=new Vector3(-11.43f,-1.13f,0f);
	    this.planeEndPosition=new Vector3(10.44f,6.18f,0f);

	    this.backgroundColorStep=0;
	    this.backgroundColors=new Color[6];
	    this.backgroundColorOrder=new int[6];
	    this.backgroundColors[0]=new Color(255f/255f,124f/255f,124f/255f);
		this.backgroundColors[1]=new Color(255f/255f,225f/255f,118f/255f);
		this.backgroundColors[2]=new Color(140f/255f,225f/255f,133f/255f);
		this.backgroundColors[3]=new Color(137f/255f,255f/255f,229f/255f);
		this.backgroundColors[4]=new Color(169f/255f,190f/255f,255f/255f);
		this.backgroundColors[5]=new Color(227f/255f,180f/255f,255f/255f);

		int rnd = Random.Range(0,backgroundColors.Length-1);
		for(int i=0;i<backgroundColors.Length;i++)
		{
			this.backgroundColorOrder[i]=(rnd+i)%backgroundColors.Length;

		}
		this.backgroundColorRatio=0f;
		this.backgroundColorSpeed=0.025f;



		this.meteorsStartPosition=new Vector3[5];
		this.meteorsEndPosition=new Vector3[5];
		this.meteorsSpeed=new float[5];
		this.meteorsRatio=new float[5];

		this.meteorsStartPosition[0]=new Vector3(-0.37f,6.54f,0f);
		this.meteorsEndPosition[0]=new Vector3(-11.46f,-0.39f,0f);
		this.meteorsSpeed[0]=1.5f;

		this.meteorsStartPosition[1]=new Vector3(4.8f,7.11f,0f);
		this.meteorsEndPosition[1]=new Vector3(-11.93f,-3.34f,0f);
		this.meteorsSpeed[1]=1.5f;

		this.meteorsStartPosition[2]=new Vector3(8.11f,6.33f,0f);
		this.meteorsEndPosition[2]=new Vector3(-11.42f,-5.87f,0f);
		this.meteorsSpeed[2]=1f;

		this.meteorsStartPosition[3]=new Vector3(11.67f,6.64f,0f);
		this.meteorsEndPosition[3]=new Vector3(-9.76f,-6.75f,0f);
		this.meteorsSpeed[3]=0.8f;

		this.meteorsStartPosition[4]=new Vector3(11.39f,4.08f,0f);
		this.meteorsEndPosition[4]=new Vector3(-5.86f,-6.7f,0f);
		this.meteorsSpeed[4]=1f;

    }

   
    // Update is called once per frame
    void Update () 
    {
		if(toMovePlane)
        {
            this.movePlane();
        }
        if(toMoveMeteors)
        {
        	this.moveMeteors();
        }
        this.changeColor();
        this.moveClouds();

//        this.mountainsPosition=this.mountainsPosition+Time.deltaTime*mountainsSpeed;
//        if(this.mountainsPosition>=this.mountainsEndPosition)
//        {
//            this.mountainsPosition=this.mountainsStartPosition;
//        }
        
//        Vector3 mountainsPosition = this.mountains.transform.position;
//        mountainsPosition.x=this.mountainsPosition;
//        this.mountains.transform.position=mountainsPosition;

       

        if(this.meteorAnimation)
        {
            this.meteorColorRatio=this.meteorColorRatio+Time.deltaTime*meteorSpeed;
            if(this.meteorColorRatio>1f)
            {
                this.colorationNumber++;
                this.meteorColorRatio=0f;
            }
            if(this.colorationNumber%2==0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color=new Color(this.meteorStartColor.r+this.meteorColorRatio*(this.meteorEndColor.r-this.meteorStartColor.r),this.meteorStartColor.g+this.meteorColorRatio*(this.meteorEndColor.g-this.meteorStartColor.g),this.meteorStartColor.b+this.meteorColorRatio*(this.meteorEndColor.b-this.meteorStartColor.b));
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().color=new Color(this.meteorEndColor.r-this.meteorColorRatio*(this.meteorEndColor.r-this.meteorStartColor.r),this.meteorEndColor.g-this.meteorColorRatio*(this.meteorEndColor.g-this.meteorStartColor.g),this.meteorEndColor.g-this.meteorColorRatio*(this.meteorEndColor.b-this.meteorStartColor.b));
            }
            if(this.colorationNumber>6)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
                this.meteorAnimation=false;
            }
        }
        
    }
    void Start()
    {
    }
    public void MeteorsBackground()
    {
        this.colorationNumber=0;
        this.meteorSpeed=4f;
        this.meteorAnimation=true;
    }

	public void changeColor()
	{
		backgroundColorRatio=backgroundColorRatio+Time.deltaTime*this.backgroundColorSpeed;
		if(backgroundColorRatio>1f)
		{
			backgroundColorRatio=0f;
			if(backgroundColorStep==11)
			{
				backgroundColorStep=0;
			}
			else
			{
				backgroundColorStep++;
			}
		}

		if(backgroundColorStep%2==0)
		{
			this.mountains.GetComponent<SpriteRenderer>().color=new Color(1f+backgroundColorRatio*(backgroundColors[backgroundColorOrder[backgroundColorStep/2]].r-1f),
				1f+backgroundColorRatio*(backgroundColors[backgroundColorOrder[backgroundColorStep/2]].g-1f),
				1f+backgroundColorRatio*(backgroundColors[backgroundColorOrder[backgroundColorStep/2]].b-1f));
		}
		else
		{
			this.mountains.GetComponent<SpriteRenderer>().color=new Color(backgroundColors[backgroundColorOrder[(backgroundColorStep-1)/2]].r+backgroundColorRatio*(1f-backgroundColors[backgroundColorOrder[(backgroundColorStep-1)/2]].r),
				backgroundColors[backgroundColorOrder[(backgroundColorStep-1)/2]].g+backgroundColorRatio*(1f-backgroundColors[backgroundColorOrder[(backgroundColorStep-1)/2]].g),
				backgroundColors[backgroundColorOrder[(backgroundColorStep-1)/2]].b+backgroundColorRatio*(1f-backgroundColors[backgroundColorOrder[(backgroundColorStep-1)/2]].b));
		}
	}
	public void moveClouds()
	{
		this.cloudsPosition=this.cloudsPosition+Time.deltaTime*cloudsSpeed;
        if(this.cloudsPosition>=this.cloudsEndPosition)
        {
            this.cloudsPosition=this.cloudsStartPosition;
        }
		Vector3 cloudsPosition = this.clouds.transform.position;
        cloudsPosition.x=this.cloudsPosition;
        this.clouds.transform.position=cloudsPosition;
	}

	public void launchMeteors()
	{
		for(int i=0;i<this.meteors.Length;i++)
		{
			this.meteors[i].transform.position=this.meteorsStartPosition[i];
			this.meteorsRatio[i]=0f;
			this.meteors[i].SetActive(true);
		}
		this.toMoveMeteors=true;
	}
	public void moveMeteors()
	{
		bool isOver=true;
		for(int i=0;i<this.meteors.Length;i++)
		{
			if(this.meteorsRatio[i]<1f)
			{
				this.meteorsRatio[i]=this.meteorsRatio[i]+Time.deltaTime*meteorsSpeed[i];
				this.meteors[i].transform.position=new Vector3(this.meteorsStartPosition[i].x+this.meteorsRatio[i]*(this.meteorsEndPosition[i].x-this.meteorsStartPosition[i].x),
					this.meteorsStartPosition[i].y+this.meteorsRatio[i]*(this.meteorsEndPosition[i].y-this.meteorsStartPosition[i].y),
					0f);
			}
			if(this.meteorsRatio[i]>=1f)
			{
				this.meteors[i].SetActive(false);
			}
			else
			{
				isOver=false;
			}
		}
		if(isOver)
		{
			this.toMoveMeteors=false;
		}

	}

    public IEnumerator launchPlane()
    {

        yield return new WaitForSeconds(25f);
        this.toMovePlane=true;
        this.plane.transform.position=this.planeStartPosition;
        this.planeRatio=0f;
    }
    public void movePlane()
    {

        planeRatio=planeRatio+Time.deltaTime*planeSpeed;
        this.plane.transform.position=new Vector3(this.planeStartPosition.x+planeRatio*(this.planeEndPosition.x-this.planeStartPosition.x),
            this.planeStartPosition.y+planeRatio*(this.planeEndPosition.y-this.planeStartPosition.y),0f);

        if(planeRatio>1f)
        {
            this.toMovePlane=false;
        }
    }

}
