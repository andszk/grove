﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Preventions;

  public class Lull : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Lull")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Text(
          "Prevent all combat damage that would be dealt this turn.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPlayer(
              selector: e => e.Controller,
              modifiers: () =>
                {
                  var cp = new ContinuousEffectParameters
                    {
                      CardFilter = (card, self) => card.Is().Creature,
                      Modifier = () => new AddDamagePrevention(new PreventCombatDamage())
                    };

                  return new AddContiniousEffect(new ContinuousEffect(cp)) {UntilEot = true};
                });

            p.TimingRule(new Turn(passive: true));
            p.TimingRule(new Steps(Step.DeclareBlockers));
          });
    }
  }
}