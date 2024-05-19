using UnityEngine;

public class ItemEquip : MonoBehaviour
{
    public GameObject[] itemPrefabs; // Array to hold the prefabs of your items
    public Transform equipmentPoint1; // Transform where the first item will be equipped
    public Transform equipmentPoint2; // Transform where the second item will be equipped

    private GameObject equippedItem1; // Currently equipped item at point 1
    private GameObject equippedItem2; // Currently equipped item at point 2
    private int currentItemIndex1 = 0; // Index of the currently equipped item at point 1
    private int currentItemIndex2 = 0; // Index of the currently equipped item at point 2

    void Start()
    {
        EquipItem(currentItemIndex1, 0); // Equip the initial item at point 1
        EquipItem(currentItemIndex2, 1); // Equip the initial item at point 2
    }

    void Update()
    {
        // Equip using keys 1 and 2 for item 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem(0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipItem(1, 0);
        }

        // Equip using keys 3 and 4 for item 2
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipItem(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipItem(1, 1);
        }

        // Equip using mouse wheel for item 1
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentItemIndex1 = (currentItemIndex1 + 1) % itemPrefabs.Length;
            EquipItem(currentItemIndex1, 0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentItemIndex1 = (currentItemIndex1 - 1 + itemPrefabs.Length) % itemPrefabs.Length;
            EquipItem(currentItemIndex1, 0);
        }

        // Equip using right mouse wheel for item 2 (optional)
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && Input.GetMouseButton(1))
        {
            currentItemIndex2 = (currentItemIndex2 + (Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : -1) + itemPrefabs.Length) % itemPrefabs.Length;
            EquipItem(currentItemIndex2, 1);
        }
    }

    void EquipItem(int index, int equipmentPointIndex)
    {
        if (equipmentPointIndex == 0)
        {
            // Unequip the currently equipped item at point 1
            if (equippedItem1 != null)
            {
                Destroy(equippedItem1);
            }

            // Instantiate and equip the new item at point 1
            equippedItem1 = Instantiate(itemPrefabs[index], equipmentPoint1.position, equipmentPoint1.rotation, equipmentPoint1);
        }
        else if (equipmentPointIndex == 1)
        {
            // Unequip the currently equipped item at point 2
            if (equippedItem2 != null)
            {
                Destroy(equippedItem2);
            }

            // Instantiate and equip the new item at point 2
            equippedItem2 = Instantiate(itemPrefabs[index], equipmentPoint2.position, equipmentPoint2.rotation, equipmentPoint2);
        }
    }
}