﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

  public class DeathlessAngel : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Deathless Angel")
        .ManaCost("{4}{W}{W}")
        .Type("Creature Angel")
        .Text("{Flying}{EOL}{W}{W}: Target creature is indestructible this turn.")
        .FlavorText(
          "'I should have died that day, but I suffered not a scratch. I awoke in a lake of blood, none of it apparently my own.'{EOL}—The War Diaries")
        .Power(5)
        .Toughness(7)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying,
          C.ActivatedAbility(
            "{W}{W}: Target creature is indestructible this turn.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.Amount = "{W}{W}".ParseManaAmount()),
            C.Effect<ApplyModifiersToTarget>((e, c) => e.Modifiers(
              c.Modifier<AddStaticAbility>((m, _) => { m.StaticAbility = Static.Indestructible; },
                untilEndOfTurn: true))),
            C.Selector(validator: Selectors.Creature()),
            targetFilter: TargetFilters.ShieldIndestructible(),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.Protector));
    }
  }
}