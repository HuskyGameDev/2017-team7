public class MinigameData {

    public struct Standing {
        public int playerNumber; /* 1 - 4 */
        public int standing; /* 1 - 4 */
    }

    public static MinigameData instance = new MinigameData();
    public static void Instantiate()
    {
        instance = new MinigameData();
    }

    public static int minigamesLeft;
    //Set from minigame function SetPlayerStandings
    public static Standing[] standings;

}