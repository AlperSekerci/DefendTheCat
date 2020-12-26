using UnityEngine;
using UnityEngine.UI;

public class Warrior : MonoBehaviour
{
    public float maxHealth = 100;
    public float HealthPerc => HP / maxHealth;
    public AnimatedBar hpBar;
    private float targetPosX;
    public float moveSmoothness = 15;
    private bool updatedPosOnce = false;
    public float dps = 100;
    public bool Dead { get; private set; } = false;

    private float _hp = 0;
    public float HP
    {
        get { return _hp; }
        set
        {
            if (Dead) return;
            _hp = Mathf.Clamp(value, 0, maxHealth);
            hpBar.Value = HealthPerc;
            if (value <= 0)
            {
                Die();                
            }
        }
    }

    public enum Element
    {
        Fire,
        Water,
        Count
    }
    public Element element;

    public Sprite[] tokens;
    public Image image;

    public void SetElement(Element element)
    {
        this.element = element;
        if (tokens.Length > 0) image.sprite = tokens[(int)element];
    }

    private void LateUpdate()
    {
        if (moveSmoothness <= 0) return;
        float posX = Mathf.Lerp(transform.position.x, targetPosX, Time.deltaTime * moveSmoothness);
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    private void Die()
    {
        Dead = true;
        Debug.Log(name + " died");
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        targetPosX = targetPos.x;

        if (!updatedPosOnce)
        {
            transform.position = targetPos;
            updatedPosOnce = true;
            return;
        }

        transform.position = new Vector3(transform.position.x, targetPos.y, targetPos.z);        
    }
}