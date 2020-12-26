using UnityEngine;

public class AnimatedBar : MonoBehaviour
{
    private float value = 1;
    public float Value
    {
        get { return value; }
        set
        {
            if (value < 0)
            {
                this.value = 0;
            }
            else if (value > 1)
            {
                this.value = 1;
            }
            else
            {
                this.value = value;
            }
        }
    }

    private float mainMaxWidth;   
    public RectTransform main;
    public RectTransform shadow;
    public float shadowInterp = 3f;    
    private bool started = false;

    void Start()
    {
        if (started) return;
        started = true;
        mainMaxWidth = main.sizeDelta.x;       
    }
    
    void Update()
    {
        float mainWidth = main.sizeDelta.x;
        float mainPerc = mainWidth / mainMaxWidth;       

        float width = value * mainMaxWidth;
        
        if (value <= mainPerc)
        {
            main.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            float shadowWidth = shadow.sizeDelta.x;
            float lerpedWidth = Mathf.Lerp(shadowWidth, width, Time.deltaTime * shadowInterp);
            shadow.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lerpedWidth);
        }
        else
        {   
            shadow.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            float shadowWidth = main.sizeDelta.x;
            float lerpedWidth = Mathf.Lerp(shadowWidth, width, Time.deltaTime * shadowInterp);
            main.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lerpedWidth);
        }        
    }
}