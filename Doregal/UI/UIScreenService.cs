using Doregal.UI.Screens;
using System;
using System.Collections.Generic;
using Ultraviolet.Content;
using Ultraviolet.UI;

namespace Doregal.UI
{
    /// <summary>
    /// Represents a service which provides instances of UI screen types upon request.
    /// </summary>
    public sealed class UIScreenService
    {
        public UIScreenService(ContentManager globalContent)
        {
            Register(new TitleScreen(globalContent, this));
            Register(new MainScreen(globalContent, this));
        }

        public T Get<T>() where T : UIScreen
        {
            screens.TryGetValue(typeof(T), out UIScreen screen);
            return (T)screen;
        }

        private void Register<T>(T instance) where T : UIScreen => screens[typeof(T)] = instance;

        // State values.
        private readonly Dictionary<Type, UIScreen> screens =
            new Dictionary<Type, UIScreen>();
    }
}