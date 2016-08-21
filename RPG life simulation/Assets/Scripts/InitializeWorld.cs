using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/*
 *  Script for initializing the world
 *      - Sets ground and objects randomly
 */
public enum Types { TREE, ROCK, GRASS };
public class InitializeWorld : MonoBehaviour
{

    public int columns = 5;
    public int rows = 5;

    public GameObject grassTile;
    public GameObject waterTile;
    public GameObject beachTile;

    public GameObject[] treeObjects;
    public GameObject[] rockObjects;
    public GameObject[] grassObjects;

    private ObjectProperties objProp;

    private List<Vector3> gridPositions = new List<Vector3>(); // All possible locations
    private List<bool> occupiedLocations = new List<bool>();    // All taken locations
    private Transform worldHolder;

    public void InitializeList()
    {
        // Cler the list
        gridPositions.Clear();

        // Loop through list and add x and y coordinates, set all occupied locations to false
        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
                gridPositions.Add(new Vector3(x, 0f, z));
                occupiedLocations.Add(false);
            }
        }
    }

    public void SetupGround()
    {
        //  Set all objects under World-transform
        worldHolder = new GameObject("World").transform;

        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
                GameObject toInstatiate = grassTile;

                if (x == 0 || x == rows - 1 || z == 0 || z == columns - 1)
                {
                    toInstatiate = beachTile;
                }

                GameObject instance = Instantiate(toInstatiate, new Vector3(x, 0f, z), Quaternion.Euler(90, 0, 0)) as GameObject;
                instance.transform.SetParent(worldHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        // Randomize a coordinate
        int randomIndex = 0;

        bool success = false;
        int check = 0;
        while (!success || check == 1000)
        {
            randomIndex = Random.Range(0, gridPositions.Count);
            if (occupiedLocations[randomIndex] == false)
            {
                occupiedLocations[randomIndex] = true;
                success = true;
            }
            check++;
        }
        Vector3 randomPosition = gridPositions[randomIndex];

        return randomPosition;
    }

    // Calculates how many objects are instantiated at town of current size
    int CalculateAmount(int number)
    {
        int sum = rows * columns;
        int temp = (int)((sum / rows) + (sum / columns)) / number;

        return temp;
    }

    public void LayoutObjectAtRandom(GameObject[] arrayOfObjects, Types typeOfObject)
    {
        if (arrayOfObjects != null)
        {
            objProp = arrayOfObjects[0].GetComponent<ObjectProperties>();

            int incidence = objProp.incidenceNumber;
            Debug.Log("incidence: " + incidence);
            int calculatedAmount = CalculateAmount(incidence);

            //  Set correct amount of objects into the world
            for (int i = 0; i < calculatedAmount; i++)
            {
                Vector3 randomPosition = RandomPosition();
                int rotation;
                float height;

                if (typeOfObject == Types.TREE)
                {
                    rotation = -90;
                    height = 0.5f;
                }
                else if (typeOfObject == Types.GRASS)
                {
                    rotation = 0;
                    height = 0f;
                }
                else if (typeOfObject == Types.ROCK)
                {
                    rotation = 90;
                    height = 0.1f;
                }
                else
                {
                    rotation = 0;
                    height = 0f;
                }

                GameObject choosedObject = arrayOfObjects[Random.Range(0, arrayOfObjects.Length)];
                GameObject toInstantiate = Instantiate(choosedObject, randomPosition, Quaternion.Euler(rotation, 0, 0)) as GameObject;
                toInstantiate.transform.localPosition = new Vector3(toInstantiate.transform.position.x, height, toInstantiate.transform.position.z);
                toInstantiate.transform.SetParent(worldHolder);
            }
        }
        else
        {
            Debug.Log("arrayOfObjects is null");
        }
    }
}
