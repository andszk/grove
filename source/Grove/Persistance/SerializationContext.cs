﻿namespace Grove.Persistance
{
  using Gameplay;

  public class SerializationContext
  {
    public GameRecorder Recorder { get { return Game.Recorder; } }
    public Game Game { get; set; }
  }
}