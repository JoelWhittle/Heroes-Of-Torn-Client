using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour
{

    public Node[,] Graph = new Node[Map.width,Map.height];
    public static Pathfinding Instance;
	// Use this for initialization
	void Start ()
	{

	    Instance = this;

	}

    private void ResizeArray(ref Node[,] original, int cols, int rows)
    {

        //create a new 2 dimensional array with

        //the size we want
        Node[,] newArray = new Node[cols, rows];
        //copy the contents of the old array to the new one

        Array.Copy(original, newArray, original.Length);

        //set the original to the new array

        original = newArray;

    }

    public void Init(int w, int h)
{

        ResizeArray(ref Graph,w,h);
    for (int x = 0; x < w; x++)
    {
        for (int y = 0; y < h; y++)
        {
            Graph[x, y] = new Node();
            Graph[x, y].x = x;
            Graph[x, y].y = y;

        }
    } 
}
    public float CostToEnterTile(Node SourceNode, Node TargetNode, bool IsCrowFlying)
    {

    



            if (UnitCanEnterTile(TargetNode.CorrespondingHexGameObject()) == false)
                return Mathf.Infinity;

            float cost = TargetNode.CorrespondingHexGameObject().GetComponent<Hex>().GetMovementCost();

            if (IsCrowFlying == true)
            {
                return 1;
            }
            else
            {

            return cost;
        }

    }

    	public bool UnitCanEnterTile(GameObject hex) {

		// We could test the unit's walk/hover/fly type against various
		// terrain flags here to see if they are allowed to enter the tile.

    	    return hex.GetComponent<Hex>().IsWalkable();
    	}

    public List<GameObject> FindNEWPath(GameObject CurHex, GameObject TargetHex)
    {
        List<GameObject> ReturnList = new List<GameObject>();


        int TargetX = TargetHex.GetComponent<Hex>().x;
        int TargetY = TargetHex.GetComponent<Hex>().y;
		

		if( UnitCanEnterTile(TargetHex) == false ) {
			// We probably clicked on a mountain or something, so just quit out.
		    return ReturnList;
		}

	

		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();


        Node source = null;
        if (CurHex != null)
        {
          source = Graph[
                       CurHex.GetComponent<Hex>().x,
                       CurHex.GetComponent<Hex>().y
                       ];
           
	
        }
        else
        {
            ;
        }
        


		Node target = Graph[
		                    TargetHex.GetComponent<Hex>().x, 
		                    TargetY
		                    ];
        source.dist = 0;
        source.prev = null;
	
        

		// Initialize everything to have INFINITY distance, since
		// we don't know any better right now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
        foreach (Node v in Graph)
        {
            if (v != null)
            {
                if (v != source)
                {
                    v.dist = Mathf.Infinity;

                    v.prev = null;

                }

                unvisited.Add(v);
            }
        }
        while(unvisited.Count > 0) {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;

			foreach(Node possibleU in unvisited) {
				if(u == null || possibleU.dist < u.dist) {
					u = possibleU;
				}
			}

			if(u == target) {
				break;	// Exit the while loop!
			}

			unvisited.Remove(u);

			foreach(Node v in u.NeighbourNodes()) {
			
				float alt = u.dist + CostToEnterTile(u,v, false);
				if( alt < v.dist ) {
				v.dist = alt;
				v.prev= u;
				}
			}
		}

		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.

		if(target.prev == null) {
			// No route between our target and the source
		    return null;
		    ;
		}

		List<Node> currentPath = new List<Node>();

		Node curr = target;

		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = curr.prev;
		}

		// Right now, currentPath describes a route from out target to our source
		// So we need to invert it!

		currentPath.Reverse();

        foreach (Node n in currentPath)
        {
            GameObject go = n.CorrespondingHexGameObject();
            ReturnList.Add(go);
            
        }
        Debug.Log(ReturnList.Count.ToString());
        return ReturnList;
}

    public int GetDistanceAsCrowFlys(GameObject CurHex, GameObject TargetHex)
    {
        List<GameObject> ReturnList = new List<GameObject>();


        int TargetX = TargetHex.GetComponent<Hex>().x;
        int TargetY = TargetHex.GetComponent<Hex>().y;


        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();


        Node source = null;
        if (CurHex != null)
        {
            source = Graph[
                         CurHex.GetComponent<Hex>().x,
                         CurHex.GetComponent<Hex>().y
                         ];


        }
        else
        {
            ;
        }



        Node target = Graph[
                            TargetHex.GetComponent<Hex>().x,
                            TargetY
                            ];
        source.dist = 0;
        source.prev = null;



        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value
        foreach (Node v in Graph)
        {
            if (v != null)
            {
                if (v != source)
                {
                    v.dist = Mathf.Infinity;

                    v.prev = null;

                }

                unvisited.Add(v);
            }
        }
        while (unvisited.Count > 0)
        {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || possibleU.dist < u.dist)
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;	// Exit the while loop!
            }

            unvisited.Remove(u);

            foreach (Node v in u.NeighbourNodes())
            {

                float alt = u.dist + 1;
                if (alt < v.dist)
                {
                    v.dist = alt;
                    v.prev = u;
                }
            }
        }

        // If we get there, the either we found the shortest route
        // to our target, or there is no route at ALL to our target.

        if (target.prev == null)
        {
            // No route between our target and the source
            return 999;
            ;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        // Step through the "prev" chain and add it to our path
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = curr.prev;
        }


        return currentPath.Count - 1;
    }

    public List<GameObject> FindMovementRadius(GameObject CurHex, float MovementSpeed)
    {
        List<GameObject> ReturnList = new List<GameObject>();


        // Setup the "Q" -- the list of nodes we haven't checked yet.
        List<Node> unvisited = new List<Node>();


        Node source = null;
        if (CurHex != null)
        {
            source = Graph[
                         CurHex.GetComponent<Hex>().x,
                         CurHex.GetComponent<Hex>().y
                         ];


        }
        else
        {
            ;
        }


        source.dist = 0;
        source.prev = null;



        // Initialize everything to have INFINITY distance, since
        // we don't know any better right now. Also, it's possible
        // that some nodes CAN'T be reached from the source,
        // which would make INFINITY a reasonable value
        foreach (Node v in Graph)
        {
            if (v != null)
            {
                if (v != source)
                {
                    v.dist = Mathf.Infinity;

                    v.prev = null;

                }

                unvisited.Add(v);
            }
        }
        while (unvisited.Count > 0)
        {
            // "u" is going to be the unvisited node with the smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || possibleU.dist < u.dist)
                {
                    u = possibleU;
                }
            }

         

            unvisited.Remove(u);

            foreach (Node v in u.NeighbourNodes())
            {

                float alt = u.dist + CostToEnterTile(u, v, false);
                if (alt < v.dist)
                {
                    v.dist = alt;
                    v.prev = u;
                }
            }
        }

        foreach (Node n in Graph)
        {
            if (n.dist < MovementSpeed)
            {
                ReturnList.Add(n.CorrespondingHexGameObject());
            }
        }

        Debug.Log(ReturnList.Count.ToString());
        return ReturnList;
    }



    public IEnumerator LerpObjectFromAToB(GameObject objectToLerp, Vector3 targetPosition, float time, AnimationCurve animationCurve)
    {
        Vector3 StartPosition = objectToLerp.transform.position;
        float timer = 0.0f;

        while (timer <= time)
        {
          objectToLerp.transform.position = Vector3.Lerp(StartPosition, targetPosition,
                animationCurve.Evaluate(timer / time));
            timer += Time.deltaTime;
            yield return null;

        }
        yield return null;
    }


    public List<Node> SortOpenNodesList(List<Node> inputList,Node TargetNode )
    {

        inputList.Sort((a, b) => { return a.GetFCost(TargetNode).CompareTo(b.GetFCost(TargetNode)); });

        return inputList;
    }

    public class Node
    {
     public  Node ParentNode;
     public int PathGeneration;
     public float GCost;
     
     public int x;
     public int y;
    
     public int Elevation = 0;
        public Node prev;
        public float dist;
        
        public bool IsWalkable()
        {
            return Map.GoHex[x, y].GetComponent<Hex>().IsWalkable();
        }
        public float GetFCost(Node TargetNode)
        {
            return PathGeneration + GCost + HCostToNode(TargetNode);
        }
        public  float GetMovementCost()
        {
            return CorrespondingHexGameObject().GetComponent<Hex>().GetMovementCost();
        }
        public GameObject CorrespondingHexGameObject()
        {
            return Map.GoHex[x, y];
        }

        public int HCostToNode(Node TargetNode)
        {

            return Math.Abs(x - TargetNode.x ) + Math.Abs( y - TargetNode.y);
        }

        public List<Node> NeighbourNodes()
        {
            List<Node> ReturnList = new List<Node>();
            foreach (GameObject NeighbourHexGameObject in CorrespondingHexGameObject().GetComponent<Hex>().GetNeighbours())
            {
                Hex cHex = NeighbourHexGameObject.GetComponent<Hex>();
                Node CorrespondingNode = GameObject.FindGameObjectWithTag("Map").GetComponent<Pathfinding>().Graph[cHex.x,cHex.y];
                ReturnList.Add(CorrespondingNode);

            }
            return ReturnList;
        }

        public List<Node> ReturnPath()
        {
            List<Node> ReturnList = new List<Node>();
            Node CurrentNode = this;
            for (int i = 0 ; i < PathGeneration ; i++)
            {
                ReturnList.Add(CurrentNode.ParentNode);
                CurrentNode = CurrentNode.ParentNode;
            }
            return ReturnList;
        }

        public void Clean()
        {
            ParentNode = null;
            PathGeneration = 0;
            GCost = 0;
        }


    }
}
