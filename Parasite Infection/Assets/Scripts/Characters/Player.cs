using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character  {
  public Player() {}
  public void Save() {
    ES3.Save<GameObject>("Player", this.gameObject, "PlayerInfo.json");
  }

  public void Load() {
      ES3.Load<GameObject>("Player", "PlayerInfo.json");
  }

  private void Update() {
    this.Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))); 
  }

  private void OnTriggerStay2D(Collider2D collision) {
    if (collision.GetComponent<NPC>() != null) {
      collision.GetComponent<NPC>().Interact(this);
    }
  }
}
