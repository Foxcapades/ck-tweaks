using System;
using UnityEngine;

namespace FXCPDS.Minion.Lifespan {
  public class LifespanMenuOption : RadicalPauseMenuOption {
    private const string labelKey = "Label";
    private const string secondsKey = "Seconds";

    public const string InfiniteKey = "Infinite";

    public override void OnParentMenuActivation() {
      base.OnParentMenuActivation();

      labelText.Render(ExtendedMinionLifespan.ModI8NPrefix + labelKey);
      updateText(ExtendedMinionLifespan.Lifespan);
    }

    public override void OnActivated() {
      base.OnActivated();
      OnSkimRight();
    }

    public override bool OnSkimRight() {
      update(nextLifespan());
      return true;
    }

    public override bool OnSkimLeft() {
      update(prevLifespan());
      return true;
    }

    public static void AppendTo(RadicalMenu menu, GameObject prefab) {
      var root = menu.transform.Find("Options");
      var scroll = root.GetChild(0);

      var instance = Instantiate(prefab, scroll);
      scroll.hasChanged = false;

      var component = instance.GetComponent<LifespanMenuOption>();
      component.parentMenu = menu;
      menu.menuOptions.Add(component);
    }


    private short nextLifespan() => ExtendedMinionLifespan.Lifespan switch {
      60  => 90,
      90  => 120,
      120 => 300,
      300 => -1,
      -1  => 60,
      _   => 120,
    };

    private short prevLifespan() => ExtendedMinionLifespan.Lifespan switch {
      60  => -1,
      90  => 60,
      120 => 90,
      300 => 120,
      -1  => 300,
      _   => 120,
    };

    private void update(short lifespan) {
      ExtendedMinionLifespan.Lifespan = lifespan;
      updateText(lifespan);
    }

    private void updateText(short lifespan) {
      if (lifespan == -1) {
        valueText.formatFields = Array.Empty<string>();
        valueText.Render(ExtendedMinionLifespan.ModI8NPrefix + InfiniteKey);
      } else {
        valueText.formatFields = new[] { lifespan.ToString() };
        valueText.Render(ExtendedMinionLifespan.ModI8NPrefix + secondsKey);
      }
    }
  }
}