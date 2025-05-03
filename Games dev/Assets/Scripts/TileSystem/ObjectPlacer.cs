using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] PlacementSystemm placementSystemm;
    public List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position) //instantises prefab, puts it in position, and adds it to placed objects list
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;


        Button[] allButtons = newObject.GetComponentsInChildren<Button>(true); // true = include inactive

        foreach (Button btn in allButtons)
        {
            if (btn.name == "Sell")
            {
                Button sellButton = btn;
                //Debug.Log(sellButton);
                sellButton.onClick.AddListener(() => placementSystemm.SellTower());
                break;
            }
        }

        int nullindex = placedGameObjects.FindIndex(obj => obj == null); //seeing if there are any null entries (been removed)
        if (nullindex != -1)
        {
            placedGameObjects[nullindex] = newObject; //set it to the index null is at
            return nullindex; //returns index added at
        } else
        {
            placedGameObjects.Add(newObject); //else new object add
            return placedGameObjects.Count - 1; //returns the index added at
        }
    }

    internal GameObject SelectObjectAt(int gameObjectindex)
    {
        return placedGameObjects[gameObjectindex];
    }

    internal void RemoveObjectAt(int gameObjectIndex) //removes object in list via its index
    {
        if (placedGameObjects.Count <= gameObjectIndex) 
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
