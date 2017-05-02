using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
public class ButtonEventListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IUpdateSelectedHandler,IPointerEnterHandler
{
    public bool mNeedScale = true;
    public float mDuration = 0.08f;
    public float mScale = 0.9f;

    private bool mPointDown = false;
    private bool mPointUp = false;
    private float mPointUpTimer = 1.0f;
    private float mScaleSpeed = 1.0f;

    public Transform mTransform = null;
    public delegate  void ButtonEventDelegate(PointerEventData eventData);
    public ButtonEventDelegate onPointerDown = null;
    public ButtonEventDelegate onPointerUp = null;
    public ButtonEventDelegate onPointerEnter = null;
    public ButtonEventDelegate onPointerExit = null;

    public delegate  void SelectEventDelegate(BaseEventData eventData);
    public SelectEventDelegate onUpdateSelected = null;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onPointerDown != null)
        {
            onPointerDown(eventData);
        }

        if (mNeedScale)
        {
            mPointDown = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onPointerUp != null)
        {
            onPointerUp(eventData);
        }

        if (mNeedScale)
        {
            mPointDown = false;
            mPointUp = true;
            mPointUpTimer = 0.0f;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onPointerEnter != null)
        {
            onPointerEnter(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onPointerExit != null)
        {
            onPointerExit(eventData);
        }
    }

   

    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelected != null)
        {
            onUpdateSelected(eventData);
        }
    }

    // Use this for initialization
    void Start () {
        if (mTransform == null)
            mTransform = this.transform;
        mScaleSpeed = (mScale - 1.0f) / mDuration;
    }
        
    void Update () {
        if (!mNeedScale)
        {
            return;
        }

        if (mPointDown)
        {
            mTransform.localScale = new Vector3(mScale, mScale, 1.0f);
           
        }
        else if (mPointUp)
        {
            if (mPointUpTimer < mDuration && mTransform.localScale.x < 1.0f)
            {
                float fScale = mTransform.localScale.x - mScaleSpeed * Time.deltaTime;
                mTransform.localScale = new Vector3(fScale, fScale, 1.0f);
                mPointUpTimer += Time.deltaTime;
            }
            else
            {
                mPointUp = false;
                mTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        } 
    }
}
