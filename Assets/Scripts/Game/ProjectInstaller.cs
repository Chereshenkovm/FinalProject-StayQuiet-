using Core;
using Core.Sounds;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public SoundManager soundManager;

        public override void InstallBindings()
        {
            Container.Bind<SoundManager>().FromInstance(soundManager).AsSingle();
        }
    }
}