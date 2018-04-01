public class MinigameData {

    public struct Standing {
        public int playerNumber; /* 1 - 4 */
        public int standing; /* 1 - 4 */
        public Standing(int _playerNumber, int _standing)
        {
            playerNumber = _playerNumber;
            standing = _standing;
        }
    }

    public static MinigameData instance = new MinigameData();
    public static void Instantiate()
    {
        instance = new MinigameData();
    }

    public static int minigamesLeft;
    //Set from minigame function SetPlayerStandings
    public static Standing[] standings =
        {
            new Standing(1,1),
            new Standing(2,2),
            new Standing(3,3),
            new Standing(4,4)
        };

}