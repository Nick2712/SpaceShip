namespace SpaceShip
{
    public class GameController : IUpdatable, IFixedUpdatable
    {
        private readonly IFixedUpdatable _test;


        public GameController(PlanetSettings planetSettings)
        {
            _test = new MovingTest(planetSettings.Planets[0]);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            _test.FixedUpdate(fixedDeltaTime);
        }

        public void Update(float deltatime)
        {
            
        }
    }
}
