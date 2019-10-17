using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem {
  public class BattleCharacterEntry {
    public int id;
    public string characterName;
    public List<Ability> abilities;
    public Dictionary<string, int> stats;
    public Sprite sprite;

    public int level;
    public int experience;
    public List<Item> equipment;

        
    protected void LoadSprite() {
        // this.sprite = Resources.Load<Sprite>("Characters/" + characterName);
        this.sprite = Resources.Load<Sprite>("Characters/CharacterSheet_0");
    }

  }

}