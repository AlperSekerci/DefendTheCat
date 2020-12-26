using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        CheckMouseClick();
    }

    private void CheckMouseClick()
    {
        if (!Input.GetMouseButtonDown((int)MouseButton.LeftMouse)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject obj = hit.collider.gameObject;
            // Debug.Log("hit " + obj.name);
            if (hit.collider.CompareTag("BlochSphere"))
            {
                obj.GetComponent<BlochSphere>().OnClick();
            }
        }
    }
}