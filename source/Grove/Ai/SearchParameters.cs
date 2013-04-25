﻿namespace Grove.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;

  public class SearchParameters
  {
    private readonly int _maxSearchDepth;
    private readonly int _maxTargetCount;

    public SearchParameters(int maxSearchDepth, int maxTargetCount)
    {
      _maxSearchDepth = maxSearchDepth;
      _maxTargetCount = maxTargetCount;

      SearchDepth = _maxSearchDepth;
      TargetCount = _maxTargetCount;
    }

    public int SearchDepth { get; private set; }
    public int TargetCount { get; private set; }


    public void AdjustPerformance(Queue<int> searchDurations)
    {
      if (searchDurations.Last() > 4000)
      {
        if (TargetCount > 1)
        {
          TargetCount--;
        }
        if (SearchDepth > 1)
        {
          SearchDepth -= Math.Min(4, SearchDepth - 1);
        }
      }

      if (searchDurations.None(x => x > 1500))
      {
        TargetCount = _maxTargetCount;
        SearchDepth = _maxSearchDepth;
      }
    }
  }
}