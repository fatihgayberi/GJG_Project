namespace GJG.GridSystem.Match
{
    public class MatchStrategy
    {
        private MatchCheckerBase _strategy;
        public MatchCheckerBase Strategy => _strategy;

        public MatchStrategy(GameGrid gameGrid, MatchStrategyType matchStrategyType)
        {
            switch (matchStrategyType)
            {
                case MatchStrategyType.BFS:
                    _strategy = new MatchCheckerBFS(gameGrid);
                    break;
                case MatchStrategyType.DFS:
                    _strategy = new MatchCheckerDFS(gameGrid);
                    break;
            }
        }
    }
}