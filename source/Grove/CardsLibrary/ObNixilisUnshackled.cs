﻿namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ObNixilisUnshackled : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ob Nixilis, Unshackled")
        .ManaCost("{4}{B}{B}")
        .Type("Legendary Creature — Demon")
        .Text("{Flying}, {trample}{EOL}Whenever an opponent searches his or her library, that player sacrifices a creature and loses 10 life.{EOL}Whenever another creature dies, put a +1/+1 counter on Ob Nixilis, Unshackled.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Flying, Static.Trample)
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever an opponent searches his or her library, that player sacrifices a creature and loses 10 life.";
          p.Trigger(new OnPlayerSearchesLibrary(filter: (ability, player) => ability.OwningCard.Controller != player));
          p.Effect = () => new CompoundEffect(
            new PlayersSacrificePermanents(
              count: 1,
              validator: c => c.Is().Creature,
              text: "Select a creature to sacrifice."),
            new ChangeLife(amount: -10, forOpponent: true));
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever another creature dies, put a +1/+1 counter on Ob Nixilis, Unshackled.";

          p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard, 
            filter: (card, ability, _) =>
            {
              if (ability.OwningCard == card)
                return false;

              return card.Is().Creature;
            }));

          p.TriggerOnlyIfOwningCardIsInPlay = true;

          p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1))
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
        });
    }
  }
}