using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileInput : MonoBehaviour,IDragHandler,IPointerUpHandler,IPointerDownHandler {
	
	private Image joystick;
	private Image stickChild;

	private Vector2 InputV;

	private void Start(){
		joystick = GetComponent<Image> ();
		stickChild = transform.GetChild (0).GetComponent<Image> ();
	}

	public virtual void OnPointerUp(PointerEventData point){
		InputV = Vector2.zero;
		stickChild.rectTransform.anchoredPosition = Vector2.zero;
	}
	public virtual void OnPointerDown(PointerEventData point){
		OnDrag (point);
	}

	public virtual void OnDrag(PointerEventData point){
		Vector2 pos;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (joystick.rectTransform, point.position, point.pressEventCamera, out pos)) {
			pos.x = (pos.x / joystick.rectTransform.sizeDelta.x);
			pos.y = (pos.y / joystick.rectTransform.sizeDelta.y);

			InputV = new Vector2 (pos.x*2  ,pos.y*2);
			InputV = (InputV.magnitude > 1.0f) ? InputV.normalized : InputV;

			stickChild.rectTransform.anchoredPosition = new Vector2 ( InputV.x*(joystick.rectTransform.sizeDelta.x/2),InputV.y*(joystick.rectTransform.sizeDelta.y/2));
		}
	}

	public float Horizontal(){
		if (InputV.x != 0) return InputV.x;
		else return Input.GetAxis ("Horizontal");
	}

	public float Vertical(){
		if (InputV.y != 0) return InputV.y;
		else return Input.GetAxis ("Vertical");
	}
}
