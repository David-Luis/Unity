using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableComponent : MonoBehaviour
{
    private bool isPlacing = false;
    private GameObject placeablePlane;

    bool isInPlaceableZone = false;
    int collidingPlaceables = 0;

    public void StartPlacing()
    {
        isInPlaceableZone = false;
        placeablePlane = GameObject.FindGameObjectWithTag("PlaceablePlane");
        isPlacing = true;
    }

    private void Update()
    {
        if (isPlacing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance = Mathf.Infinity;
            int layer_mask = LayerMask.GetMask("placeablePlane");
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layer_mask))
            {
                transform.position = hit.point;
            }

            bool canBePlaced = CanBePlaced();
            if (canBePlaced && Input.GetMouseButtonDown(0))
            {
                Place();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlaceableZone"))
        {
            isInPlaceableZone = true;
        }

        if (other.CompareTag("Placeable"))
        {
            collidingPlaceables++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlaceableZone"))
        {
            isInPlaceableZone = false;
        }

        if (other.CompareTag("Placeable"))
        {
            collidingPlaceables--;
        }
    }

    private void Place()
    {
        isPlacing = false;
    }

    private bool CanBePlaced()
    {
        return isInPlaceableZone && collidingPlaceables == 0;
    }
}
