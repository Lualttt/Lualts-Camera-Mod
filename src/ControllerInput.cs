using UnityEngine;
using UnityEngine.XR;

namespace LualtsCameraMod {
    public class ControllerInput
    {
        private static readonly XRNode LeftNode = XRNode.LeftHand;
        public static bool LeftPrimaryButton;
        public static bool LeftSecondaryButton;
        public static bool LeftTriggerButton;
        public static bool LeftGripButton;
        public static bool Left2DAxisButton;
        public static Vector2 LeftPrimary2DAxis;
        
        private static readonly XRNode RightNode = XRNode.RightHand;
        public static bool RightPrimaryButton;
        public static bool RightSecondaryButton;
        public static bool RightTriggerButton;
        public static bool RightGripButton;
        public static bool Right2DAxisButton;
        public static Vector2 RightPrimary2DAxis;
        
        public static void UpdateInput()
        {
            InputDevices.GetDeviceAtXRNode(LeftNode).TryGetFeatureValue(CommonUsages.primaryButton, out LeftPrimaryButton);
            InputDevices.GetDeviceAtXRNode(LeftNode).TryGetFeatureValue(CommonUsages.secondaryButton, out LeftSecondaryButton);
            InputDevices.GetDeviceAtXRNode(LeftNode).TryGetFeatureValue(CommonUsages.triggerButton, out LeftTriggerButton);
            InputDevices.GetDeviceAtXRNode(LeftNode).TryGetFeatureValue(CommonUsages.gripButton, out LeftGripButton);
            InputDevices.GetDeviceAtXRNode(LeftNode).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out Left2DAxisButton);
            InputDevices.GetDeviceAtXRNode(LeftNode).TryGetFeatureValue(CommonUsages.primary2DAxis, out LeftPrimary2DAxis);
            
            InputDevices.GetDeviceAtXRNode(RightNode).TryGetFeatureValue(CommonUsages.primaryButton, out RightPrimaryButton);
            InputDevices.GetDeviceAtXRNode(RightNode).TryGetFeatureValue(CommonUsages.secondaryButton, out RightSecondaryButton);
            InputDevices.GetDeviceAtXRNode(RightNode).TryGetFeatureValue(CommonUsages.triggerButton, out RightTriggerButton);
            InputDevices.GetDeviceAtXRNode(RightNode).TryGetFeatureValue(CommonUsages.gripButton, out RightGripButton);
            InputDevices.GetDeviceAtXRNode(RightNode).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out Right2DAxisButton);
            InputDevices.GetDeviceAtXRNode(RightNode).TryGetFeatureValue(CommonUsages.primary2DAxis, out RightPrimary2DAxis);
        }
    }
}