using System.Collections.Generic;
using Scriptable_objects;
using UnityEngine;

namespace EveryProject.Scriptable_objects
{
   [CreateAssetMenu(menuName = "Custom/data/drivers")]
   public class Drivers : DataCarrier
   {
      private readonly HashSet<CheckPointChecker> _set = new HashSet<CheckPointChecker>();
      [SerializeField] private GameEvent raiseOnEmptyList;
      
      
      public void Clear()
      {
         _set.Clear();
      }

      public void Add(CheckPointChecker gameObject)
      {
         _set.Add(gameObject);
      }

      public void Remove(CheckPointChecker gameObject)
      {
         if (_set.Remove(gameObject) && _set.Count == 0 && raiseOnEmptyList)
         {
            raiseOnEmptyList.Raise();
         }
      }
   
      public void RemoveAndDestroy(CheckPointChecker gameObject)
      {
         Destroy(gameObject);
         Remove(gameObject);
      }

      public bool Contains(CheckPointChecker gameObject)
      {
         return _set.Contains(gameObject);
      }

      public bool IsEmpty()
      {
         return _set.Count == 0;
      }
   }
}
