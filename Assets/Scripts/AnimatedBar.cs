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
    
    void Start()
    {
        mainMaxWidth = main.sizeDelta.x;       
    }
    
    void Update()
    {
        float width = value * mainMaxWidth;
        main.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);   
    }
}