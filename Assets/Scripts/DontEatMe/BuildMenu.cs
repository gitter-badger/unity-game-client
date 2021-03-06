﻿using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour
{

    // Background material
    public Material backgroundMaterial;

    public DemButton buttons;

    // Currently building...
    public static BuildInfo currentlyBuilding;

    public static DemAnimalFactory currentAnimalFactory;

    // Currently about to delete?
    public static bool currentlyDeleting = false;

    // Player's current resource amount
    public static int currentResources = 250;

    // Player's current score amount
    public static int score = 0;

    // Plant prefabs
    public DemAnimalFactory[] plants;

    // Prey prefabs
    public DemAnimalFactory[] prey;


    void OnGUI()
    {
        // draw resource menu
        GUILayout.BeginArea(new Rect(0, 0, 155, 200));
        GUILayout.BeginHorizontal("box");

        // draw resource counter
        GUILayout.Button(new GUIContent("Resources: " + currentResources.ToString()), GUILayout.Height(70));

        // end GUI for resource menu
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        // draw the plant menu
        GUILayout.BeginArea(new Rect(0, 80, 100, 400));
        GUILayout.BeginVertical();

        // Draw each plant's build info


        foreach (DemAnimalFactory plant in plants)
        {

            //BuildInfo info = plant.GetComponent<BuildInfo> ();

            //GUI.enabled = currentResources >= info.price;
            // if button is clicked, then set currentlyBuilding to the info of the button you clicked
            if (GUILayout.Button(new GUIContent(plant.GetImage()), GUILayout.Width(40), GUILayout.Height(40)))
            {
                DemAudioManager.audioClick.Play();

                // If a selection is currently in progress...
                if (DemMain.currentSelection)
                {
                    // Ignore button click if for the same species
                    if (currentAnimalFactory == plant)
                        return;
                    // Otherwise, destroy the current selection before continuing
                    else
                        Destroy(DemMain.currentSelection);
						DemMain.boardController.ClearAvailableTiles (); //fix bug
                }
                // Set / reset currentlyBuilding

                //currentlyBuilding = info;
                currentAnimalFactory = plant;


                // Create the current prey object
                //GameObject currentPlant = plant.Create(); //DemAnimalFactory.Create(currentlyBuilding.name , 0 ,0) as GameObject;

                // Define the current prey's initial location relative to the world (i.e. NOT on a screen pixel basis)
                Vector3 init_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                init_pos.z = -1.5f;

                // Instantiate the current prey
                DemMain.currentSelection = plant.Create();
				//DemMain.currentSelection.GetComponent<BuildInfo>().isPlant = true;
				DemMain.currentSelection.GetComponent<BuildInfo>().speciesType = 0;


                // Set DemMain's preyOrigin as the center of the button
                DemMain.setBuildOrigin(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                DemMain.boardController.SetAvailableTiles();

                // DEBUG MESSAGE
                //Debug.Log("currentPlant set to " + currentPlant.name);
            }

        }

        // End GUI for plant menu
        GUILayout.EndVertical();
        GUILayout.EndArea();

        // Now, draw prey menu
        GUILayout.BeginArea(new Rect(300, 0, 500, 220));
        GUILayout.BeginHorizontal("box");

        // draw each prey's build info
        foreach (DemAnimalFactory singlePrey in prey)
        {

            // if button is clicked, then set currentlyBuilding to the info of the button you clicked

            if (GUILayout.Button(new GUIContent(singlePrey.GetImage()), GUILayout.Width(40), GUILayout.Height(40)))
            {
                DemAudioManager.audioClick.Play();
                // If a selection is currently in progress...
                if (DemMain.currentSelection)
                {
                    // Ignore button click if for the same species
                    if (currentAnimalFactory == singlePrey)
                        return;
                    // Otherwise, destroy the current selection before continuing
                    else
                        Destroy(DemMain.currentSelection);
						DemMain.boardController.ClearAvailableTiles ();		//fix the prey placement bug after change currentSelection from plant to prey
                }
                // Set / reset currentlyBuilding

                currentAnimalFactory = singlePrey;



                // Define the current prey's initial location relative to the world (i.e. NOT on a screen pixel basis)
                Vector3 init_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
                init_pos.z = -1.5f;


                // Instantiate the current prey
                DemMain.currentSelection = singlePrey.Create();
				DemMain.currentSelection.GetComponent<BuildInfo>().speciesType = 1;


                // Set DemMain's preyOrigin as the center of the button
                DemMain.setBuildOrigin(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                DemMain.boardController.SetAvailableTiles();


            }
        }

        // End GUI for prey menu
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }


    // this method increases score every 2s
    void increaseResources()
    {
        currentResources += 50;
    }

    
    //Loading Resources
    void Awake()
    {
        backgroundMaterial = Resources.Load<Material>("DontEatMe/Materials/DontEatMeBg");                                                                                      
    }


    // Use this for initialization
    void Start()
    {
        // set resources to grow over time
        InvokeRepeating("increaseResources", 2, 3.0F);
        
        /**
         * Elaine:
         * All this can be done in the editor beforehand since the background never changes
         * but we can decide on this later
         * I also commented out the background code in DemMain.cs since the background looks fine w/o it
         */

        // Creates the background manually
        GameObject backgroundImage = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backgroundImage.name = "DemBackGround";

        Destroy(backgroundImage.GetComponent<MeshCollider>());

        backgroundImage.GetComponent<Renderer>().material = backgroundMaterial;
        
        backgroundImage.transform.localPosition = 
            new Vector3(backgroundImage.transform.localPosition.x, backgroundImage.transform.localPosition.y, 1);
        backgroundImage.transform.localScale = new Vector3(14, 7, backgroundImage.transform.localScale.z);

        /**
         * Uncomment the following 2 lines out and comment out the OnGUI() code to check out the new buttons
         */
        // Creates the buttons
        //buttons = gameObject.AddComponent<DemButton>();
        //buttons.MakeButton(5, 2, 59, 80);

        plants = new DemAnimalFactory[6];

        plants[0] = new DemAnimalFactory("Acacia");
        plants[1] = new DemAnimalFactory("Baobab");
        plants[2] = new DemAnimalFactory("Big Tree");
        plants[3] = new DemAnimalFactory("Fruits And Nectar");
        plants[4] = new DemAnimalFactory("Grass And Herbs");
        plants[5] = new DemAnimalFactory("Trees And Shrubs");


        prey = new DemAnimalFactory[6];
        prey[0] = new DemAnimalFactory("Bohor Reedbuck");
        prey[1] = new DemAnimalFactory("Bat-Eared Fox");
        prey[2] = new DemAnimalFactory("Kori Buskard");
        prey[3] = new DemAnimalFactory("Black Backed Jackal");
        prey[4] = new DemAnimalFactory("Dwarf Mongoose");
        prey[5] = new DemAnimalFactory("Dwarf Epauletted Bat");


    }
    

    public void endGame()
    {
        /*
            Debug.Log("Game ended with X coins: " + coins);

            //LOBBY TEAM, PUT YOUR RETURN CODE HERE, PASS BACK
            //coins variable
            NetworkManager.Send(
                EndGameProtocol.Prepare(1, coins),
                ProcessEndGame
            );
      */
    }


    // Updates player's credits
    public void ProcessEndGame(NetworkResponse response)
    {
        ResponsePlayGame args = response as ResponsePlayGame;

        if (args.status == 1)
        {

            GameState.player.credits = args.creditDiff;
            Debug.Log(args.creditDiff);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
