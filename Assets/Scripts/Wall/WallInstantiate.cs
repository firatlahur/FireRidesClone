using UnityEngine;

namespace FireRidesClone.Wall
{
    public class WallInstantiate : MonoBehaviour
    {
        public GameObject wall, wallContainer;
        public Material[] wallColor;

        private float _wallGap;

        private void Awake()
        {
            _wallGap = .5f;
        }

        private void Start()
        {
            InstantiateWall();
        }

        private void InstantiateWall()
        {
            float bottomGapCalculator = 0;
            float topGapCalculator = 0;
            for (int i = 0; i < 40; i++)
            {
                bottomGapCalculator += _wallGap;
                topGapCalculator += _wallGap;


                GameObject bottomWall = Instantiate(wall,
                    wall.transform.position + new Vector3(0, Random.Range(-.5f, 1f), bottomGapCalculator),
                    Quaternion.identity);
                
                bottomWall.transform.GetComponent<MeshRenderer>().material = wallColor[0];
                bottomWall.transform.name = "Bottom_" + i.ToString();
                bottomWall.transform.parent = wallContainer.transform;

                GameObject topWall = Instantiate(wall, wall.transform.position + new Vector3(0, Random.Range(7, 8.5f), topGapCalculator), Quaternion.identity);
                topWall.transform.name = "Top_" + i.ToString();
                topWall.transform.parent = wallContainer.transform;

                if (i % 2 != 0) continue;
                topWall.transform.GetComponent<MeshRenderer>().material = wallColor[1];
                bottomWall.transform.GetComponent<MeshRenderer>().material = wallColor[1];
            }
        }
    }
}

