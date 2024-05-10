using UnityEngine;
using UnityEngine.UI;
public class SettingsPopup : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void OnSubmitName(string name)
    {
        print(name);
    }
    public void OnSpeedValue(float speed)
    {
        print(speed.ToString());
    }
}
