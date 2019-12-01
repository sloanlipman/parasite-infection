using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class Ability : MonoBehaviour {

    public string abilityName;
    public int power;
    public int energyCost;
    public enum AbilityType { Attack, Heal };
    public AbilityType abilityType;

    private Vector3 targetPosition;

    private void Update() {
      if (targetPosition != Vector3.zero) {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, .15f);

        if (Vector3.Distance(transform.position, targetPosition) < 0.25f) {
          Destroy(this.gameObject, 1);
        }
      } else {
          Destroy(this.gameObject);
      }
    }

    public void Cast(BattleCharacter caster, BattleCharacter target, int power) {
      targetPosition = target.transform.position;
      if (abilityType == AbilityType.Attack) {
        int damage = power + caster.level; // Experimental
        target.Hurt(damage);
      } else if (abilityType == AbilityType.Heal) {
        target.Heal(power);
      }
    }
  }
}