public static class MinigameData {

    public struct Standing {
        public int playerNumber; /* 1 - 4 */
        public int standing; /* 1 - 4 */
    }

    public static int minigamesLeft;
    //Set from minigame function SetPlayerStandings
    public static Standing[] standings;

}