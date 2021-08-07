using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;


public class PlaceObject : MonoBehaviour
{
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
    private bool isObjectPlaced;

    public GameObject positionIndicator;
    public GameObject prefabToPlace;
    public Camera aRCamera;
    

    private void Awake()
    {
    	aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
    	if (!isObjectPlaced)
    	{
        	UpdatePlacementPose();

        	if(placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        	{
        		placeObject();
        	}
    	}
        
    }


    private void UpdatePlacementPose()
	{
        var screenCenter = aRCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.All);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
		{
            PlacementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
            positionIndicator.SetActive(true);
            positionIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
        	positionIndicator.SetActive(false);
        }
	}

    private void placeObject()
    {
    	Instantiate(prefabToPlace, PlacementPose.position, PlacementPose.rotation);
    	isObjectPlaced = true;
    	positionIndicator.SetActive(false);

    }
    /*
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
		{
            placementIndicator.SetActive(false);
		}
	}
	*/
    
}
