﻿namespace Grove.Core
{
  using System;
  using Effects;
  using Mana;
  using Zones;

  public class ManaAbility : ActivatedAbility, IManaSource
  {
    private Func<ManaAbility, Game, IManaAmount> _getManaAmount;
    private ManaAmountCharacteristic _manaAmountCharacteristic;

    public ManaAbility(ManaAbilityParameters p) : base(p)
    {
      _getManaAmount = p.GetManaAmount;
      Priority = p.Priority;

      EffectFactory = () => new AddManaToPool(
        amount: e =>
          {
            var source = (ManaAbility) e.Source;
            return source.GetManaAmount();
          });
    }

    public int Priority { get; private set; }
    object IManaSource.Resource { get { return OwningCard; } }

    public void Consume(IManaAmount amount, ManaUsage usage)
    {
      Pay();
      OwningCard.IncreaseUsageScore();

      if (amount.Converted == GetManaAmount().Converted)
        return;

      // if effect produces more mana than needed 
      // add overflow mana to manapool.          
      var manaBag = new ManaBag(amount);
      manaBag.Consume(amount);
      OwningCard.Controller.AddManaToManaPool(manaBag.GetAmount());
    }

    public IManaAmount GetAvailableMana(ManaUsage usage)
    {
      ActivationPrerequisites prerequisites;
      return CanActivate(out prerequisites) == false ? ManaAmount.Zero : GetManaAmount();
    }

    private IManaAmount GetManaAmount()
    {
      return new AggregateManaAmount(_manaAmountCharacteristic.Value, _getManaAmount(this, Game));
    }

    public override bool CanActivate(out ActivationPrerequisites prerequisites)
    {
      int? maxX = null;
      prerequisites = null;

      if (IsEnabled && OwningCard.Zone == Zone.Battlefield && CanPay(ref maxX))
      {
        prerequisites = new ActivationPrerequisites {Description = Text};
      }

      return false;
    }

    public void AddManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmountCharacteristic.AddModifier(modifier);
    }

    public void RemoveManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmountCharacteristic.RemoveModifier(modifier);
    }

    public override void Initialize(Card owner, Game game)
    {
      base.Initialize(owner, game);

      _manaAmountCharacteristic = new ManaAmountCharacteristic(
        ManaAmount.Zero,
        ChangeTracker, null);
    }

    public void SetManaAmount(IManaAmount manaAmount)
    {
      _getManaAmount = delegate { return manaAmount; };
    }
  }
}