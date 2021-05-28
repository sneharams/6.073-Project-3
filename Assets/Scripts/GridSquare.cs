using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class GridSquare : MonoBehaviour
{
    #region Class Variables
    public GameObject objectContainer;

    public GameObject controller; // player
    public GameObject gridContainer;
    public GameObject options;
    public GameObject light;

    // Soil
    public GameObject patch;
    public GameObject patchNorth;
    public GameObject patchEast;

    // Plants
    public GameObject plantObj;
    public GameObject carrotObj;
    public GameObject flowerObj;
    // public GameObject seed;
    public ParticleSystem water;
    public GameObject hose;
    public GameObject waterPumpObj;
    public GameObject soilSackObj;

    public GameObject waterMeterObj;
    public GameObject soilMeterObj;
    public GameObject coinMeterObj;

    // Materials
    public Material validSoil;
    public Material invalidSoil;
    private Material changeMaterial;
    // public Material materialHover;
    private AudioSource audio;
    

    // Positioning
    [HideInInspector] public int xPos;
    [HideInInspector] public int yPos;
    [HideInInspector] public enum Status {
        HasObject,
        IsEmpty,
        HasSoil,
        HasPlant
    };

    // PUBLIC VARIABLES
    [HideInInspector] public Status status;
    // SCRIPT REFERENCES
    #region
        private PlayerController player;
        private Objects objects;
        private GridContainer grid;

        private MeterContainer waterMeter;
        private MeterContainer soilMeter;
        private PumpController waterPump;
        private SackController soilSack;
        private Plant plant;
        private Plant flower;
        private Plant carrot;
        private CoinMeter coinMeter;
    #endregion
    // STATE BOOLS
    #region
        [HideInInspector] public bool isDelete;
        [HideInInspector] public bool changeSoil;
        [HideInInspector] public bool patchState;
        private bool isHover;
        private bool isCtrlDown;
        private bool dispense;
        private bool openOptions;
        // private bool changeSoil;
        // private bool patchState;
    #endregion
    // TIMERS
    #region
        private float optionTimer;
        private float waterTimer;
    #endregion
    
    
    #endregion

    private void print(int group, params object[] list) 
    {
        string output = "";
        foreach (object item in list) {
            if (item is string) {
                output += item + ": ";
            } else {
                output += item + ", ";
            }
        }
        output = output.Substring(0, output.Length-2);
        if (group == 0) {
            // Debug.Log(output);
        }
        // Debug.Log(output);
    }

    #region State Updating
    void Start() {
        // Init State Bools
        isCtrlDown  = false;
        isHover     = false;
        isDelete    = false;
        changeSoil  = false;
        patchState = false;
        dispense = false;
        openOptions = false;
        // invalidSoil = validSoil;
        changeMaterial = invalidSoil;
        // Init Timers
        waterTimer = 0f;
        optionTimer = 0f;
        // Init Children
        light.SetActive(false);
        patch.SetActive(false);
        patchNorth.SetActive(false);
        patchEast.SetActive(false);
        // Init Script References
        player = controller.GetComponent<PlayerController>();
        objects = objectContainer.GetComponent<Objects>();
        grid = gridContainer.GetComponent<GridContainer>();
        waterMeter = waterMeterObj.GetComponent<MeterContainer>();
        soilMeter = soilMeterObj.GetComponent<MeterContainer>();
        waterPump = waterPumpObj.GetComponent<PumpController>();
        soilSack = soilSackObj.GetComponent<SackController>();
        flower = flowerObj.GetComponent<Plant>();
        carrot = carrotObj.GetComponent<Plant>();
        plantObj = carrotObj;
        plant = carrot;
        coinMeter = coinMeterObj.GetComponent<CoinMeter>();
        // Init Misc
        audio = GetComponent<AudioSource>();
        water.Pause();
        patch.GetComponent<MeshRenderer>().material = invalidSoil; 
    }

    void Update() {
        // For Ctrl+Click Movement
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
            isCtrlDown = true;
        } 
        else if  (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)){
            isCtrlDown = false;
        }
        
        if (patchState || status == Status.HasSoil || status == Status.HasPlant) {
            if (changeSoil) {
                patch.GetComponent<MeshRenderer>().material = changeMaterial;
                patch.SetActive(patchState);
                changeSoil = false;
            }
        } 
        else {
            if (patch.activeSelf) {
                patch.SetActive(patchState);
            }
        }
        checkSoil();

        if (dispense) {
            soilMeter.dispense();
            audio.Play();
            dispense = false;
        }

        if (water.isPlaying || player.isMoving) {
            isHover = false;
        }
        // MOVETYPES 
        if (grid.moveType == 0) {
            if (player.goalSquare.Item1 == xPos && player.goalSquare.Item2 == yPos) {
                if (isHover) {
                    print(1, player.xPos, player.yPos);
                }
                if (player.isMoving) {
                    light.SetActive(true);
                } else {
                    if (options.activeSelf) {
                        light.SetActive(true);
                    } else if (isHover) {
                        light.SetActive(true);
                    }
                    else {
                        print(0,"|__|__OPTIONS OPEN OR DON'T NEED TO OPEN");
                        light.SetActive(false);
                    }
                }
            }
            else if (isHover) {
                light.SetActive(true);
                if (plantObj.activeSelf) {
                    plant.getInfo();
                }
            } 
            else {
                light.SetActive(false);
            }
        } 
        else if (grid.moveType == 1) {
            if (player.goalSquare.Item1 == xPos && player.goalSquare.Item2 == yPos) {
                if (player.isMoving) {
                    light.SetActive(true);
                } 
            } else {
                    if (isHover || options.activeSelf) {
                        light.SetActive(true);
                        if (plantObj.activeSelf) {
                            plant.getInfo();
                        }
                        if (!options.activeSelf) {
                            optionTimer += Time.deltaTime;
                        }
                        if (optionTimer > 1) {
                            if (checkPlace()) {
                                initOptions();
                                grid.setActiveState(true, this);
                                player.setGoalSquare(xPos, yPos);
                            } else {
                                player.setGoalSquare(0,0);
                                grid.setActiveState(false, this);
                                player.turnPlayer(xPos, yPos);
                            }
                            optionTimer = 0;
                        }
                    } else {
                        light.SetActive(false);
                    }
            }
        } 
        else if (grid.moveType == 2) {
            if (player.goalSquare.Item1 == xPos && player.goalSquare.Item2 == yPos) {
                if (isHover) {
                    print(1, player.xPos, player.yPos);
                }
                if (player.isMoving) {
                    light.SetActive(true);
                } else {
                    if (options.activeSelf) {
                        light.SetActive(true);
                    } else if (isHover) {
                        light.SetActive(true);
                    }
                    else {
                        print(0,"|__|__OPTIONS OPEN OR DON'T NEED TO OPEN");
                        light.SetActive(false);
                    }
                }
            }
            else if (isHover) {
                light.SetActive(true);
                if (plantObj.activeSelf) {
                    plant.getInfo();
                }
            } 
            else {
                light.SetActive(false);
            }
        } 
        else if (grid.moveType == 3) {
            if (player.isMoving) {
                // if ( player.goalSquare.Item1 == xPos && player.goalSquare.Item2 == yPos) {
                //     light.SetActive(true);
                // }
            } 
            else {
                if ((isHover && checkPlace()) || options.activeSelf) {
                    // GetComponent<MeshRenderer>().material = materialHover;
                    light.SetActive(true);
                    if (plantObj.activeSelf) {
                        plant.getInfo();
                    }
                } 
                else {
                    light.SetActive(false);
                    // GetComponent<MeshRenderer>().material = materialNormal;
                }
            }
        } 
        else { //4
            #endregion
            if (player.goalSquare.Item1 == xPos && player.goalSquare.Item2 == yPos) {
                if (isHover) {
                    print(1, player.xPos, player.yPos);
                }
                if (player.isMoving) {
                    light.SetActive(true);
                } else {
                    if (options.activeSelf) {
                        light.SetActive(true);
                    } 
                    else if (openOptions) {
                        // print(0,"yo");
                        initOptions();
                        openOptions = false;
                    } 
                    else if (isHover) {
                        light.SetActive(true);
                        // optionTimer += Time.deltaTime;
                        // if (optionTimer > .25) {
                            if (status == Status.HasPlant) {
                                plant.showStatus(true);
                            }
                        //     optionTimer = 0;
                        // }
                    }
                    else {
                        light.SetActive(false);
                        if (status == Status.HasPlant) {
                            plant.showStatus(false);
                        }
                    }
                    // if (options.activeSelf) {
                    //     light.SetActive(true);
                    // } else {
                    //     light.SetActive(false);
                    // }
                }
            }
            else if (isHover) {
                light.SetActive(true);
                        // optionTimer += Time.deltaTime;
                        // if (optionTimer > .25) {
                            if (status == Status.HasPlant) {
                                plant.showStatus(true);
                            }
                            // optionTimer = 0;
                        // }
                    }
                    else {
                        light.SetActive(false);
                        if (status == Status.HasPlant) {
                            plant.showStatus(false);
                        }
                    }
            if (options.activeSelf && status == Status.HasPlant) {
                plant.showStatus(false);
            }
        }
    }

    public void triggerEvent(int clickMode) {
        // print(0,"Clickmode", clickMode);
        if (clickMode == 0) {           // MOVE
            player.handleClick(xPos, yPos);
        } 
        else if (clickMode == 1) {    // DELETE
            status = Status.IsEmpty;
            setSoil(false);
        } 
        else if (clickMode == 2) {    // SOIL
            if (xPos == 3 && yPos == 7) {
                soilSack.shovelMethod();
            } else if (soilMeter.canDispense()) {
                status = Status.HasSoil;
                dispense = true;
                setSoil(true);
            } else {
                status = Status.IsEmpty;
                setSoil(false);
                plant.reset();
            }
        } 
        else if (clickMode == 3) {    // SEED
            status = Status.HasPlant;
            plant.init();
        } 
        else if (clickMode == 4) {    // PUMP
            // TODO fix hardcoding
            if (xPos == 1 && yPos == 7) {
                waterPump.pump();
            }
            else if (waterMeter.dispense()) {
                water.Play();
                plant.water();
            }
        } 
        else if (clickMode == 5) {  // SACK
            // if (xPos == 1 && yPos == 7) {
            //     waterPump.pump();
            // }
            // else if (waterMeter.dispense()) {
            //     water.Play();
            //     plant.water();
            // }
        } 
        else if (clickMode == 6) {  // HARVEST
            coinMeter.add(plant.harvestPlant());
            // status = Status.HasSoil;
        }
        if (clickMode != -1) {
            grid.setActiveState(false, this);
            light.SetActive(false);
        } else {
            light.SetActive(false);
        }
        options.GetComponent<SquareMenu>().reset();
        options.SetActive(false);
    }

# region Check
    private void checkSoil() {
        patchNorth.SetActive(false);
        patchEast.SetActive(false);
        // plantObj.SetActive(false);
        if (patch.activeSelf) {
            if (grid.hasSoil(xPos+1, yPos)) {
                if (isDelete) {
                    patchEast.GetComponent<MeshRenderer>().material = invalidSoil;
                } else {
                    if (grid.getDeleteState(xPos+1, yPos)) {
                        patchEast.GetComponent<MeshRenderer>().material = invalidSoil;
                    } else {
                        // if (grid.getDeleteState(xPos+1, yPos) || isDelete) {
                        //     patchEast.GetComponent<MeshRenderer>().material = invalidSoil;
                        // }
                        patchEast.GetComponent<MeshRenderer>().material = validSoil;
                    }
                }
                patchEast.SetActive(true);
            }
            if (grid.hasSoil(xPos, yPos+1)) {
                if (grid.getDeleteState(xPos, yPos+1)) {
                    patchNorth.GetComponent<MeshRenderer>().material = invalidSoil;
                } else {
                    // if (grid.getDeleteState(xPos, yPos+1) || isDelete) {
                    //     patchNorth.GetComponent<MeshRenderer>().material = invalidSoil;
                    // }
                    patchNorth.GetComponent<MeshRenderer>().material = validSoil;
                }
                patchNorth.SetActive(true);
            }
            // if (grid.hasSoil(xPos+1, yPos)) {
            //     if (status == Status.IsEmpty && !soilMeter.canDispense()) {
            //     // if ((status == Status.IsEmpty || grid.isEmpty(xPos+1, yPos)) && !soilMeter.canDispense()) {
            //         patchEast.GetComponent<MeshRenderer>().material = invalidSoil;
            //     } else {
            //         if (grid.isEmpty(xPos+1, yPos)) {
            //             patchEast.GetComponent<MeshRenderer>().material = invalidSoil;
            //         }
            //         patchEast.GetComponent<MeshRenderer>().material = validSoil;
            //     }
            //     patchEast.SetActive(true);
            // } 
            // if (grid.hasSoil(xPos, yPos+1)) {
            //     if (status == Status.IsEmpty && !soilMeter.canDispense()) {
            //     // if ((status == Status.IsEmpty || grid.isEmpty(xPos, yPos+1)) && !soilMeter.canDispense()) {
            //         patchNorth.GetComponent<MeshRenderer>().material = invalidSoil;
            //     } else {
            //         if (grid.isEmpty(xPos, yPos+1)) {
            //             patchNorth.GetComponent<MeshRenderer>().material = invalidSoil;
            //         }
            //         patchNorth.GetComponent<MeshRenderer>().material = validSoil;
            //     }
            //     patchNorth.SetActive(true);
            // } 
            if (status != Status.HasPlant) {
                plantObj.SetActive(false);
            } else {
                if (!plantObj.activeSelf) {
                    plantObj.SetActive(true);
                }
            }
        } else {
            plantObj.SetActive(false);
        }
    }

    private bool checkValid() {
        int pX = player.xPos;
        int pY = player.yPos;
        // if (status != Status.HasObject) {
            bool validPlace = checkPlace();
            bool validMove = checkMove();
            if (validPlace || validMove) {
                return true;
            }
        // }
        return false;
    }

    private bool checkPlace() {
        int pX = player.xPos;
        int pY = player.yPos;
        bool isValid = (
            (xPos >= pX-1 && xPos <= pX+1) && 
            (yPos >= pY-1 && yPos <= pY+1) &&
            !(yPos == pY && xPos == pX)
        );
        return isValid;
    }

    private bool checkMove() {
        int pX = player.xPos;
        int pY = player.yPos;
        if (status == Status.IsEmpty && !(pX == xPos && pY == yPos)) {
            return true;
        }
        return false;
    }
# endregion


# region Mouse
    void OnMouseOver() {
        isHover = true;
    }

    void OnMouseExit() {
        isHover = false;
    }

    void OnMouseUp() {
        if (grid.moveType == 0) {
            move0();
        } else if (grid.moveType == 1) {
            move1();
        } else if (grid.moveType == 2) {
            move2();
        } else if (grid.moveType == 3) {
            move3();
        } else {
            move4();
        }

    }
# endregion


# region Move
    # region Legacy
    private void move0() {
                print(0,"MOVE");
        // OPTIONS OPEN
        if (options.activeSelf) {   // CLOSE
            print(0,"options open");
            options.GetComponent<SquareMenu>().reset();
            print(0,"rip");
            options.SetActive(false);
            grid.setActiveState(false, this);
            light.SetActive(false);
        } 
         else {
             defaultInit();
        }
        // ADJACENT
        // else if (checkValid() && isHover) {
        //         bool move = checkMove();
        //         bool delete = false;
        //         bool soil = false;
        //         bool water = false;
        //         bool seed = false;
        //         bool pump = false;
        //         bool sack = false;
        //         bool harvest = false;
        //         if (xPos == 1 && yPos == 7) {
        //             if (player.xPos == 2 && player.yPos ==7) { // water
        //                 pump = true;
        //             }
        //         } 
        //         else if (xPos == 3 && yPos == 7) {
        //             if ((player.xPos == 3 && player.yPos == 6) ||
        //                 (player.xPos == 2 && player.yPos == 7) ||
        //                 (player.xPos == 2 && player.yPos == 6) ||
        //                 (player.xPos == 4 && player.yPos == 6)
        //             ) { // soil
        //                 sack = true;
        //             }
        //         } 
        //         else if (status == Status.HasObject) {
        //             print(0,"| |_TABLE");
        //         } 
        //         else if (checkPlace() && !grid.hasObject(xPos, yPos)) {
        //             if (status == Status.IsEmpty) {
        //                 soil = true;
        //             } else {
        //                 delete = true;
        //                 if (status == Status.HasPlant) {
        //                     water = true;
        //                     if (plant.canHarvest) {
        //                         harvest = true;
        //                     }
        //                 } else {
        //                     if (grid.canSeed) {
        //                         seed = true;
        //                     }
        //                 }
        //             }
        //         }
        //         if (soil || move || delete || water || seed || pump || sack || harvest) {
        //             displayOptions(soil, water, seed, move, delete, pump, sack, harvest);
        //             player.setGoalSquare(xPos, yPos);
        //             grid.setActiveState(true, this);
        //         } else {
        //             print(0,"| |_NO OPTIONS");
        //             light.SetActive(false);
        //             player.setGoalSquare(xPos, yPos);
        //             grid.setActiveState(false, this);
        //         }
        //         player.turnPlayer(xPos, yPos);
        //     }
        
    }

    private void move1() {
        if (options.activeSelf) {
            options.SetActive(false);
            light.SetActive(false);
            grid.setActiveState(false, this);
        } else if (checkMove()) {
            player.handleClick(xPos, yPos);
            player.setGoalSquare(xPos, yPos);
        }
    }
    
    private void move2() {
        if (options.activeSelf) {   // CLOSE
            if (isCtrlDown) {
                player.handleClick(xPos, yPos);
            }
            options.GetComponent<SquareMenu>().reset();
            options.SetActive(false);
            grid.setActiveState(false, this);
            light.SetActive(false);

        } else if (isCtrlDown && checkMove()) {
            // print(0,"hi");
            player.handleClick(xPos, yPos);
            player.setGoalSquare(xPos, yPos);
            grid.setActiveState(false, this);
        }
        // ADJACENT
        else if (checkPlace()) {
            defaultInit();
        } 
        
    }

    private void defaultInit() {
        if (checkValid() && isHover) {
            // check options
            bool move = false;
            if (grid.moveType == 2 || grid.moveType == 4) {
                move = checkMove();
            }
            bool delete = false;
            bool soil = false;
            bool water = false;
            bool seed = false;
            bool pump = false;
            bool sack = false;
            bool harvest = false;
            if (xPos == 1 && yPos == 7) {
                print(0,"| |_PUMP");
                if (player.xPos == 2 && player.yPos ==7) { // water
                    pump = true;
                }
            } 
            else if (xPos == 3 && yPos == 7) {
                print(0,"| |_SACK");
                if ((player.xPos == 3 && player.yPos == 6) ||
                    (player.xPos == 2 && player.yPos == 7) ||
                    (player.xPos == 2 && player.yPos == 6) ||
                    (player.xPos == 4 && player.yPos == 6)
                ) { // soil
                    sack = true;
                }
            } 
            else if (status == Status.HasObject) {
                print(0,"| |_TABLE");

            } 
            else if (!grid.hasObject(xPos, yPos)) {
                print(0,"| |_NO OBJ");
                    if (status == Status.IsEmpty) {
                        soil = true;
                    } else {
                        delete = true;
                        if (status == Status.HasPlant) {
                            water = true;
                            if (plant.canHarvest) {
                                harvest = true;
                            }
                        } else {
                            if (grid.seedType != 0) {
                                if (grid.seedType == 1) {
                                    plantObj = carrotObj;
                                    plant = carrot;
                                } else {
                                    plantObj = flowerObj;
                                    plant = flower;
                                }
                                seed = true;
                            }
                        }
                    }
            }
            // init options
            if (soil || move || delete || water || seed || pump || sack || harvest) {
                displayOptions(soil, water, seed, move, delete, pump, sack, harvest);
                if (grid.moveType != 3) {
                    player.setGoalSquare(xPos, yPos);
                }
                grid.setActiveState(true, this);
            } 
            else {
                light.SetActive(false);
                if (grid.moveType != 3) {
                    player.setGoalSquare(xPos, yPos);
                }
                grid.setActiveState(false, this);
            }
            player.turnPlayer(xPos, yPos);
        }
    }

    private void move3() {
        if (options.activeSelf) {   // CLOSE
            options.GetComponent<SquareMenu>().reset();
            options.SetActive(false);
            grid.setActiveState(false, this);
        } else {                    // OPEN OPTIONS
            if (checkPlace()) {
                defaultInit();
                // bool move = false; //checkMove();
                // bool delete = false;
                // bool soil = false;
                // bool water = false;
                // bool seed = false;
                // bool pump = false;
                // bool sack = false;
                // bool harvest = false;
                // print(0,xPos, yPos);
                // // TODO: fix hardcoding
                // if (xPos == 1 && yPos == 7) {
                //     if (player.xPos == 2 && player.yPos ==7) { // water
                //         pump = true;
                //     }
                // } 
                // else if (xPos == 3 && yPos == 7) {
                //     if ((player.xPos == 3 && player.yPos == 6) ||
                //         (player.xPos == 2 && player.yPos == 7) ||
                //         (player.xPos == 2 && player.yPos == 6) ||
                //         (player.xPos == 4 && player.yPos == 6)
                //     ) { // soil
                //         sack = true;
                //     }
                // } else if (checkPlace() && !grid.hasObject(xPos, yPos)) {
                //     if (status == Status.IsEmpty) {
                //         soil = true;
                //     } else {
                //         delete = true;
                //         if (status == Status.HasPlant) {
                //             water = true;
                //             if (plant.canHarvest) {
                //                 harvest = true;
                //             }
                //         } else {
                //             if (grid.canSeed) {
                //                 seed = true;
                //             }
                //         }
                //     }
                // }
                // if (soil || move || delete || water || seed || pump || sack || harvest) {
                //     displayOptions(soil, water, seed, move, delete, pump, sack, harvest);
                //     grid.setActiveState(true, this);
                //     player.turnPlayer(xPos, yPos);
                // }
            }
        }
    }
    #endregion
    private void move4() {
        print(0,"MOVE");
        // OPTIONS OPEN
        if (options.activeSelf) {   // CLOSE
            print(0,"options open");
            options.GetComponent<SquareMenu>().reset();
            print(0,"rip");
            options.SetActive(false);
            grid.setActiveState(false, this);
            light.SetActive(false);
        } 
        // ADJACENT
        else if (checkPlace()) {
            defaultInit();
            // print(0,"|_ADJACENT");
            // if (checkValid() && isHover) {
            //     bool move = checkMove();
            //     bool delete = false;
            //     bool soil = false;
            //     bool water = false;
            //     bool seed = false;
            //     bool pump = false;
            //     bool sack = false;
            //     bool harvest = false;
            //     // print(0,xPos, yPos);
            //     // TODO: fix hardcoding
            //     if (xPos == 1 && yPos == 7) {
            //         print(0,"| |_PUMP");
            //         if (player.xPos == 2 && player.yPos ==7) { // water
            //             pump = true;
            //         }
            //     } 
            //     else if (xPos == 3 && yPos == 7) {
            //         print(0,"| |_SACK");
            //         if ((player.xPos == 3 && player.yPos == 6) ||
            //             (player.xPos == 2 && player.yPos == 7) ||
            //             (player.xPos == 2 && player.yPos == 6) ||
            //             (player.xPos == 4 && player.yPos == 6)
            //         ) { // soil
            //             sack = true;
            //         }
            //     } 
            //     else if (status == Status.HasObject) {
            //         print(0,"| |_TABLE");

            //     } 
            //     else if (!grid.hasObject(xPos, yPos)) {
            //         print(0,"| |_NO OBJ");
            //         if (status == Status.IsEmpty) {
            //             soil = true;
            //         } else {
            //             delete = true;
            //             if (status == Status.HasPlant) {
            //                 water = true;
            //                 if (plant.canHarvest) {
            //                     harvest = true;
            //                 }
            //             } else {
            //                 if (grid.canSeed) {
            //                     seed = true;
            //                 }
            //             }
            //         }
            //     }
            //     if (soil || move || delete || water || seed || pump || sack || harvest) {
            //         print(0,"| |_OPTIONS");
            //         displayOptions(soil, water, seed, move, delete, pump, sack, harvest);
            //         player.setGoalSquare(xPos, yPos);
            //         grid.setActiveState(true, this);
            //         // player.turnPlayer(xPos, yPos);
            //     } else {
            //         print(0,"| |_NO OPTIONS");
            //         light.SetActive(false);
            //         player.setGoalSquare(xPos, yPos);
            //         grid.setActiveState(false, this);
            //         // player.turnPlayer(xPos, yPos);
                // }
            //     player.turnPlayer(xPos, yPos);
            // }
        } 
        // MOVE EMPTY
        else if (checkMove()) {
            print(0,"|_MOVE TO");
            print(xPos, yPos);
            grid.setActiveState(false, this);
            player.handleClick(xPos, yPos);
        } 
        // MOVE NOT EMPTY
        else {                    
            print(0,"|_MOVE ADJACENT");
            if (player.hasPath(xPos, yPos)) {
                // print(0,"yo");
                if ((xPos == 1 && yPos == 7) || (xPos == 3 && yPos == 7)) { 
                    print(0,"| |_OBJ TOOL");
                    // openOptions = true;
                    grid.setActiveState(true, this);
                } 
                else if (status == Status.HasObject) {
                    print(0,"| |_OBJ STATIC");
                    grid.setActiveState(false, this);
                }
                else {
                    print(0,"| |_OBJ GARDEN");
                    // openOptions = true;
                    grid.setActiveState(true, this);
                }
                openOptions = true;
            }
        }
    }
# endregion


# region Set
    public void setSoil(bool state) {
        // if (status == Status.IsEmpty) {
        if (state != patchState) {
            changeSoil = true;
            patchState = state;
            if (patchState) {
                if (soilMeter.canDispense()) {
                    changeMaterial = validSoil;
                    // patch.GetComponent<MeshRenderer>().material = validSoil;
                } else {
                    changeMaterial = invalidSoil;
                    // patch.GetComponent<MeshRenderer>().material = invalidSoil;
                }
            }
        }
        // }

        // showPatch = state;
    }

    public void setDelete(bool state) {
        if (state) {
            changeMaterial = invalidSoil;
            changeSoil = true;
            isDelete = true;
        } else {
            changeMaterial = validSoil;
            changeSoil = true;
            isDelete = false;
        }
    }

    public void setPlant(bool state) {
        plantObj.SetActive(state);
    }
    
# endregion
   
    private void initOptions() {
        print(0,"");
        print(0,"|__INIT OPTIONS");
        if (checkValid()) {
            print(0,"|__|__VALID");
            bool move = checkMove();
            bool delete = false;
            bool soil = false;
            bool water = false;
            bool seed = false;
            bool pump = false;
            bool sack = false;
            bool harvest = false;
            // print(0,xPos, yPos);
            // TODO: fix hardcoding
            if (xPos == 1 && yPos == 7) {
                print(0,"|__|__|__PUMP");
                if (player.xPos == 2 && player.yPos ==7) { // water
                    print(0,"|__|__|__|__CAN PUMP");
                    pump = true;
                }
            } 
            else if (xPos == 3 && yPos == 7) {
                print(0,"|__|__|__SACK");
                if ((player.xPos == 3 && player.yPos == 6) ||
                    (player.xPos == 2 && player.yPos == 7) ||
                    (player.xPos == 2 && player.yPos == 6) ||
                    (player.xPos == 4 && player.yPos == 6)
                ) { // soil
                    print(0,"|__|__|__|__CAN SHOVEL");
                    sack = true;
                }
            } 
            else if (checkPlace() && !grid.hasObject(xPos, yPos)) {
                print(0,"|__|__|__ADJACENT NOT OBJECT");
                if (status == Status.IsEmpty) {
                    print(0,"|__|__|__|__EMPTY");
                    soil = true;
                } 
                else {
                    print(0,"|__|__|__|__OCCUPIED");
                    delete = true;
                    if (status == Status.HasPlant) {
                        water = true;
                        if (plant.canHarvest) {
                            harvest = true;
                        }
                    } else {
                        if (grid.seedType != 0) {
                            if (grid.seedType == 1) {
                                plantObj = carrotObj;
                                plant = carrot;
                            } else {
                                plantObj = flowerObj;
                                plant = flower;
                            }
                            seed = true;
                        }
                    }
                }
            }
            if (soil || move || delete || water || seed || pump || sack || harvest) {
                print(0,"|__|__|__DISPLAY OPTIONS");
                displayOptions(soil, water, seed, move, delete, pump, sack, harvest);
                // grid.setActiveState(true, this);
                player.turnPlayer(xPos, yPos);
            } 
            else {
                print(0,"|__|__|__NO OPTIONS");
                grid.setActiveState(false, this);
                // grid.setActiveState(true, this);
                // if (grid.moveType == 4 || grid.moveT) {
                    player.turnPlayer(xPos, yPos);
                // }
            }
        }
    }
    private void displayOptions(bool soil, bool water, bool seed, bool move, bool delete, bool pump, bool sack, bool harvest) {
        // print(0,"Soil", soil, "Water", water, "Seed", seed, "Move", move);
        options.SetActive(true);
        SquareMenu buttons = options.GetComponent<SquareMenu>();
        buttons.initOptions(soil, water, seed, move, delete, pump, sack, harvest);
    }


}
