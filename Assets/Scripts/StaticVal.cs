public enum TypePlatform { PC, Mobile }

public static class StaticVal
{
#if !UNITY_EDITOR
    public static TypePlatform type = TypePlatform.Mobile;
    public static string trecker_id_android = "26444911395610227673";
#else
    public static TypePlatform type = TypePlatform.PC;
    public static string trecker_id_android = null;
#endif

    public static DataBase dataBase;

    public static string nameSave = null;
    public static bool onYN = false;
    public static int money = 2000;
    public static int timer = 720;

    public static string idSesion;

    #region SETTINGS
    public static float alfaUi = 0.25f;
    public static float volSound = 1.0f;
    public static bool vibroMode = false;
    public static bool promptMode = true;
    public static int FPSMode = 60;
    #endregion

    #region TIMES
    public static int[] time = new int[2]{ 8, 0 };
    public static bool idRain = false;
    public static float light = 0.59f;
    #endregion

    #region CHARECTER
    public static string name = null;
    public static int idFace = 0;
    public static int notSelectedXP = 0;
    public static int[] characteristics = new int[3]{ -1, -1, -1 };
    public static int[] idSkills = new int[10]{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    #endregion

    #region PEOPLS
    public static bool[,] peopls = new bool[12, 8] { { true, true, true, true, true, true, true, true },
                                                    { true, false, true, true, false, true, false, true },
                                                    { false, false, false, true, false, true, true, true },
                                                    { false, false, false, false, false, false, true, true },
                                                    { false, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true } };

    public static bool[,] peoplsStart = new bool[12, 8] { { true, true, true, true, true, true, true, true },
                                                    { true, false, true, true, false, true, false, true },
                                                    { false, false, false, true, false, true, true, true },
                                                    { false, false, false, false, false, false, true, true },
                                                    { false, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true } };
    #endregion
}
