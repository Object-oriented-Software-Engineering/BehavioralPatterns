namespace Command1 {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
        }
    }

    public enum GameCharacterType {
        Friendly,
        Enemy,
        NPC
    }

    public class CGameCharacter{
        private GameCharacterType m_characterType;
        private float x_position;
        private float y_position;
        public float YPosition {
            get { return y_position; }
            set { y_position = value; }
        }
        public float XPosition {
            get { return x_position; }
            set { x_position = value; }
        }
        public GameCharacterType CharacterType {
            get { return m_characterType; }
            set { m_characterType = value; }
        }

    }

    public interface ICommand{
        void execute();
    }
    
    public class MoveCharacterCommand : ICommand{
        private Game m_game;
        private float m_x_movement;
        private float m_y_movement;

        public MoveCharacterCommand(Game game, float x, float y) {
            m_game = game;
            m_x_movement = x;
            m_y_movement = y;
        }

        public void execute() {
            m_game.MoveCharacters(m_x_movement, m_y_movement);
        }
    }

    public class GameMemento{
        private class CharacterState{
            public CGameCharacter Character { get; }
            public float XPosition { get; }
            public float YPosition { get; }

            public CharacterState(CGameCharacter character) {
                Character = character;
                XPosition = character.XPosition;
                YPosition = character.YPosition;
            }
        }

        private List<CharacterState> m_characterStates;

        public GameMemento(IEnumerable<CGameCharacter> characters) {
            m_characterStates = new List<CharacterState>();

            foreach (var character in characters)
                m_characterStates.Add(new CharacterState(character));
        }

        public void Restore() {
            foreach (var state in m_characterStates) {
                state.Character.XPosition = state.XPosition;
                state.Character.YPosition = state.YPosition;
            }
        }
    }
    

    public class Game{
        List<CGameCharacter> m_Friendlylist;
        List<CGameCharacter> m_EnemyList;
        List<CGameCharacter> m_NPCList;
        
        List<CGameCharacter> m_SelectedCharacters;
        private List<ICommand> m_commandHistory;
        private Stack<GameMemento> m_stateHistory;

        private float m_terrainXSize;
        private float m_terrainYSize;

        public Game() {
            m_Friendlylist = new List<CGameCharacter>();
            m_EnemyList = new List<CGameCharacter>();
            m_NPCList = new List<CGameCharacter>();
            m_SelectedCharacters = new List<CGameCharacter>();
            m_commandHistory = new List<ICommand>();
            m_stateHistory = new Stack<GameMemento>();
            m_terrainXSize =  100.0f;
            m_terrainYSize = 100.0f;
            SaveInitialState();
        }

        public IReadOnlyList<ICommand> CommandHistory {
            get { return m_commandHistory.AsReadOnly(); }
        }

        public bool CanUndo {
            get { return m_stateHistory.Count > 1; }
        }

        public void AddCharacter(CGameCharacter character) {
            switch (character.CharacterType) {
                case GameCharacterType.Friendly:
                    m_Friendlylist.Add(character);
                    break;
                case GameCharacterType.Enemy:
                    m_EnemyList.Add(character);
                    break;
                case GameCharacterType.NPC:
                    m_NPCList.Add(character);
                    break;
            }

            if (m_commandHistory.Count == 0)
                SaveInitialState();
        }

        public void SelectCharacters(float upleftx, float uplefty, float bottomrightx, float bottomrighty) {
            m_SelectedCharacters.Clear();

            foreach (var character in GetAllCharacters())
                if (character.XPosition >= upleftx && character.XPosition <= bottomrightx &&
                    character.YPosition >= bottomrighty && character.YPosition <= uplefty)
                    m_SelectedCharacters.Add(character);
        }

        public void ExecuteCommand(ICommand command) {
            command.execute();
            m_commandHistory.Add(command);
            m_stateHistory.Push(CreateMemento());
        }

        public bool Undo() {
            if (!CanUndo)
                return false;

            m_stateHistory.Pop();
            GameMemento memento = m_stateHistory.Peek();
            memento.Restore();

            if (m_commandHistory.Count > 0)
                m_commandHistory.RemoveAt(m_commandHistory.Count - 1);

            return true;
        }
        
        public void MoveCharacters(float x, float y) {
            foreach (var character in m_SelectedCharacters) {
                // distrubute the movement to all selected characters around the center of the selection
                // Calculate the center of the selection.
                // This is a simplified approach. A more robust solution would calculate the actual center.
                float selectionCenterX = m_SelectedCharacters.Average(c => c.XPosition);
                float selectionCenterY = m_SelectedCharacters.Average(c => c.YPosition);

                // Calculate the offset from the center for the current character.
                float offsetX = character.XPosition - selectionCenterX;
                float offsetY = character.YPosition - selectionCenterY;

                // Apply the new position with the distributed movement.
                character.XPosition = selectionCenterX + offsetX + (x / m_SelectedCharacters.Count);
                character.YPosition = selectionCenterY + offsetY + (y / m_SelectedCharacters.Count);
            }
        }

        private GameMemento CreateMemento() {
            return new GameMemento(GetAllCharacters());
        }

        private void SaveInitialState() {
            m_stateHistory.Clear();
            m_stateHistory.Push(CreateMemento());
        }

        private IEnumerable<CGameCharacter> GetAllCharacters() {
            return m_Friendlylist.Concat(m_EnemyList).Concat(m_NPCList);
        }
        
    }

    public class GameGUI{
        private Game m_game;

        public GameGUI(Game game) {
            m_game = game;
        }

        public void SelectCharacters(float upleftx, float uplefty, float bottomrightx, float bottomrighty) {
            m_game.SelectCharacters(upleftx, uplefty, bottomrightx, bottomrighty);
        }

        public void MoveSelectedCharacters(float x, float y) {
            ICommand command = new MoveCharacterCommand(m_game, x, y);
            m_game.ExecuteCommand(command);
        }

        public void MoveUp(float distance) {
            MoveSelectedCharacters(0.0f, distance);
        }

        public void MoveDown(float distance) {
            MoveSelectedCharacters(0.0f, -distance);
        }

        public void MoveLeft(float distance) {
            MoveSelectedCharacters(-distance, 0.0f);
        }

        public void MoveRight(float distance) {
            MoveSelectedCharacters(distance, 0.0f);
        }

        public bool Undo() {
            return m_game.Undo();
        }
    }

    
    
}
