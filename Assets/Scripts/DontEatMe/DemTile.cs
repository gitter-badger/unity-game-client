﻿using UnityEngine;
using System.Collections;

/**
    The DemTile class represents a single tile of the Don't Eat Me grid board.
*/
public class DemTile : MonoBehaviour
{
  	int idX; // X-coord for DemTile
  	int idY; // Y-coord for DemTile
    public Color currentColor;

    public bool available;

  	public GameObject resident; // Resident object (HerbivoreObject or PlantObject) placed on tile

    // Use this for initialization
    void Start ()
    {
        // Parse X and Y coords from name
        // Name format = "X,Y", so X is stored @ name[0] and Y @ name[2]
        // The char value for '0' starts at 0x30, subtract this to parse numeric value
        // NOTE: this assumes that X and Y values remain within the range [0,9]
        idX = this.name[0] - 0x30; 
        idY = this.name[2] - 0x30;
        //Debug.Log("Cube at (" + idX + ", " + idY + ")");

        // Set resident to null
        resident = null;
    }

    /**
        Activates on mouse enter.
    */
    void OnMouseEnter ()
    {
        // Set highlight color
        // TODO: change highlight color based on a tile's legality
        if (BuildMenu.currentlyBuilding) {
	        if (!resident)
	            this.GetComponent<Renderer>().material.color = Color.cyan;
	        else
	            this.GetComponent<Renderer>().material.color = Color.red;
        }
        else {
        	this.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    /**
        Activates on mouse exit.
    */
    void OnMouseExit ()
    {
        // Reset highlight color
        this.GetComponent<Renderer>().material.color = Color.white;
    }

    /**
        Activates on mouse click.
    */
    void OnMouseDown ()
    {
        // Get center coords of tile, set z offset for resident placement
        Vector3 center = this.GetComponent<Renderer>().bounds.center;
        center.z = -1.5f;

        // DEBUG
        Debug.Log("Tile (" + idX + ", " + idY + ") clicked, center @ (" + center.x + ", " + center.y + ", " + center.z + ")");

        // If tile is empty...
        if (!resident) {
            // If a creature is flagged for building...
            if (BuildMenu.currentlyBuilding) {
                // Set the resident as the DemMain's current selection if clicked within the tile; center resident on tile
                resident = DemMain.currentSelection;
                resident.transform.position = center;

                // Subtract the appropriate resources for the build
                BuildMenu.currentResources -= BuildMenu.currentlyBuilding.price;

                // Testing animation transition from IDLE to animated once placed on board
                // NOTE: only supported species (i.e. those currently with animations) should call 'SetTrigger';
                // unsupported species will cause unpredictable behavior
                // TODO: implement animations for all species
                if (
                    BuildMenu.currentlyBuilding.name == "BushHyrax" ||
                    BuildMenu.currentlyBuilding.name == "TreeMouse")
                    resident.GetComponent<Animator>().SetTrigger("initialized");

                // Set BuildMenu.currentlyBuilding to null after successful placement
                BuildMenu.currentlyBuilding = null;
                DemMain.boardController.ClearAvailableTiles();

                // DEBUG 
                if (resident)
                    Debug.Log("Placed " + resident.name + " @ " + resident.GetComponent<Transform>().position);
            }
        }
        // If tile is inhabited...
        else {
            // DEBUG
            Debug.Log("Tile inhabited by " + resident.name);
        }
    }
}
