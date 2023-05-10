using ComputerInterface;
using ComputerInterface.Interfaces;
using Zenject;

namespace LualtsCameraMod
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            // Bind your mod entry like this
            Container.Bind<IComputerModEntry>().To<ModEntry>().AsSingle();

            // I just bind another class here to demonstrate adding a command
            // of course you can request the CommandHandler in any of your types as long as you bind it
            // notice how I use BindInterfacesAndSelfTo
            // since MyModCommandManager inherits the IInitializable interface
            // the class gets instantiated even if no other class needs it
            Container.BindInterfacesAndSelfTo<MyModCommandManager>().AsSingle();
        }
    }
}