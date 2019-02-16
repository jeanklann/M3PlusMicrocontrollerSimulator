using System;

namespace IDE
{
    [Serializable]
    public class ComponentProject
    {
        public PointProject Center;
        public int RootComponent;
        public float Rotation;
        public ComponentType Type;

        public ComponentProject(Component c)
        {
            Center = c.Center;
            Rotation = c.Rotation;
            Type = c.Type;
            RootComponent = UiStatics.Circuito.Components.IndexOf(c.RootComponent);
        }

        public static implicit operator Component(ComponentProject c)
        {
            ComponentDraw draw = null;

            switch (c.Type)
            {
                case ComponentType.None:
                    break;
                case ComponentType.Input:
                    draw = Draws.Input[0];
                    break;
                case ComponentType.Output:
                    draw = Draws.Output[0];
                    break;
                case ComponentType.Disable:
                    draw = Draws.Disable[0];
                    break;
                case ComponentType.Not:
                    draw = Draws.Not[0];
                    break;
                case ComponentType.And:
                    draw = Draws.And[0];
                    break;
                case ComponentType.Nand:
                    draw = Draws.Nand[0];
                    break;
                case ComponentType.Or:
                    draw = Draws.Or[0];
                    break;
                case ComponentType.Nor:
                    draw = Draws.Nor[0];
                    break;
                case ComponentType.Xor:
                    draw = Draws.Xor[0];
                    break;
                case ComponentType.Xnor:
                    draw = Draws.Xnor[0];
                    break;
                case ComponentType.Keyboard:
                    draw = Draws.Keyboard;
                    break;
                case ComponentType.Display7Seg:
                    draw = Draws.Display7SegBase;
                    break;
                case ComponentType.Circuit:
                    break;
                case ComponentType.Microcontroller:
                    draw = Draws.Microcontroller;
                    break;
                case ComponentType.Osciloscope:
                    draw = Draws.Osciloscope;
                    break;
                case ComponentType.BlackTerminal:
                    break;
                case ComponentType.JkFlipFlop:
                    draw = Draws.JkFlipFlop;
                    break;
                case ComponentType.RsFlipFlop:
                    draw = Draws.RsFlipFlop;
                    break;
                case ComponentType.DFlipFlop:
                    draw = Draws.DFlipFlop;
                    break;
                case ComponentType.FlipFlop:
                    draw = Draws.FlipFlop;
                    break;
                case ComponentType.ControlModule:
                    draw = Draws.ControlModule;
                    break;
                case ComponentType.PortBank:
                    draw = Draws.PortBank;
                    break;
                case ComponentType.RamMemory:
                    draw = Draws.RamMemory;
                    break;
                case ComponentType.RomMemory:
                    draw = Draws.RomMemory;
                    break;
                case ComponentType.Stack:
                    break;
                case ComponentType.Tristate:
                    draw = Draws.Disable[0];
                    break;
                case ComponentType.Ula:
                    draw = Draws.Ula;
                    break;
                case ComponentType.Registrer8Bit:
                    draw = Draws.Registrer8Bit;
                    break;
                case ComponentType.Registrer8BitSg:
                    draw = Draws.Registrer8BitSg;
                    break;
                case ComponentType.Registrers:
                    draw = Draws.Registrers;
                    break;
                case ComponentType.Disable8Bit:
                    draw = Draws.Disable8Bit;
                    break;
                case ComponentType.BinTo7Seg:
                    draw = Draws.BinTo7Seg;
                    break;
                case ComponentType.Registrer8BitCBuffer:
                    draw = Draws.Registrer8BitCBuffer;
                    break;
                case ComponentType.Counter8Bit:
                    draw = Draws.Counter8Bit;
                    break;
                case ComponentType.RomAddresser:
                    draw = Draws.RomAddresser;
                    break;
                case ComponentType.Clock:
                    draw = Draws.Clock;
                    break;
            }

            var component = new Component(draw, c.Center);
            component.Rotation = c.Rotation;
            component.Type = c.Type;
            if (c.RootComponent != -1)
                component.RootComponent = UiStatics.Circuito.Components[c.RootComponent];

            return component;
        }
    }
}