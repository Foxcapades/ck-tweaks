using System.Collections.Generic;
using UnityEngine;

#nullable enable
namespace FXCPDS.Content.MoreEquipment {
  internal class ExtendedTab: CharacterWindowTab {

    private const float TabContainerOffset = 3.9f;

    /// <summary>
    /// Tab index.
    /// </summary>
    public int index;

    private void Awake() {
      onClick.AddListener(() => Manager.ui.characterWindow.SetActivePreset(index));
      adjustTabPositions();
    }

    private void Start() {
      var sibling = gameObject.transform
        .parent // custom prefab root
        .parent // PresetTabs game object
        .GetChild(0)! // preset0Tab game object
        .GetComponent<CharacterWindowTab>()!;

      inactiveColor = sibling.inactiveColor;
      icon.material = sibling.icon.material;
      SetActive(false);
    }

    /// <summary>
    /// Appends the new equipment tabs prefab container to the character window
    /// UI component.
    /// </summary>
    /// <param name="charUI">Character window UI component instance.</param>
    /// <param name="prefab">Mod equipment tabs container prefab.</param>
    internal static void appendTabs(CharacterWindowUI charUI, GameObject prefab) {
      var root = charUI.equipmentInventory.transform.Find("PresetTabs");

      var instance = Instantiate(prefab, root);

      var tab2 = root.GetChild(1).GetComponent<CharacterWindowTab>();
      var tab3 = root.GetChild(2).GetComponent<CharacterWindowTab>();

      var tab4 = instance.transform.GetChild(0).GetComponent<ExtendedTab>();
      var tab5 = instance.transform.GetChild(1).GetComponent<ExtendedTab>();

      tab4.topUIElements = tab2.bottomUIElements; // above tab 4 is tab 3 (which is below tab 2)
      tab4.leftUIElements = tab2.leftUIElements;
      tab4.rightUIElements = tab2.rightUIElements;
      // tab4 bottom is set in the prefab

      // tab5.top is set in the prefab
      tab5.leftUIElements = tab2.leftUIElements;
      tab5.rightUIElements = tab2.rightUIElements;
      tab5.bottomUIElements = tab3.bottomUIElements; // the inventory ui stuff is now below tab 5

      tab3.bottomUIElements = new List<UIelement> { tab4 }; // tab 4 is now below tab 3

      charUI.presetTabs.Add(tab4);
      charUI.presetTabs.Add(tab5);
    }

    /// <summary>
    /// Adjusts the base game tab container position to allow space for the
    /// additional equipment tabs to fit below the 3 standard tabs.
    /// </summary>
    private static void adjustTabPositions() {
      var container = Manager.ui
        .characterWindow
        .equipmentInventory
        .transform
        .Find("PresetTabs")!;

      container.transform.localPosition = new(container.transform.localPosition.x, TabContainerOffset, 0f);
    }
  }
}