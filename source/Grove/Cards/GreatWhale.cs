﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

  public class GreatWhale : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Great Whale")
        .ManaCost("{5}{U}{U}")
        .Type("Creature Whale")
        .Text("When Great Whale enters the battlefield, untap up to seven lands.")
        .FlavorText("'As a great whale dies, it flips onto its back. And so an island is born.'{EOL}—Mariners' legend")
        .Power(5)
        .Toughness(5)
        .TriggeredAbility(p =>
          {
            p.Text = "When Great Whale enters the battlefield, untap up to seven lands.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new UntapSelectedPermanents(
              minCount: 0,
              maxCount: 7,
              validator: c => c.Is().Land,
              text: "Select lands to untap."
              );
          }
        );
    }
  }
}