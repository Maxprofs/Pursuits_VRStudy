﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSize : MonoBehaviour {

    public GameObject cube, camera, indicator;
    Rect gui;
    VisualDegrees vd;

    void Start()
    {
        vd.Init(2, camera.GetComponent<Camera>());
    }

    // Update is called once per frame
    void Update () {
        string _deb = "";
        
        // create an copy of the object with the same 
        GameObject _tempObj = GameObject.Instantiate(cube);

        // deactivate its MeshRenderer
        //_tempObj.GetComponent<MeshRenderer>().enabled = false;
        
        // get bounds.extents for further calculation
        Bounds tempBounds = _tempObj.GetComponentInChildren<MeshCollider>().bounds;
        _deb += "extents: " + tempBounds.extents.ToString();

        // destroy temporary object
        Destroy(_tempObj);

        // project extents on camera plane
        Vector3 newExtents = camera.transform.rotation * tempBounds.extents;
        //_deb += " projectedExt: " + newExtents.ToString();

        // use original object's center and tempObject's extents
        Bounds goBounds = cube.GetComponent<Renderer>().bounds;
        List<Vector3> minMaxValues = new List<Vector3>();
        
        //indicator.transform.position = newExtents;

        minMaxValues.Add(new Vector3(tempBounds.center.x - tempBounds.extents.x, tempBounds.center.y + tempBounds.extents.y, tempBounds.center.z - tempBounds.extents.z));
        minMaxValues.Add(new Vector3(tempBounds.center.x - tempBounds.extents.x, tempBounds.center.y + tempBounds.extents.y, tempBounds.center.z + tempBounds.extents.z));
        minMaxValues.Add(new Vector3(tempBounds.center.x - tempBounds.extents.x, tempBounds.center.y - tempBounds.extents.y, tempBounds.center.z - tempBounds.extents.z));
        minMaxValues.Add(new Vector3(tempBounds.center.x - tempBounds.extents.x, tempBounds.center.y - tempBounds.extents.y, tempBounds.center.z + tempBounds.extents.z));
        minMaxValues.Add(new Vector3(tempBounds.center.x + tempBounds.extents.x, tempBounds.center.y + tempBounds.extents.y, tempBounds.center.z - tempBounds.extents.z));
        minMaxValues.Add(new Vector3(tempBounds.center.x + tempBounds.extents.x, tempBounds.center.y + tempBounds.extents.y, tempBounds.center.z + tempBounds.extents.z));
        minMaxValues.Add(new Vector3(tempBounds.center.x + tempBounds.extents.x, tempBounds.center.y - tempBounds.extents.y, tempBounds.center.z - tempBounds.extents.z));
        minMaxValues.Add(new Vector3(tempBounds.center.x + tempBounds.extents.x, tempBounds.center.y - tempBounds.extents.y, tempBounds.center.z + tempBounds.extents.z));

        float maxX = 0, maxY = 0, minX = float.MaxValue, minY = float.MaxValue;

        foreach (Vector3 x in minMaxValues)
        {
            Vector3 screenPoint = camera.GetComponent<Camera>().WorldToScreenPoint(camera.transform.TransformPoint(x));
            if (screenPoint.x < minX) minX = screenPoint.x;
            if (screenPoint.x > maxX) maxX = screenPoint.x;
            if (screenPoint.y < minY) minY = screenPoint.y;
            if (screenPoint.y > maxY) maxY = screenPoint.y;
        }

        //_deb += " center: " + goBounds.center;
        gui = new Rect(minX, minY, maxX - minX, maxY - minY);
        _deb += " minX: " + minX + " maxX " + maxX + " diff " + (maxX-minX);
        Debug.Log(_deb);

    }

    private void OnGUI()
    {
        GUI.Box(gui, "");
    }
}
