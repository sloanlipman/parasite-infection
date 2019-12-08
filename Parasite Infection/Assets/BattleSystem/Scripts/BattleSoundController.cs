using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSoundController : MonoBehaviour {

  private AudioSource source;
  [SerializeField] private AudioClip playerAttack;
  [SerializeField] private AudioClip enemyAttack;
  [SerializeField] private AudioClip defend;
  [SerializeField] private AudioClip item;
  [SerializeField] private AudioClip barrage;
  [SerializeField] private AudioClip fireball;
  [SerializeField] private AudioClip hydroblast;
  [SerializeField] private AudioClip heal;
  [SerializeField] private AudioClip victory;
  [SerializeField] private AudioClip defeat;

  private void Awake() {
    source = GetComponent<AudioSource>();
  }

  public void PlaySound(string sound) {

    switch (sound) {
      case "playerAttack": {
        source.clip = playerAttack;
        break;
      }

      case "enemyAttack": {
        source.clip = enemyAttack;
        break;
      }

      case "defend": {
        source.clip = defend;
        break;
      }

      case "item": {
        source.clip = item;
        break;
      }

      case "barrage": {
        source.clip = barrage;
        break;
      }

      case "fireball": {
        source.clip = fireball;
        break;
      }

      case "hydroblast": {
        source.clip = hydroblast;
        break;
      }

      case "heal": {
        source.clip = heal;
        break;
      }
      
      case "victory": {
        source.clip = victory;
        break;
      }

      case "defeat": {
        source.clip = defeat;
        break;
      }
    }
    source.Play();
  }
}
