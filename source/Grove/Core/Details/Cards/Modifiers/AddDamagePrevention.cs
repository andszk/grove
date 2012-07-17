﻿namespace Grove.Core.Details.Cards.Modifiers
{
  using Preventions;

  public class AddDamagePrevention : Modifier
  {
    private DamagePrevention _damagePrevention;
    private DamagePreventions _damagePreventions;
    public IDamagePreventionFactory Prevention { get; set; }

    public override void Apply(DamagePreventions damagePreventions)
    {
      _damagePreventions = damagePreventions;
      _damagePrevention = Prevention.Create(Target);

      AddLifetime(
        new DependantLifetime(this, _damagePrevention, ChangeTracker));

      damagePreventions.AddReceivedPrevention(_damagePrevention);
    }

    protected override void Unapply()
    {
      _damagePreventions.Remove(_damagePrevention);
    }
  }
}