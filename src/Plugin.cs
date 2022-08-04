using BepInEx;
using System;
using UnityEngine;
using UnityEngine.XR;
using BodhiDonselaar;

namespace LualtsCameraMod
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        // Lock
        bool canLock = true;
        bool locked = false;

        // Hand switching
        bool isRightHand = false;
        bool canSwitchHands = true;
        GameObject Rhand;
        GameObject Lhand;

        // Buttons
        bool buttonB;
        bool buttonA;
        bool buttonY;
        bool buttonX;
        Vector2 joyL;
        bool joyLC;
        bool joyRC;
        float triggerL;
        float triggerR;

        // Camera
        GameObject shoulderCamera;
        Camera cameraComponent;
        GameObject camera;
        float fov = 60;
        bool panorama = false;
        bool canSwitchModes = true;
        EquiCam equi;

        private readonly XRNode rNode = XRNode.RightHand;
        private readonly XRNode lNode = XRNode.LeftHand;

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            // Get locations
            Rhand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/");
            Lhand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L/");
            shoulderCamera = GameObject.Find("Shoulder Camera");

            // Makes camera object
            camera = GameObject.CreatePrimitive(PrimitiveType.Cube);
            camera.name = "Lualt's Camera";
            GameObject cameralens = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cameralens.transform.GetComponent<CapsuleCollider>().enabled = false;
            camera.transform.SetParent(shoulderCamera.transform, false);
            cameralens.transform.SetParent(camera.transform, false);
            camera.transform.GetComponent<BoxCollider>().enabled = false;
            camera.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            cameralens.transform.localScale = new Vector3(0.3f, 0.5f, 0.3f);
            cameralens.transform.localPosition = new Vector3(0f, 0f, 0.05f);
            cameralens.transform.localRotation = Quaternion.Euler(90f, 0.0f, 0.0f);

            shoulderCamera.transform.SetParent(Camera.main.transform);
            shoulderCamera.transform.localPosition = new Vector3(0.0f, 0.3f, -2.0f);
            shoulderCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            GameObject.Find("CM vcam1").SetActive(false);
            cameraComponent = shoulderCamera.GetComponent<Camera>();

            shoulderCamera.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
            equi = shoulderCamera.AddComponent<EquiCam>();
            equi.enabled = panorama;

            camera.GetComponent<MeshRenderer>().material = GameObject.Find("gorilla").GetComponent<SkinnedMeshRenderer>().material;
        }

        void FixedUpdate()
        {
            // Map buttons to variables
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out buttonB);
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out buttonA);
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out joyRC);
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerR);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out buttonY);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out buttonX);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joyL);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out joyLC);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerL);

            if (triggerL == 1f && triggerR == 1f && canLock)
            {
                locked = !locked;
                canLock = false;
                InputDevices.GetDeviceAtXRNode(lNode).SendHapticImpulse(0, 1f, 3f);
                InputDevices.GetDeviceAtXRNode(rNode).SendHapticImpulse(0, 1f, 3f);
            } else if (triggerL == 0f && triggerR == 0f && !canLock)
            {
                canLock = true;
                InputDevices.GetDeviceAtXRNode(lNode).SendHapticImpulse(0, 1f, 3f);
                InputDevices.GetDeviceAtXRNode(rNode).SendHapticImpulse(0, 1f, 3f);
            }

            if (!locked)
            {
                // Switch hands when B button is pressed
                if (buttonB && canSwitchHands)
                {
                    if (isRightHand)
                    {
                        shoulderCamera.transform.SetParent(Lhand.transform);
                        shoulderCamera.transform.localPosition = new Vector3(0.0f, 0.0f, 0.1f);
                        shoulderCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
                    }
                    else
                    {
                        shoulderCamera.transform.SetParent(Rhand.transform);
                        shoulderCamera.transform.localPosition = new Vector3(-0.1f, 0.0f, 0.0f);
                        shoulderCamera.transform.localRotation = Quaternion.Euler(0f, 270f, 90f);
                    }
                    camera.SetActive(true);
                    canSwitchHands = false;
                    isRightHand = !isRightHand;
                }
                else if (!canSwitchHands && !buttonB)
                {
                    canSwitchHands = true;
                }
                // Place camera down in the world
                else if (buttonA)
                {
                    shoulderCamera.transform.SetParent(null);
                    isRightHand = !isRightHand;
                    camera.SetActive(true);
                }
                // Make camera first person
                else if (buttonY)
                {
                    shoulderCamera.transform.SetParent(Camera.main.transform);
                    shoulderCamera.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    shoulderCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    camera.SetActive(false);
                    isRightHand = !isRightHand;
                }
                // Make camera third person
                else if (buttonX)
                {
                    shoulderCamera.transform.SetParent(Camera.main.transform);
                    shoulderCamera.transform.localPosition = new Vector3(0.0f, 0.3f, -2.0f);
                    shoulderCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    camera.SetActive(false);
                    isRightHand = !isRightHand;
                }


                // Toggle 360 panorama mode
                if (joyRC && canSwitchModes)
                {
                    panorama = !panorama;
                    equi.enabled = panorama;
                    canSwitchModes = false;
                }
                else if (!joyRC && !canSwitchModes)
                {
                    canSwitchModes = true;
                }

                // Resets FOV when left joystick is pressed
                if (joyLC)
                {
                    fov = 60;
                    cameraComponent.fieldOfView = fov;
                }
                // Decrease FOV
                else if (joyL.y > 0 && fov > 5)
                {
                    fov--;
                    cameraComponent.fieldOfView = fov;
                }
                // Increase FOV
                else if (joyL.y < 0 && fov < 160)
                {
                    fov++;
                    cameraComponent.fieldOfView = fov;
                }
            }
        }
    }
}