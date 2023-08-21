using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public float speed = 1.0f;

    private int state = 0;
    private List<GameObject> crystals = new List<GameObject>();
    private Vector3 target;
    private Vector3 startPos;
    private bool targetNeeded = true;
    private GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        // get parent
        parent = gameObject;

        // find all crystals
        // (every object that has tag "Crystal")
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Crystal");
        foreach (GameObject obj in objects)
        {
            crystals.Add(obj);
        }

        // save current pos as start pos
        startPos = parent.transform.position;


        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        // if we have a target, move towards it
        if (!targetNeeded)
        {
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, target, step);
        }

        // if we are at the target, set target needed to true
        if (parent.transform.position == target)
        {
            targetNeeded = true;
        }

        // if we have no target, find a new one
        if (targetNeeded)
        {
            // if we are in state 0 or 1, find a crystal
            if (state < 2)
            {
                // find a crystal
                GameObject crystal = crystals[Random.Range(0, crystals.Count)];

                // set target to crystal
                target = crystal.transform.position;

                // set target needed to false
                targetNeeded = false;

                
                state += 1;
            }

            // if we are in state 2, find the start pos
            else if (state == 2)
            {
                // set target to start pos
                target = startPos;

                // set target needed to false
                targetNeeded = false;

                // set state to 0
                state = 0;
            }
        }
        
    }
}
