using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Waypoint : MonoBehaviour
{
  public UnityEvent<Waypoint, Collider> col_event;

  private void OnTriggerEnter(Collider col)
  {
    col_event.Invoke(this, col);
  }
}
