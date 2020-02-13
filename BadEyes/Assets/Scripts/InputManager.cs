using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class InputManager : MonoBehaviour
{
    public PostProcessProfile postProcessingProfile;
    DepthOfField depthOfField;

    [System.Serializable]
    public class Myopia
    {
        [HideInInspector]
        public const float minFocalLength = 12.0f;
        [HideInInspector]
        public const float maxFocalLength = 36.0f;
        [HideInInspector]
        public float startFocusDistance = 0.3f;

        public float gapOfDepthOfField = 0.2f;
        [Range(minFocalLength, maxFocalLength)]
        public float startFocalLength = 12.0f;
    }

    [System.Serializable]
    public class Hypermetropia
    {
        public float gapOfDepthOfField = 0.2f;
        public float minFocalLength = 70.0f;
        public float maxFocalLength = 36.0f;
        public float startFocalLength = 70.0f;
        public float startFocusDistance = 10.0f;
    }

    public Myopia myopia = new Myopia();
    public Hypermetropia hypermetropia = new Hypermetropia();

    private void Start()
    {
        depthOfField = postProcessingProfile.GetSetting<DepthOfField>();

        depthOfField.focalLength.value = myopia.startFocalLength;
        depthOfField.focusDistance.value = myopia.startFocusDistance;
    }

    private void Update()
    {
        //Augmentation de la valeur de la longueur de la focal
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            Debug.Log("B button pressed");
            if (depthOfField.focalLength.value <= Myopia.maxFocalLength)
            {
                depthOfField.focalLength.value += myopia.gapOfDepthOfField;
                
            }
        }

        //Diminution de la valeur de la longueur de la focal
        if (OVRInput.Get(OVRInput.Button.One))
        {
            Debug.Log("A button pressed");
            if(depthOfField.focalLength.value >= Myopia.minFocalLength)
            {
                depthOfField.focalLength.value -= myopia.gapOfDepthOfField;
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp))
        {
            Debug.Log("SecondaryThumbstickUp button pressed");

        }
    }
}
