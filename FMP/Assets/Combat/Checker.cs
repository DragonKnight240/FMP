using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Trigger Enter");
        if(other.CompareTag("Blocker") || other.CompareTag("Special"))
        {
            transform.GetComponentInParent<Tile>().CanMoveOn = false;
            transform.GetComponentInParent<Tile>().FindAdjacentTiles();
            print("Block");

            if(other.CompareTag("Special"))
            {
                other.GetComponent<InteractOnGrid>().Position = transform.GetComponentInParent<Tile>().GridPosition;
                transform.GetComponentInParent<Tile>().Special = other.GetComponent<InteractOnGrid>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("Trigger Exit");
        if (other.CompareTag("Blocker") || other.CompareTag("Special"))
        {
            transform.GetComponentInParent<Tile>().CanMoveOn = true;
            transform.GetComponentInParent<Tile>().Occupied = false;
            transform.GetComponentInParent<Tile>().FindAdjacentTiles();
            print("Block");

            if (other.CompareTag("Special"))
            {
                transform.GetComponentInParent<Tile>().Special = null;
            }
        }
    }
}
