using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class InputManager : MonoBehaviour
{
    public PostProcessProfile postProcessingProfile;
    public DepthOfField depthOfField;
    private List<BadView> ListOfViews;
    private int currentIndexView;

    [SerializeField]
    private GameObject bothCanvas;
    private Canvas rightCanvas;
    private Canvas leftCanvas;
    private Coroutine m_coroutine;

    [System.Serializable]
    public class BadView
    {
        public string m_ViewName;
        public float m_minFocalLength;
        public float m_maxFocalLength;
        public float m_focusDistance;

        public float m_gapOfDepthOfField;

        public BadView(string ViewName, float minFocalLength, float maxFocalLength,float focusDistance, float gapOfDepthOfField)
        {
            m_ViewName = ViewName;
            m_minFocalLength = minFocalLength;
            m_maxFocalLength = maxFocalLength;
            m_focusDistance = focusDistance;
            m_gapOfDepthOfField = gapOfDepthOfField;
        }
    }

    [System.Serializable]
    public class Myopia : BadView
    {
        public Myopia() : base("Myopie", 25.0f, 80.0f, 1.68f, 0.3f) {}
    }

    [System.Serializable]
    public class Hypermetropia : BadView
    {
        public Hypermetropia() : base("Hypermetropie", 70.0f, 300.0f, 50f, 1.0f) {}
    }

    
    public Hypermetropia hypermetropia;
    public Myopia myopia;
    //To do
    //public None none;

    private void Start()
    {

        //Setup du canvas de changement de vue
        rightCanvas = bothCanvas.GetComponentsInChildren<Canvas>()[0];
        leftCanvas = bothCanvas.GetComponentsInChildren<Canvas>()[1];

        //Setup des differents problèmes de vue
        depthOfField = postProcessingProfile.GetSetting<DepthOfField>();

        currentIndexView = 0;
        ListOfViews = new List<BadView>();

        myopia = new Myopia();
        ListOfViews.Add(myopia);
        hypermetropia = new Hypermetropia();
        ListOfViews.Add(hypermetropia);


        if (ListOfViews.Count != 0)
        {
            SwitchView();
        }
        else
        {
            Debug.Log("Error : there is no view (yenapa)");
        }
    }

    private void Update()
    {
        //Augmentation de la valeur de la longueur de la focal
        if (OVRInput.Get(OVRInput.Button.Two))
        {
            Debug.Log("B button pressed");
            if (depthOfField.focalLength.value <= ListOfViews[currentIndexView].m_maxFocalLength)
            {
                depthOfField.focalLength.value += ListOfViews[currentIndexView].m_gapOfDepthOfField;
            }
        }

        //Diminution de la valeur de la longueur de la focal
        if (OVRInput.Get(OVRInput.Button.One))
        {
            Debug.Log("A button pressed");
            if(depthOfField.focalLength.value >= ListOfViews[currentIndexView].m_minFocalLength)
            {
                depthOfField.focalLength.value -= ListOfViews[currentIndexView].m_gapOfDepthOfField;
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickUp))
        {
            Debug.Log("SecondaryThumbstickUp button pressed");
            ++currentIndexView;
            if (currentIndexView >= ListOfViews.Count)
            {
                currentIndexView = 0;
            }
            SwitchView();
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickDown))
        {
            Debug.Log("SecondaryThumbstickDown button pressed");
            --currentIndexView;
            if (currentIndexView<0)
            {
                currentIndexView = ListOfViews.Count - 1;
            }
            SwitchView();
        }


    }


    public void SwitchView()
    {
        Debug.Log("Switching view to : " + ListOfViews[currentIndexView].m_ViewName);
        depthOfField.focalLength.value = ListOfViews[currentIndexView].m_minFocalLength;
        depthOfField.focusDistance.value = ListOfViews[currentIndexView].m_focusDistance;

        //Changing the view
        rightCanvas.GetComponentInChildren<TextMeshProUGUI>().text = ListOfViews[currentIndexView].m_ViewName;
        leftCanvas.GetComponentInChildren<TextMeshProUGUI>().text = ListOfViews[currentIndexView].m_ViewName;

        //Display for a certain time canvas
        if(m_coroutine!=null) StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine("DisplayChangingView");
    }


    IEnumerator DisplayChangingView()
    {
        bothCanvas.SetActive(true);
        yield return new WaitForSeconds(5);
        bothCanvas.SetActive(false);
    }
}
