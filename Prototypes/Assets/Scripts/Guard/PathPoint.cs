using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound
{
    public class PathPoint : MonoBehaviour
    {
        [SerializeField]
        private int minYPos, maxYPos;
        [SerializeField]
        private int minXPos, maxXPos;
        [SerializeField]
        private int dungeonCellPos,exitPosition;
        

        public List<Vector2> pathPoint { get; set; }
        public Dictionary<Vector2,bool> pointMoveUpper = new Dictionary<Vector2, bool>();
        public Dictionary<Vector2, bool> pointMoveLeft = new Dictionary<Vector2, bool>();
        public Dictionary<Vector2, bool> pointMoveRight = new Dictionary<Vector2, bool>();
        public Dictionary<Vector2, bool> pointMoveBottom = new Dictionary<Vector2, bool>();

        public List<bool> moveUpper = new List<bool>();
        public List<bool> moveLeft = new List<bool>();
        public List<bool> moveRight = new List<bool>();
        public List<bool> moveDown = new List<bool>();


        [Serializable]
        public class TilePoints
        {
            public bool Upper;
            public bool Down;
            public bool Left;
            public bool Right;
        }
        [SerializeField]
        public TilePoints [] m_tileList = new TilePoints [506];

        // Start is called before the first frame update
        void Start()
        {
            pathPoint = new List<Vector2>();
            LayoutPathPoint();
        }
        private void LayoutSixthTileInY()
        {
            m_tileList [115].Left = false;
            m_tileList [115].Right = true;
            m_tileList [115].Down = false;
            m_tileList [115].Upper = true;
            m_tileList [116].Left = true;
            m_tileList [116].Right = false;
            m_tileList [116].Down = true;
            m_tileList [116].Upper = false;
            m_tileList [117].Left = false;
            m_tileList [117].Right = false;
            m_tileList [117].Down = false;
            m_tileList [117].Upper = false;
            m_tileList [118].Left = true;
            m_tileList [118].Right = false;
            m_tileList [118].Down = false;
            m_tileList [118].Upper = false;
            m_tileList [119].Left = true;
            m_tileList [119].Right = true;
            m_tileList [119].Down = false;
            m_tileList [119].Upper = false;
            m_tileList [120].Left = true;
            m_tileList [120].Right = true;
            m_tileList [120].Down = false;
            m_tileList [120].Upper = true;
            m_tileList [121].Left = true;
            m_tileList [121].Right = false;
            m_tileList [121].Down = true;
            m_tileList [121].Upper = false;
            m_tileList [122].Left = true;
            m_tileList [122].Right = true;
            m_tileList [122].Down = false;
            m_tileList [122].Upper = false;
            m_tileList [123].Left = false;
            m_tileList [123].Right = true;
            m_tileList [123].Down = false;
            m_tileList [123].Upper = false;
            m_tileList [124].Left = true;
            m_tileList [124].Right = true;
            m_tileList [124].Down = false;
            m_tileList [124].Upper = false;
            m_tileList [125].Left = true;
            m_tileList [125].Right = false;
            m_tileList [125].Down = false;
            m_tileList [125].Upper = true;
            m_tileList [126].Left = true;
            m_tileList [126].Right = false;
            m_tileList [126].Down = true;
            m_tileList [126].Upper = false;
            m_tileList [127].Left = true;
            m_tileList [127].Right = true;
            m_tileList [127].Down = false;
            m_tileList [127].Upper = false;
            m_tileList [128].Left = true;
            m_tileList [128].Right = true;
            m_tileList [128].Down = false;
            m_tileList [128].Upper = false;
            m_tileList [129].Left = true;
            m_tileList [129].Right = true;
            m_tileList [129].Down = false;
            m_tileList [129].Upper = true;
            m_tileList [130].Left = false;
            m_tileList [130].Right = false;
            m_tileList [130].Down = true;
            m_tileList [130].Upper = true;
            m_tileList [131].Left = false;
            m_tileList [131].Right = true;
            m_tileList [131].Down = true;
            m_tileList [131].Upper = false;
            m_tileList [132].Left = true;
            m_tileList [132].Right = true;
            m_tileList [132].Down = false;
            m_tileList [132].Upper = false;
            m_tileList [133].Left = true;
            m_tileList [133].Right = true;
            m_tileList [133].Down = false;
            m_tileList [133].Upper = false;
            m_tileList [134].Left = true;
            m_tileList [134].Right = true;
            m_tileList [134].Down = false;
            m_tileList [134].Upper = true;
            m_tileList [135].Left = true;
            m_tileList [135].Right = true;
            m_tileList [135].Down = true;
            m_tileList [135].Upper = false;
            m_tileList [136].Left = true;
            m_tileList [136].Right = true;
            m_tileList [136].Down = false;
            m_tileList [136].Upper = false;
            m_tileList [137].Left = true;
            m_tileList [137].Right = true;
            m_tileList [137].Down = false;
            m_tileList [137].Upper = false;
        }
        private void LayoutFifthTileInY()
        {
            m_tileList [92].Left = true;
            m_tileList [92].Right = false;
            m_tileList [92].Down = false;
            m_tileList [92].Upper = false;
            m_tileList [93].Left = true;
            m_tileList [93].Right = true;
            m_tileList [93].Down = false;
            m_tileList [93].Upper = false;
            m_tileList [94].Left = true;
            m_tileList [94].Right = false;
            m_tileList [94].Down = false;
            m_tileList [94].Upper = false;
            m_tileList [95].Left = true;
            m_tileList [95].Right = true;
            m_tileList [95].Down = false;
            m_tileList [95].Upper = false;
            m_tileList [96].Left = true;
            m_tileList [96].Right = true;
            m_tileList [96].Down = false;
            m_tileList [96].Upper = false;
            m_tileList [97].Left = true;
            m_tileList [97].Right = true;
            m_tileList [97].Down = false;
            m_tileList [97].Upper = false;
            m_tileList [98].Left = false;
            m_tileList [98].Right = true;
            m_tileList [98].Down = false;
            m_tileList [98].Upper = true;
            m_tileList [99].Left = false;
            m_tileList [99].Right = true;
            m_tileList [99].Down = true;
            m_tileList [99].Upper = true;
            m_tileList [100].Left = false;
            m_tileList [100].Right = false;
            m_tileList [100].Down = true;
            m_tileList [100].Upper = true;
            m_tileList [101].Left = false;
            m_tileList [101].Right = true;
            m_tileList [101].Down = true;
            m_tileList [101].Upper = false;
            m_tileList [102].Left = true;
            m_tileList [102].Right = true;
            m_tileList [102].Down = false;
            m_tileList [102].Upper = true;
            m_tileList [103].Left = true;
            m_tileList [103].Right = true;
            m_tileList [103].Down = true;
            m_tileList [103].Upper = false;
            m_tileList [104].Left = true;
            m_tileList [104].Right = true;
            m_tileList [104].Down = false;
            m_tileList [104].Upper = false;
            m_tileList [105].Left = true;
            m_tileList [105].Right = true;
            m_tileList [105].Down = false;
            m_tileList [105].Upper = false;
            m_tileList [106].Left = true;
            m_tileList [106].Right = true;
            m_tileList [106].Down = false;
            m_tileList [106].Upper = false;
            m_tileList [107].Left = true;
            m_tileList [107].Right = false;
            m_tileList [107].Down = false;
            m_tileList [107].Upper = true;
            m_tileList [108].Left = true;
            m_tileList [108].Right = false;
            m_tileList [108].Down = true;
            m_tileList [108].Upper = true;
            m_tileList [109].Left = true;
            m_tileList [109].Right = true;
            m_tileList [109].Down = true;
            m_tileList [109].Upper = false;
            m_tileList [110].Left = true;
            m_tileList [110].Right = true;
            m_tileList [110].Down = false;
            m_tileList [110].Upper = false;
            m_tileList [111].Left = true;
            m_tileList [111].Right = true;
            m_tileList [111].Down = false;
            m_tileList [111].Upper = true;
            m_tileList [112].Left = true;
            m_tileList [112].Right = true;
            m_tileList [112].Down = true;
            m_tileList [112].Upper = false;
            m_tileList [113].Left = true;
            m_tileList [113].Right = true;
            m_tileList [113].Down = false;
            m_tileList [113].Upper = false;
            m_tileList [114].Left = true;
            m_tileList [114].Right = true;
            m_tileList [114].Down = false;
            m_tileList [114].Upper = false;
        }
        private void LayoutFourthTileInY()
        {
            m_tileList [69].Left = true;
            m_tileList [69].Right = true;
            m_tileList [69].Down = false;
            m_tileList [69].Upper = false;
            m_tileList [70].Left = true;
            m_tileList [70].Right = true;
            m_tileList [70].Down = false;
            m_tileList [70].Upper = false;
            m_tileList [71].Left = true;
            m_tileList [71].Right = true;
            m_tileList [71].Down = false;
            m_tileList [71].Upper = false;
            m_tileList [72].Left = true;
            m_tileList [72].Right = true;
            m_tileList [72].Down = false;
            m_tileList [72].Upper = false;
            m_tileList [73].Left = true;
            m_tileList [73].Right = true;
            m_tileList [73].Down = false;
            m_tileList [73].Upper = true;
            m_tileList [74].Left = false;
            m_tileList [74].Right = true;
            m_tileList [74].Down = true;
            m_tileList [74].Upper = true;
            m_tileList [75].Left = false;
            m_tileList [75].Right = false;
            m_tileList [75].Down = true;
            m_tileList [75].Upper = true;
            m_tileList [76].Left = false;
            m_tileList [76].Right = false;
            m_tileList [76].Down = true;
            m_tileList [76].Upper = true;
            m_tileList [77].Left = false;
            m_tileList [77].Right = false;
            m_tileList [77].Down = true;
            m_tileList [77].Upper = true;
            m_tileList [78].Left = false;
            m_tileList [78].Right = false;
            m_tileList [78].Down = true;
            m_tileList [78].Upper = true;
            m_tileList [79].Left = false;
            m_tileList [79].Right = true;
            m_tileList [79].Down = true;
            m_tileList [79].Upper = true;
            m_tileList [80].Left = false;
            m_tileList [80].Right = true;
            m_tileList [80].Down = true;
            m_tileList [80].Upper = false;
            m_tileList [81].Left = true;
            m_tileList [81].Right = true;
            m_tileList [81].Down = false;
            m_tileList [81].Upper = false;
            m_tileList [82].Left = true;
            m_tileList [82].Right = true;
            m_tileList [82].Down = false;
            m_tileList [82].Upper = false;
            m_tileList [83].Left = true;
            m_tileList [83].Right = true;
            m_tileList [83].Down = false;
            m_tileList [83].Upper = false;
            m_tileList [84].Left = true;
            m_tileList [84].Right = true;
            m_tileList [84].Down = false;
            m_tileList [84].Upper = true;
            m_tileList [85].Left = false;
            m_tileList [85].Right = true;
            m_tileList [85].Down = true;
            m_tileList [85].Upper = true;
            m_tileList [86].Left = false;
            m_tileList [86].Right = true;
            m_tileList [86].Down = true;
            m_tileList [86].Upper = true;
            m_tileList [87].Left = false;
            m_tileList [87].Right = true;
            m_tileList [87].Down = true;
            m_tileList [87].Upper = false;
            m_tileList [88].Left = false;
            m_tileList [88].Right = true;
            m_tileList [88].Down = false;
            m_tileList [88].Upper = true;
            m_tileList [89].Left = true;
            m_tileList [89].Right = true;
            m_tileList [89].Down = true;
            m_tileList [89].Upper = false;
            m_tileList [90].Left = true;
            m_tileList [90].Right = true;
            m_tileList [90].Down = false;
            m_tileList [90].Upper = false;
            m_tileList [91].Left = true;
            m_tileList [91].Right = true;
            m_tileList [91].Down = false;
            m_tileList [91].Upper = false;
        }
        private void LayoutThirdTileInY()
        {
            m_tileList [46].Left = true;
            m_tileList [46].Right = true;
            m_tileList [46].Down = false;
            m_tileList [46].Upper = false;
            m_tileList [47].Left = true;
            m_tileList [47].Right = true;
            m_tileList [47].Down = false;
            m_tileList [47].Upper = false;
            m_tileList [48].Left = false;
            m_tileList [48].Right = true;
            m_tileList [48].Down = false;
            m_tileList [48].Upper = false;
            m_tileList [49].Left = true;
            m_tileList [49].Right = true;
            m_tileList [49].Down = false;
            m_tileList [49].Upper = true;
            m_tileList [50].Left = false;
            m_tileList [50].Right = true;
            m_tileList [50].Down = true;
            m_tileList [50].Upper = true;
            m_tileList [51].Left = false;
            m_tileList [51].Right = false;
            m_tileList [51].Down = true;
            m_tileList [51].Upper = true;
            m_tileList [52].Left = false;
            m_tileList [52].Right = false;
            m_tileList [52].Down = true;
            m_tileList [52].Upper = true;
            m_tileList [53].Left = false;
            m_tileList [53].Right = false;
            m_tileList [53].Down = true;
            m_tileList [53].Upper = true;
            m_tileList [54].Left = false;
            m_tileList [54].Right = false;
            m_tileList [54].Down = true;
            m_tileList [54].Upper = true;
            m_tileList [55].Left = false;
            m_tileList [55].Right = false;
            m_tileList [55].Down = true;
            m_tileList [55].Upper = true;
            m_tileList [56].Left = false;
            m_tileList [56].Right = false;
            m_tileList [56].Down = true;
            m_tileList [56].Upper = true;
            m_tileList [57].Left = false;
            m_tileList [57].Right = false;
            m_tileList [57].Down = true;
            m_tileList [57].Upper = true;
            m_tileList [58].Left = false;
            m_tileList [58].Right = true;
            m_tileList [58].Down = true;
            m_tileList [58].Upper = false;
            m_tileList [59].Left = false;
            m_tileList [59].Right = true;
            m_tileList [59].Down = false;
            m_tileList [59].Upper = true;
            m_tileList [60].Left = false;
            m_tileList [60].Right = true;
            m_tileList [60].Down = true;
            m_tileList [60].Upper = false;
            m_tileList [61].Left = true;
            m_tileList [61].Right = true;
            m_tileList [61].Down = false;
            m_tileList [61].Upper = true;
            m_tileList [62].Left = false;
            m_tileList [62].Right = false;
            m_tileList [62].Down = true;
            m_tileList [62].Upper = true;
            m_tileList [63].Left = false;
            m_tileList [63].Right = false;
            m_tileList [63].Down = true;
            m_tileList [63].Upper = true;
            m_tileList [64].Left = false;
            m_tileList [64].Right = false;
            m_tileList [64].Down = true;
            m_tileList [64].Upper = false;
            m_tileList [65].Left = false;
            m_tileList [65].Right = false;
            m_tileList [65].Down = false;
            m_tileList [65].Upper = false;
            m_tileList [66].Left = false;
            m_tileList [66].Right = true;
            m_tileList [66].Down = false;
            m_tileList [66].Upper = true;
            m_tileList [67].Left = true;
            m_tileList [67].Right = true;
            m_tileList [67].Down = true;
            m_tileList [67].Upper = false;
            m_tileList [68].Left = true;
            m_tileList [68].Right = true;
            m_tileList [68].Down = false;
            m_tileList [68].Upper = false;
        }
        private void LayoutSecondTileInY()
        {
            m_tileList [23].Left = true;
            m_tileList [23].Right = true;
            m_tileList [23].Down = false;
            m_tileList [23].Upper = false;
            m_tileList [24].Left = false;
            m_tileList [24].Right = true;
            m_tileList [24].Down = false;
            m_tileList [24].Upper = true;
            m_tileList [25].Left = false;
            m_tileList [25].Right = false;
            m_tileList [25].Down = true;
            m_tileList [25].Upper = true;
            m_tileList [26].Left = false;
            m_tileList [26].Right = true;
            m_tileList [26].Down = true;
            m_tileList [26].Upper = true;
            m_tileList [27].Left = false;
            m_tileList [27].Right = false;
            m_tileList [27].Down = true;
            m_tileList [27].Upper = true;
            m_tileList [28].Left = false;
            m_tileList [28].Right = false;
            m_tileList [28].Down = true;
            m_tileList [28].Upper = true;
            m_tileList [29].Left = false;
            m_tileList [29].Right = false;
            m_tileList [29].Down = true;
            m_tileList [29].Upper = true;
            m_tileList [30].Left = true;
            m_tileList [30].Right = false;
            m_tileList [30].Down = true;
            m_tileList [30].Upper = true;
            m_tileList [31].Left = false;
            m_tileList [31].Right = false;
            m_tileList [31].Down = true;
            m_tileList [31].Upper = true;
            m_tileList [32].Left = false;
            m_tileList [32].Right = false;
            m_tileList [32].Down = true;
            m_tileList [32].Upper = true;
            m_tileList [33].Left = false;
            m_tileList [33].Right = false;
            m_tileList [33].Down = true;
            m_tileList [33].Upper = true;
            m_tileList [34].Left = false;
            m_tileList [34].Right = false;
            m_tileList [34].Down = true;
            m_tileList [34].Upper = true;
            m_tileList [35].Left = false;
            m_tileList [35].Right = false;
            m_tileList [35].Down = true;
            m_tileList [35].Upper = true;
            m_tileList [36].Left = false;
            m_tileList [36].Right = false;
            m_tileList [36].Down = true;
            m_tileList [36].Upper = true;
            m_tileList [37].Left = false;
            m_tileList [37].Right = false;
            m_tileList [37].Down = true;
            m_tileList [37].Upper = true;
            m_tileList [38].Left = true;
            m_tileList [38].Right = true;
            m_tileList [38].Down = true;
            m_tileList [38].Upper = false;
            m_tileList [39].Left = false;
            m_tileList [39].Right = false;
            m_tileList [39].Down = false;
            m_tileList [39].Upper = true;
            m_tileList [40].Left = false;
            m_tileList [40].Right = false;
            m_tileList [40].Down = true;
            m_tileList [40].Upper = true;
            m_tileList [41].Left = false;
            m_tileList [41].Right = false;
            m_tileList [41].Down = true;
            m_tileList [41].Upper = true;
            m_tileList [42].Left = false;
            m_tileList [42].Right = false;
            m_tileList [42].Down = true;
            m_tileList [42].Upper = true;
            m_tileList [43].Left = false;
            m_tileList [43].Right = false;
            m_tileList [43].Down = true;
            m_tileList [43].Upper = true;
            m_tileList [44].Left = false;
            m_tileList [44].Right = true;
            m_tileList [44].Down = true;
            m_tileList [44].Upper = true;
            m_tileList [45].Left = true;
            m_tileList [45].Right = true;
            m_tileList [45].Down = true;
            m_tileList [45].Upper = false;
        }
        private void LayoutFirstTileInY()
        {
            m_tileList [0].Left = false;
            m_tileList [0].Right = true;
            m_tileList [0].Down = false;
            m_tileList [0].Upper = true;
            m_tileList [1].Left = false;
            m_tileList [1].Right = false;
            m_tileList [1].Down = true;
            m_tileList [1].Upper = true;
            m_tileList [2].Left = false;
            m_tileList [2].Right = false;
            m_tileList [2].Down = true;
            m_tileList [2].Upper = true;
            m_tileList [3].Left = false;
            m_tileList [3].Right = false;
            m_tileList [3].Down = true;
            m_tileList [3].Upper = true;
            m_tileList [4].Left = false;
            m_tileList [4].Right = false;
            m_tileList [4].Down = true;
            m_tileList [4].Upper = true;
            m_tileList [5].Left = false;
            m_tileList [5].Right = false;
            m_tileList [5].Down = true;
            m_tileList [5].Upper = true;
            m_tileList [6].Left = false;
            m_tileList [6].Right = false;
            m_tileList [6].Down = true;
            m_tileList [6].Upper = true;
            m_tileList [7].Left = false;
            m_tileList [7].Right = true;
            m_tileList [7].Down = true;
            m_tileList [7].Upper = true;
            m_tileList [8].Left = false;
            m_tileList [8].Right = false;
            m_tileList [8].Down = true;
            m_tileList [8].Upper = true;
            m_tileList [9].Left = false;
            m_tileList [9].Right = false;
            m_tileList [9].Down = true;
            m_tileList [9].Upper = true;
            m_tileList [10].Left = false;
            m_tileList [10].Right = false;
            m_tileList [10].Down = true;
            m_tileList [10].Upper = true;
            m_tileList [11].Left = false;
            m_tileList [11].Right = false;
            m_tileList [11].Down = true;
            m_tileList [11].Upper = true;
            m_tileList [12].Left = false;
            m_tileList [12].Right = false;
            m_tileList [12].Down = true;
            m_tileList [12].Upper = true;
            m_tileList [13].Left = false;
            m_tileList [13].Right = false;
            m_tileList [13].Down = true;
            m_tileList [13].Upper = true;
            m_tileList [14].Left = false;
            m_tileList [14].Right = false;
            m_tileList [14].Down = true;
            m_tileList [14].Upper = false;
            m_tileList [15].Left = false;
            m_tileList [15].Right = true;
            m_tileList [15].Down = true;
            m_tileList [15].Upper = true;
            m_tileList [16].Left = false;
            m_tileList [16].Right = true;
            m_tileList [16].Down = true;
            m_tileList [16].Upper = true;
            m_tileList [17].Left = false;
            m_tileList [17].Right = false;
            m_tileList [17].Down = false;
            m_tileList [17].Upper = true;
            m_tileList [18].Left = false;
            m_tileList [18].Right = false;
            m_tileList [18].Down = true;
            m_tileList [18].Upper = true;
            m_tileList [19].Left = false;
            m_tileList [19].Right = false;
            m_tileList [19].Down = true;
            m_tileList [19].Upper = true;
            m_tileList [20].Left = false;
            m_tileList [20].Right = false;
            m_tileList [20].Down = true;
            m_tileList [20].Upper = true;
            m_tileList [21].Left = false;
            m_tileList [21].Right = false;
            m_tileList [21].Down = true;
            m_tileList [21].Upper = true;
            m_tileList [22].Left = false;
            m_tileList [22].Right = true;
            m_tileList [22].Down = true;
            m_tileList [22].Upper = false;
        }
        // Update is called once per frame
        void Update()
        {
            //if (minXPos <= maxXPos)
            //{
            //    test++;
            //    Debug.Log(test);
            //}
        }
        private void LayoutPathPoint()
        {
            for (int minX = minXPos; minX <= maxXPos; minX++)
            {
                for(int minY = minYPos; minY <= maxYPos; minY++) 
                {
                    Vector2 vector2 = new Vector2(minX, minY);
                    pathPoint.Add(vector2);
                }
            }
            for (int z = 0; z < pathPoint.Count; z++)
            {
                Debug.Log(pathPoint [z].ToString());
            }
        }
        private void OnDestroy()
        {
            pathPoint.Clear();
        }

    }
}
