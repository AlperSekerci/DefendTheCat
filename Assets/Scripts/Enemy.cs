using UnityEngine;

public class Enemy : Warrior
{
    public int road;
    public GameObject canvas;

    public override string ToString()
    {
        return "Road: " + (road + 1) + ", Element: " + element;
    }

    public void MakeVisible()
    {
        canvas.SetActive(true);
    }
}