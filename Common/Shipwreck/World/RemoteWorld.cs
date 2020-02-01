using System.Linq;

namespace Shipwreck.World
{
    public class RemoteWorld : BaseWorld
    {
        protected override void Tick(float deltaTime)
        {
            // Perform base tick
            base.Tick(deltaTime);

            // Ensure our LocalAstronaut stays up to date
            if (LocalPlayer != null)
                LocalAstronaut = State.Astronauts.FirstOrDefault(a => a.Guid == LocalPlayer.Guid);
        }
    }
}
