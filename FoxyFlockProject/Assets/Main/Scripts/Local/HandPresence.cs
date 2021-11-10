using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

    public class HandPresence : MonoBehaviour
    {
        public bool showController = false;
        public InputDeviceCharacteristics controllerCharacteristics;
        public List<GameObject> controllerPrefabs;
        public GameObject handModelPrefab;

        private Collider indexCollider;
        public PlayerMovement parent;
        public GameObject spawnedHandModel;
        private InputDevice targetDevice;
        private GameObject spawnedController;
        private Animator handAnimator;
        public bool isGrab;
        private void Start()
        {
            parent = GetComponentInParent<PlayerMovement>();

            if (!targetDevice.isValid)
            {
                TryInitialize();
            }
        }


        private void Update()
        {
            if (!indexCollider && transform.childCount > 0)
            {
                indexCollider = transform.GetChild(0).GetComponentInChildren<Collider>();
            }
            if (!targetDevice.isValid)
            {
                TryInitialize();
            }
            else
            {
                if (showController)
                {
                    if (spawnedHandModel.activeSelf)
                    {
                        spawnedHandModel.SetActive(false);
                        spawnedController.SetActive(true);
                    }
                }
                else
                {
                    if (indexCollider)
                        UpdateHandAnimation();
                }
            }
        }

        private void TryInitialize()
        {
            List<InputDevice> devices = new List<InputDevice>();

            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

            if (devices.Count > 0)
            {
                targetDevice = devices[0];

                spawnedHandModel = Instantiate(handModelPrefab, this.transform);
                handAnimator = spawnedHandModel.GetComponent<Animator>();
            //if is not menu desable ray track
            if (!ScenesManager.instance.IsMenuScene())
                spawnedHandModel.GetComponentInChildren<LineRenderer>().gameObject.SetActive(false);
            }
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 6)
            {
                isGrab = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                isGrab = false;
            }
        }
    private bool isGrabReset;
        private void UpdateHandAnimation()
        {
            if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue) && gripValue > 0.1f)
            {
                isGrabReset = false;

            if (!isGrab)
                {
                    handAnimator.SetFloat("Trigger", gripValue);
                    if (gripValue > 0.5f)
                    {
                        indexCollider.isTrigger = false;
                    }
                    else
                    {
                        indexCollider.isTrigger = true;
                    }
                }
                else
                {
                    handAnimator.SetFloat("Grip", gripValue);
                }
                
            }
        else if (!isGrabReset)
        {

            handAnimator.SetFloat("Trigger", 0);
            indexCollider.isTrigger = true;

            handAnimator.SetFloat("Grip", 0);
            isGrabReset = true;
        }

    }
    }


