using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private class CurrentSelected
    {
        public Material material;
        public Transform transform;

        public CurrentSelected(Material material, Transform transform)
        {
            this.material = material;
            this.transform = transform;
        }
    }

    public Camera camera;
    public Material selectedMaterial;
    private CurrentSelected currentSelected;
    public GameObject earthPrefab;
    private const float MAX_SELECT_DISTANCE = 5;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, MAX_SELECT_DISTANCE))
        {
            Transform objectHit = hit.transform;

            Unselect();
            currentSelected = new CurrentSelected(objectHit.GetComponent<Renderer>().material, objectHit);
            ApplySelectedMaterial();
        }
        else
        {
            Unselect();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            DeleteSelected();
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            CreateEarth(hit.normal);
        }
    }

    private void Unselect()
    {
        if (currentSelected != null)
        {
            RestoreMaterial();
            currentSelected = null;
        }
    }

    private void CreateEarth(Vector3 normal)
    {
        Debug.Log(normal);
        Instantiate(earthPrefab, new Vector3(currentSelected.transform.position.x + normal.x, currentSelected.transform.position.y + normal.y, currentSelected.transform.position.z + normal.z), Quaternion.identity);
    }

    private void DeleteSelected()
    {
        Destroy(currentSelected.transform.gameObject);
        currentSelected = null;
    }

    private void ApplySelectedMaterial()
    {
        currentSelected.transform.GetComponent<Renderer>().material = selectedMaterial;
    }

    private void RestoreMaterial()
    {
        currentSelected.transform.GetComponent<Renderer>().material = currentSelected.material;
    }
}
