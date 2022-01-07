using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionHUD : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float viewportSizeAdjustement = 0.1f;
    public Camera playerCamera; //Set in hierarchy
    public bool canDetect;

    [SerializeField] private Image topWarningImage;
    [SerializeField] private Image bottomWarningImage;
    [SerializeField] private Image rightWarningImage;
    [SerializeField] private Image leftWarningImage;

    public void CheckAndDisplayTargetDirection(Vector3 targetPosition)
    {
        if (canDetect)
        {
            //First check if the target is inside the camera viewport
            Vector3 targetViewportPos = Camera.main.WorldToViewportPoint(targetPosition);
            bool inCameraFrustum = IsInViewportRange(targetViewportPos.x) && IsInViewportRange(targetViewportPos.y);
            bool inFrontOfCamera = targetViewportPos.z > 0;

            rightWarningImage.enabled = false;
            topWarningImage.enabled = false;
            bottomWarningImage.enabled = false;
            leftWarningImage.enabled = false;

            //If it isn't, check which direction it is from the camera and activate the corresponding sprite
            if (!inCameraFrustum || !inFrontOfCamera)
            {
                switch (TargetRelativePosition(targetPosition))
                {
                    case 0:
                        rightWarningImage.enabled = true;
                        break;
                    case 1:
                        bottomWarningImage.enabled = true;
                        break;
                    case 2:
                        leftWarningImage.enabled = true;
                        break;
                    case 3:
                        topWarningImage.enabled = true;
                        break;
                }
            }

        }
    }

    private int TargetRelativePosition(Vector3 targetPosition)
    {
        Vector3 CameraToTarget = targetPosition - playerCamera.transform.position;

        float rightAngle = Vector3.Angle(CameraToTarget, playerCamera.transform.right);
        float leftAngle = Vector3.Angle(CameraToTarget, -playerCamera.transform.right);
        float upAngle = Vector3.Angle(CameraToTarget, playerCamera.transform.up);
        float downAngle = Vector3.Angle(CameraToTarget, -playerCamera.transform.up);

        float biggestAngle = Mathf.Min(rightAngle, leftAngle, upAngle, downAngle);

        if (biggestAngle == rightAngle)
        {
            return 0;
        }
        else if (biggestAngle == downAngle)
        {
            return 1;
        }
        else if (biggestAngle == leftAngle)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    public bool IsInViewportRange(float a)
    {
        return (a + viewportSizeAdjustement) > 0 && (a < 1 - viewportSizeAdjustement);
    }
}
