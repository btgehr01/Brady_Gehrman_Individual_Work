using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private Text scoreLabel;
    private int _score;
    void Start()
    {
        _score = 0;
        scoreLabel.text = _score.ToString();
        settingsPopup.Close();
    }

    public void OnOpenSettings()
    {
        settingsPopup.Open();
    }
}