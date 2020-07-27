using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSystem : MonoBehaviour
{
    public GameObject reticle;

    public Color interactiveReticleColor = Color.gray;

    public Color activeReticleColor = Color.green;

    private GazableObject currentGazeObject;

    private GazableObject currentSelectedObject;
    private RaycastHit lastHit;


    // Start is called before the first frame update
    void Start()
    {
        SetReticleColor(interactiveReticleColor);
    }

    // Update is called once per frame
    void Update()
    {

        ProcessGaze();
        CheckForInput(lastHit);
        
    }

    public void ProcessGaze()
    {
        Ray raycastRay = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        Debug.DrawRay(raycastRay.origin, raycastRay.direction * 100);

        if (Physics.Raycast(raycastRay, out hitInfo))
        {

            // Do something to the object

            // 1. Check if the object is interactable
            GameObject hitObj = hitInfo.collider.gameObject;

            // Get the GazableObject from the hit object
            GazableObject gazeObj = hitObj.GetComponentInParent<GazableObject>();

            // 2. Check if the object is a new object (first time looking)
            // 3. Set the reticle color

            // Object has a GazableObject component
            if (gazeObj != null)
            {
                // Object we're looking at is different
                if(gazeObj != currentGazeObject)
                {

                    ClearCurrentObject();
                    currentGazeObject = gazeObj;
                    currentGazeObject.OnGazeEnter(hitInfo);
                    SetReticleColor(activeReticleColor);

                }
                else
                {
                    currentGazeObject.OnGaze(hitInfo);
                }
            }
            else
            {
                ClearCurrentObject();
            }

            lastHit = hitInfo;

        }
        else
        {
            ClearCurrentObject();
        }
    }

    private void SetReticleColor(Color reticleColor)
    {
        // Set the color of the reticle
        reticle.GetComponent<Renderer>().material.SetColor("_Color", reticleColor);

    }

    private void CheckForInput(RaycastHit hitInfo)
    {

        // Check for down
        if(Input.GetMouseButtonDown(0) && currentGazeObject != null)
        {
            currentSelectedObject = currentGazeObject;
            currentSelectedObject.OnPress(hitInfo);

        }

        // Check for hold
        else if(Input.GetMouseButton(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnHold(hitInfo);

        }


        // Check for release
        else if (Input.GetMouseButtonUp(0) && currentSelectedObject != null)
        {
            currentSelectedObject.OnRelease(hitInfo);
            currentSelectedObject = null;

        }

    }

    private void ClearCurrentObject()
    {

        if (currentGazeObject != null)
        {

            currentGazeObject.OnGazeExit();
            SetReticleColor(interactiveReticleColor);
            currentGazeObject = null;


        }
    }


}
