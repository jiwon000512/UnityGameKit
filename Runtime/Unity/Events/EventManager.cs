using System;
using System.Collections.Generic;
using GameKit.Singleton;

namespace GameKit.Events
{
    public sealed class EventManager : MonoSingleton<EventManager>
    {
        private readonly Dictionary<Type, List<Delegate>> m_handlers = new Dictionary<Type, List<Delegate>>();

        public IDisposable Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (!m_handlers.TryGetValue(typeof(TEvent), out List<Delegate> list))
            {
                list = new List<Delegate>();
                m_handlers[typeof(TEvent)] = list;
            }

            list.Add(handler);
            return new Subscription(list, handler);
        }

        public void Publish<TEvent>(TEvent evt)
        {
            if (!m_handlers.TryGetValue(typeof(TEvent), out List<Delegate> list))
            {
                return;
            }

            // 핸들러 안에서 구독·해제가 일어나도 순회가 깨지지 않도록 스냅샷을 돈다.
            Delegate[] snapshot = list.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                ((Action<TEvent>)snapshot[i]).Invoke(evt);
            }
        }

        public void Clear()
        {
            m_handlers.Clear();
        }

        private sealed class Subscription : IDisposable
        {
            private List<Delegate> m_list;
            private readonly Delegate m_handler;

            public Subscription(List<Delegate> list, Delegate handler)
            {
                m_list = list;
                m_handler = handler;
            }

            public void Dispose()
            {
                m_list?.Remove(m_handler);
                m_list = null;
            }
        }
    }
}
