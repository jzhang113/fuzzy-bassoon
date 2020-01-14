using Ultraviolet;
using Ultraviolet.Core;
using Ultraviolet.Input;

namespace Doregal.Input
{
    public static class SampleInput
    {
        public static Actions GetActions(this IUltravioletInput input) =>
            Actions.Instance;

        public class Actions : InputActionCollection
        {
            public Actions(UltravioletContext uv)
                : base(uv)
            { }

            public static Actions Instance { get; } = CreateSingleton<Actions>();

            public InputAction ExitApplication { get; private set; }
            public InputAction MoveLeft { get; private set; }
            public InputAction MoveRight { get; private set; }

            /// <inheritdoc/>
            protected override void OnCreatingActions()
            {
                ExitApplication =
                    CreateAction("EXIT_APPLICATION");
                MoveLeft =
                    CreateAction("MOVE_LEFT");
                MoveRight =
                    CreateAction("MOVE_RIGHT");

                base.OnCreatingActions();
            }

            /// <inheritdoc/>
            protected override void OnResetting()
            {
                switch (Ultraviolet.Platform)
                {
                    case UltravioletPlatform.Android:
                        Reset_Android();
                        break;

                    default:
                        Reset_Desktop();
                        break;
                }
                base.OnResetting();
            }

            private void Reset_Desktop()
            {
                ExitApplication
                    .Primary = CreateKeyboardBinding(Key.Escape);
                MoveLeft
                    .Primary = CreateKeyboardBinding(Key.A);
                MoveRight
                    .Primary = CreateKeyboardBinding(Key.D);
            }

            private void Reset_Android()
            {
                this.ExitApplication
                    .Primary = CreateKeyboardBinding(Key.AppControlBack);
            }
        }
    }
}