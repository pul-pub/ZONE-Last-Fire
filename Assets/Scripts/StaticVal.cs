using Firebase;

public enum TypePlatform { PC, Mobile }

public static class StaticVal
{
#if !UNITY_EDITOR
    public static TypePlatform type = TypePlatform.Mobile;
#else
    public static TypePlatform type = TypePlatform.PC;
#endif
    public static FirebaseApp firebaseApp;

    public static string nameSave = null;
    public static bool onYN = false;
    public static int money = 2000;
    public static int timer = 720;

    //---------------Õ¿—“–Œ… »-----------------------
    public static float alfaUi = 0.25f;
    public static float volSound = 1.0f;
    public static bool vibroMode = false;
    public static bool promptMode = true;
    public static int FPSMode = 60;
    //---------------Õ¿—“–Œ… »-----------------------

    public static int[] time = new int[2]{ 8, 0 };
    public static bool idRain = false;
    public static float light = 0.59f;

    public static bool[,] peopls = new bool[8, 8] { { true, true, true, true, true, true, true, true },
                                                    { true, false, true, true, false, true, true, true },
                                                    { false, false, false, true, false, true, true, true },
                                                    { false, false, false, false, false, false, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true } };

    public static bool[,] peoplsStart = new bool[8, 8] { { true, true, true, true, true, true, true, true },
                                                    { true, false, true, true, false, true, true, true },
                                                    { false, false, false, true, false, true, true, true },
                                                    { false, false, false, false, false, false, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true },
                                                    { true, true, true, true, true, true, true, true } };
}
