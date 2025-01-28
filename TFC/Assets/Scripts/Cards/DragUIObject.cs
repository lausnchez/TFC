using UnityEngine;
using UnityEngine.EventSystems; //This allows us to use Unity's event system to detect our mouse inputs

public class DragUIObject : MonoBehaviour, IDragHandler, IPointerDownHandler //These classes hold the methods required to handle UI interactions that we need
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;   // Posición original del ratón equivalente en pantalla
    private Vector3 originalPanelLocalPosition; // Posición original del objeto en canvas
    public float movementSensitivity = 1.0f; // Adjustable sensitivity if needed

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); //Get the RectTransform component of the attached GameObject
        canvas = GetComponentInParent<Canvas>(); //Get the Canvas component of the attached GameObject

        if (canvas == null)
        {
            Debug.Log("Canvas not found.");
        }
    }

    public void OnPointerDown(PointerEventData eventData) //This is inherited from the IPointerDownHandler class referenced above
    {
        /*
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition); //Using the event system to detect what is clicked on
        originalPanelLocalPosition = rectTransform.localPosition;
        */

        /*
         ScreenPointToLocalPointInRectangle(rectTransformCanvas, vector2 puntoEnPantalla, camara, out Vector2 posicionOriginalDelPuntero)
         Convierte la posición del puntero en la pantalla a posición dentro del canvas y la guarda en out.
         */

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out originalLocalPointerPosition))
        {
            // Asigna las coordenadas locales dentro de su padre inmediato
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData) //This is inherited from the IDragHandler class referenced above
    {
        /*
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition))
        {
            localPointerPosition /= canvas.scaleFactor;

            // Adjusting the movement based on sensitivity
            Vector3 offsetToOriginal = (localPointerPosition - originalLocalPointerPosition) * movementSensitivity;
            rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;

            // Debug output
            //Debug.Log($"Drag - LocalPointerPosition: {localPointerPosition}, Offset: {offsetToOriginal}, New Position: {rectTransform.localPosition}"); //Comment out this line if not debugging an issue, otherwise it will flood the console unnecessarily
        }
        */
        if (canvas == null) return;

        // Convierte la posición del puntero a su equivalente en canvas
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition))
        {
            // Calcula cuánto se ha movido el puntero
            Vector3 offsetToOriginal = (localPointerPosition - originalLocalPointerPosition);

            // Mueve el objeto la misma distancia que se ha movido el puntero
            rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
        }
    }
}
