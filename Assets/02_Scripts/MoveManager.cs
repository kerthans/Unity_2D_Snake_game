using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;

public enum DicType
{
    Horizontal,
    Vertical,
}
public enum ChildAlignment
{
    UpperLeft,
    UpperCenter,
    UpperRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    LowerLeft,
    LowerCenter,
    LowerRight,
}

public class MoveManager : MonoBehaviour
{
    public RectTransform img;
    public RectTransform m_content;
    private ScrollRect m_scrollRect;
    private ContentSizeFitter m_Content;
    //private Image[] child;
    private List<RectTransform> child;
    SerializedProperty m_a;
    public int Left, Right, Top, Bottom, Spacing;
    public Vector2 Item;
    public DicType Unit;
    public ChildAlignment childAlignmentType;
    public bool isHorizontal, isVertical, isShowHor, isShowVer, isShowHorPadding, isShowVerPadding;
    public bool ControlChildSizeWidth, ControlChildSizeHeight
        , UseChildScaleWidth, UseChildScaleHeight
        , ChildForceExpandWidth, ChildForceExpandHeight;

    private void Awake()
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_Content = m_scrollRect.GetComponentInChildren<Mask>().GetComponentInChildren<ContentSizeFitter>();
        //child = m_Content.GetComponentsInChildren<Image>();
    }
    void Start() 
    {
        child = new List<RectTransform>();
        for (int i = 0; i < 1; i++)
        {
            RectTransform obj = Instantiate(img, m_content);
            child.Add(obj);
        }
       
        ImgLayOut();
    }

    void ImgLayOut() 
    {
        HorizontalOrVerticalLayoutGroup m_Content_LayoutGroup = null;
        switch (Unit)
        {
            case DicType.Horizontal:
                m_scrollRect.vertical = false;
                m_scrollRect.horizontal = true;
                m_Content.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                m_Content.gameObject.AddComponent<HorizontalLayoutGroup>();//添加垂直
                foreach (RectTransform item in child)
                {
                    item.sizeDelta = Item;
                }
                m_Content_LayoutGroup = m_Content.gameObject.GetComponent<HorizontalLayoutGroup>();

                break;
            case DicType.Vertical:
                m_scrollRect.horizontal = false;
                m_scrollRect.vertical = true;
                m_Content.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                m_Content.gameObject.AddComponent<VerticalLayoutGroup>();//添加水平
                foreach (var item in child)
                {
                    item.sizeDelta = Item;
                }
                m_Content_LayoutGroup = m_Content.gameObject.GetComponent<VerticalLayoutGroup>();
                break;
            default:
                break;
        }

        //赋制
        m_Content_LayoutGroup.padding = new RectOffset(Left, Right, Top, Bottom);
        m_Content_LayoutGroup.spacing = Spacing;
        m_Content_LayoutGroup.childAlignment = (TextAnchor)childAlignmentType;
        m_Content_LayoutGroup.childControlWidth = ControlChildSizeWidth;
        m_Content_LayoutGroup.childControlHeight = ControlChildSizeHeight;
        m_Content_LayoutGroup.childScaleWidth = UseChildScaleWidth;
        m_Content_LayoutGroup.childScaleHeight = UseChildScaleHeight;
        m_Content_LayoutGroup.childForceExpandWidth = ChildForceExpandWidth;
        m_Content_LayoutGroup.childForceExpandHeight = ChildForceExpandHeight;
    }
}