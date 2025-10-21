using System.Collections;
using UnityEngine;

namespace Assets.script.nav
{


   // 随机生成指定的物体
    public class ObstacleManage
    {
        // public   GameObject[] prefabs;

        // Update is called once per frame
        /*      void Update()
              {
                  if (Input.GetKeyDown(KeyCode.Space))
                  {
                      GameObject gameObj = GameObject.
                          Instantiate(prefabs[Random.Range(0, prefabs.Length)],transform);
                      gameObj.transform.localPosition = 
                          new Vector3(Random.Range(-9f,11f),0, Random.Range(-5.7f, 6f));

                  }
              }*/

        public static void createPlane(GameObject[] prefabs, Transform parent)
        {
            // 创建floor
             
        }
        public  static  void createFlower(int count,GameObject[] prefabs,Transform parent)
        {
            // 随机生成花草
            /* for(int i = 0; i < 10; i++)
             {
                 GameObject gameObj = GameObject.
                    Instantiate(prefabs[Random.Range(0, prefabs.Length)], parent);
                 gameObj.transform.localPosition =
                     new Vector3(Random.Range(-45f, 45f), 0, Random.Range(-45f, 45f));

             }*/

            if (count == 0)
            {
                count = Random.Range(100, 200);
            }
            // 随机生成蘑菇
            for (int i = 0; i < count; i++)
            {
                GameObject gameObj = GameObject.
                   Instantiate(prefabs[Random.Range(0, prefabs.Length)], parent);
                gameObj.transform.localPosition =
                    new Vector3(Random.Range(0f, 10000f), 0, Random.Range(0f, -10000f));
                gameObj.transform.rotation = Quaternion.Euler(45f, 0, 0);
                /*gameObj.transform.rotation = prefabs[0].transform.rotation;
                gameObj.transform.localScale= prefabs[0].transform.localScale;
                gameObj.transform.localPosition =
                   new Vector3(0, 10,0);*/
            }

        }

        public static void createVillage( GameObject[] prefabs, Transform parent)
        {


            // 随机部落数量
            int count = Random.Range(2, 8);
            // 随机生成蘑菇

            for (int i = 0; i < count; i++) 
            {
                GameObject gameObj = GameObject.
                   Instantiate(prefabs[Random.Range(0, prefabs.Length)], parent);
                gameObj.transform.localPosition =
                    new Vector3(Random.Range(0f, 10000f), 0, Random.Range(0f, -10000f));
                gameObj.transform.rotation = Quaternion.Euler(45f, 0, 0);
                /*gameObj.transform.rotation = prefabs[0].transform.rotation;
                gameObj.transform.localScale= prefabs[0].transform.localScale;
                gameObj.transform.localPosition =
                   new Vector3(0, 10,0);*/
            }

        }
    }
}