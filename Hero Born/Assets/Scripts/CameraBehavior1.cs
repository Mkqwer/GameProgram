using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehavior1 : MonoBehaviour
{
    public Transform Target;                     // 드래그로 Player 할당
    public Vector3 CamOffset = new Vector3(0f, 1.2f, -2.6f);
    public float RotationSpeed = 200f;
    public float PitchMin = -40f;
    public float PitchMax = 60f;
    public bool RequireRightMouse = true;        // true면 우클릭해야 회전
    public bool InvertY = false;

    private float _yaw;
    private float _pitch;

    void Start()
    {
        if (Target == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) Target = go.transform;
        }

        var euler = transform.eulerAngles;
        _yaw = euler.y;
        _pitch = euler.x;
    }

    void LateUpdate()
    {
        if (Target == null) return;

        Vector2 mouseDelta = Vector2.zero;

        // 새 Input System 우선 사용, 없으면 레거시 축 사용
        if (Mouse.current != null)
        {
            if (!RequireRightMouse || (Mouse.current.rightButton.isPressed))
                mouseDelta = Mouse.current.delta.ReadValue();
        }
        else
        {
            if (!RequireRightMouse || Input.GetMouseButton(1))
                mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 10f;
        }

        // 회전 업데이트 (마우스 움직임 -> 각도)
        _yaw += mouseDelta.x * RotationSpeed * Time.deltaTime;
        _pitch += (InvertY ? 1 : -1) * mouseDelta.y * RotationSpeed * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, PitchMin, PitchMax);

        // 카메라 위치/회전 적용 (오브핏을 회전시켜 타겟 주위를 돈다)
        Quaternion rot = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 desiredPos = Target.position + rot * CamOffset;
        transform.position = desiredPos;
        transform.rotation = rot;

        // 타겟을 정확히 바라보게 하고 싶으면 아래 주석 해제:
        // transform.LookAt(Target.position + Vector3.up * 1f);
    }
}
