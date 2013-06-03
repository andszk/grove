﻿namespace Grove.Gameplay.Decisions.Playback
{
  using System;
  using Results;

  public class ChooseEffectOptions : Decisions.ChooseEffectOptions
  {
    protected override bool ShouldExecuteQuery { get { return true; } }

    protected override void ExecuteQuery()
    {
      Result = (ChosenOptions) Game.Recorder.LoadDecisionResult();
    }
  }
}