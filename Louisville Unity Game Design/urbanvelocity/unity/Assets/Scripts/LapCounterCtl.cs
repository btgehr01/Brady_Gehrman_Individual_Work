using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapCounterCtl : MonoBehaviour
{
  public RaceManager race_mgr;

  private TextMeshProUGUI text_element_;
  private uint count_progress_ = 0;
  private uint count_total_ = 0;


  public void set_count_progress(uint count)
  {
    count_progress_ = count;
    update_display_();
  }

  public void set_count_total(uint count)
  {
    count_total_ = count;
    update_display_();
  }

  // Start is called before the first frame update
  void Start()
  {
    text_element_ = GetComponent<TextMeshProUGUI>();
    set_count_total(race_mgr.laps);
    race_mgr.event_lap_completed.AddListener(set_count_progress);
  }

  // Update is called once per frame
  void Update()
  {
  }

  private void update_display_()
  {
    text_element_.text = "Lap: " + count_progress_ + "/" + count_total_;
  }
}
