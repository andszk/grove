﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Preventions;
  using Core.Triggers;
  using Core.Zones;

  public class EnergyField : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Energy Field")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "Prevent all damage that would be dealt to you by sources you don't control.{EOL}When a card is put into your graveyard from anywhere, sacrifice Energy Field.")
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddDamagePrevention(new PreventDamage(
              sourceFilter: (pr, source) => pr.Controller != source.Controller));

            p.PlayerFilter = (player, e) => player == e.Source.Controller;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When a card is put into your graveyard from anywhere, sacrifice Energy Field.";
            p.Trigger(new OnZoneChanged(
              to: Zone.Graveyard,
              filter: (ability, card) => ability.OwningCard.Controller == card.Owner));
            p.Effect = () => new SacrificeSource();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}