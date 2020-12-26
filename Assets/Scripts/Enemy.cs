using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int road;
    
    public enum Element
    {
        Fire,
        Water,
        Count
    }
    public Element element;

    public override string ToString()
    {
        return "Road: " + (road + 1) + ", Element: " + element;
    }
}