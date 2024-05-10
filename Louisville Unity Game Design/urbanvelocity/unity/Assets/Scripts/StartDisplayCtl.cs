using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class StartDisplayCtl : MonoBehaviour
{
  public byte count = 3;
  public byte fadeout_duration = 2;
  public TextMeshProUGUI text_element;
  public UnityEvent event_countdown_finished;

  public void start_countdown(byte count)
  {
    this.count = count;
    gameObject.SetActive(true);
    StartCoroutine("countdown_couroutine_");
  }

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
  }

  private IEnumerator countdown_couroutine_()
  {
    while (count > 0)
    {
      text_element.text = count.ToString();
      yield return new WaitForSeconds(1);
      count--;
    }

    // provide a way to signal to the race manager that it's ok to actually
    // start the race
    event_countdown_finished.Invoke();

    text_element.text = "GO!";
    yield return new WaitForSeconds(fadeout_duration);
    gameObject.SetActive(false);
  }
}
