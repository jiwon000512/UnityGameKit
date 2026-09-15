using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameKit.Singleton;

namespace GameKit.UI
{
    public sealed class UIManager : MonoSingleton<UIManager>
    {
        private const string k_RootPath = "UI";
        private static readonly Vector2 k_ReferenceResolution = new Vector2(1080, 1920);

        private readonly Dictionary<Type, UIView> m_views = new Dictionary<Type, UIView>();
        private readonly List<UIView> m_stack = new List<UIView>();
        private Transform m_root;

        public T Open<T>(object args = null) where T : UIView
        {
            T view = GetView<T>();
            view.transform.SetAsLastSibling();
            view.gameObject.SetActive(true);
            m_stack.Remove(view);
            m_stack.Add(view);
            view.OnOpen(args);
            return view;
        }

        public void Close(UIView view)
        {
            if (!m_stack.Remove(view))
            {
                return;
            }

            view.OnClose();
            view.gameObject.SetActive(false);
        }

        public void CloseTop()
        {
            if (m_stack.Count > 0)
            {
                Close(m_stack[m_stack.Count - 1]);
            }
        }

        public T GetView<T>() where T : UIView
        {
            if (!m_views.TryGetValue(typeof(T), out UIView view))
            {
                GameObject prefab = Resources.Load<GameObject>($"{k_RootPath}/{typeof(T).Name}");
                view = Instantiate(prefab, GetRoot()).GetComponent<T>();
                view.gameObject.SetActive(false);
                m_views[typeof(T)] = view;
            }

            return (T)view;
        }

        private Transform GetRoot()
        {
            if (m_root == null)
            {
                var canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvas.transform.SetParent(transform, false);
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

                var scaler = canvas.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = k_ReferenceResolution;
                scaler.matchWidthOrHeight = 0.5f;

                m_root = canvas.transform;
            }

            return m_root;
        }
    }
}
