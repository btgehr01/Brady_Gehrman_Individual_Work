using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class FinishDisplayCtl : MonoBehaviour
{
  public TextMeshProUGUI text_element;
  public float timescale_cutoff = 0f;

  private float timescale_t_ = 0f;

  public void show_display()
  {
    text_element.text = "FINISH!";
    timescale_t_ = 0f;
    gameObject.SetActive(true);

    StartCoroutine("show_display_coroutine_");
  }

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }

  private IEnumerator show_display_coroutine_()
  {
    while (Time.timeScale > timescale_cutoff)
    {
      Time.timeScale = get_timescale();
      yield return null;
    }

    text_element.text = "Press any key to return to main menu...";
    yield return new WaitUntil(() => Input.anyKeyDown);
    Time.timeScale = 1f;
    SceneManager.LoadScene("Assets/Scenes/MainMenuScene.unity", LoadSceneMode.Single);
  }

  private float get_timescale()
  {
    float timescale = 1f;

    timescale_t_ += Time.unscaledDeltaTime;

    if (timescale_t_ == 0)
    {
      return 1f;
    }

    timescale = Math.Clamp((float)(0.1 / (timescale_t_ * 0.2 + 0.1) - 0.16), 0f, 1f);

    return timescale;
  }
}
