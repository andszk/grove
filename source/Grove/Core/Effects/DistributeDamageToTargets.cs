﻿namespace Grove.Effects
{
  public class DistributeDamageToTargets : Effect
  {
    public override int CalculatePlayerDamage(Player player)
    {
      for (var i = 0; i < Targets.Effect.Count; i++)
      {
        if (player == Targets.Effect[i] && IsValid(Targets.Effect[i]))
        {
          return Targets.Distribution[i];
        }
      }
      return 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      for (var i = 0; i < Targets.Effect.Count; i++)
      {
        if (creature == Targets.Effect[i] && IsValid(Targets.Effect[i]))
        {
          return Targets.Distribution[i];
        }
      }

      return 0;
    }

    protected override void ResolveEffect()
    {
      for (var i = 0; i < Targets.Effect.Count; i++)
      {
        var damageable = (IDamageable) Targets.Effect[i];
        Source.OwningCard.DealDamageTo(Targets.Distribution[i], damageable, isCombat: false);
      }
    }
  }
}