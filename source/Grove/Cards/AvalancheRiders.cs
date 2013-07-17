﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class AvalancheRiders : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Avalanche Riders")
        .ManaCost("{3}{R}")
        .Type("Creature Human Nomad")
        .Text(
          "{Haste}, {Echo} {3}{R}(At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}When Avalanche Riders enters the battlefield, destroy target land.")
        .Power(2)
        .Toughness(2)
        .Echo("{3}{R}")
        .SimpleAbilities(Static.Haste)        
        .TriggeredAbility(p =>
          {
            p.Text = "When Avalanche Riders enters the battlefield, destroy target land.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());
            p.TargetingRule(new Destroy());
          });
    }
  }
}