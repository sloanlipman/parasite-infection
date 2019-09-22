// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class NPC : Character {
 
//   private Vector2[] movementDirections = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
//   private Vector2 spawnPosition;
//   [SerializeField]
//   private bool wander;

// //   [SerializeField]
// //   private DialogData dialogData;

// //   [SerializeField]
// //   private Dialog dialog;
//   // Start is called before the first frame update
//   void Start() {
//     spawnPosition = transform.position;
//     if (wander) {
//       Wander();    
//     }
//   }

// //   public void StartDialog() {
// //     dialog.StartDialog(dialogData.dialog);
// //   }

//   public void Wander() {
//     Vector2 currentPosition = transform.position;
//     if (currentPosition == spawnPosition) {
//       int roll = Random.Range(0,3);
//       Vector2 destination = currentPosition + movementDirections[roll];
//       StartCoroutine(MoveTo(destination, Wander, Random.Range(2,5)));
//     } else {
//       StartCoroutine(MoveTo(spawnPosition, Wander, Random.Range(2,5)));
//     }
//   }
// }
