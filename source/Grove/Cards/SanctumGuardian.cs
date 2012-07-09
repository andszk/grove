﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;

  public class SanctumGuardian : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sanctum Guardian")
        .ManaCost("{1}{W}{W}")
        .Type("Creature Human Cleric")
        .Text(
          "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.")
        .FlavorText("'Protect our mother in her womb.'")
        .Power(1)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "Sacrifice Sanctum Guardian: The next time a source of your choice would deal damage to target creature or player this turn, prevent that damage.",
            C.Cost<SacrificeOwner>(),
            C.Effect<PreventDamageFromSource>((e, _) => e.OnlyOnce = true),
            
            effectSelectors: new []
              {
                C.Selector(Selectors.EffectOrPermanent()),
                C.Selector(Selectors.CreatureOrPlayer())               
              },
            
            targetFilter: TargetFilters.PreventDamageFromSource(),
            timing: Timings.NoRestrictions()
          )      
        );
    }
  }
}