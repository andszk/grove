﻿namespace Grove.Core
{
  public interface ITargetSelector
  {
    int? MaxCount { get; }
    int MinCount { get; }
    string Text { get; }
    bool IsValid(ITarget target);
  }
}