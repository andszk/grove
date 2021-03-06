﻿namespace Grove.AI.TimingRules
{
  public class OnStep : TimingRule
  {
    private readonly Step _step;

    private OnStep() {}

    public OnStep(Step step)
    {
      _step = step;
    }

    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return Turn.Step == _step && Stack.IsEmpty;
    }
  }
}