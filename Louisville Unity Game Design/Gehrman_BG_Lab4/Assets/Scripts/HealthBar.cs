using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        image.fillAmount *= 0.999f;
    }
}
