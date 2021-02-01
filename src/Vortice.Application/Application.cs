// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Vortice.Graphics;
using Vortice.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Vortice
{
    public abstract class Application : IDisposable
    {
        private readonly object _tickLock = new object();
        //private bool _isExiting;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private bool _endRunRequired;

        /// <summary>
        /// Gets a list of registered <see cref="GameSystem"/>.
        /// </summary>
        public IList<GameSystem> GameSystems { get; } = new List<GameSystem>();

        /// <summary>
        /// Gets value whether the game is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        protected Application()
            : this(new NetStandardGameContext())
        {
        }

        protected Application(GameContext context)
        {
            Guard.AssertNotNull(context, nameof(context));

            Context = context;

            // Configure and build services
            ServiceCollection services = new ServiceCollection();

            Context.ConfigureServices(services);
            ConfigureServices(services);

            Services = services.BuildServiceProvider();

            // Get required services.
            GraphicsDevice = Services.GetRequiredService<GraphicsDevice>();
            Input = Services.GetRequiredService<InputManager>();
        }

        public GameContext Context { get; }
        public IServiceProvider Services { get; }

        public GraphicsDevice? GraphicsDevice { get; }

        public InputManager? Input { get; }

        public virtual void Dispose()
        {
            foreach (GameSystem? gameSystem in GameSystems)
            {
                gameSystem.Dispose();
            }

            GraphicsDevice?.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this);
            //services.AddSingleton<IContentManager, ContentManager>();
            services.AddSingleton<InputManager>();
        }

        public void Run()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("This game is already running.");
            }

            try
            {
                // Enter main loop.
                bool blocking = Context.Run(InitializeBeforeRun, Tick);

                if (blocking)
                {
                    // If the previous call was blocking, then we can call EndRun
                    //EndRun();
                }
                else
                {
                    // EndRun will be executed on Exit
                    _endRunRequired = true;
                }
            }
            finally
            {
                if (!_endRunRequired)
                {
                    IsRunning = false;
                }
            }
        }

        public void Tick()
        {
            //if (!vgpuBeginFrame(GraphicsDevice))
            //    return;

            //var renderPass = new RenderPassDescription();
            //renderPass.colorAttachments0.clearColor = new Color4(1.0f, 0.0f, 1.0f);
            //renderPass.colorAttachments0.loadOp = LoadOp.Clear;
            //renderPass.colorAttachments0.texture = vgpuGetBackbufferTexture(GraphicsDevice);
            //vgpuCmdBeginRenderPass(GraphicsDevice, renderPass);
            //vgpuCmdEndRenderPass(GraphicsDevice);
            //vgpuEndFrame(GraphicsDevice);
        }

        private void InitializeBeforeRun()
        {
            IsRunning = true;
        }
    }
}