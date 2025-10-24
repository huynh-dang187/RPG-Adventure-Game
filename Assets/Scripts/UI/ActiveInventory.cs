using UnityEngine;
using UnityEngine.InputSystem; // nếu dùng Input System package

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        // đăng ký sự kiện ở OnEnable
        playerControls.Inventory.Keyboard.performed += OnInventoryInput;
    }

    private void OnDisable()
    {
        // hủy đăng ký và disable controls
        playerControls.Inventory.Keyboard.performed -= OnInventoryInput;
        playerControls.Disable();
    }

    // Input callback tách riêng cho rõ ràng
    private void OnInventoryInput(InputAction.CallbackContext ctx)
    {
        // đọc giá trị float rồi chuyển sang int
        int value = (int)ctx.ReadValue<float>();
        ToggleActiveSlot(value);
    }

    private void ToggleActiveSlot(int numValue)
    {
        // nếu mapping của bạn là phím số 1..n và bạn muốn index 0-based:
        int index = numValue - 1;
        ToggleActiveHighlight(index);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        // kiểm tra bounds để tránh lỗi
        int childCount = transform.childCount;
        if (indexNum < 0 || indexNum >= childCount)
        {
            Debug.LogWarning($"Index {indexNum} ngoài phạm vi (0..{childCount - 1}).");
            return;
        }

        activeSlotIndexNum = indexNum;

        // vòng lặp qua từng child (Transform)
        foreach (Transform inventorySlot in transform)
        {
            // đảm bảo child có ít nhất 1 child con trước khi GetChild(0)
            if (inventorySlot.childCount > 0)
                inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        // Bật highlight cho slot được chọn (cũng kiểm tra childCount)
        Transform selectedSlot = transform.GetChild(indexNum);
        if (selectedSlot.childCount > 0)
            selectedSlot.GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        Debug.Log(transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab.name);
    }
}
