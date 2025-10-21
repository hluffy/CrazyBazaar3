
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EventClick : MonoBehaviour, IPointerClickHandler
{

 
    private GameObject player;
    private PassNavi passNavi;
 
    private ClickHouse clickHouse;
    private void Start()
    {
 
        clickHouse = new ClickHouse();
        clickHouse.InitHouse();
        player =GameObject.FindGameObjectWithTag("Player");
    }

    public void OnPointerClick(PointerEventData eventData)
    {


        EditorGUIUtility.PingObject(gameObject);
        Selection.activeObject = gameObject;

        if (tag == "house_tree")
        {
       
            print("EventClick����ˣ�" + this.name + "-----" + this.tag);
            OnHouseClick();

        }
        else
        {
            print("EventClick����ˣ�" + this.name + "-----" + this.tag);
          
        }


    }
    private void OnGroundClick()
    {
        print("EventClick����˵���" );
        if (User.userState == UserState.Chat)
        {
            print("User.userState == UserState.Chat");
        }
    }
    private void OnHouseClick()
    {
        if (Input.GetMouseButtonUp(1))
        {
            print("�Ҽ����������");
            return;
        }
        if (User.userState != UserState.Idil) return;

        if (player == null) return;
        clickHouse.OnHouseClick(player.transform,transform);
        
    }
  
  
}
