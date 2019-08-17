using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Bunny : MonoBehaviour {

    public char speedgenex;
    public char speedgeney;
    char childspeedgenex;
    char childspeedgeney;
    public char searchrangegenex;
    public char searchrangegeney;

    public double health;
    public double thirst;
    public double hunger;
    public double speed;
    public float searchRange = 100f;

    public String gender = "";
    public bool matingMode; 

    public GameObject babyBunny;
    public GameObject collidedBunny;
     
    public Vector3 destination;
   
	// Use this for initialization
	void Start () {
		
	}
	
    public void Initialize(char newspeedgenex, char newspeedgeney, double newspeed, String newgender, float newSearch)
    {
        speedgenex = newspeedgenex;
        speedgeney = newspeedgeney;
        gender = newgender;
        speed = newspeed;
        searchRange = newSearch;
        
    }
	// Update is called once per frame
	void Update () {


        Collider[] objectsNearby = Physics.OverlapSphere(gameObject.transform.position, searchRange, 256);
        //Debug.Log(objectsNearby[0].name);
        //Debug.Log(LayerMask.GetMask("Findable Objects"));

        if (objectsNearby.Length != 0)
        {
            for (int i = 0; i < objectsNearby.Length; i++)
            {
                GameObject objectFound = objectsNearby[0].gameObject;
               // Debug.Log(LayerMask.GetMask("Findable Objects"));
              


                if (objectFound.name == "Bunny" && objectFound.GetInstanceID() != gameObject.GetInstanceID())
                {
                    Bunny foundBunny = objectFound.GetComponent<Bunny>();
                    if (matingMode)
                    {
            
                        if (foundBunny.gender != gender)
                        {
                            gameObject.GetComponent<Bunny>().destination = objectFound.transform.position;


                        }

                    }
                }


            }
        }
        
       



	}

    private void OnCollisionEnter(Collision collision)
    {
        GameObject colObject = collision.gameObject;
        if (colObject.name == "Bunny" && colObject.GetComponent<Bunny>().gender != gender && matingMode == true && gender == "Female")
        {
            collidedBunny = colObject;
            Reproduce();
        }
    }

    void Reproduce()
    {
        // double mateSpeed = 0; #getspeedinfofromDAD
        Bunny mate = collidedBunny.GetComponent<Bunny>();

        double childSpeed = DetermineChildSpeed(mate.speed, speed, mate.speedgenex, speedgenex, mate.speedgeney, speedgeney);

        String childGender = "Female";
        float childSearch = 30f;


        Bunny newBabyBunny = GameObject.Instantiate(babyBunny, Vector3.zero, Quaternion.identity).GetComponent<Bunny>();
        newBabyBunny.Initialize(childspeedgenex, childspeedgeney, childSpeed, childGender, childSearch);
    }


    double DetermineChildSpeed(double fatherSpeed, double motherSpeed, char fatherSpeedGeneX, char motherSpeedGeneX, char fatherSpeedGeneY, char motherSpeedGeneY)
    {
       
        char[] combinationOne = new char[2] { fatherSpeedGeneX, motherSpeedGeneX };
        char[] combinationTwo = new char[2] { fatherSpeedGeneX, motherSpeedGeneY };
        char[] combinationThree = new char[2] { fatherSpeedGeneY, motherSpeedGeneX };
        char[] combinationFour = new char[2] { fatherSpeedGeneY, motherSpeedGeneY };

        char[][] allCombinations = new char[4][] { combinationOne, combinationTwo, combinationThree, combinationFour };

        System.Random random = new System.Random();
        random.Next(0, 3);

        char[] childAlleles = allCombinations[random.Next(0, 3)];
        childspeedgenex = childAlleles[0];
        childspeedgeney = childAlleles[1];

        double speedMod = 1.0;
        if (Char.IsUpper(childAlleles[0]))
        {
            if (Char.IsUpper(childAlleles[1]))
            {
                speedMod = 1.1;
            }
            if (Char.IsLower(childAlleles[1]))
            {
                speedMod = 1.1;
            }

        }
        else
        {
            if (Char.IsUpper(childAlleles[1]))
            {
                speedMod = 1.1;
            }
            if (Char.IsLower(childAlleles[1]))
            {
                speedMod = 0.8;
            }

        }


        double childSpeed = ((fatherSpeed + motherSpeed)/2)* speedMod;
        Debug.Log("possible traits are: " + combinationOne[0] + combinationOne[1]);
        Debug.Log("possible traits are: " + combinationTwo[0] + combinationTwo[1]);
        Debug.Log("possible traits are: " + combinationThree[0] + combinationThree[1]);
        Debug.Log("possible traits are: " + combinationFour[0] + combinationFour[1]);

        Debug.Log("child traits are : " + childAlleles[0] + childAlleles[1]);
        Debug.Log("speedmod is: " + speedMod);
        Debug.Log("childspeed is: " + childSpeed);
        return childSpeed;
    }







}
