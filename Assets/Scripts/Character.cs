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
  void Start() {

    body = GetComponent<Rigidbody2D>();
    sprite = GetComponent<SpriteRenderer>();
  }

  public void Move (Vector2 inputVector) {
    if (inputVector != Vector2.zero) {
      sprite.flipX = inputVector.normalized.x < 0;
    }

    inputVector = inputVector.normalized * movementSpeed;
    body.velocity = inputVector;
  }

  public void TeleportTo(Vector2 targetPosition) {
    transform.position = targetPosition;
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
