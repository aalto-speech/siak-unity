using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LocalWords : MonoBehaviour {

    public static Dictionary<Level, List<string>> levelWords = new Dictionary<Level, List<string>>() {
        { Level.Forest1 , new List<string>() {"a_vokaali", "aamu", "haava", "passi", "ae_vokaali", "ääni", "lääkäri", "tänään" } },
        { Level.Ice1 , new List<string>() {"e_vokaali", "elain", "o_vokaali", "otsa", "oe_vokaali", "oljy"} },
        { Level.Sand1 , new List<string>() {"i_vokaali", "isa", "y_vokaali", "myyja", "u_vokaali", "usein", "oei_diftongi", "toissa" } },
        { Level.NoBoard1 , new List<string>() {"oi_diftongi", "iloinen", "osoite", "ui_diftongi", "kuitti", "suihku" } },
        { Level.Sand5 , new List<string>() { "uo_diftongi", "muovi", "ruoka", "huomenna", "ou_diftongi", "housut", "tarjous"} },
        { Level.Ice3 , new List<string>() {"au_diftongi", "aurinko", "vauva", "eu_diftongi", "neula", "leuka", "iu_diftongi", "tiukka", "kiuas"} },
        { Level.Forest5 , new List<string>() { "ai_diftongi", "vaimo", "avain", "aei_diftongi", "aiti", "elain", "ei", "peitto", "leipa", "yi_diftongi", "lyijykyna" } },
        { Level.SandIce1 , new List<string>() {"yo", "vyo", "pyora", "hyotya", "myöhemmin", "syömässä", "syödä", "ryöstö", "nyöri", "työntää"} },
        { Level.NoBoard2 , new List<string>() { "loytyi", "poyta", "koyha", "roykkio", "loyhka", "toyhto", "toykea", "koysi", "hoyhen", "oykkari" } },
        { Level.Ice4 , new List<string>() { "veitsi", "haarukka", "syomassa", "poyta", "mauste", "kirea", "vihrea", "vatsa_on_taysi", "vitsi", "kasi" } },
        { Level.MixAll2 , new List<string>() { "tyontaa", "taysi", "passi", "poliisi", "loytyi", "auto", "usea", "myyda", "vitsi_on_kuiva", "anteeksi_missa_on_vessa", "se_loytyy_poydalta" } },
        { Level.Sand2 , new List<string>() {"vaimo", "toyhto", "aiti", "pyora", "isa", "vauva", "minulle_kuuluu_hyvaa", "toissa_on_usea", "syon_myohemmin_lisaa"} },
        { Level.Ice5 , new List<string>() { "terveys", "han_on_hyvin_sairas", "kaytava", "vatsaan_sattuu", "paljon", "kuume", "minua_sattuu_kurkkuun", "laakari", "tarvitsen_ajan_hoitajalle", "osasto", "missa_on_neuvola", "laake", "terveyskeskus", "kasi_on_kirea", "poydalla_on_roykkio" } },
        { Level.Forest4 , new List<string>() {"ryosto", "puu", "museo", "puisto", "koira", "tuuli", "toykea", "bussi", "paljonko_kello_on", "metsa_on_vihrea", "olen_yolla_tyossa"} },
        { Level.Ice2 , new List<string>() { "nyori", "lumi", "metsa", "tuuli", "kiuas", "koysi", "tuolla_on_poliisi", "poliisi_on_pirtea"} },
        { Level.Forest2 , new List<string>() {"sormen_sormeen", "kerroksen_kerrokseen", "tulen_tuleen", "torven_torveen", "sormuksen_sormukseen", "tuloksen_tulokseen", "paatoksen", "paatokseen" } },
        { Level.NoGame1 , new List<string>() {"puron_puroon", "jakson_jaksoon", "talon_taloon", "arvon_arvoon", "onkalon", "onkaloon", "tietonne_tietoonne", "luettelon_luetteloon" } },
        { Level.ForestSand1 , new List<string>() {"harvoin", "koyha", "vitsi", "kuiva", "kyomy", "siistiytya", "neuvonta", "tauko", "kuuma", "anteeksi_missa_on_vessa", "veitsi_on_ruskea", "talossa_loyhkaa_jokin", "royhkea_ryosto"} },
        { Level.SnowForest1 , new List<string>() {"hyotya", "loytyi", "terveyskeskus", "myyda", "kaytava", "pankki", "osasto", "kasi", "poliisi", "tietokone", "myyja", "olen_myohassa_taas", "missa_on_opettaja", "toissa_on_usea"} },
        { Level.MixAll1 , new List<string>() {"teekkari", "apteekki", "anteeksi", "esteet", "edelleen", "vaatteet", "avanteen", "pahitteeksi", "poltteet", "taiteet", "aiheellinen", "tieteellinen", "teeksi", "magneetti"} },
        { Level.NoGame2 , new List<string>() {"moottori", "ehtooksi", "lontooksi", "ehtoot", "avantoon", "mikroskooppi", "palttoot", "tarjoot", "urhoollinen", "talkoollinen", "kooksi", "eurooppa"} },
        { Level.Sand4 , new List<string>() {"pieni", "pirtea", "vihrea", "kirea", "noyra", "han_on_myotatuntoinen", "tarvitsen_apua", "poliisi_on_pirtea", "taysi", "usea" } },
        { Level.NoBoard3 , new List<string>() {"ruoka", "poyta", "pyora", "jaatelo", "syoda", "keittio", "mauste", "oliivi", "pirtelo", "kayn_myos_syomassa", "se_loytyy_poydalta", "veitsi_on_ruskea", "leipa" } },
        { Level.Forest3 , new List<string>() {"silmalasit", "huivi", "hiusharja", "veitsi", "toyhto", "siistiytya", "hiukset", "vyo", "lyijykyna", "kasi_on_kirea"} },
        { Level.Sand3 , new List<string>() {"neuvonta", "bussi", "taytyy", "punainen", "radio", "poika", "paiva", "metsa_on_vihrea", "tuolla_on_poliisi", "poyta", "pyora", "kayn_myos_syomassa" } },
        { Level.NoGame3 , new List<string>() {"vesi", "lumme", "jono", "kieli", "leipa", "tytto", "ilta", "tyyli", "nappain", "jaatelo", "keittio"} },
        { Level.MixAll3 , new List<string>() { "suihku", "naimisissa", "tietokone", "koti", "televisio", "oykkari", "perhe", "haissa", "myontaa", "poyta", "pyora", "vatsa_on_taysi", "vitsi_on_kuiva"} },
    };
}