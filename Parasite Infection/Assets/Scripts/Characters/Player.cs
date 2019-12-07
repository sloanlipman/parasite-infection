using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character  {
  public Player() {}

  private void Update() {
    this.Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))); 
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.GetComponent<NPC>() != null) {
      collision.GetComponent<NPC>().Interact(this);
    } else if (collision.GetComponent<FinalBossTrigger>() != null) {
      collision.GetComponent<FinalBossTrigger>().Interact();
    }
  }
}
