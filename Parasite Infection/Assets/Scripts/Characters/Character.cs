﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour {
  // Start is called before the first frame update

  private Rigidbody2D body;
  [SerializeField]
  private float movementSpeed;
  private SpriteRenderer sprite;
  protected bool colliding;
  protected Animator animator;
  public bool IsMoving;
  void Awake() {
    body = GetComponent<Rigidbody2D>();
    sprite = GetComponent<SpriteRenderer>();
  }

  public void SetSprite(Sprite sprite) {
    this.sprite.sprite = sprite;
  }

  public void Move (Vector2 inputVector) {
    if (Time.timeScale != 0 && inputVector != Vector2.zero) {
      IsMoving = true;
      sprite.flipX = inputVector.normalized.x < 0;
    } else {
      IsMoving = false;
    }

    inputVector = inputVector.normalized * movementSpeed;
    body.velocity = inputVector;
  }

  public Rigidbody2D GetRigidbody() {
    return body;
  }

  public void TeleportTo(Vector2 targetPosition) {
    transform.position = targetPosition;
    FindObjectOfType<CraftingInventory>().Save();
    FindObjectOfType<ConsumableInventory>().Save();
  }

  public IEnumerator MoveTo(Vector2 targetPosition, System.Action callback, float delay = 0f) {
    while (targetPosition != new Vector2(transform.position.x, transform.position.y)) {
      transform.position = Vector2.MoveTowards(transform.position, targetPosition, 1f * Time.deltaTime);
      yield return null;
    }
    yield return new WaitForSeconds(delay);
    callback();
  }
}
