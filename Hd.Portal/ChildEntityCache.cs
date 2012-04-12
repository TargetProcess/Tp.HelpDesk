// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Hd.Portal.Components;

using log4net;

namespace Hd.Portal
{
	public abstract class ChildEntityCache
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void PreloadChilds(IList list)
		{
			Hashtable hashtable = new Hashtable();
			PreloadChilds(hashtable, list);
		}

		private void PreloadChilds(Hashtable hashtable, IList list)
		{
			log.Debug("Preload childs");

			int[] parentIds = new int[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				parentIds[i] = ((IEntity) list[i]).ID.Value;
			}

			log.Debug("Process childs loading is started");
			ProcessEntries(hashtable, parentIds);
			log.Debug("Process childs loading is completed");
			Context.SetValue(GetCacheName(), hashtable);
		}

		protected abstract void ProcessEntries(Hashtable hashtable, int[] ids);

		protected void AppendGroupToHashtable(int id, IList childs, Hashtable hashtable)
		{
			if (!hashtable.Contains(id))
			{
				hashtable.Add(id, childs);
			}
		}

		public IList GetCachedChilds(int ID)
		{
			Hashtable cachedTeams = Context.GetValue(GetCacheName()) as Hashtable;

			if (cachedTeams == null)
			{
				return null;
			}

			return cachedTeams[ID] as IList;
		}

		private string GetCacheName()
		{
			return GetType().Name + "CACHED_CHILDS";
		}
	}
}