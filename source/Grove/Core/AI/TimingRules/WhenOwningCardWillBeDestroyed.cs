﻿namespace Grove.AI.TimingRules
{
  public class WhenOwningCardWillBeDestroyed : TimingRule
  {
    private readonly bool _considerCombat;
    private readonly bool _targetOnly;

    private WhenOwningCardWillBeDestroyed() {}

    public WhenOwningCardWillBeDestroyed(bool targetOnly = false, bool considerCombat = true)
    {
      _targetOnly = targetOnly;
      _considerCombat = considerCombat;
    }

    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return CanBeDestroyed(p.Card, _targetOnly, _considerCombat);
    }
  }
}