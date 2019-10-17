using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class PartyMember : BattleCharacter {
    PartyMemberEntry partyMemberEntry;
    public PartyMember() {}

    public PartyMember SetupPartyMember(PartyMemberEntry partyMember) {
      this.characterName = partyMember.characterName;
      this.health = partyMember.stats["health"];
      this.maxHealth = partyMember.stats["maxHealth"];
      this.attackPower = partyMember.stats["attackPower"];
      this.defensePower = partyMember.stats["defensePower"];
      this.energyPoints = partyMember.stats["energyPoints"];
      this.maxEnergyPoints = partyMember.stats["maxEnergyPoints"];
      this.speed = partyMember.stats["speed"];
      this.abilities = partyMember.abilities;
      if (partyMember.sprite != null) {
        this.sprite.sprite = partyMember.sprite;
      }
      this.equipment = partyMember.equipment;
      this.level = partyMember.level;
      this.experience = partyMember.level;
      return this;
    }


    public override void Die() {
      base.Die();
      BattleController.Instance.characters[0].Remove(this);
    }
  }
}