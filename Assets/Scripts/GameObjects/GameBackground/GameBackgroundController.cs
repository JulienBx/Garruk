using UnityEngine;
using System.Collections;

public class GameBackgroundController : MonoBehaviour {

    private GameObject mountains;
    private GameObject clouds;
    private GameObject light;
    private GameObject plane;

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


    // Use this for initialization
    void Awake () {

    this.mountains=gameObject.transform.FindChild("mountains").gameObject;
    this.clouds=gameObject.transform.FindChild("clouds").gameObject;
    this.light=gameObject.transform.FindChild("light").gameObject;
    this.plane=gameObject.transform.FindChild("plane").gameObject;

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
    
    }

   
    // Update is called once per frame
    void Update () 
    {
//        this.mountainsPosition=this.mountainsPosition+Time.deltaTime*mountainsSpeed;
//        if(this.mountainsPosition>=this.mountainsEndPosition)
//        {
//            this.mountainsPosition=this.mountainsStartPosition;
//        }
        this.cloudsPosition=this.cloudsPosition+Time.deltaTime*cloudsSpeed;
        if(this.cloudsPosition>=this.cloudsEndPosition)
        {
            this.cloudsPosition=this.cloudsStartPosition;
        }
//        Vector3 mountainsPosition = this.mountains.transform.position;
//        mountainsPosition.x=this.mountainsPosition;
//        this.mountains.transform.position=mountainsPosition;

        Vector3 cloudsPosition = this.clouds.transform.position;
        cloudsPosition.x=this.cloudsPosition;
        this.clouds.transform.position=cloudsPosition;

//        if(this.meteorAnimation)
//        {
//            this.meteorColorRatio=this.meteorColorRatio+Time.deltaTime*meteorSpeed;
//            if(this.meteorColorRatio>1f)
//            {
//                this.colorationNumber++;
//                this.meteorColorRatio=0f;
//            }
//            print(this.meteorColorRatio);
//            if(this.colorationNumber%2==0)
//            {
//                this.gameObject.GetComponent<SpriteRenderer>().color=new Color(this.meteorStartColor.r+this.meteorColorRatio*(this.meteorEndColor.r-this.meteorStartColor.r),this.meteorStartColor.g+this.meteorColorRatio*(this.meteorEndColor.g-this.meteorStartColor.g),this.meteorStartColor.b+this.meteorColorRatio*(this.meteorEndColor.b-this.meteorStartColor.b));
//            }
//            else
//            {
//                this.gameObject.GetComponent<SpriteRenderer>().color=new Color(this.meteorEndColor.r-this.meteorColorRatio*(this.meteorEndColor.r-this.meteorStartColor.r),this.meteorEndColor.g-this.meteorColorRatio*(this.meteorEndColor.g-this.meteorStartColor.g),this.meteorEndColor.g-this.meteorColorRatio*(this.meteorEndColor.b-this.meteorStartColor.b));
//            }
//            if(this.colorationNumber>6)
//            {
//                this.gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
//                this.meteorAnimation=false;
//            }
//        }
        if(toMovePlane)
        {
            this.movePlane();
        }
        

    }
    void Start()
    {
        StartCoroutine(launchPlane());
    }
//    public void MeteorsBackground()
//    {
//        this.colorationNumber=0;
//        this.meteorSpeed=4f;
//        this.meteorAnimation=true;
//    }
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
