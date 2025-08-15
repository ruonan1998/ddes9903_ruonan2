using UnityEngine;

public class ClickHighlightAndShowKey : MonoBehaviour
{
    public GameObject key;
    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private Renderer rend;
    private bool isHighlighted = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
    }

    void OnMouseDown()
    {
        if (rend != null)
        {
            isHighlighted = !isHighlighted;
            rend.material.color = isHighlighted ? highlightColor : originalColor;
        }

        if (key != null)
        {
            key.SetActive(true);
        }
    }
}