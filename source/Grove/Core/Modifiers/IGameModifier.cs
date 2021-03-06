﻿namespace Grove.Modifiers
{
  using Costs;

  public interface IGameModifier : IModifier
  {
    void Apply(DamageRedirections damageRedirections);
    void Apply(DamagePreventions damagePreventions);
    void Apply(CostModifiers costModifiers);
    void Apply(NamedGameModifiers namedGameModifiers);
  }
}