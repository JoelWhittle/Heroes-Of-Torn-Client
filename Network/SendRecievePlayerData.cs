using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;

public class SendRecievePlayerData : MonoBehaviour
{

    public static SendRecievePlayerData Instance;
    public NetworkManager cNetworkManager;
    public UnitCollection cUnitCollection;
    public GameObject goUnitVault;
    public string sGetPlayerDataPath;
    public string sReadClientsUnitsPath;
    public string sReadCataloguePath;
    public string sBeginnerCollectionDataPath;
    public string sNewCatalogueUnitPath;
    public string sSaveDeckDataPath;
    private List<string> sDeckCollectionParse = new List<string>();
    public List<string> sBeginnersCollectionDataParse = new List<string>();
    private string sCatalogueUnitWeWantToMakeDataParse;
    private string sWriteUnitParse;



    //Find references on class initialisation
    private void Start()
    {
        Instance = this;
        cNetworkManager = GameObject.Find("_NetworkManager").GetComponent<NetworkManager>();
        cUnitCollection = UnitCollection.Instance;
        goUnitVault = GameObject.Find("_ClientUnitVault");

    }

    //Handles reading in all the players data from the Database on Login, and sets up ready for play

    public IEnumerator HandleGetPlayerData(string sUsername)
    {
        string s = AsynchronousClient.Instance.Send("RequestPlayerData:" + sUsername);

        string str;
        string[] args;
        str = s;

        args = str.Split(',');


        Player cPlayer = Player.Instance;
        cPlayer.iGold = int.Parse(args[0]);
        cPlayer.iDust = int.Parse(args[1]);
        cPlayer.iRank = int.Parse(args[2]);
        sDeckCollectionParse.Add(args[3]);
        sDeckCollectionParse.Add(args[4]);
        sDeckCollectionParse.Add(args[5]);
        sDeckCollectionParse.Add(args[6]);
        sDeckCollectionParse.Add(args[7]);
        sDeckCollectionParse.Add(args[8]);
        sDeckCollectionParse.Add(args[9]);
        sDeckCollectionParse.Add(args[10]);
        sDeckCollectionParse.Add(args[11]);

        // Read in Unit Catalogue
        yield return StartCoroutine(HandleReadInUnitCatalogue());



        ///	Read in and create Unit data



        string getplayerunitsReader = AsynchronousClient.Instance.Send("RequestUsersUnits:" + sUsername);
        yield return getplayerunitsReader;

        GameObject.Find("_ClientUnitVault")
            .GetComponent<UnitParser>()
            .CreatePlayersVaultUnits(getplayerunitsReader);
        cNetworkManager.cLoginMenu.label = "Waking mercenaries up";


        //Arrange Decks
        foreach (string sDeckInfo in sDeckCollectionParse)
        {
            if (sDeckInfo != "")
            {
                Deck cDeck = GameObject.Find("_ClientUnitVault").AddComponent<Deck>();
                GameObject.Find("Player").GetComponent<UnitCollection>().Decks.Add(cDeck);


                string[] sDeckArgs = sDeckInfo.Split("(".ToCharArray());

                cDeck.sName = sDeckArgs[0];
                cDeck.sFaction = sDeckArgs[2];

                string[] sUnitIDsToFind = sDeckArgs[3].Split("*".ToCharArray());


                int iDeckSlot = 0;
                foreach (string sIDToFind in sUnitIDsToFind)
                {

                    foreach (Unit cUnit in cPlayer.cUnitCollection.AllUnits)
                    {
                        if (cUnit.sId == sIDToFind)
                        {
                            cDeck.Units.Add(cUnit);

                            if (iDeckSlot == 0)
                            {
                                cDeck.Hero = cUnit;
                            }

                        }
                    }

                    iDeckSlot++;
                }

                //DefaultDeck8(Stony_Ironbeard(Alliance(471*473*474*474*475*476*
            }
        }
        sDeckCollectionParse.Clear();
    }


    //Reads in the UnitCatalogue table in the database and parses of to sUnitCatalogue list 
    //in UnitCollection.cs
    public IEnumerator HandleReadInUnitCatalogue()
    {
      
        string sCatalogueDataParse = AsynchronousClient.Instance.Send("RequestUnitCatalogue:0");
        yield return sCatalogueDataParse;
        //Read each line from the web catalogueReader and add it to UnitCollection.sUnitCatalogue list

        string[] sCatalogueDataParseArgs = sCatalogueDataParse.Split("@".ToCharArray());
        foreach (string sSingleCatalogueUnitParse in sCatalogueDataParseArgs)
        {
            if (sSingleCatalogueUnitParse != "")
            {

                cUnitCollection.sUnitCatalogue.Add(sSingleCatalogueUnitParse);

                CatalogueUnit catalogueUnit = GameObject.Find("_UnitCatalogue").AddComponent<CatalogueUnit>();

                string[] catalogueUnitData = sSingleCatalogueUnitParse.Split(">".ToCharArray());

                Debug.Log(sSingleCatalogueUnitParse);
             //
catalogueUnit.CatalogueID = int.Parse(catalogueUnitData[0]);      
                catalogueUnit.Name = catalogueUnitData[1];
                catalogueUnit.UnitType = catalogueUnitData[2];
                catalogueUnit.Faction = catalogueUnitData[3];
                catalogueUnit.Rarity = catalogueUnitData[4];
                catalogueUnit.Collection = catalogueUnitData[5];
                catalogueUnit.Race = catalogueUnitData[6];
catalogueUnit.DamageType = catalogueUnitData[7];

               catalogueUnit.Attack =  int.Parse(catalogueUnitData[8]);
               catalogueUnit.Accuracy =  int.Parse(catalogueUnitData[9]);
               catalogueUnit.Dodge =  int.Parse(catalogueUnitData[10]);
               catalogueUnit.HitPoints =  int.Parse(catalogueUnitData[11]);
               catalogueUnit.MovementSpeed =  int.Parse(catalogueUnitData[12]);
               catalogueUnit.Magic =  int.Parse(catalogueUnitData[13]);
               catalogueUnit.MagicResistance =  int.Parse(catalogueUnitData[14]);
               catalogueUnit.FireResistance =  int.Parse(catalogueUnitData[15]);
               catalogueUnit.SlashResistance =  int.Parse(catalogueUnitData[16]);
               catalogueUnit.PiercingResistance =  int.Parse(catalogueUnitData[17]);
               catalogueUnit.BludgeoningResistance =  int.Parse(catalogueUnitData[18]);
               catalogueUnit.MinAttackRange =  int.Parse(catalogueUnitData[19]);
               catalogueUnit.MaxAttackRange =  int.Parse(catalogueUnitData[20]);
              catalogueUnit.Abilities = catalogueUnitData[21];
                catalogueUnit.Artist = catalogueUnitData[22];
                catalogueUnit.FlavourText = catalogueUnitData[23];

                UnitCollection.Instance.UnitCatalogue.Add(catalogueUnit);
                //
            }
        }
        yield return new WaitForSeconds(0);
    }


    //Reads in the Server/UnitVault/BeginnersCollection.txt so new cards can be made and decks arranged
    //for a new user
    public IEnumerator HandleReadInBeginnersCollectionData()
    {

        string s = AsynchronousClient.Instance.Send("RequestBeginnersCollection:0");
        string[] args = s.Split("#".ToCharArray());

        sBeginnersCollectionDataParse.Clear();

        foreach (string line in args)
        {
            sBeginnersCollectionDataParse.Add(line);

        }
       

        // yield return for HandleCreateUnitFromCatalogue for each unit
        //given in sBeginnersCollectionDataParse

        string sBeginnerUnits = sBeginnersCollectionDataParse[0];
        string[] sBeginnerUnitIds = sBeginnerUnits.Split(",".ToCharArray());
        Debug.Log(sBeginnerUnits);
        foreach (string sId in sBeginnerUnitIds)
        {
            int iUnitId = int.Parse(sId);
            yield return StartCoroutine(HandleCreateUnitFromCatalogue(iUnitId));
//            yield return new WaitForSeconds(1.0f);
        }

        StartCoroutine(CreateBeginnersDecks(sBeginnersCollectionDataParse));
    }


    //Creates the Decks in game from the Beginners Collection.. Recieves the data from HandeReadInBeginnersCollectionData()
    //Should only be called once per client on Sign Up
    public IEnumerator CreateBeginnersDecks(List<string> sBeginnersCollectionDataParse)
    {
        List<string> sBeginnersDeckParse = new List<string>();
        //Seperate the deck info from the SBeginnerDataParse and add to a sBeginnersDeckParse;
        int i = 0;


        foreach (string sLine in sBeginnersCollectionDataParse)
        {
            if (i > 0)
            {
                sBeginnersDeckParse.Add(sLine);
            }
            i++;
        }


        //Iterate over each line in the sBeginnersDeckParse list,
        //split the line of into individual unit catalogue ids to find
        //for each catalogue id to find, check each unit in collection and see if it matches, and its not in the dekc already .. 
        //if so add it to the deck.

        int iCurrentDeckBeingAssembled = 0;
        foreach (string sDeckData in sBeginnersDeckParse)
        {

            Deck sDeck = goUnitVault.AddComponent<Deck>();
            Player.Instance.cUnitCollection.Decks.Add(sDeck);
            sDeck.sName = "DefaultDeck" + iCurrentDeckBeingAssembled.ToString();
            sDeck.sFaction = "Alliance";

            string[] args = sDeckData.Split(",".ToCharArray());
            int iDeckSlot = 0;
            foreach (string sUnitIDToFind in args)
            {
                bool bStillLooking = true;

                foreach (Unit cCheckedUnit in Player.Instance.cUnitCollection.AllUnits)
                {

                    if (cCheckedUnit.sCatalogueID == sUnitIDToFind && bStillLooking == true)
                    {
                        //Todo GET THIS CLAAUSE WORKING!
                        if (sDeck.Units.Contains(cCheckedUnit) == false)
                        {
                            sDeck.Units.Add(cCheckedUnit);
                            //if hes the first entry in this deck, hes our hero
                            if (iDeckSlot == 0)
                            {
                                sDeck.Hero = cCheckedUnit;
                            }


                            iDeckSlot++;
                            bStillLooking = false;
                        }
                        else
                        {
                            Debug.Log("unity alrady in deck, moving on");
                        }



                    }

                }


            }

            Debug.Log(sDeck.sDeckDataParse());
            iCurrentDeckBeingAssembled++;
        }



        //Iterate over every deck and save to database
        cNetworkManager.gameObject.GetComponent<LoginMenu>().label = "Sending mercenaries to the barracks";
        int iDeckIndex = 0;
        foreach (Deck sDeck in Player.Instance.cUnitCollection.Decks)
        {

            yield return StartCoroutine(HandleSaveDeckData(iDeckIndex));
            iDeckIndex++;
        }
    }


    //Saves the Deck data of a given Deck to the users table held on server
    //Deck data consists of a string made up of the Deck Name,Faction,HeroName and a list of units
    //Should be called on saving an edited Deck, and on saving the default decks given in the BeginnerCollection
    public IEnumerator HandleSaveDeckData(int iDeckIndex)
    {

        Deck cDeck = Player.Instance.cUnitCollection.Decks[iDeckIndex];

        string deckData = cDeck.sDeckDataParse();
        string s = AsynchronousClient.Instance.Send("SaveDeck:"+ iDeckIndex.ToString() +">" + deckData);
        yield return s;
    }


    //Creates a new Unit instance from the UnitCatalogue in game ready for use, and also saves it
    //in the UnitVault table stored on the server. 
    //NOTE: Strange bug when saving two identical units .. BUG FIXED, CASCHING ERROR
    //current work around is when calling this, to make sure your calling
    //for a different ID than last time.
    public IEnumerator HandleCreateUnitFromCatalogue(int CatalogueID)
    {
        //Find unit to make
        bool bFoundUnit = false;
        while (bFoundUnit == false)
        {
            foreach (string sCatalogueUnitDataParse in cUnitCollection.sUnitCatalogue)
            {
                string[] args = sCatalogueUnitDataParse.Split(">".ToCharArray());
                if (int.Parse(args[0]) == CatalogueID)
                {
                    //FOund unit to make in catalogue
                    sCatalogueUnitWeWantToMakeDataParse = sCatalogueUnitDataParse;
                    bFoundUnit = true;
                }
            }
        }

        //Seperate catalogue data and write to a selection of strings

        string[] sCatUnitDataArgs = sCatalogueUnitWeWantToMakeDataParse.Split(">".ToCharArray());

        string sCatalogueID = sCatUnitDataArgs[0];
        string Name = sCatUnitDataArgs[1];
        string UnitType = sCatUnitDataArgs[2];
        string Faction = sCatUnitDataArgs[3];
        string Rarity = sCatUnitDataArgs[4];
        string Collection = sCatUnitDataArgs[5];
        string Race = sCatUnitDataArgs[6];
        string DamageType = sCatUnitDataArgs[7];
        string Attack = "0";
        string Accuracy = "0";
        string Dodge = "0";
        string HitPoints = "0";
        string MovementSpeed = "0";
        string Magic = "0";
        string MagicResistance = "0";
        string FireResistance = "0";
        string SlashResistance = "0";
        string PiercingResistance = "0";
        string BludgeoningResistance = "0";
        string MinAttackRange = "0";
        string MaxAttackRange = "0";
        string Abilities = sCatUnitDataArgs[21];
        string Owner = Player.Instance.sName;

    
        ////Write Unit Data to UnitVault db

        string s = AsynchronousClient.Instance.Send("RequestNewUnitVaultUnit:" + sCatalogueID);
        yield return s;

        //string catalogueunitURL = cNetworkManager.serverurl + sNewCatalogueUnitPath + sWriteUnitParse + "&junk=" + UnityEngine.Random.Range(0,100).ToString();
        //WWW getcatalogueunitidReader = new WWW(catalogueunitURL);
        //yield return getcatalogueunitidReader;
        //Debug.Log(catalogueunitURL);
        //Create a new Unit component, fill out its details and add it to gameobject UnitVault
        //and Player.Collection.AllUnits list for use now

        Unit cUnit = goUnitVault.AddComponent<Unit>();


        cUnit.sName = Name;
        cUnit.UnitType = UnitType;
        cUnit.Faction = Faction;
        cUnit.Rarity = Rarity;
        cUnit.Collection = Collection;


        cUnit.Race = sCatUnitDataArgs[6];
        cUnit.DamageType = sCatUnitDataArgs[7];
        cUnit.MinRange = int.Parse(sCatUnitDataArgs[19]);
        cUnit.MaxRange = int.Parse(sCatUnitDataArgs[20]);
        cUnit.sArtist = sCatUnitDataArgs[22];
        cUnit.sFlavourText = sCatUnitDataArgs[23];

        cUnit.sCatalogueID = sCatUnitDataArgs[0];
        cUnit.Abilities = Abilities;
        cUnit.iLevel = 1;

        cUnit.sId = s ;
        Player.Instance.cUnitCollection.AllUnits.Add(cUnit);
    }


    public IEnumerator DownloadMapMetaData()
    {
        yield return StartCoroutine(HandleReadInMapMetaData());



    }

    public IEnumerator HandleReadInMapMetaData()
    {

        string sStream = AsynchronousClient.Instance.Send("RequestMapMetaData:0");
        yield return sStream;
            string[] sMaps = sStream.Split("#".ToCharArray());

            foreach (string sMapStream in sMaps)
            {
                if (sMapStream != "")
                {
                    string[] sMapMetaData = sMapStream.Split(",".ToCharArray());


                    MapMetaData cMapMetaData = GameObject.Find("_MapManager").AddComponent<MapMetaData>();

                    GameObject.Find("_MapManager").GetComponent<MapManager>().MapMetaDatas.Add(cMapMetaData);

                    cMapMetaData.sName = sMapMetaData[0];
                    cMapMetaData.sExpansion = sMapMetaData[1];
                    cMapMetaData.sPlayers = sMapMetaData[2];
                    int.TryParse(sMapMetaData[2], out cMapMetaData.iNoOfPlayers);
                    cMapMetaData.sMapType = sMapMetaData[3];
                }
            }
        
        
    }

    //Utility class to turn an ingame string into one that can be safely transfered via php.. such as a unit name with a space
    public string sPHPReady(string sInput)
    {
        return sInput.Replace(" ", "_");
    }

    //Utility class to turn a phpesque string into an ingame string.. such as a unit name with a space
    public string sCReady(string sInput)
    {
        return sInput.Replace("_", " ");
    }



//Reads in the Race data from the Database .
    public IEnumerator DownloadRaceData()
    {
        //Download Race data
        string s = AsynchronousClient.Instance.Send("RequestRaceData:0");
        yield return s;

        //Split the data stream up into data for seperate races
        string[] raceStrings = s.Split("#".ToCharArray());

        
       //Cycle through raceString and add a Race.cs to UnitVault and fill it out..
        foreach (string raceString in raceStrings)
        {
            //..after making sure it isnt null
            if (raceString!="")
            {

                Race race = GameObject.Find("_ClientUnitVault").AddComponent<Race>();
                Races.Instance.RaceList.Add(race);
                //split the race data up into seperate parts

           string[] raceData =
                raceString.Split(">".ToCharArray());

                race.Name = raceData[0];
                race.Description = raceData[1];
                race.Attack = int.Parse(raceData[2]);
                race.Dodge = int.Parse(raceData[3]);
                race.Accuracy = int.Parse(raceData[4]);
                race.HitPoints = int.Parse(raceData[5]);
                race.Magic = int.Parse(raceData[6]);
                race.MovementSpeed = int.Parse(raceData[7]);
                race.FireResistance = int.Parse(raceData[8]);
                race.MagicResistance = int.Parse(raceData[9]);
                race.SlashResistance = int.Parse(raceData[10]);
                race.PiercingResistance = int.Parse(raceData[11]);
                race.BludgeoningResistance = int.Parse(raceData[12]);



            }

        }

    }
}

