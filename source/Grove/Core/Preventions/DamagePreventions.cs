﻿namespace Grove.Core.Preventions
{
  using System;
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class DamagePreventions : IModifiable, IHashable
  {
    private readonly TrackableList<DamagePrevention> _preventions;

    private DamagePreventions() {}

    public DamagePreventions(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _preventions = new TrackableList<DamagePrevention>(changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_preventions);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(DamagePrevention prevention)
    {
      _preventions.Add(prevention);
    }

    public void PreventDamage(Damage damage)
    {      
      foreach (DamagePrevention preventionEffect in _preventions.ToList())
      {
        preventionEffect.PreventDamage(damage);

        if (damage.Amount == 0)
          break;
      }      
    }

    public void Remove(DamagePrevention preventaion)
    {
      _preventions.Remove(preventaion);
    }

    public int EvaluateHowMuchDamageCanBeDealt(Card source, int amount, bool isCombat)
    {            
      foreach (DamagePrevention preventionEffect in _preventions.ToList())
      {
        amount = preventionEffect.EvaluateHowMuchDamageCanBeDealt(source, amount, isCombat);
        
        if (amount == 0)
          break;
      }

      return amount;
    }
  }
}