using System.Collections;
using UnityEngine;


    public enum HouseState
    {
        AtHome,
        OutHome,
    }
    public class HouseEntity  
    {
        private int id;
        private HouseState houseState;

        public HouseState HouseState { get => houseState; set => houseState = value; }
        public int Id { get => id; set => id = value; }

    }
