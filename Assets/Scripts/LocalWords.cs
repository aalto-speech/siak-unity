using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalWords : MonoBehaviour {

    public static Dictionary<Level, List<string>> levelWords = new Dictionary<Level, List<string>>() {
        { Level.Forest1 , new List<string>() { "hello", "it", "bye_bye", "cool", "eat1", "school", "home", "friend", "boy", "girl", "speak", "play", "book", "say", "come_on", "sorry", "read", "write", "learn", "come" } },
        { Level.Ice1 , new List<string>() { "river", "flower", "bush", "forest", "push", "tree", "lake", "leaf", "life", "light", "moon", "rock", "sand", "sea", "sun", "air" } },
        { Level.Sand1 , new List<string>() { "quiet", "free", "round", "wrong", "right", "dark", "dry", "low", "soft", "wet", "bad", "good", "loud", "high", "hard" } },
        { Level.NoBoard1 , new List<string>() { "farmer", "pig", "big", "sheep", "mouse", "horse", "cat", "cow", "dog", "hen", "chicken", "country", "field", "grass", "hay", "duck" } },
        { Level.Sand5 , new List<string>() { "food", "eat1", "eat2", "eat3", "apple", "bake", "berry", "breakfast", "bread", "butter", "cake", "candy", "carrot", "cook", "cut", "drink", "lunch" } },
        { Level.Ice3 , new List<string>() { "shirt", "feet", "fit", "shoe", "socks", "cap", "coat", "hat", "buy", "wash", "money", "belt", "wear", "shop", "best" } },
        { Level.Forest5 , new List<string>() { "ball", "music", "sing", "ski", "swim", "baseball", "win", "basketball", "fun", "game", "goal", "ice_hockey", "ice_skating", "movie", "party" } },
        { Level.SandIce1 , new List<string>() { "pet", "bet", "neighbour", "classmate", "sister", "teacher", "child", "children", "family", "man", "woman", "parents", "love", "baby", "young", "old" } },
        { Level.NoBoard2 , new List<string>() { "dessert", "fork", "leak", "glass", "tin", "knife", "lick", "make", "more", "plate", "spoon", "sweet", "pepper", "pear", "meat", "cup", "water", "milk" } },
        { Level.Ice4 , new List<string>() { "cheek", "listen", "chin", "ear", "eye", "face", "nose", "lips", "tongue", "hear", "feel", "taste", "smell", "sound", "look" } },
        { Level.MixAll2 , new List<string>() { "pack", "back", "elbow", "give", "fit", "feet", "finger", "arm", "hair", "hand", "neck", "body", "legs", "shoulder", "knee", "toe", "run", "take" } },
        { Level.Sand2 , new List<string>() { "cheese", "cheap", "cherry", "chew", "chip", "chocolate", "cucumber", "lemon", "meatball", "potato", "salad", "salt", "snack", "soup", "toast", "tomato", "strawberry" } },
        { Level.Ice5 , new List<string>() { "i_am", "she_is", "i_was", "you_were", "we_were", "it1", "it2", "it3", "don_t", "he_was", "cry", "smile", "laugh", "dream", "miss" } },
        { Level.Forest4 , new List<string>() { "bear", "bee", "bird", "bug", "butterfly", "dolphin", "hare", "elephant", "fish", "fox", "hedgehog", "lion", "monkey", "owl", "snake"} },
        { Level.Ice2 , new List<string>() { "jet1", "jet2", "jet3", "yet1", "yet2", "yet3", "juice", "use", "your", "jaw", "jump", "jam", "age", "orange" } },
        { Level.Forest2 , new List<string>() { "healthy1", "healthy2", "healthy3", "thick", "thin", "thing", "think", "teeth", "mouth", "north", "theatre", "month", "three", "thank_you", "thirsty", "throw", "bath", "maths" } },
        { Level.NoGame1 , new List<string>() { "feather1", "feather2", "feather3", "brother", "father", "mother", "clothes", "their", "they", "then", "there", "this", "that", "these", "those", "than", "the", "with" } },
        { Level.ForestSand1 , new List<string>() { "bedroom", "leave", "carpet", "live", "bed", "window", "sleep", "dream", "garage", "house", "kitchen", "lamp", "living_room", "lock", "phone", "room", "shower", "table", "toilet", "door" } },
        { Level.SnowForest1 , new List<string>() { "jet4", "jet5", "jet6", "yet4", "yet5", "yet6", "juice", "use", "your", "jaw", "jump", "jam", "age", "orange" } },
        { Level.MixAll1 , new List<string>() { "healthy4", "healthy5", "healthy6", "thick", "thin", "thing", "think", "teeth", "mouth", "north", "theatre", "month", "thank_you", "thirsty", "throw", "bath", "maths" } },
        { Level.NoGame2 , new List<string>() { "feather4", "feather5", "feather6", "brother", "father", "mother", "clothes", "their", "they", "then", "there", "this", "that", "these", "those", "than", "the", "with" } },
        { Level.Sand4 , new List<string>() { "here", "where", "who", "what", "why", "cool", "now", "today", "yesterday", "time", "day", "evening", "morning", "night",  } },
        { Level.NoBoard3 , new List<string>() { "leave", "english", "live", "finnish", "swedish", "finland", "finn", "england", "village", "europe", "city", "town", "language" } },
        { Level.Forest3 , new List<string>() { "jet7", "jet8", "jet9", "yet7", "yet8", "yet9", "juice", "use", "your", "jaw", "jump", "jam", "age", "orange" } },
        { Level.Sand3 , new List<string>() { "healthy7", "healthy8", "healthy9", "thick", "thin", "thing", "think", "teeth", "mouth", "north", "theatre", "month", "three", "thank_you", "thirsty", "throw", "bath", "maths" } },
        { Level.NoGame3 , new List<string>() { "feather7", "feather8", "feather9", "brother", "father", "mother", "clothes", "their", "they", "then", "there", "this", "that", "these", "those", "than", "the", "with" } },
        { Level.MixAll3 , new List<string>() { "would_you_like_some_juice", "would_you_like_some_more", "yes_please", "no_thank_you", "you_re_welcome", "here_you_are", "well_done", "my_name_is", "nice_to_meet_you", "i_live_in_finland", "i_speak_finnish", "i_am_sorry", "good_morning", "good_night", "do_you_want_to_play_with_us" } },
    };
}