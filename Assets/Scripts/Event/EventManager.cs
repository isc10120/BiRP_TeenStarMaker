using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public int maxEventCount = 3;
    [SerializeField] private Event[] events;

    [HideInInspector] public int eventCount;

    private readonly Dictionary<string, Event> eventsDict = new Dictionary<string, Event>();

    // 리셋 전까지 다시 나오면 안 되는 이벤트 기록
    private readonly List<string> prevEvents = new List<string>();

    // 현재 이벤트
    private readonly List<string> nowEvents = new List<string>();

    public Action<int, Event>[] OnEventSets = new Action<int, Event>[3];

    private void Start()
    {
        eventsDict.Clear();

        foreach (var e in events)
        {
            if (e == null) continue;
            if (string.IsNullOrEmpty(e.eventName)) continue;

            eventsDict[e.eventName] = e; // 중복 이름이면 마지막 값으로 덮어씀
        }

        GameManager.Instance.onDayChanged += ResetEvents;
    }


    // 하루에 랜덤 3개 선정(가능하면 3개, 부족하면 가능한 만큼)
    public void SetRandom3Events()
    {
        nowEvents.Clear();

        // 후보 풀 만들기: prevEvents에 없는 것들만
        List<string> candidates = new List<string>();
        foreach (var kv in eventsDict)
        {
            string eventName = kv.Key;
            if (!prevEvents.Contains(eventName))
            {
                candidates.Add(eventName);
            }
        }

        // 중복 없이 랜덤으로 3개 뽑기
        for (int i = 0; i < 3; i++)
        {
            int idx = UnityEngine.Random.Range(0, candidates.Count);
            string pickedName = candidates[idx];

            nowEvents.Add(pickedName);
            prevEvents.Add(pickedName);

            OnEventSets[i]?.Invoke(i, eventsDict[pickedName]);

            candidates.RemoveAt(idx);
        }
    }

    public List<Event> GetEvents()
    {
        List<Event> result = new List<Event>(nowEvents.Count);

        foreach (string eventName in nowEvents)
        {
            if (eventsDict.TryGetValue(eventName, out Event e) && e != null)
                result.Add(e);
            else
                Debug.LogError($"{eventName} is not a valid event");
        }

        return result;
    }


    private void ResetEvents()
    {
        prevEvents.Clear();
        SetRandom3Events();
        eventCount = 0;
    }
}
