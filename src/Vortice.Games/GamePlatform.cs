﻿// Copyright © Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System;

namespace Vortice
{
    internal partial class GamePlatform
    {
        protected GamePlatform(Game game)
        {
            Guard.AssertNotNull(game);
            Game = game;
        }

        public Game Game { get; }

        public event EventHandler<EventArgs>? Activated;

        public event EventHandler<EventArgs>? Deactivated;

        protected void OnActivated()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDeactivated()
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}
