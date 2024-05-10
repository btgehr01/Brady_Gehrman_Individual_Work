using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RaceManager : MonoBehaviour
{
  public List<Waypoint> waypoints;
  public List<Rigidbody> player_rb;
  public List<Collider> collider_whitelist;
  public UnityEvent<double> event_race_start;
  public UnityEvent<double, GameObject> event_race_end;
  public UnityEvent<byte> event_countdown_start;
  public UnityEvent<uint> event_lap_completed;
  public uint laps = 1;
  public bool auto_start = false;
  public bool first_is_last = true;
  public byte countdown_duration = 3;
  public byte start_delay = 3;

  private bool race_active_ = false;
  private Dictionary<int, uint> lap_table_ = new Dictionary<int, uint>();
  private ushort current_wp_index_ = 0;
  private bool countdown_finished_ = false;

  public void start_race()
  {
    StartCoroutine("start_race_coroutine_");
  }

  /*
   * As of now there's probably not going to be a race "winner" as we're probably only ever going
   * to have a timed-trial game mode, however doesn't hurt to go ahead and plumb it in just in case
   * we need it
   */
  public void end_race(GameObject winner)
  {
    Debug.Log("Race finished!");
    race_active_ = false;
    event_race_end.Invoke(Time.timeAsDouble, winner);
  }

  public void Start()
  {
    foreach (Waypoint wp in waypoints)
    {
      wp.col_event.AddListener(col_cb_);
    }

    if (auto_start)
    {
      start_race();
    }
  }

  public void Update()
  {

  }

  public void countdown_finished_cb()
  {
    countdown_finished_ = true;
  }

  private void col_cb_(Waypoint wp, Collider col)
  {
    bool collider_ok = false;
    ushort wp_index = 0;
    ushort last_wp_index = 0;
    bool lap_complete = false;

    if (!race_active_)
    {
      return;
    }

    foreach (Collider wl_col in collider_whitelist)
    {
      if (wl_col == col)
      {
        collider_ok = true;
      }
    }

    if (!collider_ok)
    {
      return;
    }

    Debug.Log("Got collision from: " + wp.gameObject.name);
    Debug.Log("Collided with: " + col.gameObject.name);

    for (wp_index = 0; wp_index < waypoints.Count; wp_index++)
    {
      if (wp == waypoints[wp_index])
      {
        break;
      }
    }

    Debug.Log("current waypoint: " + current_wp_index_);

    if (wp_index <= current_wp_index_ + 1)
    {
      last_wp_index = current_wp_index_;
      current_wp_index_ = wp_index;

      Debug.Log("waypoint updated: " + current_wp_index_);

      if (first_is_last)
      {
        lap_complete = current_wp_index_ == 0 && last_wp_index == waypoints.Count - 1;
      }
      else
      {
        lap_complete = current_wp_index_ == waypoints.Count - 1;
      }

      if (lap_complete)
      {
        lap_complete = false;

        try
        {
          lap_table_[col.gameObject.GetInstanceID()] += 1;
        }
        catch (KeyNotFoundException)
        {
          lap_table_.Add(col.gameObject.GetInstanceID(), 1);
        }

        Debug.Log("Laps completed by " + col.gameObject.name + ": " + lap_table_[col.gameObject.GetInstanceID()]);

        event_lap_completed.Invoke(lap_table_[col.gameObject.GetInstanceID()]);

        if (lap_table_[col.gameObject.GetInstanceID()] >= laps)
        {
          end_race(col.gameObject);
        }
      }

    }
  }

  private IEnumerator start_race_coroutine_()
  {
    Debug.Log("Starting race...");

    countdown_finished_ = false;

    Debug.Log("Freezing players...");
    foreach (Rigidbody rb in player_rb)
    {
      rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    yield return new WaitForSeconds(start_delay);

    Debug.Log("Starting countdown...");
    event_countdown_start.Invoke(countdown_duration);
    yield return new WaitUntil(() => countdown_finished_);
    Debug.Log("Countdown finished!");

    Debug.Log("Un-freezing players...");
    foreach (Rigidbody rb in player_rb)
    {
      rb.constraints = RigidbodyConstraints.None;
    }

    Debug.Log("Race started!");
    race_active_ = true;
    event_race_start.Invoke(Time.timeAsDouble); //TODO: plumb in timestamp arg
  }

}

